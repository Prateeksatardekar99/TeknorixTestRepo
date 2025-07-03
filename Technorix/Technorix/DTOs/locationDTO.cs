
using System.ComponentModel.DataAnnotations;

namespace Technorix.DTOs
{
    /// <summary>
    /// DTO used to create a new location.
    /// </summary>
    public class LocationRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [Range(100000, 999999, ErrorMessage = "Zip must be a valid 6-digit number.")]
        public int? Zip { get; set; }
    }
    /// <summary>
    /// DTO returned after location creation or retrieval.
    /// </summary>
    public class LocationResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Zip { get; set; }
    }
}
