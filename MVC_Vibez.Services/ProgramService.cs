using MVC_Vibez.Core;
using MVC_Vibez.Model;

namespace MVC_Vibez.Services;

public class ProgramService
{
    private readonly VibezDbContext _context;
    public ProgramService(VibezDbContext context) => _context = context;
    public User GetUserByEmail(string email) { return _context.Users.FirstOrDefault(u => u.Email == email); }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}