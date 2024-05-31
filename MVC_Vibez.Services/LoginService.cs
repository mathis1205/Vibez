using MVC_Vibez.Core;
using MVC_Vibez.Model;

namespace MVC_Vibez.Services;

public class LoginService
{
    private readonly VibezDbContext _context;

    public LoginService(VibezDbContext context)
    {
        _context = context;
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public User Create(User user)
    {
        user.ProfilePicture = "images/defaultuser.jpg";
        user.Password = HashingHelper.HashPassword(user.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User Update(User user)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null) return null;

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Loggedin = user.Loggedin;
        existingUser.ProfilePicture = user.ProfilePicture;
        existingUser.IsValid = user.IsValid;
        existingUser.ValidationToken = user.ValidationToken;
        existingUser.Email = user.Email;

        // Only update the password if it has changed
        if (!string.IsNullOrEmpty(user.Password) && user.Password != existingUser.Password) existingUser.Password = HashingHelper.HashPassword(user.Password);

        _context.SaveChanges();
        return existingUser;
    }
}