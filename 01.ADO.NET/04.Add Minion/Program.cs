using Microsoft.Data.SqlClient;

namespace _04.AddMinion
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

                //INPUT 
                string? minionInfoInput = Console.ReadLine();
                string? villainInfoInput = Console.ReadLine();

                await AddMinion(minionInfoInput, villainInfoInput);
            }
            finally
            {
                await connection.CloseAsync();
            }            
        }

        static async Task AddMinion(string minionInfoInput, string villainInfoInput)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            string[] minionInfo = minionInfoInput.Substring(minionInfoInput.IndexOf(':') + 2).Split(' ');
            string[] villainInfo = villainInfoInput.Substring(villainInfoInput.IndexOf(':') + 2).Split(' ');

            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string minionTown = minionInfo[2];
            string villainName = villainInfo[0];

            try
            {
                #region Town

                using SqlCommand townCommand = new SqlCommand(SqlQueries.GetTownIdByName, connection, transaction);
                townCommand.Parameters.AddWithValue("@townName", minionTown);
                var townResult = await townCommand.ExecuteScalarAsync();

                int townId = 0;
                if (townResult == null)
                {
                    using SqlCommand townInsertCommand = new SqlCommand(SqlQueries.InsertNewTown, connection, transaction);
                    townInsertCommand.Parameters.AddWithValue("@townName", minionTown);
                    townId = Convert.ToInt32(await townInsertCommand.ExecuteScalarAsync());
                    await Console.Out.WriteLineAsync($"Town {minionTown} was added to the database.");
                }
                else
                {
                    townId = (int)townResult;
                }

                #endregion

                #region Villain

                using SqlCommand villainCommand = new SqlCommand(SqlQueries.GetVillainIdByName, connection, transaction);
                villainCommand.Parameters.AddWithValue("@Name", villainName);
                var villainResult = await villainCommand.ExecuteScalarAsync();

                int villainId = 0;
                if (villainResult == null)
                {
                    using SqlCommand villainInsertCommand = new SqlCommand(SqlQueries.InsertNewVillain, connection, transaction);
                    villainInsertCommand.Parameters.AddWithValue("@villainName", villainName);
                    villainInsertCommand.Parameters.AddWithValue("@evilnessFactorId", 4);
                    villainId = Convert.ToInt32(await villainInsertCommand.ExecuteScalarAsync());
                    await Console.Out.WriteLineAsync($"Villain {villainName} was added to the database.");
                }
                else
                {
                    villainId = (int)villainResult;
                }

                #endregion

                #region Minion

                using SqlCommand minionInsertCommand = new SqlCommand(SqlQueries.InsertNewMinion, connection, transaction);
                minionInsertCommand.Parameters.AddWithValue("@name", minionName);
                minionInsertCommand.Parameters.AddWithValue("@age", minionAge);
                minionInsertCommand.Parameters.AddWithValue("@townId", townId);

                int minionId = Convert.ToInt32(await minionInsertCommand.ExecuteScalarAsync());


                using SqlCommand minionVillainInsertCommand = new SqlCommand(SqlQueries.InsertMinionVillain, connection, transaction);
                minionVillainInsertCommand.Parameters.AddWithValue("@minionId", minionId);
                minionVillainInsertCommand.Parameters.AddWithValue("@villainId", villainId);
                await minionVillainInsertCommand.ExecuteNonQueryAsync();

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");

                #endregion

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}