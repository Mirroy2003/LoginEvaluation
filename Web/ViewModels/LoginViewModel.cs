using System.ComponentModel.DataAnnotations;

namespace LoginEvaluation.Web.ViewModels;

public class LoginViewModel
{
    [Required]
    public string DocumentType { get; set; } = "DNI";

    [Required]
    [Display(Name = "DNI")]
    public string Dni { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
}
