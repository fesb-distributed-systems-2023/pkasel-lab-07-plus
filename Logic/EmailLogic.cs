using pkaselj_lab_07_.Exceptions;
using pkaselj_lab_07_.Models;
using pkaselj_lab_07_.Repositories;
using System.Reflection;
using System.Text.RegularExpressions;

namespace pkaselj_lab_07_.Logic
{
    public class EmailLogic : IEmailLogic
    {
        private readonly IEmailRepository _emailRepository;
        private const int _bodyMaxCharacters = 100;
        private const int _subjectMaxCharacters = 30;
        private const int _emailMaxCharacters = 30;
        private const string _emailRegex = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

        public EmailLogic(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        private void ValidateSenderField(string? sender)
        {
            if (sender is null)
            {
                throw new UserErrorMessage("Sender field cannot be empty.");
            }

            if (sender.Length > _emailMaxCharacters)
            {
                throw new UserErrorMessage($"Sender field too long. Exceeded {_emailMaxCharacters} characters");
            }

            if (!Regex.IsMatch(sender, _emailRegex))
            {
                throw new UserErrorMessage($"Email format invalid for sender '{sender}'");
            }
        }

        private void ValidateSubjectField(string? subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new UserErrorMessage("Email subject cannot be empty.");
            }

            if (subject.Length > _subjectMaxCharacters)
            {
                throw new UserErrorMessage($"Subject field too long. Exceeded {_subjectMaxCharacters} characters");
            }

        }

        private void ValidateBodyField(string? body)
        {
            if (body is not null && body.Length > _bodyMaxCharacters)
            {
                throw new UserErrorMessage($"Body field too long. Exceeded {_bodyMaxCharacters} characters");
            }
        }

        private void ValidateReceiverField(string? receiver)
        {
            if (receiver is null)
            {
                throw new UserErrorMessage("Receiver field cannot be empty.");
            }

            if (receiver.Length > _emailMaxCharacters)
            {
                throw new UserErrorMessage($"Receiver field too long. Exceeded {_emailMaxCharacters} characters");
            }

            if (!Regex.IsMatch(receiver, _emailRegex))
            {
                throw new UserErrorMessage($"Email format invalid for receiver '{receiver}'");
            }
        }


        public void CreateNewEmail(Email? email)
        {
            // Check all arguments
            if (email is null)
            {
                throw new UserErrorMessage("Cannot create a new mail. No mail specified or the mail is invalid.");
            }

            // Sanitize inputs
            email.ID = -1;

            // Body CAN be empty, just make sure it is not null
            if (email.Body is null)
            {
                email.Body = string.Empty;
            }

            ValidateSubjectField(email.Subject);
            ValidateBodyField(email.Body);
            ValidateSenderField(email.Sender);
            ValidateReceiverField(email.Receiver);

            // All fields validated, continue...

            // Set email timestamp to current time
            // (use UTC for cross-timezone compatibility)
            email.Timestamp = DateTime.UtcNow;

            _emailRepository.AddEmail(email);
        }

        public void UpdateEmail(int id, Email? email)
        {
            // Check all arguments
            if (email is null)
            {
                throw new UserErrorMessage("Cannot create a new mail. No mail specified or the mail is invalid.");
            }

            // Sanitize inputs
            email.ID = -1;

            // Body CAN be empty, just make sure it is not null
            if (email.Body is null)
            {
                email.Body = string.Empty;
            }

            ValidateSubjectField(email.Subject);
            ValidateBodyField(email.Body);
            ValidateSenderField(email.Sender);
            ValidateReceiverField(email.Receiver);

            // All fields validated, continue...

            _emailRepository.UpdateEmail(id, email);
        }

        public void DeleteEmail(int id)
        {
            _emailRepository.DeleteEmail(id);
        }

        public Email? GetEmail(int id)
        {
            return _emailRepository.GetEmailById(id);
        }

        public IEnumerable<Email> GetEmails()
        {
            return _emailRepository.GetAllEmails();
        }
    }
}
