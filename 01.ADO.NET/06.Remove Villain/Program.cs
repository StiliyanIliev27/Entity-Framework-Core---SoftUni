using Microsoft.Data.SqlClient;

namespace _06.Remove_Villain
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

                int inputId = int.Parse(Console.ReadLine());
                await RemoveVillain(inputId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        static async Task RemoveVillain(int inputId)
        {
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using SqlCommand cmdGetVillainById = new SqlCommand(SqlQueries.GetVillainById, connection, transaction);
                cmdGetVillainById.Parameters.AddWithValue("@villainId", inputId);
                var villainName = await cmdGetVillainById.ExecuteScalarAsync(); 

                if(villainName == null)
                {
                    await Console.Out.WriteLineAsync("No such villain was found.");
                }
                else
                {
                    using SqlCommand cmdCountReleasedMinions = new SqlCommand(SqlQueries.GetCountOfReleasedMinions, connection, transaction);
                    cmdCountReleasedMinions.Parameters.AddWithValue("@villainId", inputId);
                    int countOfReleasedMinions = Convert.ToInt32(await cmdCountReleasedMinions.ExecuteScalarAsync());

                    using SqlCommand cmdRemoveFromMinionsVillains = new SqlCommand(SqlQueries.RemoveFromMinionsVillainsByVillainId, connection, transaction);
                    cmdRemoveFromMinionsVillains.Parameters.AddWithValue("@villainId", inputId);
                    await cmdRemoveFromMinionsVillains.ExecuteNonQueryAsync();

                    using SqlCommand cmdRemoveFromVillains = new SqlCommand(SqlQueries.RemoveFromVillainsByVillainId, connection, transaction);
                    cmdRemoveFromVillains.Parameters.AddWithValue("@villainId", inputId);
                    await cmdRemoveFromVillains.ExecuteNonQueryAsync();

                    await Console.Out.WriteLineAsync($"{villainName} was deleted.");
                    await Console.Out.WriteLineAsync($"{countOfReleasedMinions} minions were released.");
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