using Microsoft.Data.SqlClient;

namespace DemoAdoDotNet
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server = DESKTOP-ME8RHIA; Database = SoftUni; Trusted_Connection = True; TrustServerCertificate = True; MultipleActiveResultSets=true;";

            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string query = "SELECT FirstName, LastName, JobTitle FROM Employees WHERE Salary > @salaryParam";

                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@salaryParam", 50000);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        string firstName = sqlDataReader["FirstName"].ToString();
                        string lastName = sqlDataReader["LastName"].ToString(); 
                        string jobTitle = sqlDataReader["JobTitle"].ToString();

                        Console.WriteLine($"{firstName} {lastName} - {jobTitle}");
                    }
                }
            }
        }
    }
}