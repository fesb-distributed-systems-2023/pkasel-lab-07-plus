﻿using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
{
    public class EmailRepository_InMemory : IEmailRepository
    {
        private List<Email> emails;

        public EmailRepository_InMemory()
        {
            emails = new List<Email>();
        }

        public void AddEmail(Email email)
        {
            ValidateEmail(email);
            email.ID = Guid.NewGuid().GetHashCode();
            email.Timestamp = DateTime.UtcNow;
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
            ValidateEmail(updatedEmail);

            // Perform access control here if needed

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
            // Perform access control here if needed

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

        private void ValidateEmail(Email email)
        {
            if (string.IsNullOrEmpty(email.Subject) ||
                string.IsNullOrEmpty(email.Sender) ||
                string.IsNullOrEmpty(email.Receiver))
            {
                throw new ArgumentException("Email fields cannot be null or empty.");
            }

        }
    }
}
