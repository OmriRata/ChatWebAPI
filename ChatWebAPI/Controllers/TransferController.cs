using Microsoft.AspNetCore.Http;
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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ChatWebAPIContext _context;
        public IConfiguration _configuration;

        public TransferController(ChatWebAPIContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }


        //[Authorize]
        // GET: api/Transfer
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Transfer>>> PostTransfer(Transfer transfer)
        {
            var ConnectedUser = Int32.Parse(HttpContext.User.Claims.ElementAt(3).Value);
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = _context.User.Include("Contacts").FirstOrDefault(x => x.UserName == transfer.To);
            if (user == null)
            {
                return NotFound();
            }
            var isContact = user.Contacts.ToList().Any(x=>x.Id == transfer.From);
            if (!isContact)
            {
                return BadRequest();
            }
            Message message = new Message() {Content=transfer.Content,Sent=true,Created=DateTime.UtcNow};
            var contact = _context.Contact.Include(x => x.Messages).FirstOrDefault(c => c.User.UserName == transfer.To && c.Id == transfer.From);
            contact.Messages.Add(message);
            contact.LastDate = message.Created;
            contact.Last = message.Content;

            await _context.SaveChangesAsync();
            //var ConnectedUser = HttpContext.User.Claims.ElementAt(3).Value;
            //HttpContext.User.Claims.
            //return await _context.Contact.Include(x => x.Messages).ToListAsync();
            return StatusCode(201);

        }
    }
}
