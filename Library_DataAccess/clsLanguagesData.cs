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
    public class clsLanguagesData
    {
        public static int? AddNewLanguage(string languageName)
        {
            int? languageID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Languages (LanguageName) VALUES (@LanguageName); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LanguageName", languageName);

                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            languageID = ID;
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
            return languageID;
        }
        public static bool UpdateLanguage(int? languageID, string languageName)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Languages SET LanguageName = @LanguageName WHERE LanguageID = @LanguageID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LanguageName", languageName);
                        command.Parameters.AddWithValue("@LanguageID", languageID);

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
        public static bool Find(int? languageID, ref string languageName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT LanguageName FROM Languages WHERE LanguageID = @LanguageID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LanguageID", languageID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                languageName = reader["LanguageName"].ToString();
                                isFound = true;
                            }
                            else
                            {
                                languageName = null;
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
        public static int? IDOfLanguageName(string languageName)
        {
            int? languageID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT LanguageID FROM Languages WHERE LanguageName = @LanguageName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LanguageName", languageName);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            languageID = ID;
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
            return languageID;
        }
        public static bool Delete(int? languageID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Languages WHERE LanguageID = @LanguageID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LanguageID", languageID);
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

        public static DataTable GetAllLanguages()
        {
            DataTable languagesTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Languages;"; // Fetch all languages
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            languagesTable.Load(reader); // Fill the DataTable with the result
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
            return languagesTable; // Return the filled DataTable
        }

    }
}
