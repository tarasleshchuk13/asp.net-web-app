using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required] 
    public string Password { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime LastLoginDate { get; set; } = DateTime.Now;

    public bool IsBlocked { get; set; } = false;
}