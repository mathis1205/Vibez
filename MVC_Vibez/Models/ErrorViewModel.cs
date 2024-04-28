namespace MVC_Vibez.Models
{
    public class ErrorViewModel
    {
        //create the variables for if it errors
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
