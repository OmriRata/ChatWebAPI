using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatWebAPI.Data;
using ChatWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChatWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ChatWebAPIContext _context;

        public ContactsController(ChatWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Contact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContact()
        {
            var connected = 1;

            if (_context.Contact == null)
            {
                return NotFound();
            }


            return await _context.Contact.Where(x => x.UserId == connected).ToListAsync();

        }

        // GET: api/Contact/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(string id)
        {
            var connected = 1;

            if (_context.Contact == null)
            {
                return NotFound();
            }
            var contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == id && x.UserId == connected).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contact/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id, Contact contact)
        {

            if (_context.Contact == null)
            {
                return NotFound();
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            var contactOrig = await _context.Contact.Where(x => x.Id == id).FirstAsync();


            contactOrig.Name = contact.Name;


            _context.Entry(contactOrig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contact
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            var ConnectedUser = 1;//HttpContext.User.Claims.ElementAt(3).Value;

            if (_context.Contact == null)
            {
                return Problem("Entity set 'ChatWebAPIContext.Contact'  is null.");
            }
            var user = _context.User.Include("Contacts").FirstOrDefault(x => x.Id == ConnectedUser);
            var userContacts = user.Contacts.ToList().Find(y => y.Id == contact.Id);
            if (userContacts != null)
            {
                return BadRequest();
            }
            contact.UserId = ConnectedUser;
            user.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contact/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            var ConnectedUser = 1;// HttpContext.User.Claims.ElementAt(3).Value;
            if (_context.Contact == null)
            {
                return NotFound();
            }
            var contactOrig = await _context.Contact.Where(x => x.Id == id).FirstAsync();
            if (contactOrig == null)
            {
                return NotFound();
            }
            var contact = await _context.Contact.Include(x => x.Messages).Where(x => x.Id == id && x.UserId == ConnectedUser).FirstOrDefaultAsync();
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
