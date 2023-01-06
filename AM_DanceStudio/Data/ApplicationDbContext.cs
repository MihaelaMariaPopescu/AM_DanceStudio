using AM_DanceStudio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AM_DanceStudio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //pushh

        public DbSet<Class> Classes { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Studio> Studios { get; set; }

    }
}