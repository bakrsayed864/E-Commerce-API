using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Own_Service.DTO;
using Own_Service.Models;
using Own_Service.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly ICustomerRepository _customerRepository;

        public AccountController(UserManager<ApplicationUser> _userManager,IConfiguration config,CommerceDbContext commerceDbContext,ICustomerRepository accountRepository)
        {
            this.userManager = _userManager;
            this.config = config;
            this._customerRepository = accountRepository;
        }
        //Create new Account
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            if(ModelState.IsValid)
            {
                //save //use identity service layer (usermanager)
                //conver DTO to ApplicationUser
                ApplicationUser user = new ApplicationUser();
                user.UserName = registerUserDTO.UserName;
                user.Email = registerUserDTO.Email;
                IdentityResult result =await userManager.CreateAsync(user, registerUserDTO.Password);
                if (result.Succeeded)
                {
                    //
                    _customerRepository.Create(registerUserDTO,user.Id);
                    //
                    return Ok("Account Created successfully");
                }
                else
                    return BadRequest(ModelState);
            }
            return BadRequest(ModelState);
        }
        //
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await userManager.FindByNameAsync(logedUser.UserName);
            bool check = await userManager.CheckPasswordAsync(user, logedUser.Password);
            if (user == null || !check)
                return Unauthorized();
            //claims token
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            //get roles 
            var roles=await userManager.GetRolesAsync(user);
            foreach(var itemRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, itemRole));
            }
            SecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:key"]));
            SigningCredentials singnCred = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JWT:validProvider"], //url web api
                audience: config["JWT:ValidConsumer"], //url consumer 
                claims:claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: singnCred
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }
    }
}
