using AutoMapper;
using Cinema.Data.Models;
using Cinema.DataProcessor.ExportDto;
using Cinema.DataProcessor.ImportDto;
using System;
using System.Linq;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        public CinemaProfile()
        {
            this.CreateMap<ProjectionDTO, Projection>();
            this.CreateMap<Customer, CustomerExportTO>()
                .ForMember(x => x.SpetMoney, y => y.MapFrom(s => Math.Round(s.Tickets.Select(t => t.Price).Sum(), 2)))
                .ForMember(x => x.SpentTime, y => y.MapFrom(s => new TimeSpan(s.Tickets.GroupBy(n => n.Projection).Select(gr => gr.First()).Sum(g => (long)g.Projection.Movie.Duration.TotalMinutes)).ToString(@"hh\:mm\:ss")));
        }
    }
}
