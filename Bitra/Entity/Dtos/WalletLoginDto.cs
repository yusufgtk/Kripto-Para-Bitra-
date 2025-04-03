

using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos
{
    public class WalletLoginDto
    {
        [Required(ErrorMessage = "Cüzdan addresinizi giriniz!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cüzdan şifrenizi giriniz!")]
        public string PrivateKey { get; set; }
    }
}
