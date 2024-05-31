using Microsoft.EntityFrameworkCore;
using MVC_Vibez.Model;

namespace MVC_Vibez.Core
{
    public class VibezDbContext : DbContext
    {
        public VibezDbContext(DbContextOptions<VibezDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}