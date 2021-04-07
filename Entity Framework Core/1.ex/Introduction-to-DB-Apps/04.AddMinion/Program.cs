using System;
using System.Data.SqlClient;

namespace AddMinion
{
    class Program
    {
        private static string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true;";
        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            string[] minionArgs = Console.ReadLine().Split(" ");
            string minionName = minionArgs[1];
            int minionAge = int.Parse(minionArgs[2]);
            string town = minionArgs[3];
            string villainName = Console.ReadLine().Split(" ")[1];

            connection.Open();
            using (connection)
            {
                //Searches for town, if not found - adds it to the DB + saves townId for later
                int townId = -1;
                SqlCommand searchTownsCmd = new SqlCommand($"SELECT Id FROM Towns WHERE Name = '{town}'", connection);
                using (searchTownsCmd)
                {
                    var returnedTownId = searchTownsCmd.ExecuteScalar();
                    townId = returnedTownId == null ? 0 : (int)returnedTownId;

                    if (townId == 0)
                    {
                        SqlCommand insertTown = new SqlCommand($"INSERT INTO Towns(Name) VALUES('{town}')", connection);
                        using (insertTown)
                        {
                            insertTown.ExecuteNonQuery();
                            Console.WriteLine($"Town {town} was added to the database.");
                        }
                        townId = (int)searchTownsCmd.ExecuteScalar();
                    }
                }
                //Searches for villain, if not found - adds it to the DB + saves villainId for later
                int villainId = -1;
                SqlCommand searchVillainsCmd = new SqlCommand($"SELECT Id FROM Villains WHERE Name = '{villainName}'", connection);
                using (searchVillainsCmd)
                {
                    var returnedVillainId = searchVillainsCmd.ExecuteScalar();
                    villainId = returnedVillainId == null ? 0 : (int)returnedVillainId;

                    if (villainId == 0)
                    {
                        SqlCommand insertVillain = new SqlCommand($"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{villainName}', 4)", connection);
                        using (insertVillain)
                        {
                            insertVillain.ExecuteNonQuery();
                            Console.WriteLine($"Villain {villainName} was added to the database.");
                        }
                        villainId = (int)searchVillainsCmd.ExecuteScalar();
                    }
                }
                //Adding minion
                SqlCommand addMinionCmd = new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, {townId})", connection);
                using (addMinionCmd)
                {
                    addMinionCmd.ExecuteNonQuery();
                }
                //Selecting minionId
                int minionId = -1;
                SqlCommand getMinionId = new SqlCommand($"SELECT Id FROM Minions WHERE Name = '{minionName}'", connection);
                using (getMinionId)
                {
                    minionId = (int)getMinionId.ExecuteScalar();
                }
                //Adding minion as servant
                SqlCommand addMinionAndVillainCmd = new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({minionId}, {villainId})", connection);
                using (addMinionAndVillainCmd)
                {
                    addMinionAndVillainCmd.ExecuteNonQuery();
                }

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }
    }
}
