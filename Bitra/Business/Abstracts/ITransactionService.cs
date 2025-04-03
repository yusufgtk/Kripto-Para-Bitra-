

using Entitiy.Entites;

namespace Business.Abstracts
{
    public interface ITransactionService 
    {
        Task AddAsync(Transaction transaction);
        void Delete(Transaction transaction);
        void Update(Transaction transaction);
        Task<bool> SaveChangesAsync();

        string CalculateHash(Transaction transaction); //transaction şifreleme
    }
}
