using System;
using Microsoft.Data.SqlClient;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class ConnectDB
{
    public static string connectionString = "Server=tcp:mhserverstud.database.windows.net,1433;Initial Catalog=DB_IOPROJECT;Persist Security Info=False;User ID=mh308876@student.polsl.pl;Password=3QoXFU6tRou9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=Active Directory Password";

    public static void ConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}