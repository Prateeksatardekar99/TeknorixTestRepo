

using System.ComponentModel.DataAnnotations;

namespace Technorix.DTOs
{
    public class JobDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public DateTime ClosingDate { get; set; }
    }
  
    public class listDTO
    {



        [Required]

        public string SearchText { get; set; } = string.Empty;  // Search string
        [Required]
        public int pageNo { get; set; } = 1;// Page number
        [Required]
        public int pageSize { get; set; } = 10;// Page size



        public int locationId { get; set; } = 0;  // Optional location id

        public int departmentId { get; set; } = 0; // Optional department id
 
}

    public class JobListResponseDto
    {
      
        public int? Total { get; set; }
    public List<JobListItemDto> Data { get; set; } = new();

}


    public class JobListItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
    }




}