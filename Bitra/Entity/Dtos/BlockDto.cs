using System.Text.Json.Serialization;

namespace Entity.Dtos
{
    public class BlockDto
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }

        //navigation prop
        [JsonIgnore]
        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
