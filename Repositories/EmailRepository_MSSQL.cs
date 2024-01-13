// Requires: Microsoft.Data.SqlClient package from NuGet (Tools > NuGet...)

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using pkaselj_lab_07_.Configuration;
using pkaselj_lab_07_.Models;

namespace pkaselj_lab_07_.Repositories
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
