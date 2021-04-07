using System;
using System.Data.SqlClient;

namespace _03.MinionNames
{
    class Program
    {
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            string selectVillianName = String.Format(@"SELECT Name FROM Villains WHERE Id = {0}", id);
            string selectMinions = String.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = {0}
                                ORDER BY m.Name", id);

            connection.Open();

            using (connection)
            {
                SqlCommand selectVilianNameCmd = new SqlCommand(selectVillianName, connection);

                using (selectVilianNameCmd)
                {
                    string villianName = (string)selectVilianNameCmd.ExecuteScalar();

                    if (String.IsNullOrEmpty(villianName))
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"Villain: {villianName}");
                    }
                }


                SqlCommand selectMinionsCmd = new SqlCommand(selectMinions, connection);
                using (selectMinionsCmd)
                {
                    SqlDataReader reader = selectMinionsCmd.ExecuteReader();

                    using (reader)
                    {
                        bool hasMinions = reader.HasRows;

                        if (!hasMinions)
                        {
                            Console.WriteLine("(no minions)");
                        }
                        else
                        {
                            using (reader)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
                                }
                            }
                        }
                    }
                }
                    
            }
        }
    }
}
