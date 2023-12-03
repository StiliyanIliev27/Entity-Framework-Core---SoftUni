namespace Invoices.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.ComponentModel;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportClientInvoiceDto[] clients = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .Select(c => new ExportClientInvoiceDto()
                {
                    InvoicesCount = c.Invoices.Count,
                    Name = c.Name,
                    NumberVat = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(i => new ExportInvoiceDto()
                    {
                        Number = i.Number,
                        Amount = (double) i.Amount,
                        DueDate = i.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        CurrencyType = i.CurrencyType,
                    }).ToArray()
                }).OrderByDescending(i => i.InvoicesCount)
                .ThenBy(i => i.Name)
                .ToArray();

            return xmlHelper.Serialize<ExportClientInvoiceDto[]>(clients, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> 
                { 
                    new StringEnumConverter() 
                }
            };

            var products = context.Products
                .Where(p => p.ProductsClients.Any(c => c.Client.Name.Length >= nameLength))
                .Select(p => new ExportProductDto()
                {
                    Name = p.Name,
                    Price = (double)p.Price,
                    Category = p.CategoryType,
                    Clients = p.ProductsClients
                    .Where(pc => pc.Client.Name.Length >= nameLength) 
                    .Select(pc => new ExportClientDto()
                    {
                        Name = pc.Client.Name,
                        NumberVat = pc.Client.NumberVat
                    }).OrderBy(p => p.Name).ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();
            
            return JsonConvert.SerializeObject(products, jsonSerializerSettings);
        }
    }
}