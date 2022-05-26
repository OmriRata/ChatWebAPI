using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatWebAPI.Data;
using ChatWebAPI.Models;

namespace ChatWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatWebAPIContext _context;

        public MessagesController(ChatWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessage()
        {
          if (_context.Message == null)
          {
              return NotFound();
          }
            return await _context.Message.ToListAsync();
        }


        // GET: api/Message
        [HttpGet("/api/contacts/{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessageByContact(string id)
        {
            var connectUser = 2;
            if (_context.Message == null)
            {
                return NotFound();
            }
            //int currentUserID = 2;
            var contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == id && x.UserId == connectUser).FirstOrDefaultAsync();
            if (contact == null)
            {
                return NotFound();

            }
            var messages = await _context.Message.Where(x => x.Contact.ContactId == contact.ContactId).ToListAsync();

            return messages;
        }

        // Post : api/Message
        [HttpPost("/api/contacts/{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> PostMessageByContact(string id,Message message)
        {
            var connectUser = 2;
            if (_context.Message == null)
            {
                return Problem("Entity set 'ChatWebAPIContext.Message'  is null.");
            }
            var contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == id && x.UserId == connectUser).FirstOrDefaultAsync();
            if (contact == null)
            {
                return NotFound();

            }
            contact.Messages.Add(message);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        [HttpGet("/api/contacts/{contact}/messages/{id}")]
        public async Task<ActionResult<Message>> GetMessageByID(string contact,int id)
        {
            var connectUser = 2;
            if (_context.Message == null)
            {
                return NotFound();
            }
            //int currentUserID = 2;
            var _contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == contact && x.UserId == connectUser).FirstOrDefaultAsync();
            if (_contact == null)
            {
                return NotFound();

            }
            var message = _contact.Messages.Where(x=>x.Id == id).FirstOrDefault();
            if (message == null)
            {
                return NotFound();

            }
            return message; 
        }


        // PUT: api/Message/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/api/contacts/{contact}/messages/{id}")]
        public async Task<IActionResult> PutMessage(string contact, int id, Message message)
        {
            var connectUser = 2;
            if (id != message.Id)
            {
                return BadRequest();
            }
            var _contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == contact && x.UserId == connectUser).FirstOrDefaultAsync();
            if (_contact == null)
            {
                return BadRequest();
            }
            var messageOrig = _contact.Messages.Where(x => x.Id == id).FirstOrDefault();
            if (messageOrig == null)
            {
                return BadRequest();
            }
            messageOrig.Created = message.Created;
            messageOrig.Content = message.Content;
            _context.Entry(messageOrig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Message/5
        [HttpDelete("/api/contacts/{contact}/messages/{id}")]
        public async Task<IActionResult> DeleteMessage(string contact, int id)
        {
            var connectUser = 2;

            if (_context.Message == null)
            {
                return NotFound();
            }
            var _contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == contact && x.UserId == connectUser).FirstOrDefaultAsync();
            if (_contact == null)
            {
                return BadRequest();
            }
            var message = _contact.Messages.Where(x => x.Id == id).FirstOrDefault();
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
