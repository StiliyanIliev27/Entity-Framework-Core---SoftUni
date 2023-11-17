using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string? inputJson;
        private static string? result;
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //01.Import users
            //inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            //result = ImportUsers(context, inputJson);

            //02.Import products
            //inputJson = File.ReadAllText(@"../../../Datasets/products.json");
            //result = ImportProducts(context, inputJson);

            //03.Import categories
            //inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
            //result = ImportCategories(context, inputJson);

            //04.Import categories and products
            //inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            //result = ImportCategoryProducts(context, inputJson); 

            //05.Export Products in Range
            //result = GetProductsInRange(context);

            //06.Export Sold Products
            //result = GetSoldProducts(context);

            //07.Export Categories by Products Count
            //result = GetCategoriesByProductsCount(context);

            //08.Export Users and Products
            //result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }
        
        //01.Problem
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //Firstly, we create our mapper.
            IMapper mapper = CreateMap();

            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson)!;

            //Foreach loop provides us additioanal validations
            ICollection<User> validUsers = new HashSet<User>();
            foreach(ImportUserDto userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);

                validUsers.Add(user);
            }

            //Our users are ready for the DB
            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }
       
        //02.Problem
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            //Firstly, we create our mapper.
            IMapper mapper = CreateMap();

            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson)!;

            //Foreach loop provides us additional validations
            ICollection<Product> validProducts = new HashSet<Product>();

            foreach(ImportProductDto productDto in productDtos)
            {
                Product product = mapper.Map<Product>(productDto); 

                validProducts.Add(product);
            }

            //Our products are ready to be inserted into Products table(DB)
            context.Products.AddRange(validProducts);
            context.SaveChanges();

            //And finally we return a string of inserted products count
            return $"Successfully imported {validProducts.Count}";
        }

        //03.Problem
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //Firstly, we create our mapper.
            IMapper mapper = CreateMap();

            ImportCategoryDto[] importCategoryDtos =
                JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson)!;

            //Using foreach loop allows us to make validations for every category.
            
            ICollection<Category> validCategories = new HashSet<Category>();
            foreach(ImportCategoryDto categoryDto in importCategoryDtos)
            {
                Category category = mapper.Map<Category>(categoryDto);

                if(category.Name == null)
                {
                    continue;
                }

                validCategories.Add(category);
            }

            //Our results are ready to be loaded into Category table.
            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            //Finally, we just have to return a string with valid categories count.
            return $"Successfully imported {validCategories.Count}";
        }

        //04.Problem
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            //Firstly, we create our mapper.
            IMapper mapper = CreateMap();

            ImportCategoryProductDto[] importCategoryProductDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson)!;

            //Using foreach loop allows us to make additional validations.           
            
            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();
            foreach(ImportCategoryProductDto categoryProductDto in importCategoryProductDtos)
            {
                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(categoryProductDto);

                validCategoryProducts.Add(categoryProduct);
            }

            //Our results are ready to "take part in our adventure" :D
            context.CategoriesProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        //05.Problem
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName,
                }).Where(p => p.price >= 500 && p.price <= 1000)
                .OrderBy(p => p.price)
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        //06.Problem
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price,
                        buyerFirstName = ps.Buyer.FirstName,
                        buyerLastName = ps.Buyer.LastName
                    }).ToArray()
                }).AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //07.Problem
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts.Average(cp => cp.Product.Price).ToString("f2"),
                    TotalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("f2"),
                }).AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categories,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver
                });
        }

        //08.Problem
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))                
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                p.Name,
                                p.Price,
                            }).ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();

            var resultUsers = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return JsonConvert.SerializeObject(resultUsers,
               Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ContractResolver = contractResolver,
                   NullValueHandling = NullValueHandling.Ignore,
               });
        }
        private static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
        private static IContractResolver ConfigureCamelCaseNaming()
        {
            return new DefaultContractResolver()
            {
                NamingStrategy = 
                    new CamelCaseNamingStrategy(false, true) 
            };
        }
    }
}