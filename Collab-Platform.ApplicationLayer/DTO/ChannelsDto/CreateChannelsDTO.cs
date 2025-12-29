using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.ChannelsDto
{
    public class CreateChannelsDTO
    {
        public string ChannelName { get; set; }
        public string? ChannelLeaderId { get; set; }
        public List<string> UserID { get; set; }
    
    }
}
