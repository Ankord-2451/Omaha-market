using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class OrderModel
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string? IdOfTransaction { get; set; }

        public string IdAndNameAndQuantityOfProduct { get; set; }
        public double PriceOfOrder { get; set; }

        [Required]
        public string Name { set; get; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

        public string? CommentToOrder { get; set; }
    }
}
