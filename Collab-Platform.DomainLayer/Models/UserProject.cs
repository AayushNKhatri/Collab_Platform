namespace Collab_Platform.DomainLayer.Models
{
    public class UserProject
    {
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectModel Project { get; set; }

    }

}
