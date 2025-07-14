using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsSubscriptionTypesData
    {
        public static int? AddNewType(int subscriptionMonths, double subscriptionTypeFees,string SubscriptionTypeName)
        {
            int? subscriptionTypeID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO SubscriptionTypes (SubscriptionMonths, SubscriptionTypeFees,SubscriptionTypeName) " +
                                   "VALUES (@SubscriptionMonths, @SubscriptionTypeFees,@SubscriptionTypeName); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionMonths", subscriptionMonths);
                        command.Parameters.AddWithValue("@SubscriptionTypeFees", subscriptionTypeFees);
                        command.Parameters.AddWithValue("@SubscriptionTypeName", SubscriptionTypeName);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            subscriptionTypeID = ID;
                        }
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
            return subscriptionTypeID;
        }
        public static bool DeleteType(int subscriptionTypeID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM SubscriptionTypes WHERE SubscriptionTypeID = @SubscriptionTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);
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
        public static bool UpdateType(int subscriptionTypeID, int subscriptionMonths, double subscriptionTypeFees,string SubscriptionTypeName)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE SubscriptionTypes SET SubscriptionMonths = @SubscriptionMonths, " +
                                   "SubscriptionTypeFees = @SubscriptionTypeFees , SubscriptionTypeName = @SubscriptionTypeName WHERE SubscriptionTypeID = @SubscriptionTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);
                        command.Parameters.AddWithValue("@SubscriptionMonths", subscriptionMonths);
                        command.Parameters.AddWithValue("@SubscriptionTypeFees", subscriptionTypeFees);
                        command.Parameters.AddWithValue("@SubscriptionTypeName", SubscriptionTypeName);
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
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
            return isUpdated;
        }
        public static bool FindTypeInfo(int subscriptionTypeID, ref int? subscriptionMonths, ref double? subscriptionTypeFees,ref string SubscriptionTypeName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT SubscriptionTypeMonths, SubscriptionTypeFees,SubscriptionTypeName FROM SubscriptionTypes WHERE SubscriptionTypeID = @SubscriptionTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                subscriptionMonths = (int?)reader["SubscriptionTypeMonths"];
                                subscriptionTypeFees = (double?)reader["SubscriptionTypeFees"];
                                SubscriptionTypeName = reader["SubscriptionTypeName"].ToString();
                                isFound = true;
                            }
                        }
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
            return isFound;
        }
        public static int? FindTypeID(int subscriptionMonths)
        {
            int? subscriptionTypeID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT SubscriptionTypeID FROM SubscriptionTypes WHERE SubscriptionMonths = @SubscriptionMonths";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionMonths", subscriptionMonths);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            subscriptionTypeID = ID;
                        }
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
            return subscriptionTypeID;
        }
        public static int? FindTypeID(string SubscriptionTypeName)
        {
            int? subscriptionTypeID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT SubscriptionTypeID FROM SubscriptionTypes WHERE SubscriptionTypeName = @SubscriptionTypeName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionTypeName", SubscriptionTypeName);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            subscriptionTypeID = ID;
                        }
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
            return subscriptionTypeID;
        }        
        public static DataTable GetAllSubscriptionTypes()
        {
            DataTable subscriptionTypesTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM SubscriptionTypes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            subscriptionTypesTable.Load(reader);
                        }
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
            return subscriptionTypesTable;
        }
        public static bool UpdateTypeFees(int subscriptionTypeID, double newFees)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE SubscriptionTypes SET SubscriptionTypeFees = @NewFees WHERE SubscriptionTypeID = @SubscriptionTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);
                        command.Parameters.AddWithValue("@NewFees", newFees);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
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
            return isUpdated;
        }


    }
}
