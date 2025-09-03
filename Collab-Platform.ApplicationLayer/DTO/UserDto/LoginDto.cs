using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.ApplicationLayer.DTO.UserDto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
