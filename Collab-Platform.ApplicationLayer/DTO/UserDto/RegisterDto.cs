using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.ApplicationLayer.DTO.UserDto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
