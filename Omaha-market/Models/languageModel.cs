using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class languageModel
    {
        [Key]
        public int id { get; set; }
        public string Key { get; set; }

        public string ValueRu { get; set; }

        public string ValueRo { get; set; }
    }
}
