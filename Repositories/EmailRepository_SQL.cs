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

        private int GetIdFromEmailAddress(string? emailAddress)
        {
            if(emailAddress is null)
            {
                throw new ArgumentNullException($"Email address is null");
            }

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID FROM Users WHERE EmailAddress == $email";

            command.Parameters.AddWithValue("$email", emailAddress);

            object? id = command.ExecuteScalar();

            if (id is null)
            {
                throw new KeyNotFoundException($"No user with email address {emailAddress}");
            }

            return (int)id;
        }

        public void AddEmail(Email email)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            int senderId = GetIdFromEmailAddress(email.Sender);

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Emails (Subject, Body, Sender, Timestamp)
                VALUES ($subject, $body, $senderId, $timestamp)";

            command.Parameters.AddWithValue("$subject", email.Subject);
            command.Parameters.AddWithValue("$body", email.Body);
            command.Parameters.AddWithValue("$senderId", senderId);
            command.Parameters.AddWithValue("$timestamp", email.Timestamp.ToString(_dbDatetimeFormat));

            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected < 1)
            {
                throw new ArgumentException("Could not insert email into database.");
            }
        }

        public void DeleteEmail(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Emails
                WHERE ID == $id";

            command.Parameters.AddWithValue("$id", id);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException($"No emails with ID = {id}.");
            }
        }

        public List<Email> GetAllEmails()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"SELECT ID, Subject, Body, Sender, Receiver, Timestamp FROM Emails";

            using var reader = command.ExecuteReader();

            var results = new List<Email>();
            while(reader.Read())
            {

                var row = new Email
                {
                    ID = reader.GetInt32(0),
                    Subject = reader.GetString(1),
                    Body = reader.GetString(2),
                    Sender = reader.GetString(3),
                    Receiver = reader.GetString(4),
                    Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
                };

                results.Add(row);
            }

            return results;
        }

        public Email? GetEmailById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"SELECT ID, Subject, Body, Sender, Receiver, Timestamp FROM Emails WHERE ID == $id";

            command.Parameters.AddWithValue("$id", id);

            using var reader = command.ExecuteReader();

            Email result = null;

            if (reader.Read())
            {
                result = new Email
                {
                    ID = reader.GetInt32(0),
                    Subject = reader.GetString(1),
                    Body = reader.GetString(2),
                    Sender = reader.GetString(3),
                    Receiver = reader.GetString(4),
                    Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
                };
            }

            return result;
        }

        public Email? GetEmailBySubject(string subject)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"SELECT ID, Subject, Body, Sender, Receiver, Timestamo FROM Emails WHERE Subject == $subject";

            command.Parameters.AddWithValue("$subject", subject);

            using var reader = command.ExecuteReader();

            Email result = null;

            if (reader.Read())
            {
                result = new Email
                {
                    ID = reader.GetInt32(0),
                    Subject = reader.GetString(1),
                    Body = reader.GetString(2),
                    Sender = reader.GetString(3),
                    Receiver = reader.GetString(4),
                    Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
                };
            }

            return result;
        }

        public void UpdateEmail(int id, Email updatedEmail)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Emails
                SET
                    Subject = $subject,
                    Body = $body,
                    Sender = $sender,
                    Receiver = $receiver,
                    Timestamp = $timestamp
                WHERE
                    ID == $id";

            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$subject", updatedEmail.Subject);
            command.Parameters.AddWithValue("$body", updatedEmail.Body);
            command.Parameters.AddWithValue("$sender", updatedEmail.Sender);
            command.Parameters.AddWithValue("$receiver", updatedEmail.Receiver);
            command.Parameters.AddWithValue("$timestamp", updatedEmail.Timestamp.ToString(_dbDatetimeFormat));

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException($"Could not update email with ID = {id}.");
            }
        }
    }
}
