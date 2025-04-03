

using Entitiy.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.Abstracts;
using Repository.Contexts;

namespace Repository.Concretes
{
    public class BlockRepository : RepositoryBase<Block>, IBlockRepository
    {
        public BlockRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Block>> GetChainAsync()
        {
            return await _context.Blocks.AsNoTracking().ToListAsync();
        }

        public async Task<Block?> GetLatestBlockAsync()
        {
            return await _context.Blocks.AsNoTracking().OrderByDescending(b => b.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsChainValidAsync()
        {
            var invalidBlockCount = await _context.Database
        .ExecuteSqlRawAsync(@"
            DECLARE @InvalidCount INT = 0;
            
            -- Hash doğrulama - C# tarafıyla aynı formatı kullanarak
            SELECT @InvalidCount = COUNT(*) 
            FROM Blocks b1
            WHERE b1.Hash != CONVERT(NVARCHAR(64), 
                HASHBYTES('SHA2_256', 
                    CAST(b1.Id AS NVARCHAR(20)) + 
                    CONVERT(NVARCHAR(50), b1.Timestamp, 126) + 
                    b1.PreviousHash + 
                    CAST(b1.Nonce AS NVARCHAR(20))
                ), 2);
            
            -- Zincir bağlantı doğrulama (previousHash kontrolü)
            IF @InvalidCount = 0
            BEGIN
                SELECT @InvalidCount = COUNT(*) 
                FROM Blocks b1
                LEFT JOIN Blocks b2 ON b1.Id = b2.Id + 1
                WHERE (b1.Id > 1 AND b2.Hash IS NULL) -- Eksik önceki blok kontrolü
                OR (b2.Hash IS NOT NULL AND b1.PreviousHash != b2.Hash) -- Önceki bloğun hash değeri kontrolü
                OR (b1.Id = 1 AND b1.PreviousHash != '0'); -- Genesis blok kontrolü
            END
            
            SELECT @InvalidCount;");

            return invalidBlockCount == 0;
        }
    }
}
