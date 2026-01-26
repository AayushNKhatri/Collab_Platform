using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _channelSerivce;
        public ChannelsController(IChannelService channelService)
        {
            _channelSerivce = channelService;
        }
        [HttpPost("Create-Channels/{TaskId}")]
        public async Task<IActionResult> CreateChannels(CreateChannelsDTO createChannelDto, [FromRoute] Guid TaskId) {
            await _channelSerivce.CreateChannel(createChannelDto, TaskId);
            return Ok(new APIResponse {
                Messege = "Channel Sucessfully Created",
                Success = true
            });
        }
        [HttpGet("{ChannelsId}")]
        public async Task<IActionResult> GetChannelsByChannelId([FromRoute] Guid ChannelsId) {
            var channels = await _channelSerivce.GetChannelsById(ChannelsId);
            return Ok(new APIResponse<ViewChannelsDTO> {
                Data = channels,
                Messege = "Channel Fetch sucessfullt",
                Success = true,
            });
        }
        [HttpGet("GetChannelsByTask/{TaskId}")]
        public async Task<IActionResult> GetChannelsByTaskId([FromRoute] Guid TaskId) {
            var channels = await _channelSerivce.GetChannlesByTask(TaskId);
            return Ok(new APIResponse<List<ViewChannelsDTO>> {
                Data = channels,
                Messege = "Channels Sucessfully fetched",
                Success = true
            });
        }
        [HttpDelete("DeleteChannels/{ChannelsId}")]
        public async Task<IActionResult> DeleteChanels([FromRoute] Guid ChannelsId) {
            await _channelSerivce.DeleteChannels(ChannelsId);
            return Ok(new APIResponse {
                Messege = "Channels Sucessfully deleted",
                Success = true,
            });
        }
        [HttpPut("UpdateChannels/{ChannelsId}")]
        public async Task<IActionResult> UpdateChannels([FromRoute] Guid ChannelsId, CreateChannelsDTO updateDTO)
        {
            await _channelSerivce.UpdateChannels(updateDTO, ChannelsId);
            return Ok(new APIResponse { 
                Messege = "Channels Sucessfully Updated",
                Success = true
            });
        }
        [HttpPut("AddUserToChannel/{ChannelsId}")]
        public async Task<IActionResult> AddUserToChannel(List<string> UserID, Guid ChannelsId) {
            await _channelSerivce.AddUserToChannel(ChannelsId, UserID);
            return Ok(new APIResponse { 
                Messege = "User sucessfully added to channel's",
                Success = true,
            });
        }
        //[HttpGet("GetChannelByProject/{ProjectId}")]
        //public async Task<IActionResult> GetProjectByChannels() { 
        //    await _channelSerivce
        //}
    }
}
