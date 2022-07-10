using Microsoft.EntityFrameworkCore;

namespace OnlinePlatformBack.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Test> Tests { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();   
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}