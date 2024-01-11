// For the `using` line below to work, go to :
// Tools > NuGet Package Manager > Manage NuGet Packages for Solution... > Browse > "Microsoft.Data.Sqlite" > Install 
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using pkaselj_lab_07_.Models;
using System.Linq;

namespace pkaselj_lab_07_.Repositories
{
    public class EmailRepository_SQL : IEmailRepository
    {
        private readonly string _connectionString = "Data Source=A:\\WebAPIDB\\Database.db.1";
        private readonly string _dbDatetimeFormat = "yyyy-MM-dd hh:mm:ss.fff";

        private int GetIdFromEmailAddress(SqliteConnection connection, string? emailAddress)
        {
            if(emailAddress is null)
            {
                throw new ArgumentNullException($"Email address is null");
            }

            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID FROM Users WHERE EmailAddress == $email";
            command.Parameters.AddWithValue("$email", emailAddress);
            object? id = command.ExecuteScalar();

            // User with email exists. Return his/her ID
            if (id is not null)
            {
                return (Int32)(Int64)id;
            }

            // User doesn't exist, create a new one and return ID
            var command_2 = connection.CreateCommand();
            command_2.CommandText = "INSERT INTO Users(EmailAddress) VALUES ($email); SELECT last_insert_rowid();";
            command_2.Parameters.AddWithValue("$email", emailAddress);
            id = command_2.ExecuteScalar();

            if (id is null)
            {
                // Insert failed...
                throw new KeyNotFoundException($"Could not insert {emailAddress}.");
            }

            return (Int32)(Int64)id;
        }

        public void AddEmail(Email email)
        {
            // First, connect to a database
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Start a transaction. Everything done to the database from now on,
            // will not be applied until transaction.Commit() is called.
            // If we encounter an error, transaction.Rollback() will reset all
            // changes applied after this line.
            var transaction = connection.BeginTransaction();

            // Get User ID from sender email
            int senderId = -1;
            try
            {
                senderId = GetIdFromEmailAddress(connection, email.Sender);
            }
            catch (Exception ex)
            {
                // In case of error, write message to the Console,
                // rollback all changes and exit this function
                Console.WriteLine($"Error: {ex.Message}");
                transaction.Rollback();
                return;
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

            // Take the ID of the last inserted row
            object? last_insert_rowid = commandInsertNewMail.ExecuteScalar();
            if(last_insert_rowid is null)
            {
                // There was an error, no row was inserted.
                // In that case, rollback/undo all changes and throw an error.
                transaction.Rollback();
                throw new ArgumentException("Could not insert email into database.");
            }

            Int64 emailId = (Int64)last_insert_rowid;

            // Cannot send an email to nobody
            if (email.Receivers is null || email.Receivers.Count() == 0)
            {
                transaction.Rollback();
                throw new ArgumentException("Mail has no receivers.");
            }

            // Add receivers one by one
            var commandConnectMailToRecevier = connection.CreateCommand();
            commandConnectMailToRecevier.CommandText = "INSERT INTO MailReceiverMap(MailID, ReceiverID) VALUES ($emailId, $receiverId)";
            commandConnectMailToRecevier.Parameters.AddWithValue("$emailId", emailId);
            commandConnectMailToRecevier.Parameters.AddWithValue("$receiverId", null);
            try
            {
                foreach (var receiverEmail in email.Receivers)
                {
                    int receiverId = GetIdFromEmailAddress(connection, receiverEmail);
                    commandConnectMailToRecevier.Parameters["$receiverId"].Value = receiverId;
                    _ = commandConnectMailToRecevier.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                transaction.Rollback();
            }

            // Apply all changes to the database
            transaction.Commit();
            connection.Close();
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
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var commandDistinctEmails = connection.CreateCommand();
            commandDistinctEmails.CommandText = @"
                SELECT Emails.ID, EmailAddress AS Sender, Subject, Body, Timestamp FROM Emails
                INNER JOIN Users ON Emails.SenderID == Users.ID";

            using var reader = commandDistinctEmails.ExecuteReader();

            // Get a list of all email. Read all fields except receiver list.
            var lstEmails = new List<Email>();
            while (reader.Read())
            {
                var email = new Email
                {
                    ID = reader.GetInt32(0),
                    Sender = reader.GetString(1),
                    Subject = reader.GetString(2),
                    Body = reader.GetString(3),
                    Timestamp = DateTime.Parse(reader.GetString(4), null),
                    Receivers = null
                };

                lstEmails.Add(email);
            }

            var commandListReceivers = connection.CreateCommand();
            commandListReceivers.CommandText = @"
                SELECT EmailAddress FROM Users
                INNER JOIN MailReceiverMap ON Users.ID == MailReceiverMap.ReceiverID
                WHERE MailReceiverMap.MailID == $emailId;";
            commandListReceivers.Parameters.AddWithValue("$emailId", null);

            foreach (var email in lstEmails)
            {
                commandListReceivers.Parameters["$emailId"].Value = email.ID;
                using var receiverReader = commandListReceivers.ExecuteReader();
                var receivers = new List<string>(); 
                while(receiverReader.Read())
                {
                    receivers.Add(receiverReader.GetString(0));
                }
                email.Receivers = receivers;
                reader.Close();
            }

            connection.Close();

            return lstEmails;
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
