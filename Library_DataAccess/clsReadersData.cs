using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsReadersData
    {
        public static int? AddNewReader(string AccountNumber,int PersonID,int CreatedByUserID,DateTime JoinDate)
        {
                int? ReaderID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Insert into readers (AccountNumber,PersonID,CreatedByUserID,JoinDate)" +
                        "Values(@AccountNumber,@PersonID,@CreatedByUserID,@JoinDate);Select Scope_Identity();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@JoinDate", JoinDate);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            ReaderID = ID;
                        }
                        else
                            ReaderID = null;
                    }
                }

            }
            catch (SqlException ex) { }
            catch (Exception ex) { }
            return ReaderID;
        }
        public static bool UpdateReader(int? ReaderID,string AccountNumber, int PersonID,int CreatedByUserID)
        {
            bool IsUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update readers set  AccountNumber = @AccountNumber,PersonID = @PersonID,CreatedByUserID = @CreatedByUserID" +
                        " where ReaderID = @ReaderID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        command.Parameters.AddWithValue("@PersonID", PersonID);                        
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        IsUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            catch (Exception ex) { }

            return IsUpdated;
        }

        public static bool FindByReaderID(
    int ReaderID,
    ref string AccountNumber,
    ref int? PersonID,ref int? CreatedByUserID,
    ref DateTime? JoinDate)
        {
            bool IsFound = false;
            try { 
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM readers WHERE ReaderID = @ReaderID";
                
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row
                            {
                                AccountNumber = reader["AccountNumber"].ToString();
                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                JoinDate = (DateTime)reader["JoinDate"];

                                IsFound = true;
                            }
                            else
                            {
                                AccountNumber = null;
                                PersonID = null;
                                JoinDate = null;
                                CreatedByUserID = null;
                                IsFound = false;
                            }
                        }
                    }

                
            }
            }
                catch (SqlException ex) {
                    //Log data base related errors
                                        }
                catch (Exception ex) { }
                return IsFound;


        }

        public static bool FindByAccountNumber(
    string AccountNumber,
    ref int? ReaderID,
    ref int? PersonID,
    ref int? CreatedByUserID,
    ref DateTime? JoinDate)
        {
                    bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM readers WHERE AccountNumber = @AccountNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row
                            {
                                ReaderID = (int)reader["ReaderID"];
                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                JoinDate = (DateTime)reader["JoinDate"];

                                IsFound = true;
                            }
                            else
                            {
                                ReaderID = null;
                                PersonID = null;
                                JoinDate = null;
                                CreatedByUserID = null;
                                IsFound = false;
                            }
                        }
                    }
                }
            }catch(SqlException ex) { }
            catch(Exception ex) { }
            return IsFound;
        }

        public static bool FindReaderByPersonID(
    int PersonID,
    ref int? ReaderID,
    ref string AccountNumber,
    ref int? CreatedByUserID,
    ref DateTime? JoinDate)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM readers WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row
                            {
                                ReaderID = (int)reader["ReaderID"];
                                AccountNumber = reader["AccountNumber"].ToString();
                                JoinDate = (DateTime)reader["JoinDate"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                //Dont worry the Using will manage resources
                                IsFound = true;
                            }
                            else
                            {
                                ReaderID = null;
                                AccountNumber = null;
                                JoinDate = null;
                                CreatedByUserID = null;
                                IsFound = false;
                            }
                        }
                    }
                }

            } catch(SqlException ex) { }
            catch (Exception ex) { }
            return IsFound;
        }

        /// <summary>
        /// Gets the account number for the last added reader
        /// </summary>
        /// <returns></returns>
        public static String GetLastAccountNumber()
        {
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "select top(1) AccountNumber from Readers order by readerID desc";
                using(SqlCommand command = new SqlCommand(query,connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["AccountNumber"].ToString(); //Return the las account number
                        }
                        else
                            return "";
                    }
                }
            }
        }

        public static bool IsReaderExists(int ReaderID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM readers WHERE ReaderID = @ReaderID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
                        int count = (int)command.ExecuteScalar();
                        IsFound = count > 0;
                    }
                }
            }catch(SqlException ex) { }
            return IsFound;
        }


        public static bool IsReaderDeleted(int ReaderID)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM readers WHERE ReaderID = @ReaderID and IsDeleted = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
                        int count = (int)command.ExecuteScalar();
                        IsDeleted = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsDeleted;
        }

        public static bool IsReaderDeleted(String AccountNumber)
        {
            bool IsDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM readers WHERE AccountNumber = @AccountNumber and IsDeleted = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        int count = (int)command.ExecuteScalar();
                        IsDeleted = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsDeleted;
        }

        public static bool IsReaderExists(string AccountNumber)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM readers WHERE AccountNumber = @AccountNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        int count = (int)command.ExecuteScalar();
                        IsFound = count > 0;
                    }
                }
            }catch (SqlException ex) { }
            return IsFound;
        }

        public static bool IsPersonAReader(int PersonID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM readers WHERE PersonID = @PersonID; ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        int count = (int)command.ExecuteScalar();
                        IsFound = count > 0;
                    }
                }
            }
            catch (SqlException ex) { }
            return IsFound;
        }


        public static bool Delete(int ReaderID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM readers WHERE ReaderID = @ReaderID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
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
        public static bool SoftDelete(int ReaderID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update readers set IsDeleted = 1 where ReaderID = @ReaderID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
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
        public static bool SoftDelete(String AccountNumber)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update readers set IsDeleted = 1 where AccountNumber = @AccountNumber;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
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
        public static bool Delete(string AccountNumber)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM readers WHERE AccountNumber = @AccountNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
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

        public static bool RestoreDeletedReader(string AccountNumber)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update readers Set IsDeleted = 0 WHERE AccountNumber = @AccountNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
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

        public static bool RestoreDeletedReader(int ReaderID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Update readers Set IsDeleted = 0 WHERE ReaderID = @ReaderID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", ReaderID);
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




        public static DataTable GetAllReaders()
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "Select * from vReadersDetails;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex) { }
            catch (Exception ex) { }
            return dataTable;
        }


    }
}
