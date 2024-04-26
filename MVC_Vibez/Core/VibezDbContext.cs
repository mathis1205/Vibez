using Microsoft.EntityFrameworkCore;
using MVC_Vibez.Models;

namespace MVC_Vibez.Core
{
    public class VibezDbContext(DbContextOptions<VibezDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        public void Seed()
        {
            var people = new List<User>
            {
                new() { FirstName = "John", LastName = "Doe", Email = "a@a", Password = "a", loggedin = false}
            };
            Users.AddRange(people);
            SaveChanges();
        }
    }
}
