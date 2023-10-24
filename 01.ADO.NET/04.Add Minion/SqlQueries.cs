using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04.AddMinion
{
    public class SqlQueries
    {
        public const string GetTownIdByName = "SELECT Id FROM Towns WHERE [Name] = @townName";

        public const string InsertNewTown = "INSERT INTO Towns([Name]) OUTPUT inserted.Id VALUES (@townName)";

        public const string GetVillainIdByName = "SELECT Id FROM Villains WHERE [Name] = @Name";

        public const string InsertNewVillain = "INSERT INTO Villains ([Name], EvilnessFactorId) OUTPUT inserted.Id VALUES (@villainName, @evilnessFactorId)";

        public const string InsertNewMinion = "INSERT INTO Minions ([Name], Age, TownId) OUTPUT inserted.Id VALUES (@name, @age, @townId)";

        public const string InsertMinionVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
    }
}
