namespace MVC_Vibez.Model;
public class LoginToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public bool IsUsed { get; set; } = false;
}