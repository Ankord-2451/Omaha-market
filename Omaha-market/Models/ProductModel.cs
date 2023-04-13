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
        public string NameRu { get; set; }

        [Required]
        public string NameRo { get; set; }

        public string Img { get; set; }
        [Required]
        public string DescriptionRu { get; set; }

        [Required]
        public string DescriptionRo { get; set; }

        [Required]
        public string CategoryRu { get; set; }

        [Required]
        public string CategoryRo { get; set; }

        [Required]
        //[Range(1, 100000)]
        public double Price { get; set; }

        public bool OnDiscount { get; set; }

        public int Amount { get; set; }

        public DateTime DateOfLastChange { get; set; }
    }
}
