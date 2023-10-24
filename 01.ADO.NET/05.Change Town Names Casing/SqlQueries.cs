using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05.Change_Town_Names_Casing
{
    public class SqlQueries
    {
        public const string GetTownsByCountryName = "SELECT t.Name \r\n   FROM Towns as t\r\n   JOIN Countries AS c ON c.Id = t.CountryCode\r\n  WHERE c.Name = @countryName";

        public const string UpdateTowns = "UPDATE Towns\r\n   SET Name = UPPER(Name)\r\n WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";
    }
}
