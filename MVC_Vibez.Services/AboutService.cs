using MVC_Vibez.Core;

namespace MVC_Vibez.Services;

public class AboutService
{
    private readonly VibezDbContext _context;

    public AboutService(VibezDbContext context)
    {
        _context = context;
    }
}