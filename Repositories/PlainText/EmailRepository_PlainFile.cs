using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using pkaselj_lab_07_.Controllers.DTOs;
using pkaselj_lab_07_.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pkaselj_lab_07_.Repositories.PlainText
{
    public class EmailRepository_PlainFile : IEmailRepository
    {
        private readonly string _fileName;
        public EmailRepository_PlainFile()
        {
            _fileName = "a:\\PRIBACIT\\SWEDES\\DB\\plaintext_db.txt";

            File.Create(_fileName).Dispose();
        }
        public void AddEmail(Email email)
        {
            ValidateEmail(email);
            email.ID = Guid.NewGuid().GetHashCode();
            email.Timestamp = DateTime.UtcNow;

            var data = LoadDatabase();
            data.Add(email);
            SaveDatabase(data);
        }

        public List<Email> GetAllEmails()
        {
            return LoadDatabase();
        }

        public Email? GetEmailById(int id)
        {
            return LoadDatabase().FirstOrDefault(e => e.ID == id);
        }

        public void UpdateEmail(int id, Email updatedEmail)
        {
            ValidateEmail(updatedEmail);

            // Perform access control here if needed

            var emails = LoadDatabase();
            Email? existingEmail = emails.FirstOrDefault(e => e.ID == id);

            if (existingEmail is not null)
            {
                // Update only if the user has permission
                // Implement access control logic as needed
                existingEmail.Subject = updatedEmail.Subject;
                existingEmail.Body = updatedEmail.Body;
                existingEmail.Sender = updatedEmail.Sender;
                existingEmail.Receiver = updatedEmail.Receiver;

                emails.Add(existingEmail);
                SaveDatabase(emails);
            }
            else
            {
                throw new KeyNotFoundException($"Email with ID '{id}' not found.");
            }
        }

        public void DeleteEmail(int id)
        {
            // Perform access control here if needed

            var emails = LoadDatabase();
            Email? emailToRemove = emails.FirstOrDefault(e => e.ID == id);

            if (emailToRemove != null)
            {
                emails.Remove(emailToRemove);
                SaveDatabase(emails);
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

        private List<Email> LoadDatabase()
        {
            List<Email> emails = new List<Email>();

            string? line;
            using (var reader = new StreamReader(_fileName))
            {
                line = reader.ReadLine();
                while (line is not null)
                {
                    emails.Add(ConvertToEntity(line));
                    line = reader.ReadLine();
                }
            }

            return emails;
        }

        private void SaveDatabase(List<Email> emails)
        {
            using (var writer = new StreamWriter(_fileName, false))
            {
                foreach (var item in emails)
                {
                    writer.WriteLine(ConvertToDto(item));
                }
            }
        }


        // Helper methods to convert between Email and line string
        private string ConvertToDto(Email email)
        {
            var segments = new string[]
            {
                email.ID.ToString(),
                email.Subject ?? string.Empty,
                email.Body ?? string.Empty,
                email.Sender ?? string.Empty,
                email.Receiver ?? string.Empty,
                email.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")
            };

            return string.Join("|", segments);
        }

        private Email ConvertToEntity(string line)
        {
            var segments = line.Split("|");

            return new Email
            {
                ID = int.Parse(segments[0]),
                Subject = segments[1],
                Body = segments[2],
                Sender = segments[3],
                Receiver = segments[4],
                Timestamp = DateTime.ParseExact(segments[5], "yyyy-MM-dd HH:mm:ss.fff", null)
            };
        }
    }
}
