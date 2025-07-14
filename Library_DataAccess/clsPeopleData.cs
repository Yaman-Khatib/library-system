using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsPeopleData
    {
        public static int? AddNewPerson(string FirstName, string SecondName, string LastName, string MotherName, bool? Gender,
                                        DateTime? BirthDate, string Mobile, string Phone, string Email, string Address,
                                        string FacebookUserName, string InstagramUserName, string School,
                                        int? EmergencyContact1ID, int? EmergencyContact2ID, string ImagePath, string Notes)
        {
            int? PersonID = null;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO People (FirstName, SecondName, LastName, MotherName, Gender, BirthDate, Mobile, Phone, Email, " +
                               "Address, ImagePath, FacebookUserName, InstagramUserName, School, EmergencyContact1ID, EmergencyContact2ID, Notes) " +
                               "VALUES (@FirstName, @SecondName, @LastName, @MotherName, @Gender, @BirthDate, @Mobile, @Phone, @Email, " +
                               "@Address, @ImagePath, @FacebookUserName, @InstagramUserName, @School, @EmergencyContact1ID, @EmergencyContact2ID, @Notes); " +
                               "SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MotherName", MotherName);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@BirthDate", BirthDate);
                    command.Parameters.AddWithValue("@Mobile", Mobile);
                    command.Parameters.AddWithValue("@Phone", String.IsNullOrEmpty(Phone) ? (object)DBNull.Value : Phone);
                    command.Parameters.AddWithValue("@Email", String.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@ImagePath", String.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);
                    command.Parameters.AddWithValue("@FacebookUserName", String.IsNullOrEmpty(FacebookUserName) ? (object)DBNull.Value : FacebookUserName);
                    command.Parameters.AddWithValue("@InstagramUserName", String.IsNullOrEmpty(InstagramUserName) ? (object)DBNull.Value : InstagramUserName);
                    command.Parameters.AddWithValue("@School", String.IsNullOrEmpty(School) ? (object)DBNull.Value : School);
                    command.Parameters.AddWithValue("@EmergencyContact1ID", EmergencyContact1ID.HasValue ? (object)EmergencyContact1ID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@EmergencyContact2ID", EmergencyContact2ID.HasValue ? (object)EmergencyContact2ID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", String.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);

                    object obj = command.ExecuteScalar();
                    if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        PersonID = ID;
                }
            }
            return PersonID;
        }

        public static bool UpdatePerson(int? PersonID, string FirstName, string SecondName, string LastName, string MotherName,
                                        bool? Gender, DateTime? BirthDate, string Mobile, string Phone, string Email,
                                        string Address, string FacebookUserName, string InstagramUserName, string School,
                                        int? EmergencyContact1ID, int? EmergencyContact2ID, string ImagePath, string Notes)
        {
            bool Result = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "UPDATE People SET FirstName = @FirstName, SecondName = @SecondName, LastName = @LastName, MotherName = @MotherName, " +
                               "Gender = @Gender, BirthDate = @BirthDate, Mobile = @Mobile, Phone = @Phone, Email = @Email, Address = @Address, " +
                               "ImagePath = @ImagePath, FacebookUserName = @FacebookUserName, InstagramUserName = @InstagramUserName, " +
                               "School = @School, EmergencyContact1ID = @EmergencyContact1ID, EmergencyContact2ID = @EmergencyContact2ID, Notes = @Notes " +
                               " WHERE PersonID = @PersonID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@MotherName", MotherName);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@BirthDate", BirthDate);
                    command.Parameters.AddWithValue("@Mobile", Mobile);
                    command.Parameters.AddWithValue("@Phone", String.IsNullOrEmpty(Phone) ? (object)DBNull.Value : Phone);
                    command.Parameters.AddWithValue("@Email", String.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@ImagePath", String.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);
                    command.Parameters.AddWithValue("@FacebookUserName", String.IsNullOrEmpty(FacebookUserName) ? (object)DBNull.Value : FacebookUserName);
                    command.Parameters.AddWithValue("@InstagramUserName", String.IsNullOrEmpty(InstagramUserName) ? (object)DBNull.Value : InstagramUserName);
                    command.Parameters.AddWithValue("@School", String.IsNullOrEmpty(School) ? (object)DBNull.Value : School);
                    command.Parameters.AddWithValue("@EmergencyContact1ID", EmergencyContact1ID.HasValue ? (object)EmergencyContact1ID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@EmergencyContact2ID", EmergencyContact2ID.HasValue ? (object)EmergencyContact2ID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", String.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                    
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    int rowsAffected = command.ExecuteNonQuery();
                    Result = rowsAffected > 0;
                }
            }
            return Result;
        }

        public static bool Find(int? PersonID, ref string FirstName, ref string SecondName, ref string LastName, ref string MotherName,
                                ref bool? Gender, ref DateTime? BirthDate, ref string Mobile, ref string Phone, ref string Email,
                                ref string Address, ref string FacebookUserName, ref string InstagramUserName, ref string School,
                                ref int? EmergencyContact1ID, ref int? EmergencyContact2ID, ref string ImagePath, ref string Notes)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM People WHERE PersonID = @PersonID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FirstName = reader["FirstName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            LastName = reader["LastName"].ToString();
                            MotherName = reader["MotherName"].ToString();
                            Gender = (bool)reader["Gender"];
                            BirthDate = (DateTime)reader["BirthDate"];
                            Mobile = reader["Mobile"].ToString();
                            Phone = (reader["Phone"] != DBNull.Value) ? reader["Phone"].ToString() : null;
                            Email = (reader["Email"] != DBNull.Value) ? reader["Email"].ToString() : null;
                            Address = reader["Address"].ToString();
                            ImagePath = (reader["ImagePath"] != DBNull.Value) ? reader["ImagePath"].ToString() : null;
                            FacebookUserName = (reader["FacebookUserName"] != DBNull.Value) ? reader["FacebookUserName"].ToString() : null;
                            InstagramUserName = (reader["InstagramUserName"] != DBNull.Value) ? reader["InstagramUserName"].ToString() : null;
                            School = (reader["School"] != DBNull.Value) ? reader["School"].ToString() : null;
                            EmergencyContact1ID = (reader["EmergencyContact1ID"] != DBNull.Value) ? (int?)reader["EmergencyContact1ID"] : null;
                            EmergencyContact2ID = (reader["EmergencyContact2ID"] != DBNull.Value) ? (int?)reader["EmergencyContact2ID"] : null;
                            Notes = (reader["Notes"] != DBNull.Value) ? reader["Notes"].ToString() : null;
                            IsFound = true;
                        }
                    }
                }
            }
            return IsFound;
        }

        // Other methods (IsPersonExist, GetAllPeople, DeletePerson) remain unchanged
        public static bool IsPersonExist(int? PersonID)
        {
            bool IsFound = false;
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                string query = "Select found=1 from people where personID = @PersonID;";
                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }

                }
            }
            return IsFound;
        }
        public static DataTable GetAllPeople()
        {
            DataTable dataTable = new DataTable();
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "Select * from people;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }

        public static bool DeletePerson(int? PersonID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM people WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // Check if any row was affected
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

            return isDeleted;
        }

    }
}

