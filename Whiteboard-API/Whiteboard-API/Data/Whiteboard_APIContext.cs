using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whiteboard_API.Model;

namespace Whiteboard_API.Data
{
    public class Whiteboard_APIContext : DbContext
    {
        public Whiteboard_APIContext (DbContextOptions<Whiteboard_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Whiteboard_API.Model.User> User { get; set; } = default!;

        public DbSet<Whiteboard_API.Model.Post> Post { get; set; }

        public DbSet<Whiteboard_API.Model.Comment> Comment { get; set; }
    }
}
