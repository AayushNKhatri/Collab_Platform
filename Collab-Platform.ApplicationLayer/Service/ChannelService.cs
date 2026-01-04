using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepo _channelRepo;
        private readonly IDataHelper _helperService;
        private readonly IUnitOfWork _unitOfWork;

        public ChannelService( IChannelRepo channelRepo, IDataHelper helperService, IUnitOfWork unitOfWork)
        {
            _channelRepo = channelRepo;
            _helperService = helperService;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateChannel(CreateChannelsDTO createChannels, Guid TaskId) {

            await _unitOfWork.BeginTranctionAsync();
            try {
                var userId = _helperService.GetTokenDetails().userId;
                List<string> newUser = new List<string>();
               
                var channels = new Channel
                {
                    ChannelId = Guid.NewGuid(),
                    ChannelName = createChannels.ChannelName,
                    CreatorId = userId,
                    TaskId = TaskId,
                    CreatedAt = new DateTime(),
                    ChannelLeaderId = createChannels.ChannelLeaderId ?? userId

                };
                await _channelRepo.AddChannel(channels);
                await _unitOfWork.SaveChangesAsync();
                if (createChannels.ChannelLeaderId != null)
                {
                    newUser.Add(createChannels.ChannelLeaderId);
                }
                newUser.Add(userId);
                if (createChannels.UserID != null) {
                    newUser.AddRange(createChannels.UserID);
                }

                var uniqueUsersId = newUser.Distinct();
                var users = uniqueUsersId.Select(id => new UserChannel
                {
                    ChannelId = channels.ChannelId,
                    UserId = id
                }).ToList();
                await _channelRepo.AddUserToChanel(users);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTranctionAsync();
            }
            catch {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
        }
        public async Task<List<ViewChannelsDTO>> GetChannlesByTask(Guid TaskId) {
            var channels = await _channelRepo.GetChannelsByTaskId(TaskId) ?? throw new KeyNotFoundException("There is no Channel for this task");
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
            var channels = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("Channel not found");
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
            var channel = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("Channel not found");
            await _channelRepo.RemoveChannle(channel);
        }
        public async Task UpdateChannels(CreateChannelsDTO updateChannels, Guid ChannelId) {
            await _unitOfWork.BeginTranctionAsync();
            try {
                var channels = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("Channel not found");
                channels.ChannelName = updateChannels.ChannelName;
                if (updateChannels.ChannelLeaderId != null && updateChannels.ChannelLeaderId != channels.ChannelLeaderId)
                {
                    channels.ChannelLeaderId = updateChannels.ChannelLeaderId;
                    var channelsUser = channels.UserChannels.FirstOrDefault(u => u.UserId == updateChannels.ChannelLeaderId);
                    channelsUser.UserId = updateChannels.ChannelLeaderId;
                    await _unitOfWork.SaveChangesAsync();
                }
                if (updateChannels.UserID != null)
                {
                    var currentUser = channels.UserChannels.Select(u => u.UserId).ToList();
                    var newEnty = updateChannels.UserID.ToHashSet();
                    var userToRemove = currentUser.ToHashSet();
                    newEnty.ExceptWith(userToRemove);
                    userToRemove.ExceptWith(newEnty);
                    var userToRemoveList = userToRemove.Select(u => new UserChannel
                    {
                        ChannelId = channels.ChannelId,
                        UserId = u
                    }).ToList();
                    await _channelRepo.RemoveUserFromChanel(userToRemoveList);
                    var userToAdd = newEnty.Select(u => new UserChannel
                    {
                        ChannelId = channels.ChannelId,
                        UserId = u
                    }).ToList();
                    await _channelRepo.AddUserToChanel(userToAdd);
                    await _unitOfWork.SaveChangesAsync();
                }
                await _unitOfWork.CommitTranctionAsync();
            } catch {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
            
        }
    }
}