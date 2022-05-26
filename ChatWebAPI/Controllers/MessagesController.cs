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

/*
        // GET: api/Message
        [HttpGet("/api/contacts/{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessageByContact(string id)
        {
            if (_context.Message == null)
            {
                return NotFound();
            }
            int currentUserID = 2;

            //var messages = await _context.Message.Where(x => x.ContactId1 == id && x.UserId==currentUserID).ToListAsync();

            return _;
        }*/


        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
          if (_context.Message == null)
          {
              return NotFound();
          }
            var message = await _context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

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

        // POST: api/Message
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
          if (_context.Message == null)
          {
              return Problem("Entity set 'ChatWebAPIContext.Message'  is null.");
          }
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (_context.Message == null)
            {
                return NotFound();
            }
            var message = await _context.Message.FindAsync(id);
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
