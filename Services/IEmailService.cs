using System.Threading.Tasks;

namespace ToDoListWebApp.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string? message);
    }
}