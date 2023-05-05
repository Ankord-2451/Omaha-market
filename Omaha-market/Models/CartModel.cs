using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdOfProduct { get; set; }

        [Required]
        public int IdOfCustomer { get; set; }

        public int Quantity { get; set; }

    }
}
