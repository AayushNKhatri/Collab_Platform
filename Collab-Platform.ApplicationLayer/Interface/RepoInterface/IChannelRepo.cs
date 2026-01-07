
using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IChannelRepo
    {
        Task AddChannel(Channel channel);
        Task RemoveChannle(Channel channel);
        Task<List<ViewChannelsDTO>> GetChannelsByTaskId(Guid TaskId);
        Task<Channel> GetChannelByID(Guid ChannelId);
        Task AddUserToChanel(List<UserChannel> User);
        Task RemoveUserFromChanel(List<UserChannel> User);
    }
}