using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;
using WebApiIdentity.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace WebApiIdentity.Tokens
{
    public class JwtGenarator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;

        public JwtGenarator(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            //_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdngvkidhfg7wefrkjbsflkjakefdhksaf"));
        }

        public string GenerateToken(List<Claim> claims)
        {

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
