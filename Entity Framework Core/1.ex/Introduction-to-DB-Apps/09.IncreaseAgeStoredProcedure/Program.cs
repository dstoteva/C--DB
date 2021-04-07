using System;
using System.Data.SqlClient;

namespace _09.IncreaseAgeStoredProcedure
{
    class Program
    {
        //Enter server name
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());
            connection.Open();
            using (connection)
            {
                SqlCommand increaseAgeCmd = new SqlCommand("usp_GetOlder", connection);
                increaseAgeCmd.CommandType = System.Data.CommandType.StoredProcedure;
                increaseAgeCmd.Parameters.AddWithValue("@id", id);

                increaseAgeCmd.ExecuteNonQuery();

                SqlCommand getMinion = new SqlCommand("SELECT Name, Age FROM Minions WHERE Id = @Id", connection);
                getMinion.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = getMinion.ExecuteReader();
                reader.Read();
                Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
            }
        }
    }
}
