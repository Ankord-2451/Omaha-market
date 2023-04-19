using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class RecentPurchasesModel
    {
        [Key]
        [AutoIncrement]
        public int ID { get; set; }
        [Required]
        public int IdOfProduct { get; set; }

        [Required]
        public int IdOfCustomer { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime date { get; set; }

    }
}
