using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class EmailModel
    {
        [Key]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
