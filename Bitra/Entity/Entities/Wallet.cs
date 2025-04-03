using System.ComponentModel.DataAnnotations;

namespace Entitiy.Entites
{
    public class Wallet
    {
        [Key]
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
    }
}
