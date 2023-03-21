using Microsoft.EntityFrameworkCore;

namespace Omaha_market.Models
{
    [Keyless]
    public class ListOfProductModel
    {
        public List<int> IdOfProduct { get; set; }
    }
}
