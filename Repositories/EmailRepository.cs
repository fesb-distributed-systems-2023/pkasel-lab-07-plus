using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
{
    // Right click on class name (EmailRepository) > Quick Actions and Refactorings > Extract Interface
    public class EmailRepository : IEmailRepository
    {
        private List<Email> emails;

        public EmailRepository()
        {
            emails = new List<Email>();
        }

        public void AddEmail(Email email)
        {
            emails.Add(email);
        }

        public List<Email> GetAllEmails()
        {
            return emails;
        }

        public Email? GetEmailById(int id)
        {
            return emails.FirstOrDefault(e => e.ID == id);
        }

        public void UpdateEmail(int id, Email updatedEmail)
        {

            Email? existingEmail = GetEmailById(id);
            if (existingEmail is not null)
            {
                // Update only if the user has permission
                // Implement access control logic as needed
                existingEmail.Subject = updatedEmail.Subject;
                existingEmail.Body = updatedEmail.Body;
                existingEmail.Sender = updatedEmail.Sender;
                existingEmail.Receiver = updatedEmail.Receiver;
            }
            else
            {
                throw new KeyNotFoundException($"Email with ID '{id}' not found.");
            }
        }

        public void DeleteEmail(int id)
        {
            Email? emailToRemove = GetEmailById(id);
            if (emailToRemove != null)
            {
                emails.Remove(emailToRemove);
            }
            else
            {
                throw new KeyNotFoundException($"Email with ID '{id}' not found.");
            }
        }


        public Email? GetEmailBySubject(string subject)
        {
            return emails.FirstOrDefault(e => e.Subject == subject);
        }
    }
}
