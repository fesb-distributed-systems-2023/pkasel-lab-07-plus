// For the `using` line below to work, go to :
// Tools > NuGet Package Manager > Manage NuGet Packages for Solution... > Browse > "Microsoft.Data.Sqlite" > Install 
using Microsoft.Data.Sqlite;
using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
{
    public class EmailRepository_SQL : IEmailRepository
    {
        private readonly string _connectionString = "Data Source=A:\\WebAPIDB\\Database.db.1";
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

            var transaction = connection.BeginTransaction();

            // Translate sender email address to User ID
            int senderId = -1;
            try
            {
                senderId = GetIdFromEmailAddress(email.Sender);
            }
            catch
            {
                transaction.Rollback();
            }

            // Insert an email to the table
            var commandInsertNewMail = connection.CreateCommand();
            commandInsertNewMail.CommandText =
            @"
                INSERT INTO Emails (Subject, Body, SenderID, Timestamp)
                VALUES ($subject, $body, $senderId, $timestamp);

                SELECT last_insert_rowid();";

            commandInsertNewMail.Parameters.AddWithValue("$subject", email.Subject);
            commandInsertNewMail.Parameters.AddWithValue("$body", email.Body);
            commandInsertNewMail.Parameters.AddWithValue("$senderId", senderId);
            commandInsertNewMail.Parameters.AddWithValue("$timestamp", email.Timestamp.ToString(_dbDatetimeFormat));

            object? last_insert_rowid = commandInsertNewMail.ExecuteScalar();
            if(last_insert_rowid is null)
            {
                // Undo changes
                transaction.Rollback();
                throw new ArgumentException("Could not insert email into database.");
            }

            int emailId = (int)last_insert_rowid;

            if (email.Receivers is null || email.Receivers.Count() == 0)
            {
                transaction.Rollback();
                throw new ArgumentException("Mail has no receivers.");
            }

            // Add receivers one by one
            var commandConnectMailToRecevier = connection.CreateCommand();
            commandConnectMailToRecevier.CommandText = "INSERT INTO MailReceiverMap(MailID, ReceiverID) VALUES ($emailId, $receiverId)";
            commandConnectMailToRecevier.Parameters.AddWithValue("$emailId", emailId);
            try
            {
                foreach (var receiverEmail in email.Receivers)
                {
                    senderId = GetIdFromEmailAddress(receiverEmail);
                    commandConnectMailToRecevier.Parameters.AddWithValue("$receiverId", receiverEmail);
                    _ = commandConnectMailToRecevier.ExecuteNonQuery();
                }
            }
            catch
            {
                transaction.Rollback();
            }
        }

        public void DeleteEmail(int id)
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText =
            //@"
            //    DELETE FROM Emails
            //    WHERE ID == $id";

            //command.Parameters.AddWithValue("$id", id);

            //int rowsAffected = command.ExecuteNonQuery();

            //if (rowsAffected < 1)
            //{
            //    throw new ArgumentException($"No emails with ID = {id}.");
            //}
        }

        public List<Email> GetAllEmails()
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText =
            //@"SELECT ID, Subject, Body, Sender, Receiver, Timestamp FROM Emails";

            //using var reader = command.ExecuteReader();

            //var results = new List<Email>();
            //while(reader.Read())
            //{

            //    var row = new Email
            //    {
            //        ID = reader.GetInt32(0),
            //        Subject = reader.GetString(1),
            //        Body = reader.GetString(2),
            //        Sender = reader.GetString(3),
            //        Receiver = reader.GetString(4),
            //        Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
            //    };

            //    results.Add(row);
            //}

            //return results;
            return new List<Email> { };
        }

        public Email? GetEmailById(int id)
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText =
            //@"SELECT ID, Subject, Body, Sender, Receiver, Timestamp FROM Emails WHERE ID == $id";

            //command.Parameters.AddWithValue("$id", id);

            //using var reader = command.ExecuteReader();

            //Email result = null;

            //if (reader.Read())
            //{
            //    result = new Email
            //    {
            //        ID = reader.GetInt32(0),
            //        Subject = reader.GetString(1),
            //        Body = reader.GetString(2),
            //        Sender = reader.GetString(3),
            //        Receiver = reader.GetString(4),
            //        Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
            //    };
            //}

            //return result;
            return null;
        }

        public Email? GetEmailBySubject(string subject)
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText =
            //@"SELECT ID, Subject, Body, Sender, Receiver, Timestamo FROM Emails WHERE Subject == $subject";

            //command.Parameters.AddWithValue("$subject", subject);

            //using var reader = command.ExecuteReader();

            //Email result = null;

            //if (reader.Read())
            //{
            //    result = new Email
            //    {
            //        ID = reader.GetInt32(0),
            //        Subject = reader.GetString(1),
            //        Body = reader.GetString(2),
            //        Sender = reader.GetString(3),
            //        Receiver = reader.GetString(4),
            //        Timestamp = DateTime.ParseExact(reader.GetString(5), _dbDatetimeFormat, null)
            //    };
            //}

            //return result;
            return null;
        }

        public void UpdateEmail(int id, Email updatedEmail)
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText =
            //@"
            //    UPDATE Emails
            //    SET
            //        Subject = $subject,
            //        Body = $body,
            //        Sender = $sender,
            //        Receiver = $receiver,
            //        Timestamp = $timestamp
            //    WHERE
            //        ID == $id";

            //command.Parameters.AddWithValue("$id", id);
            //command.Parameters.AddWithValue("$subject", updatedEmail.Subject);
            //command.Parameters.AddWithValue("$body", updatedEmail.Body);
            //command.Parameters.AddWithValue("$sender", updatedEmail.Sender);
            //command.Parameters.AddWithValue("$receiver", updatedEmail.Receiver);
            //command.Parameters.AddWithValue("$timestamp", updatedEmail.Timestamp.ToString(_dbDatetimeFormat));

            //int rowsAffected = command.ExecuteNonQuery();

            //if (rowsAffected < 1)
            //{
            //    throw new ArgumentException($"Could not update email with ID = {id}.");
            //}
        }
    }
}
