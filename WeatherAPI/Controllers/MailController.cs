using BusinessLogic.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.dtos.mailDTOs;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public IActionResult Subscribe(SubsribeUserDto subsribe)
        {
            var response = _mailService.Subscribe(subsribe);
            if(!response.Success) return BadRequest(response);

            return Ok(response);
        }
        
        [HttpDelete("{userId}")]
        public IActionResult Unsubscribe(int userId)
        {
            var response = _mailService.Unsubscribe(userId);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}
