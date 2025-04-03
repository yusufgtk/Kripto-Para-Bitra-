

namespace Repository.Abstracts
{
    public interface IRepositoryBase<T>
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<bool> SaveChangesAsync();
    }
}
