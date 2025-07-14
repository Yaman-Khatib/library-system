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
    public class clsBorrowsData
    {

        //The actual return date will be null so we dont need to insert it
        public static int? AddNewBorrow(int? readerID, int? bookID, DateTime borrowDate, DateTime dueDate, int createdByUserID, string notes)
        {
            int? borrowID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Borrows (ReaderID, BookID, BorrowDate, DueDate,ExtendedDays, CreatedByUserID, Notes) " +
                                   "VALUES (@ReaderID,  @BookID, @BorrowDate, @DueDate,0, @CreatedByUserID, @Notes); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);                        
                        command.Parameters.AddWithValue("@BookID", bookID);
                        command.Parameters.AddWithValue("@BorrowDate", borrowDate);
                        command.Parameters.AddWithValue("@DueDate", dueDate);
                        

                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                        command.Parameters.AddWithValue("@Notes", (notes == null)?(object)DBNull.Value : notes);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            borrowID = ID;
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
            return borrowID;
        }

        public static bool UpdateBorrow(int borrowID, int readerID,  int bookID, DateTime borrowDate, DateTime dueDate, DateTime? actualReturnDate, int extendedDays, int createdByUserID, string notes)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Borrows SET ReaderID = @ReaderID, BookID = @BookID, " +
                                   "BorrowDate = @BorrowDate, DueDate = @DueDate, ExtendedDays = @ExtendedDays, " +
                                   "ActualReturnDate = @ActualReturnDate, CreatedByUserID = @CreatedByUserID, Notes = @Notes " +
                                   "WHERE BorrowID = @BorrowID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BorrowID", borrowID);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        
                        command.Parameters.AddWithValue("@BookID", bookID);
                        command.Parameters.AddWithValue("@BorrowDate", borrowDate);
                        command.Parameters.AddWithValue("@DueDate", dueDate);
                        command.Parameters.AddWithValue("@ExtendedDays", extendedDays);
                        command.Parameters.AddWithValue("@ActualReturnDate", (object)actualReturnDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                        command.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);


                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
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

        public static bool ReturnBook(int? borrowID, string notes)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Borrows SET ActualReturnDate = @DateOfNow , Notes = @Notes " +
                                   "WHERE BorrowID = @BorrowID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BorrowID", borrowID);
                        command.Parameters.AddWithValue("@Notes", (notes != null)? notes : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DateOfNow",DateTime.Now);
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
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

        public static bool ExtendBorrowing(int? borrowID, int? extendDays)
        {
            bool isExtended = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Borrows SET ExtendedDays = ExtendedDays + @ExtendDays, DueDate = DATEADD(day, @ExtendDays, DueDate) " +
                                   "WHERE BorrowID = @BorrowID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BorrowID", borrowID);
                        command.Parameters.AddWithValue("@ExtendDays", extendDays);

                        int rowsAffected = command.ExecuteNonQuery();
                        isExtended = rowsAffected > 0;
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
            return isExtended;
        }


        public static bool DeleteBorrowing(int? borrowID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Borrows WHERE BorrowID = @BorrowID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BorrowID", borrowID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
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


        public static DataTable GetAllBorrowsForReader(int? readerID)
        {
            DataTable borrowsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Borrows WHERE ReaderID = @ReaderID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            borrowsTable.Load(reader);
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
                clsLogEvent.Log(ex);
                // Handle general exception (logging, rethrowing, etc.)
            }
            return borrowsTable;
        }


        public static DataTable GetAllBorrows(int statusIndex, DateTime startDate,DateTime endDate)
        {
            DataTable borrowsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM vBorrows where statusIndex = @statusIndex and BorrowDate >= @startDate and BorrowDate <= @endDate; ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@statusIndex", statusIndex);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            borrowsTable.Load(reader);
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
            return borrowsTable;
        }

        public static int GetActiveBorrowsCopyCountForBook(int bookID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "select count(borrowID) from borrows\r\nwhere ActualReturnDate is null and BookID = @bookID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bookID", bookID);
                    int count = (int)command.ExecuteScalar();
                    return count;
                }

            }
        }

        public static bool FindBorrow(int borrowID, ref int readerID, ref int bookID, ref DateTime borrowDate, ref DateTime dueDate, ref int extendedDays, ref DateTime? actualReturnDate, ref int createdByUserID, ref string notes)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Borrows WHERE BorrowID = @BorrowID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BorrowID", borrowID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                readerID = (int)reader["ReaderID"];
                                
                                bookID = (int)reader["BookID"];
                                borrowDate = (DateTime)reader["BorrowDate"];
                                dueDate = (DateTime)reader["DueDate"];
                                extendedDays = (int)reader["ExtendedDays"];
                                actualReturnDate = reader["ActualReturnDate"] != DBNull.Value ? (DateTime?)reader["ActualReturnDate"] : null;
                                createdByUserID = (int)reader["CreatedByUserID"];
                                notes = (reader["Notes"] == DBNull.Value)? null : reader["Notes"].ToString();

                                isFound = true;
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

        public static int GetNumberOfCurrentlyBorrowedMagazinesByReader(int readerID)
        {
            int borrowedCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = @"SELECT COUNT(1)
FROM Borrows b
INNER JOIN Books bk ON b.BookID = bk.BookID
INNER JOIN BookGenres bg ON bk.GenreID = bg.GenreID
WHERE bg.GenreName = 'Magazine'
  AND b.ReaderID = @ReaderID
  AND b.ActualReturnDate IS NULL;
"; // Ensure the book is currently borrowed

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        borrowedCount = (int)command.ExecuteScalar();
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
            return borrowedCount;
        }
        public static int GetNumberOfCurrentlyBorrowedClassicBooksByReader(int readerID)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = @"
  SELECT COUNT(1)
FROM Borrows b
INNER JOIN Books bk ON b.BookID = bk.BookID
INNER JOIN BookGenres bg ON bk.GenreID = bg.GenreID
WHERE bg.GenreName <> 'Magazine'
  AND b.ReaderID = @ReaderID
  AND b.ActualReturnDate IS NULL;"; // Assuming ActualReturnDate is NULL for currently borrowed books

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        count = (int)command.ExecuteScalar();
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
            return count;
        }


    }
}
