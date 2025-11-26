using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IPermissionRepo
    {
        Task<List<Permission>> GetAllPermission();
    }
}