using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07.PrintAllMinionNames
{
    class Program
    {
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            connection.Open();
            using (connection)
            {
                SqlCommand getNamesCmd = new SqlCommand("SELECT Name FROM Minions", connection);

                using (getNamesCmd)
                {
                    SqlDataReader reader = getNamesCmd.ExecuteReader();
                    using (reader)
                    {
                        List<string> minionsNames = new List<string>();
                        
                        while (reader.Read())
                        {
                            minionsNames.Add((string)reader["Name"]);
                        }
                        for (int i = 0; i < minionsNames.Count / 2; i++)
                        {
                            Console.WriteLine(minionsNames[i]);
                            Console.WriteLine(minionsNames[minionsNames.Count - (1 + i)]);
                        }
                        if(minionsNames.Count % 2 != 0)
                        {
                            Console.WriteLine(minionsNames[minionsNames.Count / 2]);
                        }
                    }
                }
            }
        }
    }
}
