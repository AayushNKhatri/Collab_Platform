using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collab_Platform.DomainLayer.Models
{
    public class CustomRoleUser
    {
        public string UserID { get; set;}
        public UserModel user { get; set;}
        public Guid CustomRoleId { get; set;}
        public CustomRoleModels customRole { get; set;}
    }
}