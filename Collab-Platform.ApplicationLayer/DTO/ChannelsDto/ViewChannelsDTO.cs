using Collab_Platform.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.ApplicationLayer.DTO.ChannelsDto
{
    public class ViewChannelsDTO
    {
        public Guid ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string ChannelLeaderId { get; set; }
        public string ChannelLeaderName { get; set; }
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public List<ChannelUser> User { get; set; }
    }
    public class ChannelUser {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
