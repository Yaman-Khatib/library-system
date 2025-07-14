using DVLD.Global_Classes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_DataAccess
{
    public class clsEmergencyContactData
    {
        public static int AddNewEmergencyContact(string Name, string PhoneNumber, string Relation)
        {
            int ContactID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO EmergencyContacts (Name, PhoneNumber, Relation) " +
                                   "VALUES (@Name, @PhoneNumber, @Relation); " +
                                   "SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@Relation", Relation);
                        object obj = command.ExecuteScalar();
                        int.TryParse(obj.ToString(), out ContactID);
                    }
                }
            }
            catch(Exception ex) { clsLogEvent.Log(ex); }
            return ContactID;
        }

        public static bool Find(int contactID, ref string Name, ref string phoneNumber, ref string relation)
        {
            bool found = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name,PhoneNumber, Relation FROM EmergencyContacts WHERE ContactID = @ContactID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", contactID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Name = reader["Name"].ToString();
                                phoneNumber = reader["PhoneNumber"].ToString();
                                relation = reader["Relation"].ToString();
                                found = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex){ clsLogEvent.Log(ex); }
            return found;
        }

        public static bool UpdateEmergencyContact(int? ContactID, string Name, string PhoneNumber, string Relation)
        {
            bool Result = false;
            try
            {


                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE EmergencyContacts SET Name = @Name,PhoneNumber = @PhoneNumber, Relation = @Relation " +
                                   "WHERE ContactID = @ContactID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", ContactID);
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@Relation", Relation);

                        int rowsAffected = command.ExecuteNonQuery();
                        Result = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex) { clsLogEvent.Log(ex); }
            return Result;
        }

        public static bool DeleteEmergencyContact(int? ContactID)
        {
            bool isDeleted = false;
            try
            {


                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM EmergencyContacts WHERE ContactID = @ContactID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ContactID", ContactID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
                    }
                }
            }catch (Exception ex) { clsLogEvent.Log(ex); }
            return isDeleted;
        }
        
        public static DataTable GetEmergencyContactsByPerson(int? PersonID)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM EmergencyContacts WHERE PersonID = @PersonID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex) { clsLogEvent.Log(ex); }
            return dataTable;
        }
    }
}
