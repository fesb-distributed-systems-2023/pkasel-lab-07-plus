using Microsoft.AspNetCore.Mvc;
using pkaselj_lab_07_.Models;
using pkaselj_lab_07_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pkaselj_lab_07_.Controllers
{

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
        public ActionResult<IEnumerable<Email>> Get()
        {
            var allEmails = emailRepository.GetAllEmails();
            return Ok(allEmails);
        }

        [HttpGet("{id}")]
        public ActionResult<Email> Get(int id)
        {
            var email = emailRepository.GetEmailById(id);
            if (email == null)
            {
                return NotFound();
            }

            return Ok(email);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Email email)
        {
            if (email == null)
            {
                return BadRequest();
            }

            emailRepository.AddEmail(email);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Email updatedEmail)
        {
            if (updatedEmail == null)
            {
                return BadRequest();
            }

            var existingEmail = emailRepository.GetEmailById(id);
            if (existingEmail == null)
            {
                return NotFound();
            }

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
    }
}
