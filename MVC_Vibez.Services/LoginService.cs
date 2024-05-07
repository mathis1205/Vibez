using MVC_Vibez.Core;
using MVC_Vibez.Model;

namespace MVC_Vibez.Services;

public class LoginService
{
    private readonly VibezDbContext _context;

    public LoginService(VibezDbContext context) => _context = context;

    public User? Create(User user)
    {
        user.ProfilePicture = "images/defaultuser.jpg";
        user.Password = HashingHelper.HashPassword(user.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
}