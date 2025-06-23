using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Technorix.Models;

public partial class Department
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
