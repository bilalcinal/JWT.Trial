using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        string signinKey = "BuBenimSigningKey";

        [HttpGet]
        public string Get(string userName, string Password)
        {
         try
         {
            var Claims = new[]{
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Email, userName)
            };
            
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://www.bilalcinal.com",
                audience:"BuBenimKullandığımTestDeğeri",
                claims: Claims,
                expires: DateTime.Now.AddDays(15),
                notBefore: DateTime.Now,
                signingCredentials: credentials
            );
 
           var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
           return token;
         }
         catch (System.Exception)
         {
            
            throw;
         }
            
        }

        
        [HttpGet]
        public bool ValidateToken(string token)
        {
            var securtiyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey));
            try
            {
                JwtSecurityTokenHandler handler = new();
                handler.ValidateToken(token, new TokenValidationParameters{
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = securtiyKey,
                  ValidateLifetime = true,
                  ValidateAudience = false,
                  ValidateIssuer = false,
                }, out SecurityToken validatedToken);
                
                var jwtToken =  (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToList();
                return true;
            }
            catch (System.Exception)
            {
                
                return false;
            }

        }
    }
}