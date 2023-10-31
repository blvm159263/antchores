using AuthService.Repositories.Models;
using AuthService.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthService.API.Controllers
{
    [Route("api/a/auths")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenticationController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [HttpPost("login")]
        public ActionResult Authenticate([FromBody] AuthenticateRequestModel authenticateRequestModel)
        {
            var account = 
                _authenService.Authenticate(authenticateRequestModel.PhoneNumber, authenticateRequestModel.Password);
            
            if (account == null)
            {
                return NotFound(new { message = "Phone number or password is incorrect" });
            }

            return Ok(account);
        }

        [HttpPost("register")]
        public ActionResult Register(AuthenticateRequestModel authenticateRequestModel)
        {
            var res = _authenService.Register(authenticateRequestModel);

            if (res == null)
            {
                return BadRequest(new { message = "Phone number is already taken" });
            }

            return Ok(res);
        }
    }
}
