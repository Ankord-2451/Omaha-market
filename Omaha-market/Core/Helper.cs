using Omaha_market.Data;
using Omaha_market.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Omaha_market.Core
{
    public static class  Helper
    {
        //Create product section
        public static ProductModel PreparationForSaveProduct(ProductModel product, AppDbContext db, IFormFile photo)
        {
                product.Img = Helper.SaveImg(photo);
               
                product.DateOfLastChange = DateTime.Now;
                product.CategoryRo = db.Category.FirstOrDefault(x => x.NameRu == product.CategoryRu).NameRo;

                return product;
        }
        public static string SaveImg(IFormFile photo)
        { 
                if (photo is null)
                {
                    return "NoImg.png";
                }
                else
                {
                string Path =$"wwwroot\\Images\\{photo.FileName}";;
               
                using (var fileStream = new FileStream(Path, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                    return photo.FileName;
                }
        }

        //Take and split section

        public static List<ProductModel> PageSplitHelper(List<ProductModel> products,int Page, out int amount)
        {
            if(products.Count != 0)
            { 
            const int PageSize = 8;
            amount = products.Count/PageSize;
            if (products.Count > amount * PageSize) amount++;
            return products.OrderBy(x => x.Id).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                amount = 1;
                return null;
            }
        }

        public static List<ProductModel> TakeProductsInCart(SessionWorker session,AppDbContext db)
        {
            List<ProductModel> products = new List<ProductModel>();

                List<CartModel> IdsOfProducts = db.ShoppingCart.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
                foreach(CartModel cart in IdsOfProducts)
                {
                    products.Add(db.Products.First(x=> x.Id==cart.IdOfProduct));
                }
                return products;
        }

        public static List<ProductModel> TakeFavoriteProducts(SessionWorker session, AppDbContext db)
        {
            List<ProductModel> products = new List<ProductModel>();

            List<favoriteModel> IdsOfProducts = db.favorite.Where(x => x.IdOfCustomer == session.GetUserId()).ToList();
            foreach (favoriteModel cart in IdsOfProducts)
            {
                products.Add(db.Products.First(x => x.Id == cart.IdOfProduct));
            }
            return products;
        }

        public static List<ProductModel> TakeProductsOnDiscount(AppDbContext db)
        {
            var products = db.Products.Where(x => x.OnDiscount).OrderBy(x => x.Id).ToList();
                if(products.Count==0)  return null;
                return products;
         
        }

        public static List<ProductModel> TakeNewProducts(AppDbContext db)
        {
            var products = db.Products.Where(x => x.DateOfLastChange >= DateTime.Now.AddDays(-7)).OrderBy(x => x.Id).ToList();
                if (products.Count == 0) return null;
            return products;

        }

        public static List<ProductModel> TakeProductsSome(AppDbContext db)
        {
            var products = db.Products.Where(x => x.FromSome).OrderBy(x => x.Id).ToList();
                if (products.Count == 0) return null;
            return products;

        }

        //Search section
        private static bool Calculate(string source1, string source2) //O(n*m)
        {
            const int AllowableErrorPercentage = 40;

            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return true;

            if (source2Length == 0)
                return true;

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

        private static IEnumerable<ProductModel> FuzzySearch(string request, List<ProductModel> AllProducts)
        {
           if (request is null)
            {
                return null;
            }
            var productsByName = AllProducts.Where(x => Calculate(request, x.NameRu) || Calculate(request, x.NameRo));

            var products =AllProducts.Where(x => x.DescriptionRu.Contains(request)|| x.DescriptionRo.Contains(request));

            return products.Union(productsByName);
        }

        public static async Task<List<ProductModel>> FuzzySearchAsync(string request, List<ProductModel> AllProducts)
        {
            return (await Task.Run(() => FuzzySearch(request, AllProducts))).ToList(); 
        }
    }
}
