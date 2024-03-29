﻿using Microsoft.Extensions.Options;
using pkaselj_lab_07_.Configuration;
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
        private readonly ValidationConfiguration _validationConfiguration;

        public EmailLogic(IEmailRepository emailRepository, IOptions<ValidationConfiguration> configuration)
        {
            _emailRepository = emailRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateSenderField(string? sender)
        {
            if (sender is null)
            {
                throw new UserErrorMessage("Sender field cannot be empty.");
            }

            if (sender.Length > _validationConfiguration.EmailMaxCharacters)
            {
                throw new UserErrorMessage($"Sender field too long. Exceeded {_validationConfiguration.EmailMaxCharacters} characters");
            }

            if (!Regex.IsMatch(sender, _validationConfiguration.EmailRegex))
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

            if (subject.Length > _validationConfiguration.SubjectMaxCharacters)
            {
                throw new UserErrorMessage($"Subject field too long. Exceeded {_validationConfiguration.SubjectMaxCharacters} characters");
            }

        }

        private void ValidateBodyField(string? body)
        {
            if (body is not null && body.Length > _validationConfiguration.BodyMaxCharacters)
            {
                throw new UserErrorMessage($"Body field too long. Exceeded {_validationConfiguration.BodyMaxCharacters} characters");
            }
        }

        private void ValidateReceiverField(string? receiver)
        {
            if (receiver is null)
            {
                throw new UserErrorMessage("Receiver field cannot be empty.");
            }

            if (receiver.Length > _validationConfiguration.EmailMaxCharacters)
            {
                throw new UserErrorMessage($"Receiver field too long. Exceeded {_validationConfiguration.EmailMaxCharacters} characters");
            }

            if (!Regex.IsMatch(receiver, _validationConfiguration.EmailRegex))
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
