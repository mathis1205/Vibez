using Microsoft.EntityFrameworkCore;
using MVC_Vibez.Model;

namespace MVC_Vibez.Core;

public class VibezDbContext(DbContextOptions<VibezDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public void Seed()
    {
        if (Users.Any()) return;

        var usersToAdd = new List<User>
        {
            new()
            {
                FirstName = "John", LastName = "Doe", Email = "a@a", Password = "a", IsValid = true, Loggedin = false, ValidationToken = Guid.NewGuid().ToString(), ProfilePicture ="images/defaultuser.jpg"
            },
            new()
            {
                FirstName = "Jane", LastName = "Doe", Email = "b@b", Password = "b", IsValid = true, Loggedin = false, ValidationToken = Guid.NewGuid().ToString(),ProfilePicture ="images/defaultuser.jpg"
            }
        };

        Users.AddRange(usersToAdd);
        SaveChanges();
    }
}