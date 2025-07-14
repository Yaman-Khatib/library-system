using DVLD.Global_Classes;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_DataAccess
{
    public class clsPaymentsData
    {
        private static string connectionString = clsDataAccessSettings.connectionString;

        // Add a new payment
        public static int? AddNewPayment(double paymentAmount, int readerID, int createdByUserID, DateTime paymentDate, int paymentTypeID)
        {
            int? paymentID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Payments (PaymentAmount, ReaderID, CreatedByUserID, PaymentDate, PaymentTypeID)
                        VALUES (@PaymentAmount, @ReaderID, @CreatedByUserID, @PaymentDate, @PaymentTypeID);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                        command.Parameters.AddWithValue("@PaymentDate", paymentDate);
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            paymentID = id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return paymentID;
        }

        // Update an existing payment
        public static bool UpdatePayment(int paymentID, double paymentAmount, int readerID, int createdByUserID, DateTime paymentDate, int paymentTypeID)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Payments
                        SET PaymentAmount = @PaymentAmount,
                            ReaderID = @ReaderID,
                            CreatedByUserID = @CreatedByUserID,
                            PaymentDate = @PaymentDate,
                            PaymentTypeID = @PaymentTypeID
                        WHERE PaymentID = @PaymentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentID", paymentID);
                        command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                        command.Parameters.AddWithValue("@PaymentDate", paymentDate);
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);

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

        // Delete a payment
        public static bool DeletePayment(int paymentID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Payments WHERE PaymentID = @PaymentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentID", paymentID);

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

        // Find payment information by ID
        public static bool FindPaymentInfo(int paymentID, ref double? paymentAmount, ref int? readerID, ref int? createdByUserID, ref DateTime? paymentDate, ref int? paymentTypeID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Payments WHERE PaymentID = @PaymentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentID", paymentID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                paymentAmount = reader["PaymentAmount"] != DBNull.Value ? (double?)reader["PaymentAmount"] : null;
                                readerID = reader["ReaderID"] != DBNull.Value ? (int?)reader["ReaderID"] : null;
                                createdByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                paymentDate = reader["PaymentDate"] != DBNull.Value ? (DateTime?)reader["PaymentDate"] : null;
                                paymentTypeID = reader["PaymentTypeID"] != DBNull.Value ? (int?)reader["PaymentTypeID"] : null;

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

        // Get all payments for a specific type and date range
        public static DataTable GetAllPayments(int paymentTypeID, DateTime startDate, DateTime endDate)
        {
            DataTable paymentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT *
                        FROM vPayments
                        WHERE PaymentDate >= @StartDate AND PaymentDate <= @EndDate AND PaymentTypeID = @PaymentTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@PaymentTypeID", paymentTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            paymentsTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return paymentsTable;
        }

        // Get payments for a specific reader
        public static DataTable GetPaymentsForReader(int readerID)
        {
            DataTable paymentsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT
                            p.PaymentID,
                            p.ReaderID,
                            pt.PaymentTypeName AS PaymentType,
                            p.PaymentAmount,
                            FORMAT(p.PaymentDate, 'yyyy-MM-dd') AS PaymentDate,
                            FORMAT(p.PaymentDate, 'HH:mm:ss') AS PaymentTime,
                            u.Username AS CreatedBy
                        FROM Payments p
                        INNER JOIN PaymentTypes pt ON p.PaymentTypeID = pt.PaymentTypeID
                        INNER JOIN Users u ON u.UserID = p.CreatedByUserID
                        WHERE p.ReaderID = @ReaderID
                        ORDER BY p.PaymentDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            paymentsTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return paymentsTable;
        }
    }
}
