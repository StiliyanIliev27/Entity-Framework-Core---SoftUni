using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Common
{
    public class ValidationConstants
    {
        //Truck
        public const int RegistrationNumberLength = 8;
        public const int VinNumberLength = 17;
        public const int TankCapacityMaxLength = 1420;
        public const int CargoCapacityMaxLength = 29000;
        public const string TruckRegistrationNumberRegEx =
            @"[A-Z]{2}\d{4}[A-Z]{2}";
        public const int TankCapacityMinLength = 950;
        public const int CargoCapacityMinLength = 5000;

        //Client
        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        public const int ClientNationalityMaxLength = 40;

        //Despatcher
        public const int DespatcherNameMaxLength = 40;
        public const int DespatcherNameMinLength = 2;
    }
}
