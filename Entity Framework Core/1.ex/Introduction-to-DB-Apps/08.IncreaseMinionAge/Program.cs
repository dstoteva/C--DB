using System;
using System.Data.SqlClient;
using System.Linq;

namespace _08.IncreaseMinionAge
{
    class Program
    {
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            int[] ids = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

            connection.Open();
            using (connection)
            {
                string queryText = @"UPDATE Minions
                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                    WHERE Id = @Id";
                SqlCommand updateMinions = new SqlCommand();
                updateMinions.Connection = connection;
                updateMinions.CommandText = queryText;
                updateMinions.Parameters.AddWithValue("@Id", 0);

                for (int i = 0; i < ids.Length; i++)
                {
                    updateMinions.Parameters["@Id"].Value = ids[i];
                    using (updateMinions)
                    {
                        updateMinions.ExecuteNonQuery();
                    }

                }
                
                SqlCommand listMinions = new SqlCommand("SELECT Name, Age FROM Minions", connection);
                using (listMinions)
                {
                    SqlDataReader reader = listMinions.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                        }
                    }
                }
            }
        }
    }
}
