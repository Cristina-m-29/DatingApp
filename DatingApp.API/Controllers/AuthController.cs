using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatingApp.API.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    { 
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IDatingRepository datingRepository, IMapper mapper){
            this._repo = repo;
            this._config = config;
            this._datingRepository = datingRepository;
            this._mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto){

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
 
            if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User{
                Username = userForRegisterDto.Username,
                Created = DateTime.Now
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto){

            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if(userFromRepo == null)
                return Unauthorized();
            
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new {
                token =  tokenHandler.WriteToken(token),
                user
            });            
        }

        [HttpPost("{userId}/logout")]
        public async Task<IActionResult> Logout(int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }
            else{
                var user = await _datingRepository.GetUser(userId);
                if (user.Username != ""){
                    user.LastActive = DateTime.Now;
                    if(await _datingRepository.SaveAll()) {
                        return NoContent();
                    }
                }
                return BadRequest ("No user found");
            }
            
        }
    }
}