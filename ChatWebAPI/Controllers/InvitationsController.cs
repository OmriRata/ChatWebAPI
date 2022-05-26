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
    public class InvitationsController : ControllerBase
    {
        private readonly ChatWebAPIContext _context;
        public IConfiguration _configuration;

        public InvitationsController(ChatWebAPIContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }


        //[Authorize]
        // GET: api/Invitations
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Invitation>>> PostInvitation(Invitation invitation)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = _context.User.Include("Contacts").FirstOrDefault(x => x.UserName == invitation.To);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine(user);
            var userContacts = user.Contacts.ToList().Any(x=>x.Id == invitation.From);
            if (userContacts)
            {
                return BadRequest();
            }
            Contact c = new Contact() { Id = invitation.From, Name = invitation.From, UserId = user.Id, Server = invitation.Server };
            user.Contacts.Add(c);
            await _context.SaveChangesAsync();
            //var ConnectedUser = HttpContext.User.Claims.ElementAt(3).Value;
            //HttpContext.User.Claims.
            //return await _context.Contact.Include(x => x.Messages).ToListAsync();
            return StatusCode(201);

        }
    }
}
