using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data;
using TicketingSystem.DTOs;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private AppDbContext dbContext;
        private TokenService tokenService;
        public UsersController(AppDbContext dbContext, TokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var emailExist = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (emailExist != null)
            {
                return BadRequest($"sorry the email {registerDto.Email} alreay registered");
            }
            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Role=Enums.Role.User
            };
            user.HashPassword = passwordHasher.HashPassword(user, registerDto.Password);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u=>u.Email==loginDto.Email);
            if (user == null)
            {
                return Unauthorized($"the user with email {loginDto.Email} not found");
            }
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.HashPassword, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("invalid email or password");
            }

            return Ok();
        }

    }
}