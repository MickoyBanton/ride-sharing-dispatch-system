using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RideSharingDispatch.Application.DTOs;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RideSharingDispatch.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration) 
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _userService.LoginAsync(email, password);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            var token = GenerateJwtToken(result);

            return Ok(token);


        }

        private TokenResponse GenerateJwtToken(LoginResult loginResult)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, loginResult.UserId.ToString()),
            new Claim(ClaimTypes.Role, loginResult.Role.ToString()),
            };

            var expires = DateTime.UtcNow.AddMinutes(
                int.Parse(jwtSettings["ExpiresInMinutes"]!)
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }


    }
}
