

using Entitiy.Entites;
using Repository.Abstracts;
using Repository.Contexts;

namespace Repository.Concretes
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context) { }
    }

}
