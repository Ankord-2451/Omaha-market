using Microsoft.EntityFrameworkCore;
using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Omaha_market.Models
{
    public enum role
    {
        Admin,
        customer
    }

    public class AccountModel
    {
        [Key]
        [AutoIncrement]
        public int ID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Name { set; get; }

        [Required]
        public role Role { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [NotMapped]
        public ListOfProductModel ShoppingCart { get; set; }

        [NotMapped]
        public ListOfProductModel Favorite { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }
    }
}
