using AutoMapper;
using CarDealer.Data;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Xml.Serialization;
using AutoMapper.QueryableExtensions;
using CarDealer.Dtos.Export;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?><sales><sale><car make=\"BMW\" model=\"M5 F10\" travelled-distance=\"435603343\" /><discount>30</discount><customer-name>Hipolito Lamoreaux</customer-name><price>139.97</price><price-with-discount>97.979</price-with-discount></sale><sale><car make=\"Opel\" model=\"Astra\" travelled-distance=\"31468479\" /><discount>0</discount><customer-name>Garret Capron</customer-name><price>253.97</price><price-with-discount>253.97</price-with-discount></sale><sale><car make=\"Opel\" model=\"Insignia\" travelled-distance=\"339785118\" /><discount>10</discount><customer-name>Emmitt Benally</customer-name><price>2417.63</price><price-with-discount>2175.867</price-with-discount></sale><sale><car make=\"BMW\" model=\"F25\" travelled-distance=\"476132712\" /><discount>0</discount><customer-name>Sylvie Mcelravy</customer-name><price>3567.74</price><price-with-discount>3567.74</price-with-discount></sale><sale><car make=\"BMW\" model=\"F04\" travelled-distance=\"443756363\" /><discount>50</discount><customer-name>Marcelle Griego</customer-name><price>117.97</price><price-with-discount>58.985</price-with-discount></sale><sale><car make=\"Opel\" model=\"Vectra\" travelled-distance=\"238042093\" /><discount>30</discount><customer-name>Cinthia Lasala</customer-name><price>465.94</price><price-with-discount>326.158</price-with-discount></sale><sale><car make=\"Opel\" model=\"Insignia\" travelled-distance=\"225253817\" /><discount>15</discount><customer-name>Donnetta Soliz</customer-name><price>2940.09</price><price-with-discount>2499.0765</price-with-discount></sale><sale><car make=\"Opel\" model=\"Omega\" travelled-distance=\"277250812\" /><discount>20</discount><customer-name>Teddy Hobby</customer-name><price>4020.62</price><price-with-discount>3216.496</price-with-discount></sale><sale><car make=\"BMW\" model=\"E67\" travelled-distance=\"476830509\" /><discount>15</discount><customer-name>Johnette Derryberry</customer-name><price>1106.84</price><price-with-discount>940.814</price-with-discount></sale></sales>";
            //
            //Console.WriteLine(expected.Substring(1495));
            Mapper.Initialize(cfg => cfg.AddProfile(new CarDealerProfile()));
            using (var db = new CarDealerContext())
            {
                Console.WriteLine(GetSalesWithAppliedDiscount(db));
            }
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(x => x.Sales.Count > 0)
                .ProjectTo<CustomerDTO>()
                .OrderByDescending(x => x.Price)
                .ToArray();

            var sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDTO[]), new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().Trim();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .ProjectTo<SaleDTO>()
                .ToArray();


            var sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(SaleDTO[]), new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, sales, namespaces);
            }

            return sb.ToString().Trim();
        }
    }
}