using MVC_Vibez.Core;
using MVC_Vibez.Models;

namespace MVC_Vibez.Services;

public class ProfileService
{
    private readonly VibezDbContext _context;

    public ProfileService(VibezDbContext context)
    {
        _context = context;
    }
}