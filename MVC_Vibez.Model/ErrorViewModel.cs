namespace MVC_Vibez.Model
{
    public class ErrorViewModel
    {
        public string? RequestId { get; init; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
