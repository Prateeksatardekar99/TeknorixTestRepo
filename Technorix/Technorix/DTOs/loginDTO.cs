using System.ComponentModel.DataAnnotations;


namespace Technorix.DTOs
{
    public class loginDTO
    {
        [Required]

        public string Username { get; set; } = null!;


        [Required]

        public string Password { get; set; } = null!;

    }
}
