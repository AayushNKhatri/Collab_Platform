using Collab_Platform.DomainLayer.EnumsAndOther;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class ResourceModel
    {
        public Guid ResourceId { get; set; }
        [Required]
        public string ResourceName { get; set; }
        [Required]
        public string ResourceDesc { get; set; }
        public string? ResourcePath { get; set; }
        public Guid? ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        public Guid? ChannelId { get; set; }

        public Channel? Channel { get; set; }
        [Required]
        public string UploadedById { get; set; }
        public UserModel UploadedBy { get; set; }
        public FileTypeEnum ResourceType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
