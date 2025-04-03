

namespace Entity.Entities
{
    public class TotalMining
    {
        public int Id { get; set; }
        public decimal CurrentTotal { get; set; }
        public decimal TotalPeek { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
