using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class Channel 
    {
        public Guid ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string CreatorId { get; set; }
        public UserModel Creator { get; set; }
        public string ChannelLeaderId { get; set; }
        public Guid TaskId { get; set; }
        public TaskModel Task { get; set; }
        public UserModel ChannelLeader { get; set; }
        public ICollection<UserChannel> UserChannels { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
