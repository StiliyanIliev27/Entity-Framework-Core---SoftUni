using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06.Remove_Villain
{
    public class SqlQueries
    {
        public const string GetVillainById = "SELECT [Name] FROM Villains WHERE Id = @villainId";

        public const string GetCountOfReleasedMinions = "SELECT \r\n\tCOUNT(MinionId) AS [Count]\r\nFROM MinionsVillains\r\nWHERE VillainId = @villainId";

        public const string RemoveFromMinionsVillainsByVillainId = "DELETE FROM MinionsVillains \r\n      WHERE VillainId = @villainId";

        public const string RemoveFromVillainsByVillainId = "DELETE FROM Villains\r\n      WHERE Id = @villainId";
    }
}
