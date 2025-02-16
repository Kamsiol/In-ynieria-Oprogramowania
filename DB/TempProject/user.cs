﻿using System;
using Microsoft.Data.SqlClient;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class ConnectDB
{
    public static string connectionString = "Server=tcp:mhserverstud.database.windows.net,1433;Initial Catalog=DB_IOPROJECT;Persist Security Info=False;User ID=mh308876@student.polsl.pl;Password=3QoXFU6tRou9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=Active Directory Password";
    public static string dataUser_path = @"C:\Users\Mariia\Desktop\University\sem 5\In-ynieria-Oprogramowania\DB\csvDATA\dataUser.csv";
    public static string adressUser_path = @"C:\Users\Mariia\Desktop\University\sem 5\In-ynieria-Oprogramowania\DB\csvDATA\dataUser.csv";

    public static void ConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

public class User
{
    public class DataUser
    {
        public int IDuser { get; set; }
        public string emailUser { get; set; }
        public int telnumUser { get; set; }
        public string passwordUser { get; set; }
        public string nameUser { get; set; }
        public string surnameUser { get; set; }
        public string birthdayUser { get; set; }
        public string sexUser { get; set; }
    }

    public static List<DataUser> ReadcsvDataUser(string filePath)
    {
        var records = new List<DataUser>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<DataUser>().ToList(); 
        }

        return records;
    }

    public static void InsertDBDataUser(List<DataUser> data, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var record in data)
            {
                string query = @"
                    INSERT INTO DB_IOPROJECT.dbo.userData (IDuser,  emailUser, telnumUser, passwordUser, nameUser, surnameUser, birthdayUser, sexUser)
                    VALUES (@IDuser,  @emailUser, @telnumUser, @passwordUser, @nameUser, @surnameUser, CONVERT(DATE, @birthdayUser, 104), @sexUser);
                ";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@IDuser", record.IDuser);
                    cmd.Parameters.AddWithValue("@emailUser", record.emailUser);
                    cmd.Parameters.AddWithValue("@telnumUser", record.telnumUser);
                    cmd.Parameters.AddWithValue("@passwordUser", record.passwordUser);
                    cmd.Parameters.AddWithValue("@nameUser", record.nameUser);
                    cmd.Parameters.AddWithValue("@surnameUser", record.surnameUser);
                    cmd.Parameters.AddWithValue("@birthdayUser", record.birthdayUser);
                    cmd.Parameters.AddWithValue("@sexUser", record.sexUser);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

public
class Program
{
    public static void Main(string[] args)
    {
        try
        {
            ConnectDB.ConnectToDatabase();

            var userData = User.ReadcsvDataUser(@"C:\Users\Mariia\Desktop\University\sem 5\In-ynieria-Oprogramowania\DB\csvDATA\dataUser.csv");
            Console.WriteLine($"Total records in CSV: {userData.Count}");

            User.InsertDBDataUser(userData, ConnectDB.connectionString);

            Console.WriteLine("Data has been successfully inserted into the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
