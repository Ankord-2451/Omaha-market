using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class ProductModel
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Img { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }

        [Required]
        public double price { get; set; }

        public bool OnDiscount { get; set; }

        public int amount{ get; set; }
    }
}
