using System.ComponentModel.DataAnnotations;

namespace MVC_Vibez.Model
{
    public class ContactFormSubmission
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
