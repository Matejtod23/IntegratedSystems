using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Theatre.Web.Models;

namespace Theatre.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<TheatreUser>
    {
        public virtual DbSet<TheatreUser> TheatreUsers { get; set; }
        public virtual DbSet<TheatreShow> TheatreShows { get; set; }    
        public virtual DbSet<Ticket> Ticket { get; set; }   

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
