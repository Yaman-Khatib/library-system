using DVLD.Global_Classes;
using Library_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Data
{
    public static class clsBookSaleData
    {
        private static string connectionString = clsDataAccessSettings.connectionString;

        public static int? AddNewSale(int bookID, int readerID, int paymentID, int soldByUserID, string notes)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO BookSales (BookID, ReaderID, PaymentID, SoldByUserID, Notes)
                        VALUES (@BookID, @ReaderID, @PaymentID, @SoldByUserID, @Notes);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookID", bookID);
                        cmd.Parameters.AddWithValue("@ReaderID", readerID);
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                        cmd.Parameters.AddWithValue("@SoldByUserID", soldByUserID);
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : (int?)null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                clsLogEvent.Log(ex);
                return null;
            }
        }

        public static bool UpdateSale(int saleID, int bookID, int readerID, int paymentID, int soldByUserID, string notes)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE BookSales
                        SET BookID = @BookID,
                            ReaderID = @ReaderID,
                            PaymentID = @PaymentID,
                            SoldByUserID = @SoldByUserID,
                            Notes = @Notes
                        WHERE BookSaleID = @SaleID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleID);
                        cmd.Parameters.AddWithValue("@BookID", bookID);
                        cmd.Parameters.AddWithValue("@ReaderID", readerID);
                        cmd.Parameters.AddWithValue("@PaymentID", paymentID);
                        cmd.Parameters.AddWithValue("@SoldByUserID", soldByUserID);
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                clsLogEvent.Log(ex);
                return false;
            }
        }

        public static bool FindSale(int? saleID, ref int? bookID, ref int? readerID, ref int? paymentID, ref int? soldByUserID, ref string notes)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT BookID, ReaderID, PaymentID, SoldByUserID, Notes
                        FROM BookSales
                        WHERE BookSaleID = @SaleID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookID = reader.GetInt32(reader.GetOrdinal("BookID"));
                                readerID = reader.GetInt32(reader.GetOrdinal("ReaderID"));
                                paymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"));
                                soldByUserID = reader.GetInt32(reader.GetOrdinal("SoldByUserID"));
                                notes = reader["Notes"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Notes")) : null;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
            }
            return false;
        }

        public static DataTable GetAllSales()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM vBookSales";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
                return null;
            }
        }

        public static bool DeleteSale(int saleID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM BookSales WHERE BookSaleID = @SaleID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogEvent.Log(ex);
                return false;
            }
        }
    }
}
