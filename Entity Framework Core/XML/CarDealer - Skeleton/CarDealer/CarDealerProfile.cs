using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Models;
using System;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<Customer, CustomerDTO>()
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Sales.Select(t => t.Car.PartCars.Select(z => z.Part.Price).Sum()).Sum()));

            this.CreateMap<Car, CarDTO>();

            this.CreateMap<Sale, SaleDTO>()
                .ForMember(x => x.Customer, y => y.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.Price, y => y.MapFrom(s => (double)s.Car.PartCars.Sum(t => t.Part.Price)))
                .ForMember(x => x.PriceWithDiscount, y => y.MapFrom(s => (double)((1.00m - (s.Discount * 0.01m)) * s.Car.PartCars.Sum(t => t.Part.Price))));
        }
    }
}
