using System.ComponentModel.DataAnnotations;

namespace FortivexAPI.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }


}
