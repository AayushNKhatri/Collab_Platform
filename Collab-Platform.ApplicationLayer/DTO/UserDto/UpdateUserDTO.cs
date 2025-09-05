using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.ApplicationLayer.DTO.UserDto
{
    public class UpdateUserDTO
    {
        [EmailAddress]
        public string? Email { get; set; }   
        public string? currentPassword { get; set; }
        
        public string? newPassword { get; set; }
        
        public string? UserName { get; set; }
     
        public string? PhoneNumber { get; set; }
    }
}
