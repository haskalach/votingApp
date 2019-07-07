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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Application.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class VoterController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<User> _userManager;

        public VoterController (IGeneralRepository repo, IMapper mapper, IHostingEnvironment hostingEnvironment, UserManager<User> userManager) {
            _userManager = userManager;
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

        [HttpGet ("Code/{code}")]
        public async Task<IActionResult> GetVoterByCode (string code) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            int intCode;
            Int32.TryParse (code, out intCode);
            var OrganizationId = userFromRepo.OrganizationId;
            var Organization = await _repo.GetOrganization (OrganizationId?? default (int));
            var VoterTypeId = Organization.VoterTypeId;
            var voterFromRepo = await _repo.GetVoter (intCode, VoterTypeId, OrganizationId ?? default (int));
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

        [HttpGet ("contact/{Id}")]
        public async Task<IActionResult> Contact (int Id) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var voterFromRepo = await _repo.GetVoterById (Id, OrganizationId ?? default (int));
            voterFromRepo.Contacted = true;
            if (await _repo.SaveAll ()) {
                return Ok (voterFromRepo);
            } else {
                return BadRequest ("Could not Set Contacted For This Voter");
            }

        }

        [HttpPost ("attend")]
        public async Task<IActionResult> Attend (AttendDto attendDto) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var voterFromRepo = await _repo.GetVoterById (attendDto.Id, OrganizationId ?? default (int));
            voterFromRepo.Attend = attendDto.Attend;
            if (await _repo.SaveAll ()) {
                return Ok (voterFromRepo);
            } else {
                return BadRequest ("Could not Set Attend For This Voter");
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

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost ("upload/{Id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile (int Id) {
            var Organization = await _repo.GetOrganizationByType (Id);
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
                        if (sheet == null) {
                            return BadRequest (" wrong excel sheet name");
                        } else {
                            foreach (DataRow row in sheet.Rows) {
                                Voter dataItem = new Voter ();
                                User RefUser = new User ();
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
                                dataItem.Abroad = NullToString (row["Abroad"]).ToLower () != "true" ? false : true;
                                dataItem.VoterTypeId = Id;
                                if (NullToString (row["Reference"]) != String.Empty) {
                                    RefUser.Name = NullToString (row["Reference"]);
                                    RefUser.UserName = CamelCase (Organization.Name) + "-" + NullToString (row["Reference"]);
                                    RefUser.OrganizationId = Organization.Id;
                                    var user = await _userManager.FindByNameAsync (RefUser.UserName);
                                    if (user == null) {
                                        List<string> selectedRoles = new List<string> ();
                                        selectedRoles.Add ("Reference");
                                        var result = await _userManager.CreateAsync (RefUser, "password");
                                        user = await _userManager.FindByNameAsync (RefUser.UserName);
                                        var Rolesresult = await _userManager.AddToRolesAsync (user, selectedRoles);

                                    }
                                    dataItem.ReferenceId = user.Id;
                                }

                                // bool exists = false;

                                Int32.TryParse (row["Code"].ToString (), out code);
                                dataItem.Code = code;
                                // list.ForEach (item => {
                                //     if (item.Code == dataItem.Code) {
                                //         exists = true;
                                //     }
                                // });
                                // if (await _repo.GetVoter (dataItem.Code, Id, 0) == null && exists == false) {
                                //     list.Add (dataItem);
                                // }
                                list.Add (dataItem);
                                _repo.Add (dataItem);

                            }
                            System.IO.File.Delete (fullPath);
                        }

                    }
                }
                if (list.Count == 0) {
                    return Ok ();
                }
                if (await _repo.SaveAll ()) {
                    return Ok ();
                } else {
                    return BadRequest ("something wrong happend while saving");
                }
            } catch (System.Exception ex) {
                return BadRequest ("Upload Failed: " + ex.Message);
            }

        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpDelete ("oldData/{Id}")]
        public async Task<IActionResult> deleteOldData (int Id) {
            var voters = await _repo.GetAllVotersByType (Id);
            var changes = false;
            foreach (var voter in voters) {
                if (voter != null) {
                    changes = true;
                    _repo.Delete (voter);
                }

            }
            var Organization = await _repo.GetOrganizationByType (Id);
            var referenceUsers = await _repo.GetOrganizationReferences (Organization.Id, 0);
            foreach (var user in referenceUsers) {
                changes = true;
                _repo.Delete (user);

            }
            if (await _repo.SaveAll ()) {
                return Ok ();
            } else {
                return BadRequest ("could not delete old data");
            }
        }

        [HttpGet ("configuration")]
        public async Task<IActionResult> GetConfiguration () {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var Organization = await _repo.GetOrganization (OrganizationId?? default (int));
            var VoterTypeId = Organization.VoterTypeId;
            var configList = await _repo.GetConfigList (VoterTypeId, OrganizationId?? default (int));
            return Ok (configList);
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet ("Export/{Id}")]
        public async Task<IActionResult> Excel (int Id) {

            // await Task.Yield ();
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format ("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo (Path.Combine (sWebRootFolder, sFileName));
            if (file.Exists) {
                file.Delete ();
                file = new FileInfo (Path.Combine (sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage (file)) {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add ("Voters");
                var voters = await _repo.GetAllVotersByType (Id);
                var votersForExport = _mapper.Map<IEnumerable<VotersExportDto>> (voters);
                worksheet.Cells.LoadFromCollection (votersForExport, true);

                package.Save (); //Save the workbook.
            }
            Export EportObj = new Export ();
            EportObj.URL = URL;
            return Ok (EportObj);
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
        static string CamelCase (string s) {
            if (s == null || s.Length < 2)
                return s;

            // Split the string into words.
            string[] words = s.Split (
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower ();
            for (int i = 1; i < words.Length; i++) {
                result +=
                    words[i].Substring (0, 1).ToUpper () +
                    words[i].Substring (1);
            }

            return result;
        }
    }
}