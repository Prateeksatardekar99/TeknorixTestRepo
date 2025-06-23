using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Technorix.Models;

public partial class Location
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]

    public string? City { get; set; }
    [Required]

    public string? State { get; set; }
    [Required]
    public string? Country { get; set; }

    public int? Zip { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
