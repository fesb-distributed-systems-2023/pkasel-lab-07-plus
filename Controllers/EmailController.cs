using Microsoft.AspNetCore.Mvc;
using pkaselj_lab_07_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using pkaselj_lab_07_.Filters;
using pkaselj_lab_07_.Controllers.DTOs;
using pkaselj_lab_07_.Repositories;

namespace pkaselj_lab_07_.Controllers
{

    [CustomExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmailDto_Out>> Get()
        {
            // 1. Validation
                // No validation
            // 2. Convert DTO to Model
                // Nothing to do
            // 3. Forward call to the repository
            var allEmails = emailRepository.GetAllEmails();
            // 4. Convert Model to DTO
            var emailDtos = allEmails.Select(email => ConvertToDto(email)).ToList();
            // 5. Return result
            return Ok(emailDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<EmailDto_Out> Get(int id)
        {
            // 1. Validation
                // No validation
            // 2. Convert DTO to Model
                // Nothing to do
            // 3. Forward call to the repository
            var email = emailRepository.GetEmailById(id);

            // 4. Check for errors
            if (email == null)
            {
                return NotFound();
            }

            // 5. Convert Model to DTO
            var emailDto = ConvertToDto(email);

            // 6. Return result
            return Ok(emailDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] EmailDto_In emailDto)
        {
            // 1. Validation
            if (emailDto == null)
            {
                return BadRequest();
            }

            // 2. Convert DTO to Model
            var email = ConvertToEntity(emailDto);

            // 3. Forward call to the repository
            emailRepository.AddEmail(email);

            // 4. Check for errors
                // Nothing to check

            // 5. Convert Model to DTO
                // Nothing to do

            // 6. Return result
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] EmailDto_In emailDto)
        {
            // 1. Validation
            if (emailDto == null)
            {
                return BadRequest();
            }

            // 2. Convert DTO to Model
            var updatedEmail = ConvertToEntity(emailDto);

            // 3. Forward call to the repository
            var existingEmail = emailRepository.GetEmailById(id);

            // 4. Check for errors
            if (existingEmail == null)
            {
                return NotFound();
            }

            // 3. Forward call to the repository
            emailRepository.UpdateEmail(id, updatedEmail);

            // 4. Check for errors
                // Nothing to check

            // 5. Convert Model to DTO
                // Nothing to do

            // 6. Return result
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            // 1. Validation
                // Nothing to do

            // 2. Convert DTO to Model
                // Nothing to do

            // 3. Forward call to the repository
            var email = emailRepository.GetEmailById(id);

            // 4. Check for errors
            if (email == null)
            {
                return NotFound();
            }

            // 3. Forward call to the repository
            emailRepository.DeleteEmail(id);

            // 4. Check for errors
                // Nothing to check

            // 5. Convert Model to DTO
                // Nothing to do

            // 6. Return result
            return Ok();
        }

        // Helper methods to convert between Email and EmailDto

        // Model to DTO
        private EmailDto_Out ConvertToDto(Email email)
        {
            return new EmailDto_Out
            {
                ID = email.ID,
                Subject = email.Subject,
                Body = email.Body,
                Sender = email.Sender,
                Receiver = email.Receiver,
                Timestamp = email.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")
            };
        }

        // DTO to Model
        private Email ConvertToEntity(EmailDto_In emailDto)
        {
            return new Email
            {
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                Sender = emailDto.Sender,
                Receiver = emailDto.Receiver
            };
        }
    }
}
