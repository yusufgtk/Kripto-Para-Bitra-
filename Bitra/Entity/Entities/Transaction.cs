
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entitiy.Entites
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id artık elle atanacak
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public string? SignKey { get; set; }
        public string? Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        //navigation prop
        public int BlockId { get; set; }
        public Block Block { get; set; }
    }
}
