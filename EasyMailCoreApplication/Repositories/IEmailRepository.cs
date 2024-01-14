using EasyMailCoreApplication.Models;

namespace EasyMailCoreApplication.Repositories
{
    public interface IEmailRepository
    {
        void AddEmail(Email email);
        void DeleteEmail(int id);
        List<Email> GetAllEmails();
        Email? GetEmailById(int id);
        Email? GetEmailBySubject(string subject);
        void UpdateEmail(int id, Email updatedEmail);
    }
}