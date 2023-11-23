using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<Supplier, ExportLocalSupplierDto>()
                .ForMember(d => d.PartsCount,
                    opt => opt.MapFrom(s => s.Parts.Count));

            //Part
            this.CreateMap<ImportPartDto, Part>();

            this.CreateMap<Part, PartInfo>();

            //Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(d => d.Parts,
                    opt => opt.DoNotValidate());

            this.CreateMap<Car, ExportCarWithDistanceDto>();
            
            this.CreateMap<Car, ExportBMWDto>();
            
            this.CreateMap<Car, ExportCarInfoDto>()
                .ForMember(d => d.Parts,
                    opt => opt.MapFrom(s => s.PartsCars.Select(pc => pc.Part)
                    .OrderByDescending(p => p.Price)
                    .ToArray()));

            //this.CreateMap<Car, CarInfo>(); -- Problem 19

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate,
                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            this.CreateMap<Customer, ExportCustomerDto>()
                .ForMember(d => d.BoughtCars,
                    opt => opt.MapFrom(s => s.Sales.Count()))
                .ForMember(d => d.SpentMoney,
                    opt => opt.MapFrom(s => s.IsYoungDriver
                        ? s.Sales.SelectMany(sale => sale.Car.PartsCars.Select(pc => Math.Round(pc.Part.Price * 0.95m, 2))).Sum()
                        : s.Sales.SelectMany(sale => sale.Car.PartsCars.Select(pc => pc.Part.Price)).Sum()));


            //Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId,
                    opt => opt.MapFrom(s => s.CarId));

            //this.CreateMap<Sale, ExportSaleDto>()
            //    .ForMember(d => d.Discount,
            //        opt => opt.MapFrom(s => s.Discount))
            //    .ForMember(d => d.Name,
            //        opt => opt.MapFrom(s => s.Customer.Name))
            //    .ForMember(d => d.Price,
            //        opt => opt.MapFrom(s => s.Car.PartsCars.Sum(pc => pc.Part.Price)));
            //.ForMember(d => d.PriceWithDiscount,
            //    opt => opt.MapFrom(s => s.Car.PartsCars.Sum(pc => pc.Part.Price))); -- Problem 19
        }
    }
}
