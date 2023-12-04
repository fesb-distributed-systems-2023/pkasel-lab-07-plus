using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
{
    public interface IEmailRepository
    {
        void AddEmail(Email email);
        void DeleteEmail(int id);
        List<Email> GetAllEmails();
        Email? GetEmailById(int id);
        void UpdateEmail(int id, Email updatedEmail);
    }
}