using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public interface IUnitOfWorkRepositories : IRepositories
    {
        Task<int> SaveChangesAsync();
    }
}