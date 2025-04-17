using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AcademicChatBot.Service.Implementation
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateAccessToken(Guid userId, string role, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                         new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new("AccountId", userId.ToString()),
                          new Claim(ClaimTypes.Role,role),
                          new Claim(ClaimTypes.Email, email),
                     };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"])),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            int byteLength = 30;
            var randomBytes = new byte[byteLength];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string base64Token = Convert.ToBase64String(randomBytes)
                                      .Replace("+", "-")
                                      .Replace("/", "_")
                                      .TrimEnd('=');
            return base64Token.Length > 40 ? base64Token.Substring(0, 40) : base64Token;
        }
    }
}
