using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.ProjectDto
{
    public class AddUserProjectDto
    {
        public List<string> UserId { get; set; }
        public Guid ProjectId { get; set; }
       
    }
}
