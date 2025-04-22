using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AcademicChatBot.Service.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Guid? GetStudentIdFromToken(HttpRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                errorMessage = "No token provided";
                return null;
            }

            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var studentIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "StudentId");

                if (studentIdClaim == null)
                {
                    errorMessage = "StudentId claim not found";
                    return null;
                }

                return Guid.Parse(studentIdClaim.Value);
            }
            catch (Exception ex)
            {
                errorMessage = $"Error parsing token: {ex.Message}";
                return null;
            }
        }
        public string GetRoleFromToken(HttpRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                errorMessage = "No token provided";
                return null;
            }

            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (roleClaim == null)
                {
                    errorMessage = "Role claim not found";
                    return null;
                }

                return roleClaim.Value;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error parsing token: {ex.Message}";
                return null;
            }
        }

        public string GenerateAccessToken(Guid userId, RoleName role, string email, Guid? studentId = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                         new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new("AccountId", userId.ToString()),
                          new Claim(ClaimTypes.Role,role.ToString()),
                          new Claim(ClaimTypes.Email, email),
                     };
            if (studentId.HasValue)
            {
                claims.Add(new Claim("StudentId", studentId.Value.ToString()));
            }
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

        public Guid? GetUserIdFromToken(HttpRequest request, out string errorMessage)
        {
            errorMessage = string.Empty;
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                errorMessage = "No token provided";
                return null;
            }

            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var accountIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "AccountId");

                if (accountIdClaim == null)
                {
                    errorMessage = "AccountId claim not found";
                    return null;
                }

                return Guid.Parse(accountIdClaim.Value);
            }
            catch (Exception ex)
            {
                errorMessage = $"Error parsing token: {ex.Message}";
                return null;
            }
        }
    }
}
