using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.Minion_Names
{
    public class SqlQueries
    {      
        public const string GetVillainById = "SELECT Name FROM Villains WHERE Id = @Id";

        public const string GetOrderedMinionsByVillainId = "SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,\r\nm.Name, \r\n m.Age\r\nFROM MinionsVillains AS mv\r\nJOIN Minions As m ON mv.MinionId = m.Id\r\nWHERE mv.VillainId = @Id\r\nORDER BY m.Name";
    }
}
