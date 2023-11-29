using MediaCenter.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    }
}
