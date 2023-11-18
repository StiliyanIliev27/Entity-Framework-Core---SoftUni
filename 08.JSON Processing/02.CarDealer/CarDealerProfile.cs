using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

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

            //Car
            this.CreateMap<ImportCarDto, Car>();

            this.CreateMap<Car, ExportCarFromMakeToyotaDto>();

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();
           
            this.CreateMap<Customer, ExportOrderedCustomerDto>()
                .ForMember(d => d.CustomerName,
                    opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CustomerBirthDate,
                    opt => opt.MapFrom(s => s.BirthDate))
                .ForMember(d => d.CustomerIsYoungDriver,
                    opt => opt.MapFrom(s => s.IsYoungDriver));           

            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
