using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace _03.Minion_Names
{
    internal class Program
    {
        private const string connectionString = "Server = DESKTOP-ME8RHIA; Database = MinionsDB; Trusted_Connection = True; TrustServerCertificate = True; MultipleActiveResultSets=true;";
        private static SqlConnection? connection;
        static async Task Main(string[] args)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                await GetOrderedMinionsById(1);
                await GetOrderedMinionsById(2);
                await GetOrderedMinionsById(6);
                await GetOrderedMinionsById(8);
            }
            finally
            {
                connection?.Dispose();
            }                      
        }

        static async Task GetOrderedMinionsById(int id)
        {
            using SqlCommand command = new SqlCommand(SqlQueries.GetVillainById, connection);
            command.Parameters.AddWithValue("@Id", id);
            var result = await command.ExecuteScalarAsync();

            if(result == null)
            {
                await Console.Out.WriteLineAsync($"No villain with ID {id} exists in the database.");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Villain: {result}");

                using SqlCommand commandGetMinionData = new SqlCommand(SqlQueries.GetOrderedMinionsByVillainId, connection);
                commandGetMinionData.Parameters.AddWithValue("@Id", id);

                var minionReader = await commandGetMinionData.ExecuteReaderAsync();

                if(minionReader.HasRows == false)
                {
                    await Console.Out.WriteLineAsync("(no minions)");
                }
                else
                {
                    while (await minionReader.ReadAsync())
                    {
                        int rowNumber = Convert.ToInt32(minionReader["RowNum"]);
                        string? minionName = minionReader["Name"].ToString();
                        int minionAge = Convert.ToInt32(minionReader["Age"]);

                        await Console.Out.WriteLineAsync($"{rowNumber}. {minionName} {minionAge}");
                    }
                }
            }
        }
    }
}