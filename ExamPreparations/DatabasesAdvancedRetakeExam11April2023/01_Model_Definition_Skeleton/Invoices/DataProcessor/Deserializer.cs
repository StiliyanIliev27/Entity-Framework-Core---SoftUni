namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            string rootName = "Clients";
            XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            ImportClientDto[] clientDtos = xmlHelper.Deserialize<ImportClientDto[]>(xmlString, rootName);

            ICollection<Client> validClients = new HashSet<Client>();
            foreach (var client in clientDtos)
            {
                if (!IsValid(client))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                var clientToAdd = new Client
                {
                    Name = client.Name,
                    NumberVat = client.NumberVat
                };

                foreach (var address in client.Addresses)
                {
                    if (IsValid(address))
                    {
                        clientToAdd.Addresses.Add(new Address()
                        {
                            City = address.City,
                            Country = address.Country,
                            PostCode = address.PostCode,
                            StreetName = address.StreetName,
                            StreetNumber = address.StreetNumber
                        });
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }

                validClients.Add(clientToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }

            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();

            ImportInvoiceDto[] invoiceDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString)!;

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();
            foreach (ImportInvoiceDto invoiceDto in invoiceDtos)
            {
                if (!IsValid(invoiceDto) || invoiceDto.DueDate < invoiceDto.IssueDate)
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };

                validInvoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString)!;
            int[] clientIds = context.Clients.Select(c => c.Id).ToArray();

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach(ImportProductDto productDto in productDtos)
            {
                if(!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType
                };
                
                foreach(var clientId in productDto.Clients.Distinct())
                {
                    if(clientIds.Contains(clientId))
                    {
                        product.ProductsClients.Add(new ProductClient()
                        {
                            ClientId = clientId
                        });
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }
                validProducts.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, productDto.Name, product.ProductsClients.Count));
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
