using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.dtos.mailDTOs;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Authorize("Admin")]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        [HttpPost]
        public IActionResult Subscribe(SubsribeUserDto subsribe)
        {
            return null;
        }
        
        [HttpDelete("{userId}")]
        public IActionResult Unsubscribe(int userId)
        {
            return null;
        }
    }
}
