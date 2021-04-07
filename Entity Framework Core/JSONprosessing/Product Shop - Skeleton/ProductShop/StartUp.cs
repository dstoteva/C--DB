using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new ProductShopContext())
            {
                //var inputJSON = File.ReadAllText("./../../../Datasets/categories-products.json");
               
                Console.WriteLine(GetUsersWithProducts(db));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}"; 
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson).Where(x => x.Name != null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products.Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var result = context.Users
                .Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(y => y.BuyerId.HasValue))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Where(y => y.BuyerId.HasValue)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var result = context.Categories
                .Select(p => new
                {
                    category = p.Name,
                    productsCount = p.CategoryProducts.Count,
                    averagePrice = p.CategoryProducts.Average(x => x.Product.Price).ToString("f2"),
                    totalRevenue = p.CategoryProducts.Sum(x => x.Product.Price).ToString("f2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(z => z.BuyerId.HasValue).Count(),
                        products = u.ProductsSold
                        .Where(z => z.BuyerId.HasValue)
                        .Select(y => new
                        {
                            name = y.Name,
                            price = y.Price
                        })
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var json = JsonConvert.SerializeObject(new { usersCount = users.Count, users = users }, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }
    }
}