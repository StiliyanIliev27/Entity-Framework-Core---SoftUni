using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01.Initial_Setup
{
    public static class SQLQueries
    {
        public const string CreateDB = "CREATE DATABASE MinionsDB";
       
        public const string CreateDBTables = "USE MinionsDB\r\n\r\nCREATE TABLE Countries(\r\n\tId INT PRIMARY KEY IDENTITY\r\n\t,Name VARCHAR(50)\r\n)\r\n\r\nCREATE TABLE Towns(\r\n\tId INT PRIMARY KEY IDENTITY\r\n\t,Name VARCHAR(50)\r\n\t,CountryCode INT FOREIGN KEY REFERENCES Countries(Id)\r\n)\r\n\r\nCREATE TABLE Minions(\r\n\tId INT PRIMARY KEY IDENTITY\r\n\t,Name VARCHAR(30)\r\n\t,Age INT\r\n\t,TownId INT FOREIGN KEY REFERENCES Towns(Id)\r\n)\r\n\r\nCREATE TABLE EvilnessFactors(\r\n\tId INT PRIMARY KEY IDENTITY\r\n\t,Name VARCHAR(50)\r\n)\r\n\r\nCREATE TABLE Villains(\r\n\tId INT PRIMARY KEY IDENTITY\r\n\t,[Name] VARCHAR(50)\r\n\t,EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)\r\n)\r\n\r\nCREATE TABLE MinionsVillains(\r\n\tMinionId INT FOREIGN KEY REFERENCES Minions(Id)\r\n\t,VillainId INT FOREIGN KEY REFERENCES Villains(Id)\r\n\t,CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId)\r\n)";

        public const string InsertIntoDBTables = "USE MinionsDB INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')\r\n\r\nINSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 5)\r\n\r\nINSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)\r\n\r\nINSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')\r\n\r\nINSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)\r\n\r\nINSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,7),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1)";
    }
}
