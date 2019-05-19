using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Models;
using AutoMapper;
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
        public async Task<IActionResult> AddVoter (VoterForCreationDto voterForCreationDto) {

            var voterToCreate = _mapper.Map<Engeneres> (voterForCreationDto);

            _repo.Add (voterToCreate);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add voter");
        }

        [HttpGet]
        public async Task<IActionResult> GetVoters () {
            var voterFromRepo = await _repo.GetVoters ();

            var voterList = _mapper.Map<ICollection<VoterForReturnDto>> (voterFromRepo);
            return Ok (voterList);
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
    }
}