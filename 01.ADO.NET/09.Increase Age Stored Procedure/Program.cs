using Microsoft.Data.SqlClient;

namespace _09.Increase_Age_Stored_Procedure
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

                int inputData = int.Parse(Console.ReadLine());
                await IncreaseAgeById(inputData);
            }
            finally
            {
                connection?.Close();
            }
        }

        static async Task IncreaseAgeById(int inputData)
        {
            using SqlCommand cmdIncreaseAge = new SqlCommand(SqlQueries.ExecuteStoredProcedure, connection);
            cmdIncreaseAge.Parameters.AddWithValue("@id", inputData);
            await cmdIncreaseAge.ExecuteNonQueryAsync();

            using SqlCommand cmdGetUpdatedMinions = new SqlCommand(SqlQueries.GetNameAndAgeFromMinionsById, connection);
            cmdGetUpdatedMinions.Parameters.AddWithValue("@Id", inputData);
            using SqlDataReader reader = await cmdGetUpdatedMinions.ExecuteReaderAsync();

            while(reader.Read())
            {
                string name = reader["Name"].ToString();
                int age = Convert.ToInt32(reader["Age"]);
                await Console.Out.WriteLineAsync($"{name} – {age} years old");
            }
        }
    }
}