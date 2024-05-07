using System.ComponentModel.DataAnnotations;

namespace MVC_Vibez.Models
{
    public class ContactFormSubmission
    {
        //Create the email and message variables and make it that they are required
        [Required] public string Email { get; set; }
        [Required] public string Message { get; set; }
    }
}
