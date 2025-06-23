using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Technorix.Models;

public partial class Job
{
     public int Id { get; set; }
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    
    public DateTime Posteddate { get; set; }

    public DateTime Closingdate { get; set; }

    public int Locationid  { get; set; }

    public int Departmentid  { get; set; }
    
    public virtual Department Department { get; set; } = null!;
    
    public virtual Location Location { get; set; } = null!;
}
