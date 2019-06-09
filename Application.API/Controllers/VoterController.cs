using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Helpers;
using Application.API.Models;
using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
namespace Application.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class VoterController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public VoterController (IGeneralRepository repo, IMapper mapper, IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddVoter (VoterForCreationDto voterForCreationDto) {
            var voterToCreate = _mapper.Map<Voter> (voterForCreationDto);

            _repo.Add (voterToCreate);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add voter");
        }

        [HttpGet]
        public async Task<IActionResult> GetVoters ([FromQuery] VoterParams voterParams) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            if (userFromRepo.OrganizationId > 0) {
                var orgFromRepo = await _repo.GetOrganization (userFromRepo.OrganizationId ?? default (int));
                voterParams.voterTypeId = orgFromRepo.VoterTypeId;
            }

            var voterFromRepo = await _repo.GetVoters (voterParams);

            var voterList = _mapper.Map<ICollection<VoterForReturnDto>> (voterFromRepo);
            Response.AddPagination (voterFromRepo.CurrentPage, voterFromRepo.PageSize, voterFromRepo.TotalCount, voterFromRepo.TotalPages);
            return Ok (voterList);
        }

        [HttpGet ("ReferenceVoters")]
        public async Task<IActionResult> GetReferenceVoters ([FromQuery] VoterParams voterParams) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var voterFromRepo = await _repo.GetReferenceVoters (userId, voterParams);
            var voterList = _mapper.Map<ICollection<VoterForReturnDto>> (voterFromRepo);
            Response.AddPagination (voterFromRepo.CurrentPage, voterFromRepo.PageSize, voterFromRepo.TotalCount, voterFromRepo.TotalPages);
            return Ok (voterList);
        }

        [HttpGet ("{Id}")]
        public async Task<IActionResult> GetVoter (int Id) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var voterFromRepo = await _repo.GetVoterById (Id, OrganizationId ?? default (int));
            var voterToReturn = _mapper.Map<VoterForReturnDto> (voterFromRepo);
            return Ok (voterToReturn);
        }

        [HttpPut ("{Id}")]
        public async Task<IActionResult> EditVoter (int Id, VoterForEditDto voterForEditDto) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var voterFromRepo = await _repo.GetVoterById (Id, OrganizationId ?? default (int));
            _mapper.Map (voterForEditDto, voterFromRepo);
            if (await _repo.SaveAll ()) {
                return NoContent ();
            }
            throw new Exception ($"Updating Voter {Id} failed on save");
        }

        [HttpPost ("vote")]
        public async Task<IActionResult> Vote (VotingYearForCreationDto votingYearForCreationDto) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            votingYearForCreationDto.OrganizationId = OrganizationId ?? default (int);
            var OrganizationFromRepo = await _repo.GetOrganization (OrganizationId ?? default (int));
            var year = DateTime.Today.Year.ToString ();
            votingYearForCreationDto.Year = year;
            var VotingYear = _mapper.Map<VotingYears> (votingYearForCreationDto);
            var VotingYearFromRepo = await _repo.GetVotingYear (VotingYear.VoterId, VotingYear.OrganizationId, VotingYear.Year);
            if (VotingYearFromRepo != null) {
                return BadRequest ("Already Voted This Year");
            } else {
                _repo.Add (VotingYear);
                if (await _repo.SaveAll ()) {
                    return Ok ();
                } else {
                    return BadRequest ("Could not Voter For This User");
                }
            }

        }

        [HttpPut ("updateReference")]

        public async Task<IActionResult> updateReference (ReferenceUpdateDto referenceUpdateDto) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var OrganizationFromRepo = await _repo.GetOrganization (OrganizationId ?? default (int));
            var voterFromRepo = await _repo.GetVoterById (referenceUpdateDto.Id, OrganizationId ?? default (int));
            _mapper.Map (referenceUpdateDto, voterFromRepo);
            if (await _repo.SaveAll ()) {
                return NoContent ();
            }
            throw new Exception ($"Could Not Set Reference Use For {referenceUpdateDto.Id}");
        }

        [HttpPost ("upload/{Id}"), DisableRequestSizeLimit]
        public ActionResult UploadFile (int Id) {
            try {
                var file = Request.Form.Files[0];
                var list = new List<Voter> ();
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine (Directory.GetCurrentDirectory (), folderName);
                if (!Directory.Exists (newPath)) {
                    Directory.CreateDirectory (newPath);
                }
                if (file.Length > 0) {
                    string fileName = ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"');
                    string fullPath = Path.Combine (newPath, fileName);
                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        file.CopyTo (stream);
                    }

                    using (Stream inputStream = file.OpenReadStream ()) {
                        IExcelDataReader reader;

                        reader = ExcelReaderFactory.CreateOpenXmlReader (inputStream);

                        List<string> header = new List<string> ();
                        if (reader.Read ()) {
                            for (int cell = 0; cell < reader.FieldCount; cell++) {
                                header.Add (reader[cell]?.ToString ().Trim ().ToUpper ());
                            }
                        }
                        DataTableCollection sheets = reader.AsDataSet (GetDataSetConfig ()).Tables;
                        DataTable sheet = sheets["Sheet1"];

                        foreach (DataRow row in sheet.Rows) {
                            Voter dataItem = new Voter ();
                            int code;

                            DateTime birthDate;
                            DateTime registration;
                            DateTime graduation;
                            DateTime.TryParse (row["BirthDate"].ToString (), out birthDate);
                            DateTime.TryParse (row["Registration"].ToString (), out registration);
                            DateTime.TryParse (row["Graduation"].ToString (), out graduation);
                            dataItem.FirstNameArabic = NullToString (row["FirstNameArabic"]);
                            dataItem.FatherNameArabic = NullToString (row["FatherNameArabic"]);
                            dataItem.FamilyArabic = NullToString (row["FamilyArabic"]);
                            dataItem.FirstName = NullToString (row["FirstName"]);
                            dataItem.FatherName = NullToString (row["FatherName"]);
                            dataItem.Family = NullToString (row["Family"]);
                            dataItem.Nationality = NullToString (row["Nationality"]);
                            dataItem.Speciality = NullToString (row["Speciality"]);
                            dataItem.SubChapter = NullToString (row["SubChapter"]);
                            dataItem.BirthDate = birthDate;
                            dataItem.BirthCountry = NullToString (row["BirthCountry"]);
                            dataItem.BirthPlace = NullToString (row["BirthPlace"]);
                            dataItem.CivilIdMouhavaza = NullToString (row["CivilIdMouhavaza"]);
                            dataItem.CivilIdKadaa = NullToString (row["CivilIdKadaa"]);
                            dataItem.CivilIdRegion = NullToString (row["CivilIdRegion"]);
                            dataItem.RegisteryNumber = NullToString (row["RegisteryNumber"]);
                            dataItem.CivilIdPlace = NullToString (row["CivilIdPlace"]);
                            dataItem.Registration = registration;
                            dataItem.LastCoveredYear = NullToString (row["LastCoveredYear"]);
                            dataItem.Graduation = graduation;
                            dataItem.School = NullToString (row["School"]);
                            dataItem.GraduationCountry = NullToString (row["GraduationCountry"]);
                            dataItem.AddressWork = NullToString (row["AddressWork"]);
                            dataItem.MobileWork = NullToString (row["MobileWork"]);
                            dataItem.PhoneWork = NullToString (row["PhoneWork"]);
                            dataItem.AddressHome = NullToString (row["AddressHome"]);
                            dataItem.MobileHome = NullToString (row["MobileHome"]);
                            dataItem.PhoneHome = NullToString (row["PhoneHome"]);
                            dataItem.Email = NullToString (row["Email"]);
                            dataItem.Religion = NullToString (row["Religion"]);
                            dataItem.Politic = NullToString (row["Politic"]);
                            dataItem.VoterTypeId = Id;
                            bool exists = false;

                            Int32.TryParse (row["Code"].ToString (), out code);
                            dataItem.Code = code;
                            list.ForEach (item => {
                                if (item.Code == dataItem.Code) {
                                    exists = true;
                                }
                            });
                            if (_repo.GetVoter (dataItem.Code, Id).Result == null && exists == false) {
                                list.Add (dataItem);
                                _repo.Add (dataItem);
                            }

                        }
                        System.IO.File.Delete (fullPath);
                    }
                }
                if (list.Count == 0) {
                    return Ok (list);
                }
                if (_repo.SaveAll ().Result) {
                    return Ok (list);
                } else {
                    return BadRequest ("something wrong happend while saving");
                }
            } catch (System.Exception ex) {
                return BadRequest ("Upload Failed: " + ex.Message);
            }
        }
        private static ExcelDataSetConfiguration GetDataSetConfig () {
            return new ExcelDataSetConfiguration {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration () {
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader => Console.WriteLine ("{0}: {1}", rowReader[0], rowReader[1])
                    }
            };
        }
        static string NullToString (object Value) {

            // Value.ToString() allows for Value being DBNull, but will also convert int, double, etc.
            return Value == null ? "" : Value.ToString ();

            // If this is not what you want then this form may suit you better, handles 'Null' and DBNull otherwise tries a straight cast
            // which will throw if Value isn't actually a string object.
            //return Value == null || Value == DBNull.Value ? "" : (string)Value;

        }
    }
}