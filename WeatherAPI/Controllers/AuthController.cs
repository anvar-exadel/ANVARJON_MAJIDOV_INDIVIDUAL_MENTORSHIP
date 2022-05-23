using BusinessLogic.interfaces;
using DatabaseAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.apiResponse.serviceResponse;
using Shared.dtos.auth;
using Shared.models;
using System.Linq;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //register
        [HttpPost("register")]
        public ActionResult<ServiceResponse<UserDto>> Register(RegisterDto registerDto)
        {
            ServiceResponse<UserDto> response = _authService.Register(registerDto);
            if(!response.Success) return BadRequest(response);

            return Ok(response);
        }

        //login
        [HttpPost("login")]
        public ActionResult<ServiceResponse<UserDto>> Login(LoginDto loginDto)
        {
            ServiceResponse<UserDto> response = _authService.Login(loginDto);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}
