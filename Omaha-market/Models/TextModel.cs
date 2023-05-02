using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Omaha_market.Models
{
    public class TextModel
    {
        [Key]
        [AutoIncrement]
        public int ID { get; set; }

        public string SomeRu { set; get; }

        public string BannerRu { set; get; }

        public string SomeRo { set; get; }

        public string BannerRo { set; get; }
    }
}
