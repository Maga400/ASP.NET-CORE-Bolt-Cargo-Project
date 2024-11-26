namespace BoltCargo.WebUI.Services.Abstracts
{
    public interface IEmailService
    {
        void SendVerificationCode(string recipientEmail);
        void SendEmailNotification(string recipientEmail,string message);
        (bool IsValid, string Message) CheckVerificationCode(int inputCode);
    }
}
