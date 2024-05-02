using MVC_Vibez.Core;
using MVC_Vibez.Models;

namespace MVC_Vibez.Services;

public class LoginService
{
    private readonly VibezDbContext _context;

    public LoginService(VibezDbContext context)
    {
        _context = context;
    }

    public User? Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
}