using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestChatServer.Data.Repositories
{
    public interface IRepository<T> where T : Entity.Entity, new()
    {
        bool AutoSaveChanges { get; set; }

        IQueryable<T> Items { get; }

        T Get(long id);
        Task<T> GetAsync(long id, CancellationToken cancel = default);

        T Add(T item);
        Task<T> AddAsync(T item, CancellationToken cancel = default);

        void Update(T item);
        Task UpdateAsync(T item, CancellationToken cancel = default);

        T Remove(long id);
        Task<T> RemoveAsync(long id, CancellationToken cancel = default);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
