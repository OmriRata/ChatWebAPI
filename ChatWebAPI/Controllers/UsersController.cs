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
    public class UsersController : ControllerBase
    {
        private readonly ChatWebAPIContext _context;
        public IConfiguration _configuration;

        public UsersController(ChatWebAPIContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }


        //[Authorize]
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            //var ConnectedUser = HttpContext.User.Claims.ElementAt(3).Value;
            //HttpContext.User.Claims.
            return await _context.User.Include(x => x.Contacts).ToListAsync();

        }




        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.Include(x => x.Contacts).Where(x => x.Id == id).FirstAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(string username, string nickname, string password)
        {
            var isExist = _context.User.Any(x => x.UserName == username);
          if (_context.User == null)
          {
              return Problem("Entity set 'ChatWebAPIContext.User'  is null.");
          }
            User user = new User() { UserName = username, NickName = nickname, Password = password };
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("~/api/login")]
        public async Task<ActionResult<string>> Login(string username, string password)
        {

            if (ModelState.IsValid)
            {
                if (_context.User == null)
                {
                    return Problem("Entity set 'WebAPIContext.User'  is null.");
                }
                var user = _context.User.FirstOrDefault(e => e.UserName == username && e.Password == password);
                if (user != null)
                {
                    //HttpContext.Session.SetString("userName", UserName);
                    Console.WriteLine(user.Id);

                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["JwtParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                    new Claim("UserId",user.Id.ToString())
                };
                    var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtParams:SecretKey"]));
                    var mac = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["JwtParams:Issuer"],
                        _configuration["JwtParams:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: mac
                        );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                return BadRequest();
            }
            return NoContent();

            /*
            if (ModelState.IsValid)
            {

            }
                if (_context.User == null)
            {
                return Problem("Entity set 'ChatWebAPIContext.User'  is null.");
            }

            bool isValid = _context.User.Any(u => u.UserName == username  && u.Password == password);
            if (isValid)
            {

                FormsAuthentication.SetAuthCookie(model.Username, false);
                return NoContent();
            }

            ModelState.AddModelError("", "Invalid Username/Password!");
            return NoContent();
            */
        }


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
