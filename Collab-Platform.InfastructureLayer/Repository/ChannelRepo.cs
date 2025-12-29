using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class ChannelRepo : IChannelRepo
    {
        private readonly ApplicationDbContext _db;

        public ChannelRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddChannel(Channel channel)
        {
            await _db.AddAsync(channel); 
        }
        public async Task RemoveChannle(Channel channel)
        {
            _db.Remove(channel);
        }
        public async Task<List<Channel>> GetChannelsByTaskId(Guid TaskId)
        {
            return await _db.Channels
                        .Include(x => x.Creator)
                        .Include(x => x.Task)
                        .Where(x => x.TaskId == TaskId)
                        .ToListAsync();
        }
        public async Task<Channel> GetChannelByID(Guid ChannelId)
        {
            return await _db.Channels
                        .Include(x => x.Creator)
                        .Include(x => x.Task)
                        .FirstOrDefaultAsync(x => x.ChannelId == ChannelId);
        }
        public async Task AddUserToChanel(List<UserChannel> User) {
            await _db.UserChannels.AddRangeAsync(User);
        }
        public async Task RemoveUserFromChanel(List<UserChannel> User) {
            _db.UserChannels.RemoveRange(User);
        }
    }
}