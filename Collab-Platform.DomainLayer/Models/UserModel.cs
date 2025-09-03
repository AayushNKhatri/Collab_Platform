using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class UserModel : IdentityUser
    {
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
        public ICollection<SubtaskModel> Subtasks { get; set; }
        public ICollection<Channel> UserChannels { get; set; }

    }
}
