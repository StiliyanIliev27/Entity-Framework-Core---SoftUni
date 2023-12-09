namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            XmlHelper xmlHelper = new XmlHelper();
            string rootName = "Patients";

            DateTime _date = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            ExportPatientDto[] patientDtos = context.Patients
                .ToArray()
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > _date))
                .Select(p => new ExportPatientDto()
                {
                    Gender = p.Gender.ToString().ToLower(),
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medicines = p.PatientsMedicines
                        .Where(pm => pm.Medicine.ProductionDate > _date)
                        .OrderByDescending(m => m.Medicine.ExpiryDate)
                        .ThenBy(m => m.Medicine.Price)
                        .Select(m => new ExportPatientMedicineDto()
                        {
                            Category = m.Medicine.Category.ToString().ToLower(),
                            Name = m.Medicine.Name,
                            Price = m.Medicine.Price.ToString("f2"),
                            Producer = m.Medicine.Producer,
                            ExpiryDate = m.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })
                        .ToArray()
                }).OrderByDescending(p => p.Medicines.Count())
                .ThenBy(p => p.Name)
                .ToArray();

            return xmlHelper.Serialize(patientDtos, rootName);
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            ExportMedicineDto[] medicineDtos = context.Medicines
                .AsEnumerable()
                .Where(m => m.Category == (Category)medicineCategory &&
                    m.Pharmacy.IsNonStop == true)
                .Select(m => new ExportMedicineDto()
                {
                    Name = m.Name,
                    Price = m.Price.ToString("f2"),
                    Pharmacy = new ExportPharmacyDto()
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber
                    }
                }).OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .ToArray();

            return JsonConvert.SerializeObject(medicineDtos, Formatting.Indented);
        }
    }
}
