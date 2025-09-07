using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IProjectInterface
    {
        Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto); 
    }
}
