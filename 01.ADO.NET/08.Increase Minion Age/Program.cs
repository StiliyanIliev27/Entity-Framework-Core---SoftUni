using Microsoft.Data.SqlClient;

namespace _08.Increase_Minion_Age
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

                int[] inputData = Console.ReadLine()!
                     .Split(' ')
                     .Select(int.Parse)
                     .ToArray();

                await UpdateMinionsById(inputData);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        static async Task UpdateMinionsById(int[] inputData)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                for (int i = 0; i < inputData.Length; i++)
                {
                    using SqlCommand cmdUpdateMinions = new SqlCommand(SqlQueries.UpdateMinionsById, connection, transaction);
                    cmdUpdateMinions.Parameters.AddWithValue("@Id", inputData[i]);
                    await cmdUpdateMinions.ExecuteNonQueryAsync();
                }

                using SqlCommand cmdGetAllMinions = new SqlCommand(SqlQueries.GetNameAndAgeFromMinions, connection, transaction);
                using SqlDataReader reader = await cmdGetAllMinions.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string? name = reader["Name"].ToString();
                    int age = Convert.ToInt32(reader["Age"]);
                    await Console.Out.WriteLineAsync($"{name} {age}");
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            
        }
    }
}