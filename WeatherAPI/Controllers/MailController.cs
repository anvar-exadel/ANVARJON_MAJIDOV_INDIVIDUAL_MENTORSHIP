using BusinessLogic.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shared.dtos.mailDTOs;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailSubService _mailService;
        private readonly IConfiguration _configuration;

        private readonly int requestTimeout;
        public MailController(IMailSubService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
            _configuration = configuration;

            requestTimeout = _configuration.GetValue<int>("WeatherAppSettings:timeoutInMilliseconds");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Subscribe(SubsribeUserDto subsribe)
        {
            var response = _mailService.Subscribe(subsribe, requestTimeout);
            if(!response.Success) return BadRequest(response);

            return Ok(response);
        }
        
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Unsubscribe(int userId)
        {
            var response = _mailService.Unsubscribe(userId);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetReport(int userId)
        {
            var response = _mailService.GetReport(userId, requestTimeout);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}
