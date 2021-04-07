using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Users><count>54</count><users><User><firstName>Cathee</firstName><lastName>Rallings</lastName><age>33</age><SoldProducts><count>9</count><products><Product><name>Fair Foundation SPF 15</name><price>1394.24</price></Product><Product><name>Finasteride</name><price>1374.01</price></Product><Product><name>EMEND</name><price>1365.51</price></Product><Product><name>IOPE RETIGEN MOISTURE TWIN CAKE NO.21</name><price>1257.71</price></Product><Product><name>ESIKA</name><price>879.37</price></Product><Product><name>GOONG SECRET CALMING BATH</name><price>742.47</price></Product><Product><name>allergy eye</name><price>426.91</price></Product><Product><name>Allergena</name><price>109.32</price></Product><Product><name>Glyburide</name><price>95.1</price></Product></products></SoldProducts></User><User><firstName>Margi</firstName><lastName>Ellerton</lastName><age>23</age><SoldProducts><count>7</count><products><Product><name>Glycopyrrolate</name><price>1471.43</price></Product><Product><name>Amitriptyline Hydrochloride</name><price>1453.96</price></Product><Product><name>Topical 60 Sec Sodium Fluoride</name><price>1228.84</price></Product><Product><name>Baza Antifungal</name><price>1162.34</price></Product><Product><name>GProducting Pains</name><price>1077.37</price></Product><Product><name>TYLENOL COLD MULTI-SYMPTOM DAYTIME</name><price>1010.81</price></Product><Product><name>Foaming Hand Sanitizer</name><price>624.72</price></Product></products></SoldProducts></User><User><firstName>Purcell</firstName><lastName>Prewett</lastName><age>47</age><SoldProducts><count>6</count><products><Product><name>cough and sore throat</name><price>1482.68</price></Product><Product><name>Pollens - Trees, Birch Mix</name><price>1153.54</price></Product><Product><name>PREMIER VALUE ALLERGY</name><price>1127.61</price></Product><Product><name>smart sense nighttime cold and flu relief</name><price>1101.77</price></Product><Product><name>Prostate</name><price>716.05</price></Product><Product><name>PRIMAXIN</name><price>686.66</price></Product></products></SoldProducts></User><User><firstName>Etta</firstName><lastName>Arnaudi</lastName><age>32</age><SoldProducts><count>6</count><products><Product><name>Flumazenil</name><price>1151.37</price></Product><Product><name>Ringers</name><price>1054.37</price></Product><Product><name>pain relief</name><price>938.23</price></Product><Product><name>Ketorolac Tromethamine</name><price>608.18</price></Product><Product><name>Agaricus Equisetum Special Order</name><price>585.93</price></Product><Product><name>Yellow Jacket hymenoptera venom Venomil Diagnostic</name><price>23.58</price></Product></products></SoldProducts></User><User><firstName>Duky</firstName><lastName>Bowller</lastName><age>30</age><SoldProducts><count>6</count><products><Product><name>Propranolol Hydrochloride</name><price>546.95</price></Product><Product><name>Extra Strength Pain Reliever PM</name><price>542.72</price></Product><Product><name>Peter Island Continous sunscreen kids</name><price>471.3</price></Product><Product><name>CARBIDOPA AND LEVODOPA</name><price>441.64</price></Product><Product><name>Fluoxetine</name><price>385.37</price></Product><Product><name>CEDAX</name><price>342.86</price></Product></products></SoldProducts></User><User><firstName>Brig</firstName><lastName>Mullineux</lastName><age>39</age><SoldProducts><count>6</count><products><Product><name>Nevirapine</name><price>1374.72</price></Product><Product><name>NEO-POLY-BAC HYDRO</name><price>967.32</price></Product><Product><name>Homeopathic Rheumatism</name><price>967.08</price></Product><Product><name>ziprasidone hydrochloride</name><price>628.66</price></Product><Product><name>Labetalol hydrochloride</name><price>436.38</price></Product><Product><name>kirkland signature minoxidil</name><price>49.17</price></Product></products></SoldProducts></User><User><firstName>Bernadette</firstName><lastName>Ensor</lastName><age>25</age><SoldProducts><count>6</count><products><Product><name>Alcohol Free Antiseptic</name><price>1486.07</price></Product><Product><name>Warfarin Sodium</name><price>1379.79</price></Product><Product><name>CLARINS Ever Matte SPF 15 - 105 Nude</name><price>696.06</price></Product><Product><name>Topiramate</name><price>578.77</price></Product><Product><name>Lamotrigine Extended Release</name><price>245.63</price></Product><Product><name>ERYTHROMYCIN Base Filmtab</name><price>117.84</price></Product></products></SoldProducts></User><User><firstName>Osborn</firstName><lastName>McGettigan</lastName><age>28</age><SoldProducts><count>6</count><products><Product><name>Effervescent Cold Relief</name><price>1436.07</price></Product><Product><name>Gemcitabine</name><price>594.79</price></Product><Product><name>Gehwol med Lipidro</name><price>421.24</price></Product><Product><name>Fexofenadine HCl and Pseudoephedrine HCI</name><price>73.07</price></Product><Product><name>ORCHID SECRET PACT</name><price>59.53</price></Product><Product><name>REYATAZ</name><price>41.97</price></Product></products></SoldProducts></User><User><firstName>Chrissy</firstName><lastName>Falconbridge</lastName><age>50</age><SoldProducts><count>5</count><products><Product><name>Topex</name><price>1258.49</price></Product><Product><name>Retin-A MICRO</name><price>995.98</price></Product><Product><name>Aspirin</name><price>925.45</price></Product><Product><name>Meloxicam</name><price>809.18</price></Product><Product><name>Goats Milk</name><price>298.53</price></Product></products></SoldProducts></User><User><firstName>Vivie</firstName><lastName>Tyrwhitt</lastName><age>29</age><SoldProducts><count>5</count><products><Product><name>Etodolac</name><price>1443.13</price></Product><Product><name>Wintergreen Isopropyl Alcohol</name><price>1397.57</price></Product><Product><name>VITALUMIERE AQUA</name><price>1293.09</price></Product><Product><name>Ondansetron</name><price>1249.76</price></Product><Product><name>Aspen</name><price>1046.46</price></Product></products></SoldProducts></User></users></Users>";
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());
            using (var db = new ProductShopContext())
            {
                var actual = GetUsersWithProducts(db);
                Console.WriteLine(actual);

                //Console.WriteLine(actual.Length + " " + expected.Length);
                
            }
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UsersDTO[]), new XmlRootAttribute("Users"));

            UsersDTO[] users;
            using (var reader = new StringReader(inputXml))
            {
                users = (UsersDTO[])xmlSerializer.Deserialize(reader);
            }

            var result = Mapper.Map<User[]>(users);

            context.Users.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {result.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerialier = new XmlSerializer(typeof(ProductsDTO[]), new XmlRootAttribute("Products"));

            ProductsDTO[] products;
            using (var reader = new StringReader(inputXml))
            {
                products = (ProductsDTO[])xmlSerialier.Deserialize(reader);
            }

            var result = Mapper.Map<Product[]>(products);

            context.Products.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";

        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoriesDTO[]), new XmlRootAttribute("Categories"));

            CategoriesDTO[] categoryDTOS;
            using (var reader = new StringReader(inputXml))
            {
                categoryDTOS = (CategoriesDTO[])xmlSerializer.Deserialize(reader);
            }

            var categories = Mapper.Map<Category[]>(categoryDTOS).Where(x => x.Name != null).ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";

        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoriesProductsDTO[]), new XmlRootAttribute("CategoryProducts"));

            CategoriesProductsDTO[] categoriesproducts;
            using (var reader = new StringReader(inputXml))
            {
                categoriesproducts = ((CategoriesProductsDTO[])xmlSerializer.Deserialize(reader))
                    .Where(x => x.CategoryId != null && x.ProductId != null).ToArray();
            }

            var result = Mapper.Map<CategoryProduct[]>(categoriesproducts);

            context.CategoryProducts.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {result.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductsInRangeDTO>()
                .ToArray();

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var serializer = new XmlSerializer(typeof(ProductsInRangeDTO[]), new XmlRootAttribute("Products"));

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, products, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(x => x.ProductsSold.Count > 0)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    products = x.ProductsSold.Select(y => new
                    {
                        y.Name,
                        y.Price
                    })
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ProjectTo<GetSoldProductsUser>()
                .ToArray();

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var serializer = new XmlSerializer(typeof(GetSoldProductsUser[]), new XmlRootAttribute("Users"));

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, products, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AveragePrice = x.CategoryProducts.Select(y => y.Product.Price).Average(),
                    TotalPrice = x.CategoryProducts.Select(y => y.Product.Price).Sum()
                })
                .OrderByDescending(x => x.ProductsCount)
                .ThenBy(x => x.TotalPrice)
                .ProjectTo<CategoriesByCountDTO>()
                .ToArray();

            var sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesByCountDTO[]), new XmlRootAttribute("Categories"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, categories, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            // var users = context.Users.Where(x => x.ProductsSold.Any())
            //     .OrderByDescending(x => x.ProductsSold.Count)
            //     .Take(10)
            //     .Select(x => new 
            //     {
            //         x.FirstName,
            //         x.LastName,
            //         x.Age,
            //         ProductsSold = new
            //         {
            //             x.ProductsSold.Count,
            //             Products = x.ProductsSold.Select(z => new
            //             {
            //                 z.Name,
            //                 z.Price
            //             })
            //             .OrderByDescending(z => z.Price)
            //             .ToArray()
            //         }
            //     })
            //     .ProjectTo<UserDTO>()
            //     .ToArray();

            var users = context.Users.Where(x => x.ProductsSold.Any())
                .OrderByDescending(x => x.ProductsSold.Count)
                .Take(10)
                .ProjectTo<UserDTO>()
                .ToArray();

            var result = new UserInfoDTO
            {
                Count = context.Users.Where(x => x.ProductsSold.Any()).Count(),
                Users = users
            };

            var sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(UserInfoDTO), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, result, namespaces);
            }

            return sb.ToString();
        }
    }
}