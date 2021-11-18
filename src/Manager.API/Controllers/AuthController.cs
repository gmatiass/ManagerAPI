using Manager.API.Utilities;
using Manager.API.ViewModels;
using Manager.Core.Exceptions;
using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Manager.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public async Task<IActionResult> Login ([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                var authenticated = await _authService.CreateSession(loginViewModel.Login, loginViewModel.Password);

                return Ok(new ResultViewModel
                {
                    Message = "Login successfully.",
                    Success = true,
                    Data = authenticated
                });
            }        
            catch(DomainException)
            {
                return StatusCode(401, Responses.UnauthorizedErrorMessage());
            }
            catch (Exception)
            {

                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}
