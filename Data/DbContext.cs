using Microsoft.EntityFrameworkCore;

using Models;
namespace context
{


    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<Reunion> Reunions { get; set; }
        public DbSet<ProfProfile> ProfProfiles { get; set; }

        public DbSet<StudentProfile> StudentProfile { get; set; }


    }
}