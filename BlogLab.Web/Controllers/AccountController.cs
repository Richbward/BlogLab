using BlogLab.Models.Account;
using BlogLab.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenservice;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly SignInManager<ApplicationIdentity> _signInManager;

        public AccountController(
            ITokenService tokenservice,
            UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationIdentity> signInManager
            )
        {
            _tokenservice = tokenservice;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicatrionUserCreate applicatrionUserCreate)
        {
            var applicationUserIdentity = new ApplicationUserIdentity
            {
                Username = applicatrionUserCreate.Username,
                Email = applicatrionUserCreate.Email,
                Fullname = applicatrionUserCreate.Fullname
            };

            var result = await _userManager.CreateAsync(applicationUserIdentity,
                applicatrionUserCreate.Password);

            if (result.Succeeded)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Id = applicationUserIdentity.Id,
                    Username = applicationUserIdentity.Username,
                    Fullname = applicationUserIdentity.Fullname,
                    Email = applicationUserIdentity.Email,
                    Token = _tokenservice.CreateToken(applicationUserIdentity)
                };

                return user;
            }

            return BadRequest(result.Errors);
        }
    }
}
