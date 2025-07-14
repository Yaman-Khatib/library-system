using DVLD.Global_Classes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_DataAccess
{
    public class clsCourseEnrolmentData
    {
        public static int? AddNewEnrolment(int courseID, int readerID, int paymentID, DateTime courseEnrolmentDate, string notes, int? createdByUserID)
        {
            int? courseEnrolmentID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO CoursesEnrolments (CourseID, ReaderID, PaymentID, CourseEnrolmentDate, Notes, CreatedByUserID) " +
                                   "VALUES (@CourseID, @ReaderID, @PaymentID, @CourseEnrolmentDate, @Notes, @CreatedByUserID); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseID);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@PaymentID", paymentID);
                        command.Parameters.AddWithValue("@CourseEnrolmentDate", courseEnrolmentDate);
                        command.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByUserID", (object)createdByUserID ?? DBNull.Value);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            courseEnrolmentID = ID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return courseEnrolmentID;
        }

        public static bool UpdateEnrolment(int courseEnrolmentID, int courseID, int readerID, int paymentID, DateTime courseEnrolmentDate, string notes, int? createdByUserID)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE CoursesEnrolments SET CourseID = @CourseID, ReaderID = @ReaderID, PaymentID = @PaymentID, " +
                                   "CourseEnrolmentDate = @CourseEnrolmentDate, Notes = @Notes, CreatedByUserID = @CreatedByUserID " +
                                   "WHERE CourseEnrolmentID = @CourseEnrolmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseEnrolmentID", courseEnrolmentID);
                        command.Parameters.AddWithValue("@CourseID", courseID);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@PaymentID", paymentID);
                        command.Parameters.AddWithValue("@CourseEnrolmentDate", courseEnrolmentDate);
                        command.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByUserID", (object)createdByUserID ?? DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isUpdated;
        }

        public static bool DeleteEnrolment(int courseEnrolmentID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM CoursesEnrolments WHERE CourseEnrolmentID = @CourseEnrolmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseEnrolmentID", courseEnrolmentID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isDeleted;
        }

        public static bool FindEnrolment(int courseEnrolmentID, ref int? courseID, ref int? readerID, ref int? paymentID, ref DateTime? courseEnrolmentDate, ref string notes, ref int? createdByUserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM CoursesEnrolments WHERE CourseEnrolmentID = @CourseEnrolmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseEnrolmentID", courseEnrolmentID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                courseID = reader["CourseID"] != DBNull.Value ? (int?)reader["CourseID"] : null;
                                readerID = reader["ReaderID"] != DBNull.Value ? (int?)reader["ReaderID"] : null;
                                paymentID = reader["PaymentID"] != DBNull.Value ? (int?)reader["PaymentID"] : null;
                                courseEnrolmentDate = reader["CourseEnrolmentDate"] != DBNull.Value ? (DateTime?)reader["CourseEnrolmentDate"] : null;
                                notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null;
                                createdByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isFound;
        }

        public static bool IsReaderEnrolled(int readerID, int courseID)
        {
            bool isEnrolled = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM CoursesEnrolments WHERE ReaderID = @ReaderID AND CourseID = @CourseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@CourseID", courseID);

                        // ExecuteScalar is used to return the first column of the first row in the result set
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        isEnrolled = count > 0; // If count is greater than 0, the reader is enrolled
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isEnrolled;
        }


        public static DataTable GetAllEnrollments()
        {
            DataTable enrollmentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM CoursesEnrolments";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            enrollmentsTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return enrollmentsTable;
        }

        public static DataTable GetAllEnrollments(int courseID)
        {
            DataTable enrollmentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM vEnrollments WHERE CourseID = @CourseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            enrollmentsTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return enrollmentsTable;
        }

        public static int GetAllEnrollmentsCount(int courseID)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM CoursesEnrolments WHERE CourseID = @CourseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseID);

                        object obj = command.ExecuteScalar();
                        int.TryParse(obj.ToString(), out count);
                        return count;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return count;
        }

        public static DataTable GetAllEnrollmentsForReader(int readerID)
        {
            DataTable enrollmentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM CoursesEnrolments WHERE ReaderID = @ReaderID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            enrollmentsTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogEvent.Log(ex);
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return enrollmentsTable;
        }
    }
}
