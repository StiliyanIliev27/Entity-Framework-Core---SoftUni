using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Castle.Core.Resource;
using System.IO;
using System.Runtime.CompilerServices;

namespace CarDealer
{
    public class StartUp
    {
        private static string result = string.Empty;
        private static string? inputXml;
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //09.Import Suppliers
            //inputXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            //result = ImportSuppliers(context, inputXml);

            //10.Import Parts
            //inputXml = File.ReadAllText(@"../../../Datasets/parts.xml");
            //result = ImportParts(context, inputXml);

            //11.Import Cars
            //inputXml = File.ReadAllText(@"../../../Datasets/cars.xml");
            //result = ImportCars(context, inputXml);

            //12.Import Customers
            //inputXml = File.ReadAllText(@"../../../Datasets/customers.xml");
            //result = ImportCustomers(context, inputXml);

            //13.Import Sales
            //inputXml = File.ReadAllText(@"../../../Datasets/sales.xml");
            //result = ImportSales(context, inputXml);

            //14.Export Cars With Distance
            //result = GetCarsWithDistance(context);

            //15.Export Cars from Make BMW
            //result = GetCarsFromMakeBmw(context);

            //16.Export Local Suppliers
            //result = GetLocalSuppliers(context);

            //17.Export Cars With Their List Of Parts
            //result = GetCarsWithTheirListOfParts(context);

            //18.Export Total Sales By Customer
            //result = GetTotalSalesByCustomer(context);  

            //19. Export Sales With Applied Discount
            result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result);
        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Suppliers";
            XmlHelper xmlHelper = new XmlHelper();

            ImportSupplierDto[] supplierDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, rootName);

            Supplier[] importSuppliers = mapper.Map<Supplier[]>(supplierDtos);
            context.Suppliers.AddRange(importSuppliers);
            context.SaveChanges();

            return $"Successfully imported {importSuppliers.Length}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Parts";
            XmlHelper xmlHelper = new XmlHelper();

            ImportPartDto[] partDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, rootName);

            ICollection<Part> validParts = new HashSet<Part>();
            foreach(ImportPartDto partDto in partDtos)
            {
                if(!context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(partDto);
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Cars";
            XmlHelper xmlHelper = new XmlHelper();

            ImportCarDto[] carDtos = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, rootName);

            ICollection<Car> validCars = new HashSet<Car>();
            foreach(ImportCarDto carDto in carDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make) ||
                   string.IsNullOrEmpty(carDto.Model))
                {
                    continue;
                }

                Car car = mapper.Map<Car>(carDto);

                foreach(var importCarPartDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if(!context.Parts.Any(p => p.Id == importCarPartDto.PartId))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        PartId = importCarPartDto.PartId
                    };
                    
                    car.PartsCars.Add(partCar);
                }
                
                validCars.Add(car);
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Customers";
            XmlHelper xmlHelper = new XmlHelper();

            ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, rootName);

            Customer[] inportCustomers = mapper.Map<Customer[]>(customerDtos);
            context.Customers.AddRange(inportCustomers);
            context.SaveChanges();
            
            return $"Successfully imported {inportCustomers.Length}";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();
            string rootName = "Sales";
            XmlHelper xmlHelper = new XmlHelper();

            ImportSaleDto[] saleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, rootName);

            ICollection<Sale> validSales = new HashSet<Sale>();
            foreach (ImportSaleDto saleDto in saleDtos)
            {
                if(!context.Cars.Any(c => c.Id == saleDto.CarId))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(saleDto);
                validSales.Add(sale);
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}";
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "cars";
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarWithDistanceDto[] carDtos = context.Cars
                .Where(c => c.TraveledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(carDtos, rootName);
        }

        //Problem 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "cars";
            XmlHelper xmlHelper = new XmlHelper();

            ExportBMWDto[] carDtos = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportBMWDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(carDtos,rootName);
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "suppliers";
            XmlHelper xmlHelper = new XmlHelper();

            ExportLocalSupplierDto[] localSupplierDtos = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(localSupplierDtos, rootName);
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "cars";
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarInfoDto[] carDtos = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarInfoDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(carDtos, rootName);
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "customers";
            XmlHelper xmlHelper = new XmlHelper();

            ExportCustomerDto[] customerDtos = context.Customers
                .Where(c => c.Sales.Any())
                .ProjectTo<ExportCustomerDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            return xmlHelper.Serialize(customerDtos, rootName);

        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            IMapper mapper = CreateMap();
            string rootName = "sales";
            XmlHelper xmlHelper = new XmlHelper();

            ExportSaleDto[] salesDtos = context
                .Sales
                .Select(s => new ExportSaleDto()
                {
                    SingleCar = new CarInfo()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    Name = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(p => p.Part.Price),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

            return xmlHelper.Serialize(salesDtos, rootName);
        }
        private static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}