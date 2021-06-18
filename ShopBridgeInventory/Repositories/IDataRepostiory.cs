namespace ShopBridgeInventory.Repositories
{
    using ShopBridgeInventory.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataRepostiory<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(long id);
        Task<long> Add(TEntity entity);
        Task Update(TEntity entity);
        Task<int> Delete(TEntity entity);
    }
}
