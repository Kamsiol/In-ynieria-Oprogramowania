using System;
using System.Data.SqlClient;

public class LoginDAO
{
    private string _connectionString;
    
    public bool LoginUser(string email, string password)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM userData WHERE emailUser = @Email AND passwordUser = @Password";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
    }


    public bool RegisterUser(string email, string name, string surname, string password, string telnum)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(1) FROM userData WHERE emailUser = @Email OR telnumUser = @Telnum";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Email", email);
                    checkCommand.Parameters.AddWithValue("@Telnum", telnum);

                    int existingUser = (int)checkCommand.ExecuteScalar();
                    if (existingUser > 0)
                    {
                        throw new Exception("Email or phone number already exists");
                    }
                }

                string insertQuery = "INSERT INTO userData (emailUser, nameUser, surnameUser, passwordUser, telnumUser) " +
                                     "VALUES (@Email, @Name, @Surname, @Password, @Telnum)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Surname", surname);
                    insertCommand.Parameters.AddWithValue("@Password", password);
                    insertCommand.Parameters.AddWithValue("@Telnum", telnum);

                    insertCommand.ExecuteNonQuery();
                }
            }
            return true; 
        }
        catch (Exception)
        {
            return false;
        }
    }
}
