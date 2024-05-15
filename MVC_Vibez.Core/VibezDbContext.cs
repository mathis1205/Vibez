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
                FirstName = "John", LastName = "Doe", Email = "brechtvdk05@gmail.com", Password = "ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb", IsValid = true, Loggedin = false, ValidationToken = Guid.NewGuid().ToString(), ProfilePicture ="images/defaultuser.jpg"
            },
            new()
            {
                FirstName = "Jane", LastName = "Doe", Email = "b@b", Password = "3e23e8160039594a33894f6564e1b1348bbd7a0088d42c4acb73eeaed59c009d", IsValid = true, Loggedin = false, ValidationToken = Guid.NewGuid().ToString(),ProfilePicture ="images/defaultuser.jpg"
            }
        };

        Users.AddRange(usersToAdd);
        SaveChanges();
    }
}