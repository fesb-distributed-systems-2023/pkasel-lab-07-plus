using Grpc.Core;
using gRPC_API;

using EasyMailCoreApplication.Logic;
using EasyMailCoreApplication.Models;

namespace gRPC_API.Services
{
    public class EmailAPI : EmailHandler.EmailHandlerBase
    {
        private readonly IEmailLogic _emailLogic;
        public EmailAPI(IEmailLogic emailLogic)
        {
            _emailLogic = emailLogic;
        }

        public override Task<GetEmailsResponse> GetEmails(GetEmailsRequest request, ServerCallContext context)
        {
            var result = new GetEmailsResponse();
            result.Emails.Clear();
            result.Emails.Add(_emailLogic.GetEmails().Select( x => ModelToOutputType(x) ));
            return Task.FromResult(result);
        }

        public override Task<CreateEmailResponse> CreateEmail(CreateEmailRequest request, ServerCallContext context)
        {
            _emailLogic.CreateNewEmail(InputTypeToModel(request.Email));
            return Task.FromResult(new CreateEmailResponse { });
        }

        private EmailOutputType ModelToOutputType(Email model)
        {
            var output_type = new EmailOutputType
            {
                Id = model.ID,
                Subject = model.Subject,
                Body = model.Body,
                Timestamp = model.Timestamp.ToString(),
                Sender = model.Sender
            };

            output_type.Receivers.Clear();
            output_type.Receivers.AddRange(model.Receivers);

            return output_type;
        }

        private Email InputTypeToModel(EmailInputType input_type)
        {
            return new Email
            {
                ID = -1,
                Subject = input_type.Subject,
                Body = input_type.Body,
                Sender = input_type.Sender,
                Timestamp = DateTime.MinValue,
                Receivers = input_type.Receivers.Clone()
            };
        }
    }
}