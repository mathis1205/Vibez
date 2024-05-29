using Microsoft.AspNetCore.Authentication.Cookies;
using MVC_Vibez.Core;
using MVC_Vibez.Model;
using MVC_Vibez.Services;
#if !DEBUG
using (var context = new VibezDbContext())
{
    // Migrate the database
    context.Database.Migrate();
}
#endif

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VibezDbContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Home/Index";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });
builder.Services.Configure<EmailService.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<GeniusSearchOptions>(builder.Configuration.GetSection("GeniusSearch"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<GeniusSearch>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var vibezDbContect = scope.ServiceProvider.GetRequiredService<VibezDbContext>();
    vibezDbContect.SeedAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();