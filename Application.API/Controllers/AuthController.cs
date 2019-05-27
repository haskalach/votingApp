using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.API.Data;
using Application.API.Dtos;
using Application.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.API.Controllers {
    [AllowAnonymous]
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGeneralRepository _repo;
        public AuthController (IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IGeneralRepository repo) {
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto) {

            var userToCreate = _mapper.Map<User> (userForRegisterDto);

            var result = await _userManager.CreateAsync (userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto> (userToCreate);

            if (result.Succeeded) {
                return CreatedAtRoute ("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }
            return BadRequest (result.Errors);
        }

        [HttpPost ("createReference")]
        public async Task<IActionResult> CreateReferenceUser (UserForReferenceRegisterDto userForReferenceRegisterDto) {
            var userId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (userId);
            var OrganizationId = userFromRepo.OrganizationId;
            var userToCreate = _mapper.Map<User> (userForReferenceRegisterDto);
            var result = await _userManager.CreateAsync (userToCreate, "password");
            List<string> selectedRoles = new List<string> ();
            selectedRoles.Add ("Reference");
            var Rolesresult = await _userManager.AddToRolesAsync (userToCreate, selectedRoles);
            var userToReturn = _mapper.Map<UserForDetailedDto> (userToCreate);

            userToCreate.OrganizationId = OrganizationId;

            if (result.Succeeded && Rolesresult.Succeeded&& await _repo.SaveAll ()) {
                return CreatedAtRoute ("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }
            return BadRequest (result.Errors);
        }

        [HttpPost ("login")]
        public async Task<IActionResult> login (UserForLoginDto userForLoginDto) {

            var user = await _userManager.FindByEmailAsync (userForLoginDto.Email);
            if (user == null) {
                return BadRequest ("wrong user email");
            }
            var result = await _signInManager.CheckPasswordSignInAsync (user, userForLoginDto.Password, false);
            if (result.Succeeded) {
                var appUser = await _userManager.Users.Include (p => p.Photos).FirstOrDefaultAsync (u => u.NormalizedEmail == userForLoginDto.Email.ToUpper ());
                var roles = await _userManager.GetRolesAsync (appUser);
                if ((roles.IndexOf ("Admin") == -1) && (user.OrganizationId == null)) {
                    return BadRequest ("Could Not sign in Untill User is linked to organization");
                }

                var userToreturn = _mapper.Map<UserForListDto> (appUser);
                return Ok (new {
                    token = GenerateJwtToken (appUser).Result,
                        user = userToreturn
                });
            }

            return Unauthorized ();

        }

        private async Task<string> GenerateJwtToken (User user) {

            var claims = new List<Claim> {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Email, user.Email)
            };
            if (user.OrganizationId.HasValue && user.OrganizationId > 0) {
                var organization = await _repo.GetOrganization (user.OrganizationId ?? default (int));
                claims.Add (new System.Security.Claims.Claim ("organizationType", organization.VoterType.Name));
            }

            var roles = await _userManager.GetRolesAsync (user);

            foreach (var role in roles) {
                claims.Add (new Claim (ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config.GetSection ("AppSettings:Token").Value));

            var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (claims),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler ();

            var token = tokenHandler.CreateToken (tokenDescriptor);

            return tokenHandler.WriteToken (token);
        }

    }
}