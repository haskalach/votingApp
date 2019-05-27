using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Helpers;
using Application.API.Models;
using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Application.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class EngenereController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EngenereController (IGeneralRepository repo, IMapper mapper, IHostingEnvironment hostingEnvironment) {
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddVoter (EngenereForCreationDto engenereForCreationDto) {

            var voterToCreate = _mapper.Map<Engeneres> (engenereForCreationDto);

            _repo.Add (voterToCreate);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add voter");
        }

        [HttpGet]
        public async Task<IActionResult> GetVoters ([FromQuery] EngenereParams engenereParams) {
            var voterFromRepo = await _repo.GetEngeneres (engenereParams);

            var voterList = _mapper.Map<ICollection<EngenereForReturnDto>> (voterFromRepo);
            Response.AddPagination (voterFromRepo.CurrentPage, voterFromRepo.PageSize, voterFromRepo.TotalCount, voterFromRepo.TotalPages);
            return Ok (voterList);
        }

        [HttpPost ("upload"), DisableRequestSizeLimit]
        public ActionResult UploadFile () {
            try {
                var file = Request.Form.Files[0];
                var list = new List<Engeneres> ();
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
                            Engeneres dataItem = new Engeneres ();
                            int code;

                            DateTime birthDate;
                            DateTime registration;
                            DateTime graduation;

                            Int32.TryParse (row["Code"].ToString (), out code);
                            DateTime.TryParse (row["BirthDate"].ToString (), out birthDate);
                            DateTime.TryParse (row["Registration"].ToString (), out registration);
                            DateTime.TryParse (row["Graduation"].ToString (), out graduation);
                            dataItem.Code = code;
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

                            bool exists = false;
                            list.ForEach (item => {
                                if (item.Code == dataItem.Code) {
                                    exists = true;
                                }
                            });
                            if (_repo.GetEngenere (dataItem.Code).Result == null && exists == false) {
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

        [HttpGet]
        [Route ("Export")]
        public async Task<FileStream> Export () {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format ("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo (Path.Combine (Directory.GetCurrentDirectory (), sFileName));
            if (file.Exists) {
                file.Delete ();
                file = new FileInfo (Path.Combine (Directory.GetCurrentDirectory (), sFileName));
            }
            using (ExcelPackage package = new ExcelPackage (file)) {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add ("Engeneres");
                //First add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Salary (in $)";

                //Add values
                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "Jon";
                worksheet.Cells["C2"].Value = "M";
                worksheet.Cells["D2"].Value = 5000;

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "Graham";
                worksheet.Cells["C3"].Value = "M";
                worksheet.Cells["D3"].Value = 10000;

                worksheet.Cells["A4"].Value = 1002;
                worksheet.Cells["B4"].Value = "Jenny";
                worksheet.Cells["C4"].Value = "F";
                worksheet.Cells["D4"].Value = 5000;

                package.Save (); //Save the workbook.
            }
            var fil = Path.Combine (Directory.GetCurrentDirectory (), sFileName);
            return new FileStream (fil, FileMode.Open, FileAccess.Read);
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