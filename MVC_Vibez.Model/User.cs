using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_Vibez.Model;

public class User
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	[EmailAddress] public string Email { get; set; }
	[PasswordPropertyText] public string Password { get; set; }
	public bool Loggedin { get; set; }
	public LoginToken? LoginToken { get; set; }
	public string ValidationToken { get; set; }
	public bool IsValid { get; set; }
	public string? ProfilePicture { get; set; }
}
