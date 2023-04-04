using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trinity.Persistence.Contracts
{
  public interface IStaticPersistence<D> where D : class
  {
    Task<IEnumerable<D>> GetAllAsync();
    Task<D?> GetByIdAsync(string id);
        Task<D?> GetByEmailAsync(string email);
  }
}
