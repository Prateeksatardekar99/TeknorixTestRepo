using System;
using System.ComponentModel.DataAnnotations;

namespace Technorix.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username cannot exceed 50 characters.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    public string PasswordHash { get; set; } = null!;

    [Required(ErrorMessage = "User role is required.")]
    [StringLength(50)]
    public string UserRole { get; set; } = null!;

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}
