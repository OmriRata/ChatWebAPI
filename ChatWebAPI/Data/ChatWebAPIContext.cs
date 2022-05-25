using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatWebAPI.Models;

namespace ChatWebAPI.Data
{

    public class ChatWebAPIContext : DbContext
    {
        public ChatWebAPIContext (DbContextOptions<ChatWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<ChatWebAPI.Models.User>? User { get; set; }

        public DbSet<ChatWebAPI.Models.Contact>? Contact { get; set; }

        public DbSet<ChatWebAPI.Models.Message>? Message { get; set; }


    }
}
