using System.ComponentModel.DataAnnotations;

namespace MVC_Vibez.Model;

public class ResetPasswordModel
{
    public string Token { get; set; }

    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required, Compare("NewPassword"), DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; }
}