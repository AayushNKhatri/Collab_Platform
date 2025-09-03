using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IUnitOfWork
    {
        Task BeginTranctionAsync();
        Task CommitTranctionAsync();
        Task RollBackTranctionAsync();
        Task<int> SaveChangesAsync();
    }
}
