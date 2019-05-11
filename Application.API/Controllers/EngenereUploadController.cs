using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Models;
using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Application.API.Controllers {
    [Produces ("application/json")]
    [Route ("api/[controller]")]
    public class EngenereUploadController : Controller {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IGeneralRepository _repo;

        public EngenereUploadController (IHostingEnvironment hostingEnvironment, IMapper mapper, IGeneralRepository repo) {
            _mapper = mapper;
            _repo = repo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost, DisableRequestSizeLimit]
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
                            int vote;
                            int attend;
                            int transport;
                            int voted;
                            DateTime birthDate;
                            DateTime registration;
                            DateTime graduation;
                            Int32.TryParse (row["Code"].ToString (), out code);
                            Int32.TryParse (row["Vote"].ToString (), out vote);
                            Int32.TryParse (row["Attend"].ToString (), out attend);
                            Int32.TryParse (row["Transport"].ToString (), out transport);
                            Int32.TryParse (row["Voted"].ToString (), out voted);
                            DateTime.TryParse (row["BirthDate"].ToString (), out birthDate);
                            DateTime.TryParse (row["Registration"].ToString (), out registration);
                            DateTime.TryParse (row["Graduation"].ToString (), out graduation);
                            dataItem.Code = code;
                            dataItem.FirstName = row["FirstName"].ToString ();
                            dataItem.Speciality = row["Speciality"].ToString ();
                            dataItem.SubChapter = row["SubChapter"].ToString ();
                            dataItem.Religion = row["Religion"].ToString ();
                            dataItem.Politic = row["Politic"].ToString ();
                            dataItem.Reference = row["Reference"].ToString ();
                            dataItem.VotedYear = row["VotedYear"].ToString ();
                            dataItem.BirthDate = birthDate;
                            dataItem.BirthCountry = row["BirthCountry"].ToString ();
                            dataItem.BirthPlace = row["BirthPlace"].ToString ();
                            dataItem.CivilIdMother = row["CivilIdMother"].ToString ();
                            dataItem.CivilIdKad = row["CivilIdKad"].ToString ();
                            dataItem.CivilIdRegion = row["CivilIdRegion"].ToString ();
                            dataItem.RegisteryNumber = row["RegisteryNumber"].ToString ();
                            dataItem.CivilIdPlace = row["CivilIdPlace"].ToString ();
                            dataItem.Registration = registration;
                            dataItem.Graduation = graduation;
                            dataItem.School = row["School"].ToString ();
                            dataItem.GraduationLocation = row["GraduationLocation"].ToString ();
                            dataItem.AddressWork = row["AddressWork"].ToString ();
                            dataItem.MobileWork = row["MobileWork"].ToString ();
                            dataItem.PhoneWork = row["PhoneWork"].ToString ();
                            dataItem.AddressHome = row["AddressHome"].ToString ();
                            dataItem.MobileHome = row["MobileHome"].ToString ();
                            dataItem.PhoneHome = row["PhoneHome"].ToString ();
                            dataItem.AddressHome = row["AddressHome"].ToString ();
                            dataItem.Email = row["Email"].ToString ();
                            dataItem.Vote = vote;
                            dataItem.Attend = attend;
                            dataItem.Transport = transport;
                            dataItem.Voted = voted;

                            // list.Add (dataItem);
                            _repo.Add (dataItem);
                        }
                        System.IO.File.Delete (fullPath);
                    }
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
        [HttpGet]
        [Route ("data.csv")]
        [Produces ("text/csv")]
        public IActionResult GetDataAsCsv () {
            return Ok (DummyData ());
        }

        private static IEnumerable<Engeneres> DummyData () {
            var model = new List<Engeneres> {
                new Engeneres {
                Id = 1
                },
                new Engeneres {
                Id = 2
                }
            };

            return model;
        }

    }
}