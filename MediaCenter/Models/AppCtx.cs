using MediaCenter.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MediaCenter.Models
{
    public class AppCtx : IdentityDbContext<User>
    {
        public AppCtx(DbContextOptions<AppCtx> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<PostStatus> PostStatuses { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Like> Likes { get; set; }
    }
}
