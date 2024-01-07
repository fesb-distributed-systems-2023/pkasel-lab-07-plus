using Microsoft.AspNetCore.Mvc;
using pkaselj_lab_07_.Controllers.DTO;
using pkaselj_lab_07_.Filters;
using pkaselj_lab_07_.Logic;
using pkaselj_lab_07_.Models;
using pkaselj_lab_07_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pkaselj_lab_07_.Controllers
{

    [LogFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailLogic _emailLogic;

        public EmailController(IEmailLogic emailLogic)
        {
            this._emailLogic = emailLogic;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmailInfoDTO>> Get()
        {
            var allEmails = _emailLogic.GetEmails().Select(x => EmailInfoDTO.FromModel(x));
            return Ok(allEmails);
        }

        [HttpGet("{id}")]
        public ActionResult<EmailInfoDTO> Get(int id)
        {
            var email = _emailLogic.GetEmail(id);
            if (email == null)
            {
                return NotFound($"Email with ID {id} not found.");
            }

            return Ok( EmailInfoDTO.FromModel(email) );
        }

        [HttpPost]
        public ActionResult Post([FromBody] NewEmailDTO email)
        {
            if (email == null)
            {
                return BadRequest($"Wrong email format!");
            }

            _emailLogic.CreateNewEmail( email.ToModel() );

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] NewEmailDTO updatedEmail)
        {
            if (updatedEmail == null)
            {
                return BadRequest($"Wrong email format!");
            }

            var existingEmail = _emailLogic.GetEmail(id);
            if (existingEmail == null)
            {
                return NotFound($"Email with ID {id} not found.");
            }

            _emailLogic.UpdateEmail( id, updatedEmail.ToModel() );

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var email = _emailLogic.GetEmail(id);
            if (email == null)
            {
                return NotFound($"Email with ID {id} not found.");
            }

            _emailLogic.DeleteEmail(id);

            return Ok();
        }
    }
}
