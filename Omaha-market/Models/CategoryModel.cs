using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class CategoryModel
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string NameRu { get; set; }

        [Required]
        public string NameRo { get; set; }
    }
}
