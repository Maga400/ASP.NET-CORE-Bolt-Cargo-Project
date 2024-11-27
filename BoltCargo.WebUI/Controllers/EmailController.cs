using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly UserManager<CustomIdentityUser> _userManager;
        public EmailController(IEmailService emailService, ICustomIdentityUserService customIdentityUserService, UserManager<CustomIdentityUser> userManager)
        {
            _emailService = emailService;
            _customIdentityUserService = customIdentityUserService;
            _userManager = userManager;
        }

        [HttpPost("emailVerificationCode")]
        public IActionResult SendVerificationCode([FromQuery] string email)
        {
            try
            {
                _emailService.SendVerificationCode(email);
                return Ok(new { Message = "Verification code sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error sending verification code.", Error = ex.Message });
            }
        }

        [HttpPost("emailNotification")]
        public IActionResult SendNotificationMessage([FromQuery] string email,[FromQuery] string message)
        {
            try
            {
                _emailService.SendEmailNotification(email,message);
                return Ok(new { Message = "Email Notification sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error sending email notification code.", Error = ex.Message });
            }
        }

        [HttpPost("emailNotificationDrivers")]
        public async Task<IActionResult> SendNotificationDriversMessage([FromQuery] string message, [FromQuery] string carType)
        {
            try
            {
                var drivers = await _userManager.GetUsersInRoleAsync("Driver");
                var filteredDrivers = drivers.Where(d => d.CarType == carType).ToList();

                foreach (var driver in filteredDrivers) 
                {
                    _emailService.SendEmailNotification(driver.Email, message);
                }

                return Ok(new { Message = "Email Notification sent successfully for drivers." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error sending email notification code.", Error = ex.Message });
            }
        }

        [HttpPost("verificationCode")]
        public IActionResult CheckVerificationCode([FromBody] int code)
        {
            var result = _emailService.CheckVerificationCode(code);
            if (result.IsValid)
            {
                return Ok(new { Message = result.Message });
            }

            return BadRequest(new { Message = result.Message });
        }
    }
}
