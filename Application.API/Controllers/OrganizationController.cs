using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        public OrganizationController (IGeneralRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet]
        public async Task<IActionResult> GetOrganizations () {
            var organizationsToReturn = await _repo.GetOrganizations ();
            return Ok (organizationsToReturn);
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetOrganization (int id) {
            var organizationToReturn = await _repo.GetOrganization (id);
            return Ok (organizationToReturn);
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddOrganization (OrganizationForCreationDto organizationForCreationDto) {

            var organizationToCreate = _mapper.Map<Organization> (organizationForCreationDto);

            _repo.Add (organizationToCreate);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add Organzation");
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPut("UpdateType")]
        public async Task<IActionResult> SetOrganizationType (OrganizationTypeUpdateDto organizationTypeUpdateDto) {
            var OrganizationFromRepo = await _repo.GetOrganization (organizationTypeUpdateDto.Id);
            _mapper.Map (organizationTypeUpdateDto, OrganizationFromRepo);
            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add Organzation");
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost ("type")]
        public async Task<IActionResult> AddOrganizationType (OrganizationTypeForCreationDto organizationTypeForCreationDto) {

            var organizationTypeToCreate = _mapper.Map<VoterType> (organizationTypeForCreationDto);

            _repo.Add (organizationTypeToCreate);

            if (await _repo.SaveAll ()) {
                return Ok ();
            }
            return BadRequest ("Could not add Organzation type");
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet ("type")]
        public async Task<IActionResult> GetOrganizationTypes () {

            var organizationTypes = await _repo.GetOrganizationTypes ();
            return Ok (organizationTypes);
        }
    }
}