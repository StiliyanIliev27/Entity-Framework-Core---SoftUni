using Microsoft.Data.SqlClient;

namespace _07.Print_All_Minion_Names
{
    public class Program
    {
        private const string connectionString = "Server = DESKTOP-ME8RHIA; Database = MinionsDB; Trusted_Connection = True; TrustServerCertificate = True; MultipleActiveResultSets=true;";
        private static SqlConnection? connection;
        static async Task Main(string[] args)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                await PrintMinionNamesInSpecificWay();

                await connection.CloseAsync();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        static async Task PrintMinionNamesInSpecificWay()
        {
            string query = "SELECT [Name] FROM Minions";          
            using SqlCommand command = new SqlCommand(query, connection);
            List<string> minionNames = new();           
            
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while(reader.Read())
            {
                minionNames.Add(reader["Name"].ToString()!);
            }

            for(int i = 0; i < minionNames.Count / 2; i++)
            {
                await Console.Out.WriteLineAsync(minionNames[i]);
                await Console.Out.WriteLineAsync(minionNames[minionNames.Count - 1 - i]);
            }
            
            if(minionNames.Count % 2 == 1)
            {
                await Console.Out.WriteLineAsync(minionNames[minionNames.Count / 2]);
            }
        }
    }
}