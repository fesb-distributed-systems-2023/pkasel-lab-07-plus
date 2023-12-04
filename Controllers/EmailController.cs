using Microsoft.AspNetCore.Mvc;
using pkaselj_lab_07_.Models;
using pkaselj_lab_07_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using pkaselj_lab_07_.Filters;
using pkaselj_lab_07_.Controllers.DTOs;

namespace pkaselj_lab_07_.Controllers
{

    [CustomExceptionFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailRepository emailRepository;

        public EmailController(EmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmailDto_Out>> Get()
        {
            var allEmails = emailRepository.GetAllEmails();
            var emailDtos = allEmails.Select(email => ConvertToDto(email)).ToList();
            return Ok(emailDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<EmailDto_Out> Get(int id)
        {
            var email = emailRepository.GetEmailById(id);
            if (email == null)
            {
                return NotFound();
            }

            var emailDto = ConvertToDto(email);
            return Ok(emailDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] EmailDto_In emailDto)
        {
            if (emailDto == null)
            {
                return BadRequest();
            }

            var email = ConvertToEntity(emailDto);
            emailRepository.AddEmail(email);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] EmailDto_In emailDto)
        {
            if (emailDto == null)
            {
                return BadRequest();
            }

            var existingEmail = emailRepository.GetEmailById(id);
            if (existingEmail == null)
            {
                return NotFound();
            }

            var updatedEmail = ConvertToEntity(emailDto);
            emailRepository.UpdateEmail(id, updatedEmail);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var email = emailRepository.GetEmailById(id);
            if (email == null)
            {
                return NotFound();
            }

            emailRepository.DeleteEmail(id);

            return Ok();
        }

        // Helper methods to convert between Email and EmailDto
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
