using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwtoken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        string singinKey = "BuBenimUzunOlmasiGerekenSigningKeyDegerim";

        [HttpGet]
        public string Get(string userName, string password)
        {
            var claims = new[]{
                new Claim(ClaimTypes.Name,userName),
                new Claim(JwtRegisteredClaimNames.Email,userName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(singinKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://ww.emreavarisli.com",
                audience: "BuBenimKullandigimAudienceDegeri",
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                notBefore: DateTime.Now,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        [HttpGet("ValidateToken")]
        public bool Validatetoken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(singinKey));
            try
            {
                JwtSecurityTokenHandler handler = new();
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
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