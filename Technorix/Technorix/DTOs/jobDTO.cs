using System.ComponentModel.DataAnnotations;

namespace Technorix.DTOs
{
    /// <summary>
    /// DTO used for creating or updating a Job.
    /// </summary>
    public class JobRequestDto
    {
        /// <summary>
        /// The title of the job.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The job description.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the location where the job is based.
        /// </summary>
        [Required]
        public int LocationId { get; set; }

        /// <summary>
        /// The ID of the department offering the job.
        /// </summary>
        [Required]
        public int DepartmentId { get; set; }

        /// <summary>
        /// The date when the job listing will close.
        /// </summary>
        [Required]
        public DateTime ClosingDate { get; set; }
    }

    /// <summary>
    /// DTO used for paginated job listing requests with filters.
    /// </summary>
    public class JoblistDTO
    {
        /// <summary>
        /// Text to search for in job titles or descriptions.
        /// </summary>
        
        public string SearchText { get; set; } = string.Empty;

        /// <summary>
        /// Page number to retrieve.
        /// </summary>
        [Required]
        public int pageNo { get; set; } = 1;

        /// <summary>
        /// Number of jobs per page.
        /// </summary>
        [Required]
        public int pageSize { get; set; } = 10;

        /// <summary>
        /// Optional location filter (0 = All locations).
        /// </summary>
        public int locationId { get; set; } = 0;

        /// <summary>
        /// Optional department filter (0 = All departments).
        /// </summary>
        public int departmentId { get; set; } = 0;
    }

    /// <summary>
    /// Response DTO for a paginated list of jobs.
    /// </summary>
    public class JobListResponseDto
    {
        /// <summary>
        /// Total number of jobs matching the filters.
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// List of job items in the current page.
        /// </summary>
        public List<JobListItemResponseDto> Data { get; set; } = new();
    }

    /// <summary>
    /// Summary DTO for each job displayed in job listings.
    /// </summary>
    public class JobListItemResponseDto
    {
        /// <summary>
        /// Unique job identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique code for the job.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Job title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Location name.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Department name.
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Date the job was posted.
        /// </summary>
        public DateTime PostedDate { get; set; }

        /// <summary>
        /// Closing date for the job application.
        /// </summary>
        public DateTime ClosingDate { get; set; }
    }

    /// <summary>
    /// JWT token configuration settings.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// JWT issuer.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// JWT audience.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// JWT secret key (should be 32+ characters for HMAC).
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Expiry time for the token in minutes.
        /// </summary>
        public int ExpiryMinutes { get; set; }
    }


    public class JobDetailsResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LocationResponseDto Location { get; set; } = new();
        public DepartmentResponseDto Department { get; set; } = new();
        public DateTime Posteddate { get; set; }
        public DateTime Closingdate { get; set; }
    }

}
