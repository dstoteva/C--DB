namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using System.Text;
    using System.IO;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using System.Data.SqlTypes;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ProjectDTO[]), new XmlRootAttribute("Projects"));

            ProjectDTO[] projects;

            using (var reader = new StringReader(xmlString))
            {
                projects = (ProjectDTO[])serializer.Deserialize(reader);
            }

            foreach (var p in projects)
            {
                if (IsValid(p))
                {
                    Project project;
                    if (string.IsNullOrWhiteSpace(p.DueDate))
                    {
                        project = new Project()
                        {
                            Name = p.Name,
                            OpenDate = DateTime.ParseExact(p.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        };
                    }
                    else
                    {
                        project = new Project()
                        {
                            Name = p.Name,
                            OpenDate = DateTime.ParseExact(p.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),

                            DueDate = DateTime.ParseExact(p.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        };
                    }


                    context.Projects.Add(project);

                    foreach (var t in p.Tasks)
                    {
                        if (IsValid(t) && (project.DueDate == null || (project.OpenDate <= DateTime.ParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            && project.DueDate >= DateTime.ParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))))
                        {
                            var task = new Task()
                            {
                                Name = t.Name,
                                OpenDate = DateTime.ParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                DueDate = DateTime.ParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExecutionType = (ExecutionType)t.ExecutionType,
                                LabelType = (LabelType)t.LabelType,
                                Project = project
                            };

                            context.Tasks.Add(task);
                        }
                        else
                        {
                            sb.AppendLine(ErrorMessage);
                        }
                    }
                    context.SaveChanges();
                    sb.AppendLine($"Successfully imported project - {project.Name} with {project.Tasks.Count} tasks.");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var employees = JsonConvert.DeserializeObject<EmployeeDTO[]>(jsonString);

            foreach (var emp in employees)
            {
                if (IsValid(emp))
                {
                    var employee = new Employee()
                    {
                        Username = emp.Username,
                        Email = emp.Email,
                        Phone = emp.Phone
                    };

                    context.Employees.Add(employee);
                    foreach (var t in emp.Tasks.Distinct())
                    {
                        if (context.Tasks.Any(x => x.Id == t))
                        {
                            var empTask = new EmployeeTask()
                            {
                                TaskId = t,
                                Employee = employee
                            };

                            context.EmployeesTasks.Add(empTask);
                        }
                        else
                        {
                            sb.AppendLine(ErrorMessage);
                        }
                    }
                    context.SaveChanges();
                    sb.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}