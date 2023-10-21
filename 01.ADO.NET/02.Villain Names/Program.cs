using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace _02.Villain_Names
{
    internal class Program
    {
        private const string connectionString = "Server = DESKTOP-ME8RHIA; Database = MinionsDB; Trusted_Connection = True; TrustServerCertificate = True; MultipleActiveResultSets=true;";
        private static SqlConnection? sqlConnection;
        static async Task Main(string[] args)
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                await sqlConnection.OpenAsync();

                using SqlCommand sqlCommand = new SqlCommand(SqlQueries.GetVillainsWithMoreThan3Minions, sqlConnection);
                using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    string? villainName = sqlDataReader["Name"].ToString();
                    int numberOfMinions = Convert.ToInt32(sqlDataReader["MinionsCount"]);
                    Console.WriteLine($"{villainName} - {numberOfMinions}");
                }
            }
            finally
            {
               sqlConnection?.Dispose();
            }
           
        }
    }
}