namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using static System.Reflection.Metadata.BlobBuilder;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //02.Age Restriction
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, input));            

            //03.Golden Books
            //Console.WriteLine(GetGoldenBooks(db));

            //04.Books by Price
            //Console.WriteLine(GetBooksByPrice(db));

            //05.Not Released In
            //int inputYear = int.Parse(Console.ReadLine()!);
            //Console.WriteLine(GetBooksNotReleasedIn(db, inputYear));

            //06.Book Titles by Category
            //string input = Console.ReadLine()!;
            //Console.WriteLine(GetBooksByCategory(db, input));

            //07.Released Before Date
            //string input = Console.ReadLine()!;
            //Console.WriteLine(GetBooksReleasedBefore(db, input));

            //08.Author Search
            //string input = Console.ReadLine()!;
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //09.Book Search
            //string input = Console.ReadLine()!;
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //10.Book Search by Author
            //string input = Console.ReadLine()!;
            //Console.WriteLine(GetBooksByAuthor(db, input));

            //11.Count Books
            //int input = int.Parse(Console.ReadLine()!);
            //Console.WriteLine(CountBooks(db, input));

            //12.Total Book Copies
            //Console.WriteLine(CountCopiesByAuthor(db));

            //13.Profit by Category
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //14. Most Recent Books
            //Console.WriteLine(GetMostRecentBooks(db));

            //15.Increase Prices
            // IncreasePrices(db);

            //16.Remove Books
            //Console.WriteLine(RemoveBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = (AgeRestriction) Enum.Parse(typeof(AgeRestriction), command, true);

            string[] bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)                
                .OrderBy(b => b.Title)
                .Select(b => b.Title)   
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)                
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                }).ToArray()
                .OrderByDescending(b => b.Price);

            StringBuilder sb = new StringBuilder();

            foreach(var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            string[] booksNotReleasedIn = context.Books
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, booksNotReleasedIn);
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] booksByCategory = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();
                
            return string.Join(Environment.NewLine, booksByCategory);
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var booksReleasedBefore = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", null))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                }).ToArray();

            StringBuilder sb = new StringBuilder(); 

            foreach(var b in booksReleasedBefore)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorFullNamesEndingIn = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                    {
                         FullName = a.FirstName + " " + a.LastName,
                    }).ToArray()
                .OrderBy(a => a.FullName).ToArray();

            StringBuilder sb = new();

            foreach(var a in authorFullNamesEndingIn)
            {
                sb.AppendLine(a.FullName);
            }

            return sb.ToString().TrimEnd();               
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] bookTitlesContaining = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();    

            return string.Join(Environment.NewLine, bookTitlesContaining);
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksByAuthor = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author = b.Author.FirstName + " " + b.Author.LastName
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var b in booksByAuthor)
            {
                sb.AppendLine($"{b.Title} ({b.Author})");
            }

            return sb.ToString().TrimEnd();
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            Book[] books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();

            return books.Length;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                    BookCopies = a.Books.Sum(b => b.Copies)
                }).OrderByDescending(b => b.BookCopies)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var a in authors)
            {
                sb.AppendLine($"{a.AuthorFullName} - {a.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                 .Select(c => new
                 {
                     CategoryName = c.Name,
                     TotalProfit = c.CategoryBooks.Sum(bc => bc.Book.Copies * bc.Book.Price)
                 }).OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var b in books)
            {
                sb.AppendLine($"{b.CategoryName} ${b.TotalProfit:f2}");
            }
            
            return sb.ToString().TrimEnd();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesWithMostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3)
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        }).ToArray()
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var c in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach(var b in c.MostRecentBooks)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            Book[] books = context.Books
                .Where(b => b.Copies < 4200).ToArray();

            foreach(var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return books.Count();
        }
    }
}


