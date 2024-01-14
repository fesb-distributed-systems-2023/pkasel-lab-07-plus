// Requires: Microsoft.Data.SqlClient package from NuGet (Tools > NuGet...)

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using EasyMailCoreApplication.Configuration;
using EasyMailCoreApplication.Models;

namespace EasyMailCoreApplication.Repositories
{
    public class EmailRepository_MSSQL : IEmailRepository
    {
        private readonly string _connectionString;
        private readonly string _dbDatetimeFormat = "yyyy-MM-dd hh:mm:ss.fff";
        public EmailRepository_MSSQL(IOptions<DBConfiguration> configuration)
        {
            _connectionString = configuration.Value.ConnectionString;
        }

        public void AddEmail(Email email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[dbo].[CreateEmail]", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            /*
             * [dbo].[CreateEmail] Stored Procedure parameters
             * 
             * 	@Subject VARCHAR(MAX)
	         *  @Body VARCHAR(MAX)
	         *  @Timestamp_ VARCHAR(MAX)
	         *  @Sender VARCHAR(MAX)
	         *  @ReceiverList VARCHAR(MAX) -- Receiver emails separated by ';'
            */

            string receiverList = string.Join(';', email.Receivers ?? Array.Empty<string>());

            command.Parameters.AddWithValue("@Subject",         email.Subject);
            command.Parameters.AddWithValue("@Body",            email.Body);
            command.Parameters.AddWithValue("@Timestamp_",      email.Timestamp.ToString(_dbDatetimeFormat));
            command.Parameters.AddWithValue("@Sender",          email.Sender);
            command.Parameters.AddWithValue("@ReceiverList",    receiverList);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return;
        }

        public void DeleteEmail(int id)
        {
            throw new NotImplementedException();
        }

        public List<Email> GetAllEmails()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[dbo].[GetAllEmails]", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();
            using var reader = command.ExecuteReader();

            var lstEmails = new List<Email>();

            while (reader.Read())
            {
                var emailID = reader.GetInt32(0);

                Email? email = lstEmails.Where(x => x.ID == emailID).FirstOrDefault();
                if (email is null)
                { // Email does not exist, read all fields
                    email = new Email
                    {
                        ID = emailID,
                        Sender = reader.GetString(1),
                        Subject = reader.GetString(2),
                        Body = reader.GetString(3),
                        Timestamp = DateTime.ParseExact(reader.GetString(4), _dbDatetimeFormat, null),
                        Receivers = new List<string> { reader.GetString(5) }
                    };
                    lstEmails.Add(email);
                }
                else
                { // Email alread exists, read only receiver
                    email.Receivers = email.Receivers!.Concat( new [] { reader.GetString(5) });
                }
            }

            connection.Close();
            return lstEmails;
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
