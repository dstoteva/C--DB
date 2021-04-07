using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05.ChangeTownNamesCasing
{
    class Program
    {
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            string country = Console.ReadLine();

            connection.Open();
            using (connection)
            {
                string updateQueryText = String.Format(@"UPDATE Towns
                                           SET Name = UPPER(Name)
                                           WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = '{0}')", country);
                SqlCommand updateTownsCmd = new SqlCommand(updateQueryText, connection);

                using (updateTownsCmd)
                {
                    int rowsAffected = updateTownsCmd.ExecuteNonQuery();
                    if(rowsAffected == -1 || rowsAffected == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{rowsAffected} town names were affected.");
                        SqlCommand selectTownsCmd = new SqlCommand(String.Format(@"SELECT t.Name 
                                                                    FROM Towns as t
                                                                    JOIN Countries AS c ON c.Id = t.CountryCode
                                                                    WHERE c.Name = '{0}'", country), connection);
                        using (selectTownsCmd)
                        {
                            SqlDataReader reader = selectTownsCmd.ExecuteReader();

                            using (reader)
                            {
                                    List<string> townNames = new List<string>();
                                    while (reader.Read())
                                    {
                                        townNames.Add(reader[0].ToString());
                                    }
                                    Console.WriteLine("[{0}]", String.Join(", ", townNames.ToArray()));
                            }
                        }
                    }
                }
            }
        }
    }
}
