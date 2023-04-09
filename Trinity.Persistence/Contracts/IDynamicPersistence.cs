using System.Threading.Tasks;

namespace Trinity.Persistence.Contracts
{
    public interface IDynamicPersistence<D> where D : class
    {
        Task<bool> AddAsync(D entity);
        Task<bool> UpdateAsync(D entity);
        Task<bool> DeleteAsync(string id);
    }
}
