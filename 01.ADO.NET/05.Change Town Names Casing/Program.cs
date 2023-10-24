
using Microsoft.Data.SqlClient;

namespace _05.Change_Town_Names_Casing
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

                string country = Console.ReadLine();
                await ChangeTownNamesByCountry(country);
            }
            finally
            {
                await connection.CloseAsync();
            }
            
        }

        static async Task ChangeTownNamesByCountry(string country)
        {
            using SqlCommand command = new SqlCommand(SqlQueries.GetTownsByCountryName, connection);
            command.Parameters.AddWithValue("@countryName", country);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            HashSet<string> changedTowns = new HashSet<string>();
            int changedTownsCount = 0;
            if(reader.HasRows == false)
            {
                await Console.Out.WriteLineAsync("No town names were affected.");
            }
            else
            {
                while (await reader.ReadAsync())
                {
                    SqlCommand updateTownsCommand = new SqlCommand(SqlQueries.UpdateTowns, connection);
                    updateTownsCommand.Parameters.AddWithValue("@countryName", country);
                    changedTowns.Add(reader["Name"].ToString().ToUpper());
                    changedTownsCount++;
                }
                await Console.Out.WriteLineAsync($"{changedTownsCount} town names were affected.");
                var result = changedTowns.ToArray();
                await Console.Out.WriteLineAsync($"[{string.Join(", ", result)}]");
            }           
        }
    }
}