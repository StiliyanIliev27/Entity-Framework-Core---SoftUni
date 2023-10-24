using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09.Increase_Age_Stored_Procedure
{
    public class SqlQueries
    {
        public const string ExecuteStoredProcedure = "EXEC usp_GetOlder @id";

        public const string GetNameAndAgeFromMinionsById = "SELECT Name, Age FROM Minions WHERE Id = @Id";

    }
}
