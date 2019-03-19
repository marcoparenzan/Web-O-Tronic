using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebOTronic.GameApp.Models;

namespace WebOTronic.GameApp.Controllers
{
    [ApiController]
    public class SignalRController : ControllerBase 
    {
        private IConfiguration _configuration;

        public SignalRController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("client/negotiate")]
        [HttpPost]
        public SignalRConnectionInfo GetSignalRInfo()
        {
            var userId = Request.Headers["x-ms-client-principal-id"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: nameof(Hubs.GameHub),
                audience: nameof(Hubs.GameHub),
                claims: new[] {
                    new Claim(ClaimTypes.Name, userId)
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var info = new SignalRConnectionInfo
            {
                Url = userId,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return info;
        }
    }
}   