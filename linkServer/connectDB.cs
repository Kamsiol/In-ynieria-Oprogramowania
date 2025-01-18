using System;
using Microsoft.Data.SqlClient;

public class ConnectDB
{
    public static void ConnectToDatabase()
    {
        try
        {
            //єдиний рядок, який по-суті тут треба. конект до бд. потестити, чи все ок
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string not found in environment variables.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static void Main()
    {
        ConnectToDatabase();
    }
}
