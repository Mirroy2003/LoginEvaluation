using System;

namespace LoginEvaluation.Web.ViewModels;

public class LockoutViewModel
{
    public string Email { get; set; } = string.Empty;
    public DateTime? LockoutEndUtc { get; set; }
}

