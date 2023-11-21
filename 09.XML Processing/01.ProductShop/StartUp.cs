using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop
{
    public class StartUp
    {
        private static string result = string.Empty;
        private static string inputXml = string.Empty;
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //01.Import Users
            //inputXml = File.ReadAllText("../../../Datasets/users.xml");
            //result = ImportUsers(context, inputXml);

            //02.Import Products
            //inputXml = File.ReadAllText("../../../Datasets/products.xml");
            //result = ImportProducts(context, inputXml);

            //03.Import Categories
            //inputXml = File.ReadAllText("../../../Datasets/categories.xml");
            //result = ImportCategories(context, inputXml);

            //04.Import Categories and Products
            //inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //result = ImportCategoryProducts(context, inputXml);

            //05.Export Products In Range
            //result = GetProductsInRange(context);

            //06.Export Sold Products
            //result = GetSoldProducts(context);

            //07.Export Categories By Products Count
            //result = GetCategoriesByProductsCount(context);

            //08.Export Users and Products
            //result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }
       
        //Problem 01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Users";
            XmlHelper xmlHelper = new XmlHelper();

            ImportUserDto[] userDtos = xmlHelper.Deserialize<ImportUserDto[]>(inputXml, rootName);

            ICollection<User> validUsers = new HashSet<User>();
            foreach(ImportUserDto userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);

                validUsers.Add(user);   
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Products";
            XmlHelper xmlHelper = new XmlHelper();

            ImportProductDto[] productDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, rootName);

            ICollection<Product> validProducts = new HashSet<Product>();
            foreach(ImportProductDto productDto in productDtos)
            {                                
                Product product = mapper.Map<Product>(productDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Categories";
            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryDto[] categoryDtos = xmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, rootName);

            ICollection<Category> validCategories = new HashSet<Category>();
            foreach(ImportCategoryDto categoryDto in categoryDtos)
            {
                if(string.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "CategoryProducts";
            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryProductDto[] categoryProductDtos = 
                xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, rootName);

            List<int> validCategoryIds = context.Categories.Select(c => c.Id).ToList();
            List<int> validProductIds = context.Products.Select(p => p.Id).ToList();

            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();
            foreach(ImportCategoryProductDto categoryProductDto in categoryProductDtos)
            {
                if (validCategoryIds.Contains(categoryProductDto.CategoryId) &&
                        validProductIds.Contains(categoryProductDto.ProductId))
                {
                    CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(categoryProductDto);
                    validCategoryProducts.Add(categoryProduct);
                }              
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "Products";
            XmlHelper xmlHelper = new XmlHelper();

            ExportProductInRangeDto[] productsDtos = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)  
                .Take(10)
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(productsDtos, rootName);
        }

        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "Users";
            XmlHelper xmlHelper = new XmlHelper();

            ExportSoldProductDto[] soldProductDtos = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)                
                .Select(p => new ExportSoldProductDto
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    AllSoldProducts = p.ProductsSold.Select(p => new ExportAllSoldProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .ToArray();

            return xmlHelper.Serialize(soldProductDtos, rootName);
        }

        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "Categories";
            XmlHelper xmlHelper = new XmlHelper();

            ExportCategoryDto[] categories = context.Categories
                .AsNoTracking()
                .Select(c => new ExportCategoryDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(c => c.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(c => c.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize(categories, rootName);
        }

        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "Users";
            XmlHelper xmlHelper = new XmlHelper();

            var userAndProductDto = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)  
                .Take(10)
                .Select(u => new UserInfo()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsCount()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new SoldProduct()
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).OrderByDescending(p => p.Price).ToArray()
                    }
                }).ToArray();

            ExportUserCountDto usersProducts = new ExportUserCountDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = userAndProductDto
            };

            return xmlHelper.Serialize(usersProducts, rootName);    
        }
        private static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
    }
}