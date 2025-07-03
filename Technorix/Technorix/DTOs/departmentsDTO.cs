
using System.ComponentModel.DataAnnotations;

namespace Technorix.DTOs
{
    /// <summary>
    /// DTO used to create a new department.
    /// </summary>
    public class DepartmentCreateDto
    {
       
        /// <summary>
        /// Title of the department.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
    }



  



    /// <summary>
    /// DTO returned after department creation or retrieval.
    /// </summary>
    public class DepartmentResponseDto
        {
            /// <summary>
            /// ID of the department.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Title of the department.
            /// </summary>
            public string Title { get; set; } = string.Empty;
        }
    }

