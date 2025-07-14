using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class clsSubscriptionsData
    {
        public static int? AddNewSubscription(int readerID, DateTime startDate, DateTime expirationDate, int subscriptionReason, int subscriptionTypeID, int paymentID,int discount, int createdByUserID)
        {
            int? subscriptionID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Subscriptions (ReaderID, StartDate, ExpirationDate, SubscriptionReason, SubscriptionTypeID,discount, PaymentID, CreatedByUserID) " +
                                   "VALUES (@ReaderID, @StartDate, @ExpirationDate, @SubscriptionReason, @SubscriptionTypeID,@discount, @PaymentID, @CreatedByUserID); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                        command.Parameters.AddWithValue("@SubscriptionReason", subscriptionReason);
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);
                        command.Parameters.AddWithValue("@discount", discount); 
                        command.Parameters.AddWithValue("@PaymentID", paymentID);
                            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            subscriptionID = ID;
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
            return subscriptionID;
        }

        public static bool UpdateSubscription(int subscriptionID, int readerID, DateTime startDate, DateTime expirationDate, int subscriptionReason, int subscriptionTypeID,int discount, int PaymentID, int createdByUserID)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Subscriptions SET ReaderID = @ReaderID, StartDate = @StartDate, ExpirationDate = @ExpirationDate, " +
                                   "SubscriptionReason = @SubscriptionReason, SubscriptionTypeID = @SubscriptionTypeID,discount = @discount ,PaymentID = @PaymentID, CreatedByUserID = @CreatedByUserID " +
                                   "WHERE SubscriptionID = @SubscriptionID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                        command.Parameters.AddWithValue("@SubscriptionReason", subscriptionReason);
                        command.Parameters.AddWithValue("@SubscriptionTypeID", subscriptionTypeID);
                            command.Parameters.AddWithValue("@discount", discount);
                        command.Parameters.AddWithValue("@PaymentID", PaymentID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID); 
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
        public static bool FindLastSubscription(int readerID, ref int? subscriptionID, ref DateTime? startDate, ref DateTime? expirationDate, ref int? subscriptionReason, ref int? subscriptionTypeID,ref int discount, ref int? PaymentID, ref int? createdByUserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 * FROM Subscriptions WHERE ReaderID = @ReaderID ORDER BY ExpirationDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                subscriptionID = reader["SubscriptionID"] != DBNull.Value ? (int?)reader["SubscriptionID"] : null;
                                startDate = reader["StartDate"] != DBNull.Value ? (DateTime?)reader["StartDate"] : null;
                                expirationDate = reader["ExpirationDate"] != DBNull.Value ? (DateTime?)reader["ExpirationDate"] : null;
                                subscriptionReason = reader["SubscriptionReason"] != DBNull.Value ? (int?)reader["SubscriptionReason"] : null;
                                subscriptionTypeID = reader["SubscriptionTypeID"] != DBNull.Value ? (int?)reader["SubscriptionTypeID"] : null;
                                PaymentID = reader["PaymentID"] != DBNull.Value ? (int?)reader["PaymentID"] : null;
                                createdByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                discount =  (int)reader["discount"];
                                
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

        public static bool Find(int? subscriptionID, ref int? readerID, ref DateTime? startDate, ref DateTime? expirationDate, ref int? subscriptionReason, ref int? subscriptionTypeID,ref int discount, ref int? PaymentID, ref int? createdByUserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Subscriptions WHERE SubscriptionID = @SubscriptionID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionID", subscriptionID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If a matching record is found
                            {
                                readerID = (int)reader["ReaderID"];
                                startDate = reader["StartDate"] != DBNull.Value ? (DateTime?)reader["StartDate"] : null;
                                expirationDate = reader["ExpirationDate"] != DBNull.Value ? (DateTime?)reader["ExpirationDate"] : null;
                                subscriptionReason = reader["SubscriptionReason"] != DBNull.Value ? (int?)reader["SubscriptionReason"] : null;
                                subscriptionTypeID = reader["SubscriptionTypeID"] != DBNull.Value ? (int?)reader["SubscriptionTypeID"] : null;
                                PaymentID = reader["PaymentID"] != DBNull.Value ? (int?)reader["PaymentID"] : null;
                                createdByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                discount = (int)reader["discount"];
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


        public static bool DeleteSubscription(int subscriptionID)
        {
            bool isDeleted = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Subscriptions WHERE SubscriptionID = @SubscriptionID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SubscriptionID", subscriptionID);

                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
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
        
        
        public static DataTable GetAllSubscriptions()
        {
            DataTable subscriptionsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Subscriptions";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            subscriptionsTable.Load(reader);
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
            return subscriptionsTable;
        }

        public static DataTable GetSubscriptionsForReader(int readerID)
        {
            DataTable subscriptionsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "\r\n\r\nSelect S.SubscriptionID,SubscriptionTime = Format(S.StartDate ,'hh:mm tt'),SubscriptionDate = Format(S.StartDate ,'yyyy-MM-dd')," +
                        "ExpirationDate =  FORMAT(S.ExpirationDate, 'yyyy-MM-dd'),St.SubscriptionTypeName,p.PaymentAmount,Concat(s.Discount,'%') as Discount ,u.UserName as CreatedByUser\r\nfrom \r\n" +
                        "Subscriptions s inner join Users u on s.CreatedByUserID = u.UserID  inner join SubscriptionTypes st on st.SubscriptionTypeID = s.SubscriptionTypeID inner join Payments p on p.PaymentID = s.PaymentID" +
                        " where s.ReaderID = @ReaderID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            subscriptionsTable.Load(reader);
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
            return subscriptionsTable;
        }

        public static DataTable GetSubscriptionsForReader(string AccountNumber)
        {
            DataTable subscriptionsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Subscriptions WHERE AccountNumber = @AccountNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", AccountNumber);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            subscriptionsTable.Load(reader);
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
            return subscriptionsTable;
        }


        public static bool DoesReaderHaveAnActiveSubscription(int readerID)
        {
            bool hasActiveSubscription = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Subscriptions WHERE ReaderID = @ReaderID AND ExpirationDate > @CurrentDate";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReaderID", readerID);
                        command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);

                        int count = (int)command.ExecuteScalar();
                        hasActiveSubscription = count > 0; // If there is at least one active subscription
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
            return hasActiveSubscription;
        }




    }
}
