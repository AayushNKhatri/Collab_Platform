using Collab_Platform.DomainLayer.EnumsAndOther;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class ChatModel
    {
        public Guid ChatId { get; set; }

        [Required]
        public string ChatContent { get; set; }
        [Required]
        public string SenderId { get; set; }
        public UserModel Sender { get; set; }
        public string? ReciverId { get; set; }
        public UserModel Reciver { get; set; }
        [Required]
        public ChatDomain ChatDomain { get; set; }
        [Required]
        public ChatTypes ChatType { get; set; }
        public int? ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        public Guid? ChannelId { get; set; }
        public Channel Channel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
