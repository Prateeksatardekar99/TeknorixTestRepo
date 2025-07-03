using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Technorix.Models;

public partial class Job
{
     public int Id { get; set; }
    public string Code { get; set; } = null!;
    [Required(ErrorMessage = "Job title is required.")]
    [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
    public string Title { get; set; } = null!;
    
    [MaxLength]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Posted date is required.")]
    [DataType(DataType.Date)]
    public DateTime Posteddate { get; set; }

    [Required(ErrorMessage = "Closing date is required.")]
    [DataType(DataType.Date)]
    public DateTime Closingdate { get; set; }
    [Required(ErrorMessage = "Locationid is required.")]

    public int Locationid  { get; set; }
    [Required(ErrorMessage = "Departmentid is required.")]

    public int Departmentid  { get; set; }
    [JsonIgnore]
    public virtual Department Department { get; set; } = null!;
    [JsonIgnore]
    public virtual Location Location { get; set; } = null!;
}
