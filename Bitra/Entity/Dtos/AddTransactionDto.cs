using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos
{
    public class AddTransactionDto
    {
        [Required(ErrorMessage = "Sender is required!")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "Receiver is required!")]
        public string Receiver { get; set; }

        [Range(0.00001,100000000, ErrorMessage = "Min:0.00001 - Max:100000000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Sign is required!")]
        public string SignKey { get; set; }
    }
}

