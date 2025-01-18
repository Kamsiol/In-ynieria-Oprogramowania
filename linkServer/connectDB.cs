using System;
using System.IO;
using Microsoft.Data.SqlClient;

public class ConnectDB
{
    public static void ConnectToDatabase()
    {
        try
        {
            // Динамічно визначаємо шлях до файлу
            string directoryPath = Directory.GetCurrentDirectory(); // Поточна папка
            string[] files = Directory.GetFiles(directoryPath, "connection_string_*.txt");

            if (files.Length == 0)
            {
                Console.WriteLine("No connection string file found.");
                return;
            }

            // Беремо останній створений файл
            string latestFile = files[0];
            foreach (string file in files)
            {
                if (File.GetCreationTime(file) > File.GetCreationTime(latestFile))
                {
                    latestFile = file;
                }
            }

            // Читаємо рядок підключення з файлу
            string connectionString = File.ReadAllText(latestFile);

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string is empty.");
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
