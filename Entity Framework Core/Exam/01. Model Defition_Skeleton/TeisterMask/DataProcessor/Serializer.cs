namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var tasks = context.Employees.Where(x => x.EmployeesTasks.Any(y => y.Task.OpenDate >= date))
                .Select(x => new
                {
                    Username = x.Username,
                    Tasks = x.EmployeesTasks.Where(z => z.Task.OpenDate >= date)
                    .OrderByDescending(y => y.Task.DueDate)
                    .ThenBy(y => y.Task.Name)
                    .Select(y => new
                    {
                        TaskName = y.Task.Name,
                        OpenDate = y.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = y.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = y.Task.LabelType.ToString(),
                        ExecutionType = y.Task.ExecutionType.ToString()
                    })

                }).OrderByDescending(x => x.Tasks.Count())
                .ThenBy(x => x.Username)
                .Take(10)
                .ToList();

            return JsonConvert.SerializeObject(tasks, Formatting.Indented);

        }
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ProjectDTO[]), new XmlRootAttribute("Projects"));

            var projects = context.Projects.Where(x => x.Tasks.Any())
                .OrderByDescending(x => x.Tasks.Count)
                .ThenBy(x => x.Name)
                .Select(x => new ProjectDTO
                {
                    TasksCount = x.Tasks.Count,
                    ProjectName = x.Name, 
                    HasEndDate = x.DueDate == null ? "No" : "Yes",
                    Tasks = x.Tasks.Select(y => new TaskDTO
                    {
                        Name = y.Name,
                        Label = y.LabelType.ToString()
                    })
                    .OrderBy(z => z.Name)
                    .ToArray()
                })
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, projects, namespaces);
            }
            return sb.ToString();
        }

    }
}