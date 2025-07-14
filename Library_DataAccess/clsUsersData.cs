using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsUsersData
    {
        // Method to add a new user and return the inserted UserID
        public static int? AddNewUser(int PersonID,string UserName, string Password, int Permissions)
        {
            int? UserID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO users (PersonID,UserName, Password, Permissions) " +
                                   "VALUES (@PersonID,@UserName, @Password, @Permissions); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Permissions", Permissions);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            UserID = ID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL error here

            }
            catch (Exception ex)
            {
                // Log general error here

            }
            return UserID;
        }

        // Method to update an existing user
        public static bool UpdateUser(int UserID,string UserName, string Password, int PersonID,  int Permissions)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE users SET PersonID = @PersonID,UserName = @UserName, Password = @Password, " +
                                   "Permissions = @Permissions WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Permissions", Permissions);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@UserName", UserName);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0; // Check if any row was affected
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL error here

            }
            catch (Exception ex)
            {
                // Log general error here

            }
            return isUpdated;
        }

        // Method to find a user by UserID
        public static bool FindByUserID(
            int UserID,
            ref string UserName,
            ref string Password,
            ref int? PersonID,
            ref int? Permissions,
            ref bool isDeleted)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row
                            {
                                PersonID = (int)reader["PersonID"];
                                Password = reader["Password"].ToString();
                                Permissions = (int)reader["Permissions"];
                                UserName = reader["UserName"].ToString();
                                isDeleted = (bool)reader["isDeleted"];
                                isFound = true;
                            }
                            else
                            {
                                PersonID = null;
                                Password = null;
                                Permissions = null;
                                UserName = null;
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL error here

            }
            catch (Exception ex)
            {
                // Log general error here

            }
            return isFound;
        }

        // Method to find a user by PersonID
        public static bool FindByPersonID(
            int PersonID,
            ref int? UserID,
            ref string UserName,
            ref string Password,
            ref int? Permissions,
            ref bool isDeleted
            )
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row
                            {
                                UserID = (int)reader["UserID"];
                                Password = reader["Password"].ToString();
                                Permissions = (int)reader["Permissions"];
                                UserName = reader["UserName"].ToString();
                                isDeleted = (bool)reader["isDeleted"];
                                isFound = true;
                            }
                            else
                            {
                                UserID = null;
                                Password = null;
                                Permissions = null;
                                UserName = null;
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL error here

            }
            catch (Exception ex)
            {
                // Log general error here

            }
            return isFound;
        }

        public static bool GetUserInfoByUserNameAndPassword(
    string userName,
    string password,
    ref int? userID,
    ref int? personID,
    ref string accountNumber,
    ref int? permissions,ref bool IsDeleted)
        {
            bool isAuthenticated = false; // Assume the user is not authenticated
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT UserID, PersonID,Permissions,IsDeleted " +
                                   "FROM users WHERE UserName = @UserName AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password); // Ensure to handle password securely in production

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a row is found
                            {
                                // Fill the reference parameters with user info
                                userID = (int)reader["UserID"];
                                personID = (int)reader["PersonID"];                                
                                permissions = reader["Permissions"] != DBNull.Value ? (int?)reader["Permissions"] : null;
                                IsDeleted = (bool)reader["IsDeleted"];
                                isAuthenticated = true; // User is authenticated
                            }
                            else
                            {
                                // Reset parameters if authentication fails
                                userID = null;
                                personID = null;
                                accountNumber = null;
                                permissions = null;
                                IsDeleted = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL error here
            }
            catch (Exception ex)
            {
                // Handle general error here
            }

            return isAuthenticated; // Return true if authenticated, false otherwise
        }



        public static DataTable GetAllUsers()
    {
        DataTable usersTable = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM vUsersInfo where IsDeleted != 1"; // Fetch all users that arent deleted
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                            usersTable.Load(reader); // Fill the DataTable with the result
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Log SQL error here
            
        }
        catch (Exception ex)
        {
            // Log general error here
            
        }
        return usersTable; // Return the filled DataTable
    }

        public static bool SoftDelete(int UserID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update users set IsDeleted = 1 WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // Check if any row was affected
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
            }

            return isDeleted;
        }

        public static bool DeleteUser(int UserID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "update users set IsDeleted = 1 where userID = @UserID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // Check if any row was affected
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
            }

            return isDeleted;
        }

        public static bool IsUserExistsByUserID(int UserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM users WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        int count = (int)command.ExecuteScalar();
                        isFound = count > 0; // Check if any row exists
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
            }

            return isFound;
        }

        public static bool IsUserExistsByPersonID(int PersonID)
        {
            bool isUser = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM users WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        int count = (int)command.ExecuteScalar();
                        isUser = count > 0; // Check if any user exists for the given PersonID
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
            }

            return isUser;
        }

        public static bool ChangePassword(int UserID, string newPassword)
        {
            bool isChanged = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE users SET Password = @Password WHERE UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@UserID", UserID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isChanged = rowsAffected > 0; // Check if the password was changed
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
            }

            return isChanged;
        }



    }
}

