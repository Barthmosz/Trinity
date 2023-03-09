using System.Threading.Tasks;

namespace Trinity.Persistence.Contracts
{
  public interface IBasePersistence<D> : IStaticPersistence<D> where D : class
  {
    Task<bool> Add(D entity);
    Task<bool> Update(D entity);
    Task<bool> Delete(string id);
  }
}
