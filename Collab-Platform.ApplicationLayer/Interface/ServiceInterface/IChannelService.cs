using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IChannelService
    {
        Task CreateChannel(CreateChannelsDTO createChannels, Guid TaskId);
        Task<List<ViewChannelsDTO>> GetChannlesByTask(Guid TaskId);
        Task<ViewChannelsDTO> GetChannelsById(Guid ChannelId);
        Task DeleteChannels(Guid ChannelId);
    }
}