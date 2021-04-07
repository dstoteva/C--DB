namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies.Where(x => x.Rating >= rating && x.Projections.Any(y => y.Tickets.Count > 0))
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.Projections.SelectMany(y => y.Tickets).Select(y => y.Price).Sum())
                .Take(10)
                .Select(x => new
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("F2"),
                    TotalIncomes = x.Projections.SelectMany(y => y.Tickets).Select(y => y.Price).Sum().ToString("F2"),
                    Customers = x.Projections.SelectMany(y => y.Tickets)
                    .Select(y => new
                    {
                        y.Customer.FirstName,
                        y.Customer.LastName,
                        Balance = y.Customer.Balance.ToString("F2")
                    })
                    .OrderByDescending(y => y.Balance)
                    .ThenBy(y => y.FirstName)
                    .ThenBy(y => y.LastName)
                    .ToList()
                })
                .ToList();

            
            return JsonConvert.SerializeObject(movies, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var sb = new StringBuilder();
            var customers = context.Customers.Where(x => x.Age >= age)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpetMoney = x.Tickets.Sum(y => y.Price),
                    SpentTime = new TimeSpan(x.Tickets.Sum(y => y.Projection.Movie.Duration.Ticks)).ToString(@"hh\:mm\:ss")
                })
                .OrderByDescending(x => x.SpetMoney)
                .Take(10)
                .ToArray();

            var result = customers.Select(x => new CustomerExportTO
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                SpetMoney = x.SpetMoney.ToString("F2"),
                SpentTime = x.SpentTime
            })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(CustomerExportTO[]), new XmlRootAttribute("Customers"));

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, result, namespaces);
            }

            return sb.ToString().Trim();
        }
    }
}