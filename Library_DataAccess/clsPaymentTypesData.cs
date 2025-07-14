using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsPaymentTypesData
    {
        
        public static int? AddNewPaymentType(string paymentTypeName)
        {
            int? paymentTypeID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO PaymentTypes (PaymentTypeName) VALUES (@PaymentTypeName); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentTypeName", paymentTypeName);
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            paymentTypeID = ID;
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
            return paymentTypeID;
        }

        // Method to update an existing Payment Type
        public static bool UpdatePaymentType(int paymentTypeID, string paymentTypeName)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE PaymentTypes SET PaymentTypeName = @PaymentTypeName WHERE PaymentTypeID = @PaymentTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);
                        command.Parameters.AddWithValue("@PaymentTypeName", paymentTypeName);

                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0; // Check if any row was affected
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

        // Method to delete a Payment Type
        public static bool DeletePaymentType(int paymentTypeID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM PaymentTypes WHERE PaymentTypeID = @PaymentTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);
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

        // Method to find a Payment Type by PaymentTypeID
        public static bool FindPaymentTypeInfo(int paymentTypeID,ref string paymentTypeName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM PaymentTypes WHERE PaymentTypeID = @PaymentTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                paymentTypeName = reader["PaymentTypeName"].ToString();
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
        public static int? GetPaymentTypeID(string paymentTypeName)
        {
            int? TypeID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT PaymentTypeID FROM PaymentTypes WHERE PaymentTypeName = @PaymentTypeName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentTypeName", paymentTypeName);
                        object obj = command.ExecuteScalar();
                        if (obj != null && int.TryParse(obj.ToString(), out int ID))
                        {
                            TypeID = ID;
                        }
                        else
                            TypeID = null;
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

            return TypeID;            
        }

        public static DataTable GetAllPaymentTypes()
        {
            DataTable paymentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM PaymentTypes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            paymentsTable.Load(reader);
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
            return paymentsTable;
        }

        


    }
}
