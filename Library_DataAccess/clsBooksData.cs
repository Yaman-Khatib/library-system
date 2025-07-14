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
    public class clsBooksData
    {        

        public static int? AddNew(string title, string author, string isbn, string serialNumber, int genreID, int languageID,  int copiesCount, string description)
        {
            int? bookID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO books (Title, Author, ISBN, SerialNumber, GenreID, LanguageID,  CopiesCount, Description) " +
                                   "VALUES (@Title, @Author, @ISBN, @SerialNumber, @GenreID, @LanguageID,  @CopiesCount, @Description); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@ISBN", isbn?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                        command.Parameters.AddWithValue("@GenreID", genreID);
                        command.Parameters.AddWithValue("@LanguageID", languageID);
                       
                        command.Parameters.AddWithValue("@CopiesCount", copiesCount);
                        command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);

                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            bookID = ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return bookID;
        }

        public static bool Update(int bookID, string title, string author, string isbn, string serialNumber, int genreID, int languageID,  int copiesCount, string description)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE books SET Title = @Title, Author = @Author, ISBN = @ISBN, SerialNumber = @SerialNumber, " +
                                   "GenreID = @GenreID, LanguageID = @LanguageID, CopiesCount = @CopiesCount, Description = @Description " +
                                   "WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@ISBN", isbn?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                        command.Parameters.AddWithValue("@GenreID", genreID);
                        command.Parameters.AddWithValue("@LanguageID", languageID);
                        
                        command.Parameters.AddWithValue("@CopiesCount", copiesCount);
                        command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@BookID", bookID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (logging, rethrowing, etc.)
            }
            return isUpdated;
        }

        public static bool FindBySerialNumber(string serialNumber, ref int? bookID, ref string title, ref string author, ref string isbn, ref int? genreID, ref int? languageID,  ref int? copiesCount, ref string description)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM books WHERE SerialNumber = @SerialNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                bookID = (int)reader["BookID"];
                                title = reader["Title"].ToString();
                                author = reader["Author"].ToString();
                                isbn = (reader["ISBN"] != (object)DBNull.Value)? reader["ISBN"].ToString() : null;
                                genreID = reader["GenreID"] != DBNull.Value ? (int?)reader["GenreID"] : null;
                                languageID = reader["LanguageID"] != DBNull.Value ? (int?)reader["LanguageID"] : null;
                                
                                copiesCount = reader["CopiesCount"] != DBNull.Value ? (int?)reader["CopiesCount"] : null;
                                description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
                                isFound = true;
                            }
                            else
                            {
                                // Reset the output parameters if the book is not found
                                bookID = null;
                                title = null;
                                author = null;
                                isbn = null;
                                genreID = null;
                                languageID = null;
                                
                                copiesCount = null;
                                description = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (logging, rethrowing, etc.)
                clsLogEvent.Log(ex);
            }
            return isFound;
        }


        public static bool UpdateCopiesCount(int bookID, int newCopiesCount)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE books SET CopiesCount = @CopiesCount WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CopiesCount", newCopiesCount);
                        command.Parameters.AddWithValue("@BookID", bookID);

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


        


        public static bool Delete(int bookID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update books Set isdeleted = 1 WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookID);
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

        public static bool RestoreDeletedBook(int bookID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update books Set isdeleted = 0 WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookID);
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

        public static bool RestoreDeletedBook(String BookNumber)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update books Set isdeleted = 0 WHERE SerialNumber = @BookNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookNumber", BookNumber);
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


        public static bool IsBookDeleted(String SerialNumber)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Books WHERE SerialNumber = @SerialNumber and IsDeleted = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        int count = (int)command.ExecuteScalar();
                        IsDeleted = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsDeleted;
        }
        public static bool IsBookDeleted(int BookID)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Books WHERE BookID = @BookID and IsDeleted = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", BookID);
                        int count = (int)command.ExecuteScalar();
                        IsDeleted = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsDeleted;
        }



        

        //Find by BookID
        public static bool Find(int bookID, ref string title, ref string author, ref string isbn,ref string serialNumber, ref int? genreID, ref int? languageID,  ref int? copiesCount,ref string Description)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM books WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                title = reader["Title"].ToString();
                                author = reader["Author"].ToString();
                                isbn = (reader["ISBN"] != (object)DBNull.Value) ? reader["ISBN"].ToString() : null;
                                genreID = reader["GenreID"] != DBNull.Value ? (int?)reader["GenreID"] : null;
                                languageID = reader["LanguageID"] != DBNull.Value ? (int?)reader["LanguageID"] : null;
                                
                                copiesCount = reader["CopiesCount"] != DBNull.Value ? (int?)reader["CopiesCount"] : null;
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
                                serialNumber = reader["SerialNumber"].ToString();
                                isFound = true;
                            }
                            else
                            {
                                // Reset the output parameters if the book is not found
                                title = null;
                                author = null;
                                isbn = null;
                                genreID = null;
                                languageID = null;
                                
                                copiesCount = null;
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


        //Find by Book title
        public static bool FindByTitle(string title, ref int? bookID, ref string author, ref string isbn, ref string serialNumber, ref int? genreID, ref int? languageID,  ref int? copiesCount, ref string Description)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM books WHERE title = @title";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", isbn);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                bookID = (int)reader["BookID"];
                                isbn = (reader["ISBN"] != (object)DBNull.Value) ? reader["ISBN"].ToString() : null;
                                author = reader["Author"].ToString();
                                genreID = reader["GenreID"] != DBNull.Value ? (int?)reader["GenreID"] : null;
                                languageID = reader["LanguageID"] != DBNull.Value ? (int?)reader["LanguageID"] : null;
                                
                                copiesCount = reader["CopiesCount"] != DBNull.Value ? (int?)reader["CopiesCount"] : null;
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
                                serialNumber = reader["SerialNumber"].ToString();
                                isFound = true;
                            }
                            else
                            {
                                // Reset the output parameters if the book is not found
                                bookID = null;
                                isbn = null;
                                author = null;
                                genreID = null;
                                languageID = null;
                                
                                copiesCount = null;
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


        ////Find by ISBN
        //public static bool FindByISBN(string isbn, ref int? bookID, ref string title, ref string author,ref string serialNumber, ref int? genreID, ref int? languageID, ref int? copiesCount,  ref string Description)
        //{
        //    bool isFound = false;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
        //        {
        //            connection.Open();
        //            string query = "SELECT * FROM books WHERE ISBN = @ISBN";

        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                command.Parameters.AddWithValue("@ISBN", isbn);
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read()) // If a matching record is found
        //                    {
        //                        bookID = (int)reader["BookID"];
        //                        title = reader["Title"].ToString();
        //                        author = reader["Author"].ToString();
        //                        genreID = reader["GenreID"] != DBNull.Value ? (int?)reader["GenreID"] : null;
        //                        languageID = reader["LanguageID"] != DBNull.Value ? (int?)reader["LanguageID"] : null;                                
        //                        copiesCount = reader["CopiesCount"] != DBNull.Value ? (int?)reader["CopiesCount"] : null;
        //                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;
        //                        serialNumber = reader["SerialNumber"].ToString();
        //                        isFound = true;
        //                    }
        //                    else
        //                    {
        //                        // Reset the output parameters if the book is not found
        //                        bookID = null;
        //                        title = null;
        //                        author = null;
        //                        genreID = null;
        //                        languageID = null;                                
        //                        copiesCount = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        // Handle SQL exception (logging, rethrowing, etc.)
        //        clsLogEvent.Log(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle general exception (logging, rethrowing, etc.)
        //        clsLogEvent.Log(ex);
        //    }
        //    return isFound;
        //}

        public static DataTable GetAllBooks(string Language)
        {
            DataTable booksTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM vBooks where Language = @Language And IsDeleted != 1;"; // Fetch all undeleted books

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Language", Language);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            booksTable.Load(reader); // Load the DataTable with the data from the reader
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
            return booksTable; // Return the filled DataTable
        }
        public static DataTable GetAllAuthors()
        {
            DataTable AuthorsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT distinct Author FROM Books ;"; // Fetch all genres
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            AuthorsTable.Load(reader); // Fill the DataTable with the result
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
            return AuthorsTable; // Return the filled DataTable
        }
    }
}
