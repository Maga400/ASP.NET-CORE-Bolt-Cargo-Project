using BoltCargo.WebUI.Services.Abstracts;
using System.Net;
using System.Net.Mail;

namespace BoltCargo.WebUI.Services.Concretes
{
    public class EmailService : IEmailService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public EmailService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public int SendVerificationCode(string recipientEmail)
        {
            try
            {
                int verificationCode = Random.Shared.Next(111111, 999999);

                var emailBody = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            background-color: #f4f4f9;
                            padding: 20px;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: auto;
                            background: #fff;
                            padding: 20px;
                            border: 1px solid #ddd;
                            border-radius: 5px;
                        }}
                        .header {{
                            text-align: center;
                            background-color: #007BFF;
                            color: #fff;
                            padding: 10px;
                            border-radius: 5px 5px 0 0;
                        }}
                        .content {{
                            margin-top: 20px;
                            text-align: center;
                        }}
                        .code {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #007BFF;
                        }}
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                            font-size: 12px;
                            color: #555;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <h1>Email Verification</h1>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>Your verification code is:</p>
                            <p class='code'>{verificationCode}</p>
                            <p>Please use this code to complete your verification.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
                        </div>
                    </div>
                </body>
            </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["SmtpSettings:Email"]),
                    Subject = "Your Verification Code",
                    Body = emailBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(recipientEmail);

                using var smtpClient = new SmtpClient(_configuration["SmtpSettings:Host"])
                {
                    Port = int.Parse(_configuration["SmtpSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["SmtpSettings:Email"],
                        _configuration["SmtpSettings:Password"]
                    ),
                    EnableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"])
                };

                smtpClient.Send(mailMessage);

                //_httpContextAccessor.HttpContext.Session.SetInt32("VerificationCode", verificationCode);
                return verificationCode;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public (bool IsValid, string Message) CheckVerificationCode(int inputCode)
        {
            int? storedCode = _httpContextAccessor.HttpContext.Session.GetInt32("VerificationCode");

            if (storedCode == null)
            {
                return (false, "Verification code has expired or does not exist.");
            }

            if (storedCode == inputCode)
            {
                return (true, "Verification code is valid.");
            }

            return (false, "Verification code is invalid.");
        }

        public void SendEmailNotification(string recipientEmail, string message)
        {
            try
            {
                var senderEmail = _configuration["SmtpSettings:Email"];
                var emailSubject = "Important Notification";

                var emailBody = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background-color: #f9f9f9;
                        margin: 0;
                        padding: 0;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: 50px auto;
                        background: #ffffff;
                        border: 1px solid #e0e0e0;
                        border-radius: 8px;
                        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        overflow: hidden;
                    }}
                    .email-header {{
                        background-color: #007BFF;
                        color: #ffffff;
                        text-align: center;
                        padding: 20px;
                    }}
                    .email-header img {{
                        max-width: 100px;
                        margin-bottom: 10px;
                    }}
                    .email-header h1 {{
                        margin: 0;
                        font-size: 24px;
                    }}
                    .email-body {{
                        padding: 20px;
                        color: #333333;
                    }}
                    .email-body p {{
                        margin: 15px 0;
                        font-size: 16px;
                        line-height: 1.6;
                    }}
                    .email-footer {{
                        text-align: center;
                        font-size: 14px;
                        color: #888888;
                        padding: 15px;
                        border-top: 1px solid #e0e0e0;
                        background-color: #f9f9f9;
                    }}
                    .email-footer a {{
                        color: #007BFF;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>
                        <img src='https://via.placeholder.com/100x50.png?text=Logo' alt='Company Logo' />
                        <h1>Your Notification</h1>
                    </div>
                    <div class='email-body'>
                        <p>Hello,</p>
                        <p>{message}</p>
                        <p>Best regards,<br/>Your Company</p>
                    </div>
                    <div class='email-footer'>
                        <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
                        <p>
                            Need help? <a href='mailto:support@yourcompany.com'>Contact us</a>
                        </p>
                    </div>
                </div>
            </body>
        </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = emailSubject,
                    Body = emailBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(recipientEmail);

                using var smtpClient = new SmtpClient(_configuration["SmtpSettings:Host"])
                {
                    Port = int.Parse(_configuration["SmtpSettings:Port"]),
                    Credentials = new NetworkCredential(senderEmail, _configuration["SmtpSettings:Password"]),
                    EnableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"])
                };

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
