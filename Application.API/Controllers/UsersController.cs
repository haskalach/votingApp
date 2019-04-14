using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Helpers;
using Application.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers {
    [ServiceFilter (typeof (LogUserActivity))]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IGeneralRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UsersController (IGeneralRepository repo, IMapper mapper, UserManager<User> userManager) {
            _userManager = userManager;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet ("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser (int id) {
            var user = await _repo.GetUser (id);
            var userToReturn = _mapper.Map<UserForDetailedDto> (user);
            return Ok (userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser (UserForUpdateDto userForUpdateDto) {
            // if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value)) {
            //     return Unauthorized ();
            // }
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            _mapper.Map (userForUpdateDto, userFromRepo);
            if (await _repo.SaveAll ()) {
                return NoContent ();
            }
            throw new Exception ($"Updating user {userId} failed on save");
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost ("assignOrganization")]
        public async Task<IActionResult> AssignOrganization (AssignOrganizationDto assignOrganizationDto) {
            var user = await _userManager.FindByEmailAsync (assignOrganizationDto.UserEmail);
            var organization = await _repo.GetOrganization (assignOrganizationDto.OrganizationId);
            if (organization == null) {
                user.OrganizationId = null;
            } else {
                user.OrganizationId = assignOrganizationDto.OrganizationId;
            }

            if (await _repo.SaveAll ()) {
                return NoContent ();
            } else {
                return BadRequest ("could not set organization for user" + user.UserName);
            }
        }

    }
}