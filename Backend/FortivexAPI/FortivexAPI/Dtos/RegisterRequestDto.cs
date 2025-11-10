using System.ComponentModel.DataAnnotations;

namespace FortivexAPI.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }
        [EmailAddress]
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }

}
