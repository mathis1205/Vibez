namespace MVC_Vibez.Models;

public class Token
{
	//create the variables for the token
	public string access_token { get; set; }

	public string token_type { get; set; }

	public int expires_in { get; set; }
}