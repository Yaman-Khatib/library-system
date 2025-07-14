using DVLD.Global_Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsBookGenresData
    {
        public static int? AddNewGenre(string genreName)
        {
            int? genreID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO BookGenres (GenreName) VALUES (@GenreName); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreName", genreName);

                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            genreID = ID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return genreID;
        }

        public static bool UpdateGenre(int? genreID, string genreName)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE BookGenres SET GenreName = @GenreName WHERE GenreID = @GenreID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreName", genreName);
                        command.Parameters.AddWithValue("@GenreID", genreID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0; // Check if any row was affected
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return isUpdated;
        }


        public static bool Find(int? genreID, ref string genreName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT GenreName FROM BookGenres WHERE GenreID = @GenreID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreID", genreID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                genreName = reader["GenreName"].ToString();
                                isFound = true;
                            }
                            else
                            {
                                genreName = null;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return isFound;
        }


        public static int? IDOfGenreName(string genreName)
        {
            int? genreID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT GenreID FROM BookGenres WHERE GenreName = @GenreName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreName", genreName);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            genreID = ID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return genreID;
        }


        public static bool Delete(int? genreID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM BookGenres WHERE GenreID = @GenreID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreID", genreID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // Check if any row was affected
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return isDeleted;
        }
        public static DataTable GetAllGenres()
        {
            DataTable genresTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT  * FROM BookGenres ;"; // Fetch all genres
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            genresTable.Load(reader); // Fill the DataTable with the result
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                // Handle general exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return genresTable; // Return the filled DataTable
        }
    }


}
