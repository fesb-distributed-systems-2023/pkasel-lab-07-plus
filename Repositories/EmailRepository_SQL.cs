// For the `using` line below to work, go to :
// Tools > NuGet Package Manager > Manage NuGet Packages for Solution... > Browse > "Microsoft.Data.Sqlite" > Install 
using Microsoft.Data.Sqlite;
using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
{
    public class EmailRepository_SQL : IEmailRepository
    {
        private readonly string _connectionString = "Data Source=A:\\WebAPIDB\\Database.db";
        private readonly string _dbDatetimeFormat = "yyyy-MM-dd hh:mm:ss.fff";

        public void AddEmail(Email email)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Emails (Subject, Body, Sender, Receiver, Timestamp)
                VALUES ($subject, $body, $sender, $receiver, $timestamp)";

            command.Parameters.AddWithValue("$subject", email.Subject);
            command.Parameters.AddWithValue("$body", email.Body);
            command.Parameters.AddWithValue("$sender", email.Sender);
            command.Parameters.AddWithValue("$receiver", email.Receiver);
            command.Parameters.AddWithValue("$timestamp", email.Timestamp.ToString(_dbDatetimeFormat));

            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected < 1)
            {
                throw new ArgumentException("Could not insert email into database.");
            }
        }

        public void DeleteEmail(int id)
        {
            throw new NotImplementedException();
        }

        public List<Email> GetAllEmails()
        {
            throw new NotImplementedException();
        }

        public Email? GetEmailById(int id)
        {
            throw new NotImplementedException();
        }

        public Email? GetEmailBySubject(string subject)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmail(int id, Email updatedEmail)
        {
            throw new NotImplementedException();
        }
    }
}
