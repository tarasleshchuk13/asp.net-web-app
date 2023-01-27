using System.ComponentModel.DataAnnotations;

namespace Application.Models.ViewModels;

public class RegisterVM
{
    [Required]
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}