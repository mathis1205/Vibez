using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_Vibez.Models;

public class User
{
    //create the different variables for the user 
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //make it required to be an email address
    [EmailAddress] public string Email { get; set; }
    //make it that it is an actual passwordtext
    [PasswordPropertyText] public string Password { get; set; }
    public bool loggedin { get; set; } = false;
}