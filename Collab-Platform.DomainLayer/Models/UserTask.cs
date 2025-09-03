namespace Collab_Platform.DomainLayer.Models
{
    public class UserTask
    {
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public Guid TaskId { get; set; }
        public TaskModel Task { get; set; }
    }
}
