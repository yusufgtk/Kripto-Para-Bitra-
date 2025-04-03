
using System.Text.Json.Serialization;

namespace Entity.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public string? Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
        public string SignKey { get; set; }
        public DateTime Timestamp { get; set; }

        //navigation prop
        public int BlockIndex { get; set; }
        [JsonIgnore]
        public BlockDto Block { get; set; }
    }
}
