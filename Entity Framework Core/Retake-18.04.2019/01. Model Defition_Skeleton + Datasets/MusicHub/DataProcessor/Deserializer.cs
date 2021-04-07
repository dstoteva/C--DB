namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var writers = JsonConvert.DeserializeObject<Writer[]>(jsonString);

            foreach (var writer in writers)
            {
                if (isValid(writer))
                {
                    context.Writers.Add(writer);
                    context.SaveChanges();
                    sb.AppendLine($"Imported {writer.Name}");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            return sb.ToString().Trim();
        }

        private static bool isValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validator, validationResult, validateAllProperties: true);
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var producers = JsonConvert.DeserializeObject<ProducerDTO[]>(jsonString);

            foreach (var prod in producers)
            {
                if (isValid(prod))
                {
                    var p = new Producer()
                    {
                        Name = prod.Name,
                        Pseudonym = prod.Pseudonym,
                        PhoneNumber = prod.PhoneNumber
                    };
                    context.Producers.Add(p);
                    foreach (var a in prod.Albums)
                    {
                        var album = new Album()
                        {
                            Name = a.Name,
                            ReleaseDate = DateTime.ParseExact(a.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Producer = p
                        };
                        context.Albums.Add(album);
                    }
                    context.SaveChanges();

                    if (p.PhoneNumber == null)
                    {
                        sb.AppendLine($"Imported {prod.Name} with no phone number produces {prod.Albums.Length} albums");
                    }
                    else
                    {
                        sb.AppendLine($"Imported {prod.Name} with phone: {prod.PhoneNumber} produces {prod.Albums.Length} albums");
                    }
                    
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString().Trim();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }
    }
}