using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public EngenereUploadController (IHostingEnvironment hostingEnvironment, IMapper mapper) {
            _mapper = mapper;
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
                            Engeneres dataItem = Activator.CreateInstance<Engeneres> ();
                            dataItem = row;
                            list.Add (dataItem);
                        }

                        return Json (list.ToArray ());
                    }
                }
                return Json ("Upload Successful.");
            } catch (System.Exception ex) {
                return Json ("Upload Failed: " + ex.Message);
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

    }
}