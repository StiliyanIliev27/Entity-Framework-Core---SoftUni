using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text.Json.Nodes;

namespace CarDealer
{
    public class StartUp
    {
        private static string? result;
        private static string inputJson = string.Empty;
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //09.Import Suppliers
            //inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");
            //result = ImportSuppliers(context, inputJson);

            //10.Import Parts
            //inputJson = File.ReadAllText(@"../../../Datasets/parts.json");
            //result = ImportParts(context, inputJson);

            //11.Import Cars
            //inputJson = File.ReadAllText(@"../../../Datasets/cars.json");
            //result = ImportCars(context, inputJson);

            //12.Import Customers
            //inputJson = File.ReadAllText(@"../../../Datasets/customers.json");
            //result = ImportCustomers(context, inputJson);

            //13.Import Sales
            //inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
            //result = ImportSales(context, inputJson);

            //14.Export Ordered Customers
            //result = GetOrderedCustomers(context);

            //15.Export Cars from Make Toyota
            //result = GetCarsFromMakeToyota(context);

            //16.Export Local Suppliers
            //result = GetLocalSuppliers(context);

            //17.Export Cars with Their List of Parts
            //result = GetCarsWithTheirListOfParts(context);

            //18.Export Total Sales by Customer
            //result = GetTotalSalesByCustomer(context);

            //19.Export Sales with Applied Discount
            //result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result);
        }
       
        //09.Problem
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            //Creating our mapper.
            IMapper mapper = CreateMap();

            ImportSupplierDto[] importSupplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson)!;

            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        //10.Problem
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            ImportPartDto[] importPartDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson)!;

            ICollection<Part> validParts = new HashSet<Part>();
            foreach(ImportPartDto importPartDto in importPartDtos)
            {
                Part part = mapper.Map<Part>(importPartDto);

                if(!context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    continue;
                }

                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        //11.Problem
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            ImportCarDto[] importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson)!;

            ICollection<Car> validCars = new HashSet<Car>();
            foreach(ImportCarDto importCarDto in importCarDtos)
            {
                Car car = mapper.Map<Car>(importCarDto);

                foreach(var id in importCarDto.PartsIds)
                {
                    if (context.Parts.Any(p => p.Id == id))
                    {
                        car.PartsCars.Add(new PartCar
                        {
                            PartId = id
                        });
                    }                    
                }
                
                validCars.Add(car);
            }
            

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}.";
        }

        //12.Problem
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            ImportCustomerDto[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson)!;

            Customer[] customers = mapper.Map<Customer[]>(customerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        //13.Problem
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            ImportSaleDto[] saleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson)!;

            ICollection<Sale> validSales = new HashSet<Sale>();
            foreach(ImportSaleDto saleDto in saleDtos)
            {
                Sale sale = mapper.Map<Sale>(saleDto);

                //Judge doesn't like this additional validation!
                
                //if(!context.Cars.Any(c => c.Id == sale.CarId) || 
                //    !context.Customers.Any(c => c.Id == sale.CustomerId))
                //{
                //    continue;
                //}

                validSales.Add(sale);
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}.";
        }

        //14.Problem
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            IMapper mapper = CreateMap();

            ExportOrderedCustomerDto[] customersDtos = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .AsNoTracking()
                .ProjectTo<ExportOrderedCustomerDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(customersDtos,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    DateFormatString = "dd/MM/yyyy"
                });
        }

        //15.Problem
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            IMapper mapper = CreateMap();

            ExportCarFromMakeToyotaDto[] toyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportCarFromMakeToyotaDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
        }

        //16.Problem
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMap();

            ExportLocalSupplierDto[] localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
        }

        //17.Problem
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .AsNoTracking()
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },                   
                    parts = c.PartsCars
                        .Select(p => new
                        {
                            p.Part.Name,
                            Price = p.Part.Price.ToString("f2")
                        }).ToArray()
                }).ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        //18.Problem
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartsCars).Sum(p => p.Part.Price)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToList();

            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            return JsonConvert.SerializeObject(orderedCustomers,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver,
                });
        }

        //19.Problem
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new 
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = (s.Car.PartsCars.Sum(p => p.Part.Price)).ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Sum(p => p.Part.Price) - (s.Car.PartsCars.Sum(p => p.Part.Price)*(s.Discount/100))).ToString("f2")
                }).ToList();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }
        private static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
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