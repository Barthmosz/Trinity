using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trinity.Persistence.Contracts
{
  public interface IStaticPersistence<D> where D : class
  {
    Task<IEnumerable<D>> GetAllAsync();
  }
}
