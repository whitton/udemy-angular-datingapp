using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private SecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.UserName));

            var descriptor = new SecurityTokenDescriptor();
            descriptor.Subject = new ClaimsIdentity(claims);
            descriptor.Expires = DateTime.Now.AddDays(14);
            descriptor.SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);           

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token); // Serializes a JWT object into a string
        }
    }
}