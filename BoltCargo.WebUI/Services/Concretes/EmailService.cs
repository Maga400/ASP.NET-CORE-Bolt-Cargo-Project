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

        public void SendVerificationCode(string recipientEmail)
        {
            int verificationCode = Random.Shared.Next(111111, 999999);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpSettings:Email"]),
                Subject = "Your Verification Code",
                Body = $"Your verification code is: {verificationCode}"
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

            _httpContextAccessor.HttpContext.Session.SetInt32("VerificationCode", verificationCode);
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
                var emailSubject = "Your Email Notification";

                // HTML tasarımını oluştur
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
                            <h1>Your Notification</h1>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>{message}</p>
                            <p>Best regards,<br/>Your Company</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
                        </div>
                    </div>
                </body>
            </html>";

                // E-posta mesajını oluştur
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = emailSubject,
                    Body = emailBody,
                    IsBodyHtml = true // HTML formatını etkinleştir
                };
                mailMessage.To.Add(recipientEmail);

                // SMTP istemcisini yapılandır
                using var smtpClient = new SmtpClient(_configuration["SmtpSettings:Host"])
                {
                    Port = int.Parse(_configuration["SmtpSettings:Port"]),
                    Credentials = new NetworkCredential(senderEmail, _configuration["SmtpSettings:Password"]),
                    EnableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"])
                };

                // E-postayı gönder
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Hata durumunda log veya işlem yapılabilir
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

    }

}
