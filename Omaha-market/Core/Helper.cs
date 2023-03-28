using Omaha_market.Data;
using Omaha_market.Models;

namespace Omaha_market.Core
{
    public static class  Helper
    {
        public static List<ProductModel> PageSplitHelper(List<ProductModel> products,int Page)
        {
            const int PageSize = 12;
            return products.OrderBy(x => x.Id).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
        }

        public static List<ProductModel> TakeProductsInCart(SessionWorker session,AppDbContext db)
        {
            List<ProductModel> products = new List<ProductModel>();

                List<CartModel> IdsOfProducts = db.ShoppingCart.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
                foreach(CartModel cart in IdsOfProducts)
                {
                    products.Add(  (ProductModel)db.Products.Where(x=> x.Id==cart.IdOfProduct)  );
                }
                return products;
        }

        public static List<ProductModel> TakeFavoriteProducts(SessionWorker session, AppDbContext db)
        {
            List<ProductModel> products = new List<ProductModel>();

            List<favoriteModel> IdsOfProducts = db.favorite.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
            foreach (favoriteModel cart in IdsOfProducts)
            {
                products.Add((ProductModel)db.Products.Where(x => x.Id == cart.IdOfProduct));
            }
            return products;
        }
    }
}
