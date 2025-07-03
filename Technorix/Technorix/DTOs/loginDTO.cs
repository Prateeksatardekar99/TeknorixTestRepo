using System.ComponentModel.DataAnnotations;


namespace Technorix.DTOs
{
    public class loginRequestDTO
    {
        [Required]

        public string Username { get; set; } = null!;


        [Required]

        public string Password { get; set; } = null!;

    }

    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
