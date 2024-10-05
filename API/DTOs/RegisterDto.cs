using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
  // Username field is required and must be provided 
  // and initialized with an empty string to avoid null values
  [Required]
  public string Username { get; set; } = string.Empty;

  [Required] public string? KnownAs { get; set; }
  [Required] public string? Gender { get; set; }
  [Required] public string? DateOfBirth { get; set; }
  [Required] public string? City { get; set; }
  [Required] public string? Country { get; set; }

  // Password field is required and must be between 4 and 8 characters long
  // and initialized with an empty string to avoid null values
  [Required]
  [StringLength(8, MinimumLength = 4)]
  public string Password { get; set; } = string.Empty;
}
