using EasyMailCoreApplication.Models;

namespace EasyMailCoreApplication.Logic
{
    public interface IEmailLogic
    {
        void CreateNewEmail(Email? email);
        void DeleteEmail(int id);
        Email? GetEmail(int id);
        IEnumerable<Email> GetEmails();
        void UpdateEmail(int id, Email? email);
    }
}