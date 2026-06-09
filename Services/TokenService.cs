using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

using System.Text;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            this._config = config;
        }
        public string GenerateToken(User user)
        {
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["key"]));
            var creds = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
            new (ClaimTypes.Email,user.Email),
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new(ClaimTypes.Role,user.Role.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Issuer"],
                audience: _config["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
            );
          return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}