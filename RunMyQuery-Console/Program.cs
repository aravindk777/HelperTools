using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace RunMyQuery_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Beginning the Execution...");
            ExecuteQuery();
            Console.WriteLine("Enter any key to exit...");
            Console.ReadLine();
        }

        static void ExecuteQuery()
        {
            string queryText = string.Empty;
            SqlConnection executeQueryConn = GetConnection();
            if (executeQueryConn.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("connection opened successfully. ");
                try
                {
                    Console.WriteLine("Select your choice of input: Press (1)-Type query | (2)-Read from file");
                    ConsoleKeyInfo opted = Console.ReadKey(true);

                    switch (opted.Key)
                    {
                        case ConsoleKey.NumPad1:
                        case ConsoleKey.D1:
                            Console.WriteLine("You have selected to type your own query.");
                            queryText = GetQueryFromConsole();
                            break;

                        case ConsoleKey.NumPad2:
                        case ConsoleKey.D2:
                            Console.WriteLine("You have selected to read query from the file.");
                            queryText = GetQueryFromFile();
                            break;
                    }

                    if (!string.IsNullOrEmpty(queryText))
                    {

                        SqlCommand executeCmd = new SqlCommand(queryText, executeQueryConn);
                        int resultSetsAffected = executeCmd.ExecuteNonQuery();

                        Console.WriteLine(string.Format("<{0}> - record(s) affected!", resultSetsAffected.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error occured: " + ex.Message);
                }
            }
            else
            {
                Console.Error.WriteLine("Error occurred in opening up a connection. Check your connection string!");
            }
        }

        private static string GetQueryFromFile()
        {
            string query = string.Empty;
            Console.Write("Enter the path of the file: ");
            string filePath = Console.ReadLine();
            if (!string.IsNullOrEmpty(filePath))
            {
                query = File.ReadAllText(filePath);
            }
            return query;
        }

        private static string GetQueryFromConsole()
        {
            Console.Write("Enter your query here: ");
            string query = Console.ReadLine();

            return query;
        }

        private static SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error occured: " + ex.Message);
            }
            return connection;
        }

        /// <summary>
        /// GetConnectionString
        /// </summary>
        /// <returns>connectionString</returns>
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProdConnect"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.Error.WriteLine("No config found!");
            }

            return connectionString;
        }
    }
}
