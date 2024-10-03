using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

// Specifies that this class maps to the "Photos" table in the database
[Table("Photos")]
public class Photo
{
  public int Id { get; set; }
  public required string Url { get; set; }
  public bool IsMain { get; set; }

  // Public ID of the photo, used by external services (optional field)
  public string? PublicId { get; set; }

  // Foreign key linking the photo to the AppUser entity
  public int AppUserId { get; set; }

  // Navigation property representing the associated AppUser entity
  // The photo belongs to a single AppUser
  public AppUser AppUser { get; set; } = null!;
}
