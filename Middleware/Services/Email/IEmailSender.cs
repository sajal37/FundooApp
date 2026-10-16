using ModelLayer.Model;

namespace Middleware.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MessageModel message);
    }
}
