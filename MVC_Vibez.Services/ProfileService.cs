using MVC_Vibez.Core;

namespace MVC_Vibez.Services;

public class ProfileService
{
    private readonly VibezDbContext _context;

    public ProfileService(VibezDbContext context) => _context = context;
}