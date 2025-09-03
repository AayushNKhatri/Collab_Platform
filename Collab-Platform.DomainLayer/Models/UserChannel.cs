namespace Collab_Platform.DomainLayer.Models
{
    public class UserChannel
    {
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
