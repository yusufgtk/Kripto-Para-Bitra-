

using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos
{
    public class GenerateWalletDto
    {
        [Required(ErrorMessage = "Private key is required!")]
        public string PrivateKey { get; set; }
    }
}
