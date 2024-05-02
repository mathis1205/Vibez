using MVC_Vibez.Core;

namespace MVC_Vibez.Services;

public class LibraryService
{
    private readonly VibezDbContext _context;

    public LibraryService(VibezDbContext context)
    {
        _context = context;
    }
}