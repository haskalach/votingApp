using System.Collections.Generic;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class VoterController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        public VoterController (IGeneralRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
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
        public async Task<IActionResult> GetVoters () {
            var voterFromRepo = await _repo.GetVoters();

            var voterList = _mapper.Map<ICollection<VoterForReturnDto>> (voterFromRepo);
            return Ok (voterList);
        }
    }
}