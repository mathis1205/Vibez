using Microsoft.EntityFrameworkCore;
using MVC_Vibez.Models;

namespace MVC_Vibez.Core
{
    public class VibezDbContext(DbContextOptions<VibezDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public void Seed()
        {
            if (!Users.Any())
            {
                var people = new List<User>
                {
                    new User { FirstName = "John", LastName = "Doe", Email = "a@a", Password = "a", IsValid = true,loggedin = false, ValidationToken = Guid.NewGuid().ToString()}
                };
                Users.AddRange(people);
                SaveChanges();
            }
        }
    }
}