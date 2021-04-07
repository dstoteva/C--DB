using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new CarDealerContext())
            {
                Console.WriteLine(GetSalesWithAppliedDiscount(db));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => context.Suppliers.Select(s => s.Id).Contains(p.SupplierId))
                .ToArray();



            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}.";

        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<List<ImportCarDTO>>(inputJson, new JsonSerializerSettings()
            {
            });

            var cars = new List<Car>();
            var carParts = new List<PartCar>();

            foreach (var c in carsDto)
            {

                var car = new Car();
                car.Make = c.make;
                car.Model = c.model;
                car.TravelledDistance = c.travelledDistance;

                foreach (var p in c.partsId.Distinct())
                {
                    var carPart = new PartCar()
                    {
                        PartId = p,
                        Car = car
                    };
                    carParts.Add(carPart);
                }

                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.PartCars.AddRange(carParts);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new 
                {
                    x.Name,
                    x.BirthDate,
                    x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ToList();

            return JsonConvert.SerializeObject(customers, new JsonSerializerSettings() 
            {   
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented, 
                DateFormatString = "dd/MM/yyyy"
            });
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            return JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.Where(x => !x.IsImporter)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToList();

            return JsonConvert.SerializeObject(suppliers, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(x => new
            {
                car = new
                {
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                },
                parts = x.PartCars.Select(y => new
                {
                    y.Part.Name,
                    Price = y.Part.Price.ToString("f2")
                }).ToArray()
            });

            return JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(x => x.Sales.Any())
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count(),
                    spentMoney = x.Sales.Select(y => y.Car.PartCars.Select(z => z.Part.Price).Sum()).Sum()
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            return JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("f2"),
                    price = x.Car.PartCars.Select(y => y.Part.Price).Sum().ToString("f2"),
                    priceWithDiscount = Math.Round((x.Car.PartCars.Select(y => y.Part.Price).Sum())*(1.00m - (x.Discount * 0.01m)), 2).ToString("f2")
                })
                .ToList();

            return JsonConvert.SerializeObject(sales, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }
    }
}