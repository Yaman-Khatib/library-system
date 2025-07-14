using DVLD.Global_Classes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_DataAccess
{
    public class clsCoursesData
    {
        public static int? AddNewCourse(int courseTypeID, DateTime startDate, DateTime endDate, decimal fees, int? maxParticipants,int TutorID, string notes)
        {
            int? courseID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Courses (CourseTypeID, StartDate, EndDate, Fees, MaxParticipants,TutorID, Notes) " +
                                   "VALUES (@CourseTypeID, @StartDate, @EndDate, @Fees, @MaxParticipants,@TutorID , @Notes); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseTypeID", courseTypeID);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@Fees", fees);
                        command.Parameters.AddWithValue("@TutorID", TutorID); 
                        command.Parameters.AddWithValue("@MaxParticipants", (object)maxParticipants ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            courseID = ID;
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
            return courseID;
        }

        public static bool UpdateCourse(int courseID, int courseTypeID, DateTime startDate, DateTime endDate, decimal fees, int? maxParticipants,int TutorID, string notes)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Courses SET CourseTypeID = @CourseTypeID, StartDate = @StartDate, EndDate = @EndDate, " +
                                   "Fees = @Fees, MaxParticipants = @MaxParticipants,TutorID = @TutorID, Notes = @Notes " +
                                   "WHERE CourseID = @CourseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseID);
                        command.Parameters.AddWithValue("@CourseTypeID", courseTypeID);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@Fees", fees);
                        command.Parameters.AddWithValue("@TutorID", TutorID); 
                        command.Parameters.AddWithValue("@MaxParticipants", (object)maxParticipants ?? DBNull.Value);
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

        public static bool DeleteCourse(int courseID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    // First, delete related enrollments
                    string deleteEnrollmentsQuery = "DELETE FROM CoursesEnrolments WHERE CourseID = @CourseID";
                    using (SqlCommand deleteEnrollmentsCommand = new SqlCommand(deleteEnrollmentsQuery, connection))
                    {
                        deleteEnrollmentsCommand.Parameters.AddWithValue("@CourseID", courseID);
                        deleteEnrollmentsCommand.ExecuteNonQuery();
                    }


                    string query = "DELETE FROM Courses WHERE CourseID = @CourseID";

                    using (SqlCommand deletCourseCommand = new SqlCommand(query, connection))
                    {
                        deletCourseCommand.Parameters.AddWithValue("@CourseID", courseID);

                        int rowsAffected = deletCourseCommand.ExecuteNonQuery();
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

        public static bool IsCourseDeleted(int CourseID)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM courses WHERE CourseID = @CourseID and IsDeleted = 1;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", CourseID);
                        int count = (int)command.ExecuteScalar();
                        IsDeleted = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsDeleted;
        }

        public static bool FindCourse(int courseID, ref int? courseTypeID, ref DateTime? startDate, ref DateTime? endDate, ref decimal? fees, ref int? maxParticipants,ref int? TutorID, ref string notes)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Courses WHERE CourseID = @CourseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                courseTypeID = (int?)reader["CourseTypeID"];
                                startDate = (DateTime?)reader["StartDate"];
                                endDate = (DateTime?)reader["EndDate"];
                                fees = (decimal)reader["Fees"];
                                
                                maxParticipants = reader["MaxParticipants"] != DBNull.Value ? (int?)reader["MaxParticipants"] : null;
                                notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null;
                                TutorID = (int)reader["TutorID"];
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

        public static DataTable GetAllCourses(DateTime StartDate, DateTime EndDate)
        {
            DataTable coursesTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Select * from vCourses where (IsDeleted != 1) and (StartDate between @StartDate and @EndDate) Order by(StartDate) Desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", StartDate);
                        command.Parameters.AddWithValue("@EndDate", EndDate);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            coursesTable.Load(reader);
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
            return coursesTable;
        }
    }
}
