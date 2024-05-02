using MVC_Vibez.Core;

namespace MVC_Vibez.Services;

public class HomeService
{
    private readonly VibezDbContext _context;

    public HomeService(VibezDbContext context)
    {
        _context = context;
    }
}