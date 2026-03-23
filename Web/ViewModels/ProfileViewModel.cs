namespace LoginEvaluation.Web.ViewModels;

public class ProfileViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
