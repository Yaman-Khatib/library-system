using DVLD.Global_Classes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_DataAccess
{
    public class clsTutorsData
    {
        // Add a new Tutor
        public static int? AddNewTutor(int personID, int createdByUserID)
        {
            int? tutorID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Tutors (PersonID, CreatedByUserID) VALUES (@PersonID, @CreatedByUserID); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", personID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            tutorID = id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return tutorID;
        }

        // Update an existing Tutor
        public static bool UpdateTutor(int tutorID, int personID, int createdByUserID)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Tutors SET PersonID = @PersonID, CreatedByUserID = @CreatedByUserID WHERE TutorID = @TutorID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TutorID", tutorID);
                        command.Parameters.AddWithValue("@PersonID", personID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isUpdated;
        }

        // Delete a Tutor
        public static bool DeleteTutor(int tutorID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Tutors WHERE TutorID = @TutorID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TutorID", tutorID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isDeleted;
        }

        // Find a Tutor by ID
        public static bool FindTutor(int tutorID, ref int? personID, ref int? createdByUserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT PersonID, CreatedByUserID FROM Tutors WHERE TutorID = @TutorID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TutorID", tutorID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                personID = reader["PersonID"] != DBNull.Value ? (int?)reader["PersonID"] : null;
                                createdByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return isFound;
        }

        // Get tutor ID by name
        public static int? GetIDByTutorName(string name)
        {
            int? tutorID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT TutorID FROM Tutors t INNER JOIN People p ON t.PersonID = p.PersonID WHERE p.FirstName + ' ' + p.LastName = @name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tutorID = reader["TutorID"] != DBNull.Value ? (int?)reader["TutorID"] : null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return tutorID;
        }

        // Get all Tutors
        public static DataTable GetAllTutors()
        {
            DataTable tutorsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM vTutors;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            tutorsTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return tutorsTable;
        }
    }
}
