using Microsoft.EntityFrameworkCore;
using MVC_Vibez.Core;
using MVC_Vibez.Model;

namespace MVC_Vibez.Services;

public class ProgramService
{
    private readonly VibezDbContext _context;

    public ProgramService(VibezDbContext context)
    {
        _context = context;
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    public void UpdateUser(User user)
    {
        var dbUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (dbUser == null) return;
        _context.SaveChanges();
    }
}