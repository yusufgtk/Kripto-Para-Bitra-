
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitiy.Entites
{
    public class Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id artık elle atanacak
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }

        //navigation prop
        public ICollection<Transaction> Transactions { get; set; }
    }
}
