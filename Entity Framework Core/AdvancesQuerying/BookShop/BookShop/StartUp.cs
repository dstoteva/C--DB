namespace BookShop
{
    using BookShop.Models;
    using Data;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);
                Console.WriteLine(RemoveBooks(db));
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.EditionType.ToString() == "Gold" && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(p => p.Price > 40)
                .Select(b => new { b.Title, b.Price })
                .OrderByDescending(b => b.Price)
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - ${b.Price:f2}"));

            return sb.ToString().Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] genres = input.Split(" ").Select(g => g.ToLower()).ToArray();
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.BookCategories.Any(c => genres.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();
            var input = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            context.Books
                .Where(b => b.ReleaseDate < input)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new { b.Title, b.EditionType, b.Price })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(a => a)
                .ToList()
                .ForEach(a => sb.AppendLine(a));

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author = b.Author.FirstName + " " + b.Author.LastName
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} ({b.Author})"));

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Authors
                .Select(a => new
                {
                    Author = a.FirstName + " " + a.LastName,
                    Copies = a.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(b => b.Copies)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a.Author} - {a.Copies}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Categories
                .Select(b => new
                {
                    b.Name,
                    Profit = b.CategoryBooks.Select(c => c.Book.Price * c.Book.Copies).Sum()
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.Name} ${c.Profit:f2}"));


            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks.Select(b => new { b.Book.ReleaseDate, b.Book.Title}).OrderByDescending(b => b.ReleaseDate).Take(3)
                })
                .OrderBy(c => c.Name)
                .ToList()
                .ForEach(c => {
                    sb.AppendLine($"--{c.Name}");
                    foreach (var b in c.Books)
                    {
                        sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                    }
                });

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.SaveChanges();
            
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200).ToList();

            context.BooksCategories.Where(b => books.Contains(b.Book)).ToList().ForEach(b => context.Remove(b));

            books.ForEach(b => context.Books.Remove(b));
            context.SaveChanges();

            return books.Count;
        }
    }

}
