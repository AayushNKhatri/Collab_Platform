using AutoMapper;
using Collab_Platform.ApplicationLayer.DTO.ChannelsDto;
using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Helper
{
    public class MapperConfig : Profile
    {
        public MapperConfig() {
            //Channel DTO
            CreateMap<UserChannel, ChannelUser>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Channel, ViewChannelsDTO>()
                .ForMember(des => des.TaskName, opt => opt.MapFrom(src => src.Task.TaskName))
                .ForMember(des => des.ChannelLeaderName, opt => opt.MapFrom(src => src.ChannelLeader.UserName))
                .ForMember(des => des.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName))
                .ForMember(des => des.User, opt => opt.MapFrom(src => src.UserChannels));

            //Project DTO
            CreateMap<UserProject, UserProjectDetailsDto>()
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<ProjectModel, ProjectDetailDto>()
                .ForMember(des => des.UserDetails, opt => opt.MapFrom(src => src.UserProjects));
        }
    }
}
