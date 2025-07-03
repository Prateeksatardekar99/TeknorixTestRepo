using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Technorix.Models;

public partial class Location
{
    
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100)]
    public string State { get; set; } = null!;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Range(100000, 999999, ErrorMessage = "Zip must be a 6-digit number.")]

    [Required(ErrorMessage = "Zip is required.")]
     public int? Zip { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}