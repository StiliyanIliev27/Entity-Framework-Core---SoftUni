using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08.Increase_Minion_Age
{
    public class SqlQueries
    {
        public const string GetNameAndAgeFromMinions = "SELECT Name, Age FROM Minions";

        public const string GetNameOfMinion = "SELECT [Name] FROM Minions";

        public const string UpdateMinionsById = " UPDATE Minions\r\n   SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1\r\n WHERE Id = @Id";
    }
}
