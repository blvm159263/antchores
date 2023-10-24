using AuthService.Repositories.Models;
using AuthService.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthService.API.Controllers
{
    [Route("api/auths")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenticationController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [HttpPost]
        public ActionResult Authenticate([FromBody] AuthenticateRequestModel authenticateRequestModel)
        {
            var account = _authenService.Authenticate(authenticateRequestModel.PhoneNumber, authenticateRequestModel.Password);

            Console.WriteLine(account);

            if (account == null)
            {
                return BadRequest(new { message = "Phone number or password is incorrect" });
            }

            return Ok(account);
        }
    }
}
