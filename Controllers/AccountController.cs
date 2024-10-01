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
        private readonly ITokenService tokenService;

        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration config, ICustomerRepository accountRepository, ITokenService tokenService)
        {
            this.userManager = _userManager;
            this.config = config;
            this._customerRepository = accountRepository;
            this.tokenService = tokenService;
        }
        //Create new Account
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            //save //use identity service layer (usermanager)
            //conver DTO to ApplicationUser
            ApplicationUser user = new ApplicationUser();
            user.UserName = registerUserDTO.UserName;
            user.Email = registerUserDTO.Email;
            IdentityResult result = await userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded)
            {
                //
                _customerRepository.Create(registerUserDTO, user.Id);
                //
                return Ok("Account Created successfully");
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
            //creat Token
            JwtSecurityToken token = await tokenService.CreateToken(user);
            return Ok(new
            {
                Owntoken = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            }) ;
        }
    }
}
