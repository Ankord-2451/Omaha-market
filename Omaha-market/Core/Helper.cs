using Omaha_market.Data;
using Omaha_market.Models;
using PactNet.Core;

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

        public static List<ProductModel> TakeProductsOnDiscount(AppDbContext db)
        {
          return db.Products.Where(x=>x.OnDiscount).OrderBy(x=>x.Id).ToList();
        }

        //Search section
        private static bool Calculate(string source1, string source2) //O(n*m)
        {
            const int AllowableErrorPercentage = 30;

            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return false;

            if (source2Length == 0)
                return false;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return (matrix[source1Length, source2Length]*100/ source2Length) < AllowableErrorPercentage;
        }

        private static List<ProductModel> FuzzySearch(string request, List<ProductModel> AllProducts)
        {
            List<ProductModel> products = new List<ProductModel>();
            for(int i= 0;i<AllProducts.Count();i++)
            {
                products = (List<ProductModel>)AllProducts.Where(x => Calculate(request, x.Name));
            }
            return products;
        }

        //public static Async Task<List<ProductModel>> 
    }
}
