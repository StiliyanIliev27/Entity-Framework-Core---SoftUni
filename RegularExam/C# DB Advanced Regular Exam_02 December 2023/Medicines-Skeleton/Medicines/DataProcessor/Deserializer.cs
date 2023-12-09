namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            
            ImportPatientDto[] patientDtos =
                JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString)!;

            int[] validMedicineIds = context.Medicines
                .Select(m => m.Id)
                .ToArray();

            ICollection<Patient> validPatients = new HashSet<Patient>();
            foreach(ImportPatientDto patientDto in patientDtos)
            {
                if(!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender
                };

                foreach(int id in patientDto.Medicines)
                {
                    if(patient.PatientsMedicines.Any(pm => pm.MedicineId == id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    patient.PatientsMedicines.Add(new PatientMedicine()
                    {
                        MedicineId = id
                    });
                }
                validPatients.Add(patient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient,
                    patient.FullName, patient.PatientsMedicines.Count()));
            }
            context.Patients.AddRange(validPatients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            string rootName = "Pharmacies";

            ImportPharmacyDto[] pharmacyDtos = 
                xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, rootName);

            ICollection<Pharmacy> validPharmacies = new HashSet<Pharmacy>();
            foreach (ImportPharmacyDto pharmacyDto in pharmacyDtos)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(pharmacyDto.IsNonStop != "false" && pharmacyDto.IsNonStop != "true")
                {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop),
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber
                };

                ICollection<Medicine> validMedicines = new HashSet<Medicine>();
                foreach(ImportMedicineDto medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto) || medicineDto.Producer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    DateTime productionDate = DateTime.ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime expiryDate = DateTime.ParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if(productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(pharmacy.Medicines.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    pharmacy.Medicines.Add(new Medicine()
                    {
                        Category = (Category)medicineDto.Category,
                        Name = medicineDto.Name,
                        Price = medicineDto.Price,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicineDto.Producer
                    });
                }
                validPharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy,
                    pharmacy.Name, pharmacy.Medicines.Count()));
            }
            context.Pharmacies.AddRange(validPharmacies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
