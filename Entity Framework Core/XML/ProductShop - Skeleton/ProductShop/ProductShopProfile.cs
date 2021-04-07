using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<CategoriesDTO, Category>();

            this.CreateMap<UsersDTO, User>();

            this.CreateMap<ProductsDTO, Product>()
                .ForMember(x => x.BuyerId, y => y.MapFrom(s => s.BuyerId.HasValue ? s.BuyerId : null));

            this.CreateMap<CategoriesProductsDTO, CategoryProduct>();

            this.CreateMap<Product, ProductsInRangeDTO>()
                .ForMember(x => x.Buyer, y => y.MapFrom(s => s.Buyer.FirstName + " " + s.Buyer.LastName));

            this.CreateMap<Product, GetSoldProductsProduct>();

            this.CreateMap<User, GetSoldProductsUser>()
                .ForMember(x => x.Products, y => y.MapFrom(s => s.ProductsSold));

            this.CreateMap<Category, CategoriesByCountDTO>()
                .ForMember(x => x.ProductsCount, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Select(z => z.Product.Price).Average()))
                .ForMember(x => x.TotalPrice, y => y.MapFrom(s => s.CategoryProducts.Select(z => z.Product.Price).Sum()));




            this.CreateMap<Product, ProductDTO>();

            this.CreateMap<User, ProductInfoDTO>()
                .ForMember(x => x.Count, y => y.MapFrom(s => s.ProductsSold.Count))
                .ForMember(x => x.Products, y => y.MapFrom(s => s.ProductsSold.OrderByDescending(z => z.Price)));

            this.CreateMap<User, UserDTO>()
                .ForMember(x => x.ProductsSold, y => y.MapFrom(s => s));

                

        }
    }
}
