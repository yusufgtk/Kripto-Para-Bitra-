namespace Entity.Dtos
{
    public class WalletDto
    {
        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
        public string Address { get; private set; }
        public decimal Balance { get; private set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
    }
}
