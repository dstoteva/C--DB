namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        private static bool isValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validator, validationResult, validateAllProperties: true);
        }
        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var movies = JsonConvert.DeserializeObject<Movie[]>(jsonString);

            foreach (var m in movies)
            {
                if (isValid(m) && !context.Movies.Any(x => x.Title == m.Title))
                {
                    context.Movies.Add(m);
                    context.SaveChanges();
                    sb.AppendLine(String.Format(SuccessfulImportMovie, m.Title, m.Genre, m.Rating.ToString("f2")));
                }
                else 
                { 
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString().Trim();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var halls = JsonConvert.DeserializeObject<HallDTO[]>(jsonString);

            foreach (var hall in halls)
            {
                if (isValid(hall) && hall.Seats > 0)
                {
                    var h = new Hall
                    {
                        Name = hall.Name,
                        Is3D = hall.Is3D,
                        Is4Dx = hall.Is4Dx
                    };

                    context.Halls.Add(h);
                    for (int i = 0; i < hall.Seats; i++)
                    {
                        var seat = new Seat()
                        {
                            Hall = h
                        };
                        context.Seats.Add(seat);
                    }
                    context.SaveChanges();
                    string projectionType = "";
                    if (hall.Is4Dx)
                    {
                        if (hall.Is3D)
                        {
                            projectionType = "4Dx/3D";
                        }
                        else
                        {
                            projectionType = "4Dx";
                        }
                    }
                    else if(hall.Is3D)
                    {
                        projectionType = "3D";
                    }
                    else
                    {
                        projectionType = "Normal";
                    }
                    sb.AppendLine(String.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString().Trim();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ProjectionDTO[]), new XmlRootAttribute("Projections"));

            ProjectionDTO[] projections;
            using (var reader = new StringReader(xmlString))
            {
                projections = (ProjectionDTO[])serializer.Deserialize(reader);
            }

            foreach (var projection in projections)
            {
                if (isValid(projection) && context.Halls.Find(projection.HallId) != null && context.Movies.Find(projection.MovieId) != null)
                {
                    var p = new Projection
                    {
                        MovieId = projection.MovieId,
                        HallId = projection.HallId,
                        DateTime = DateTime.Parse(projection.DateTime, CultureInfo.InvariantCulture)
                    };
                    context.Projections.Add(p);
                    context.SaveChanges();

                    sb.AppendLine(string.Format(SuccessfulImportProjection, context.Movies.Find(p.MovieId).Title, p.DateTime.ToString("MM/dd/yyyy")));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(CustomerDTO[]), new XmlRootAttribute("Customers"));

            CustomerDTO[] customers;
            using (var reader = new StringReader(xmlString))
            {
                customers = (CustomerDTO[])serializer.Deserialize(reader);
            }

            foreach (var c in customers)
            {
                if (isValid(c))
                {
                    var customer = new Customer()
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Age = c.Age,
                        Balance = c.Balance
                    };
                    context.Customers.Add(customer);
                    foreach (var t in c.Tickets)
                    {
                        var ticket = new Ticket()
                        {
                            Customer = customer,
                            ProjectionId = t.ProjectionId,
                            Price = t.Price

                        };
                        context.Tickets.Add(ticket);
                    }
                    context.SaveChanges();

                    sb.AppendLine(String.Format(SuccessfulImportCustomerTicket, c.FirstName, c.LastName, c.Tickets.Length));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString().Trim();
        }
    }
}