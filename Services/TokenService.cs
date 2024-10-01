using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Own_Service.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Own_Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SecurityKey signingKey;

        public TokenService(IConfiguration config,UserManager<ApplicationUser> userManager)
        {
            this.config = config;
            this.userManager = userManager;
            signingKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:key"]));
        }

        public async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            //claims token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //get roles
            var roles = await userManager.GetRolesAsync(user);
            foreach (var itemRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, itemRole));
            }
            SigningCredentials singnCred = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JWT:validProvider"], //url web api
                audience: config["JWT:ValidConsumer"], //url consumer 
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: singnCred
                );
            return token;
        }
    }
}
