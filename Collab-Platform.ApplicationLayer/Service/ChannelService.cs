using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.DTO.Mapper;
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
            return await _channelRepo.GetChannelsByTaskId(TaskId) ?? throw new KeyNotFoundException("There is no Channel for this task");
        }
        public async Task<ViewChannelsDTO> GetChannelsById(Guid ChannelId) {
            var channels = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("Channel not found");
            var mapData = ChannelMapper.ToChannelDTO(channels);
            return mapData;
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
                    var userToAdd = newEnty.Distinct().Select(u => new UserChannel
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

        public async Task AddUserToChannel(Guid ChannelId, List<string>UserId) {
            var channel = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("Channel not found");
            var currentChannelUser = channel.UserChannels.Select(u => u.UserId).ToHashSet();
            var newChannelUser = UserId.Where(u => !currentChannelUser.Contains(u)).Select(u => new UserChannel
            {
                UserId = u,
                ChannelId = channel.ChannelId
            }).ToList();
            await _channelRepo.AddUserToChanel(newChannelUser);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task EditUserToChannel(Guid ChannelId, List<string> UserId) {
            var channel = await _channelRepo.GetChannelByID(ChannelId) ?? throw new KeyNotFoundException("This channel does not exsit");
        }
        
    }
}