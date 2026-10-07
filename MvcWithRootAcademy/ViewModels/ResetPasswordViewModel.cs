using System.ComponentModel.DataAnnotations;

namespace MvcWithRootAcademy.ViewModels;

public class ResetPasswordViewModel
{

    [Required(ErrorMessage = "Password Is Required")]
    [MinLength(length: 5, ErrorMessage = "Minimum Password Length Is 5")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }


    [Required(ErrorMessage = "Password Is Required")]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not Match Password")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; }
}
