using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.ApplicationLayer.DTO.UserDto
{
    public class UserProfileDto
    {
        public string Email { get; set; }
        public bool isEmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
