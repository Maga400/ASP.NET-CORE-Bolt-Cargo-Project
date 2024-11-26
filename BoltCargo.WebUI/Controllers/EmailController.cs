using BoltCargo.WebUI.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("emailVerificationCode")]
        public IActionResult SendVerificationCode([FromBody] string email)
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
        public IActionResult SendNotificationMessage([FromBody] string email,string message)
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
