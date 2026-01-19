using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.Mapper
{
    public class ChannelMapper
    {
        public static ViewChannelsDTO ToChannelDTO(Channel channel)
        {
            return new ViewChannelsDTO
            {
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
                CreatorId = channel.CreatorId,
                CreatorName = channel.Creator.UserName,
                ChannelLeaderId = channel.ChannelLeaderId,
                ChannelLeaderName = channel.ChannelLeader.UserName,
                TaskId = channel.TaskId,
                TaskName = channel.Task.TaskName,
                User = channel.UserChannels.Select(u => new ChannelUser {
                    UserId = u.UserId,
                    UserName = u.User.UserName
                }).ToList()
            };
        }
    }
}
