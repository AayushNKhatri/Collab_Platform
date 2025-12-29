using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepo _channelRepo;
        private readonly IHelperService _helperService;
        private readonly IUnitOfWork _unitOfWork;

        public ChannelService( IChannelRepo channelRepo, IHelperService helperService)
        {
            _channelRepo = channelRepo;
            _helperService = helperService;
        }
        public async Task CreateChannel(CreateChannelsDTO createChannels, Guid TaskId) {
            try {
                var userId = _helperService.GetTokenDetails().userId;
                await _unitOfWork.BeginTranctionAsync();
                var channels = new Channel
                {
                    ChannelId = Guid.NewGuid(),
                    ChannelName = createChannels.ChannelName,
                    CreatorId = userId,
                    TaskId = TaskId,
                    CreatedAt = new DateTime()
                };
                if (createChannels.ChannelLeaderId == null)
                {
                    channels.ChannelLeaderId = createChannels.ChannelLeaderId;
                }
                channels.ChannelLeaderId = userId;
                await _channelRepo.AddChannel(channels);
                await _unitOfWork.SaveChangesAsync();
                var userForChanels = createChannels.UserID;
                userForChanels.Add(userId);
                var users = userForChanels.Select(u => new UserChannel
                {
                    ChannelId = channels.ChannelId,
                    UserId = u
                }).ToList();
                await _channelRepo.AddUserToChanel(users);
                await _unitOfWork.CommitTranctionAsync();

            }
            catch {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
        }
        public async Task<List<ViewChannelsDTO>> GetChannlesByTask(Guid TaskId) {
            var channels = await _channelRepo.GetChannelsByTaskId(TaskId) ?? throw new KeyNotFoundException("There is no Channel for task");
            var channelsReturn = channels.Select(u => new ViewChannelsDTO
            {
                ChannelId = u.ChannelId,
                ChannelName = u.ChannelName,
                ChannelLeaderId = u.ChannelLeaderId,
                ChannelLeaderName = u.ChannelName,
                CreatorId = u.CreatorId,
                CreatorName = u.Creator.UserName,
                TaskId = u.TaskId,
                TaskName = u.Task.TaskName,
                User = u.UserChannels.Select(u => new ChannelUser
                {
                    UserId = u.UserId,
                    UserName = u.User.UserName
                }).ToList()
            }).ToList();
            return channelsReturn;
        }
        public async Task<ViewChannelsDTO> GetChannelsById(Guid ChannelId) {
            var channels = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("The channel with that Id not found");
            var channelsReturn = new ViewChannelsDTO
            {
                ChannelId = channels.ChannelId,
                ChannelName = channels.ChannelName,
                ChannelLeaderId = channels.ChannelLeaderId,
                ChannelLeaderName = channels.ChannelName,
                CreatorId = channels.CreatorId,
                CreatorName = channels.ChannelName,
                TaskId = channels.TaskId,
                TaskName = channels.Task.TaskName,
                User = channels.UserChannels.Select(u => new ChannelUser
                {
                    UserId = u.UserId,
                    UserName = u.User.UserName
                }).ToList()
            };
            return channelsReturn;
        }
        public async Task DeleteChannels(Guid ChannelId)
        {
            var channel = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("This channel does not found");
            await _channelRepo.RemoveChannle(channel);
        }
    }
}