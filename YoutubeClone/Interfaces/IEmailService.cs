using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
    }
}