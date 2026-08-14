using System;
using System.Data.SqlClient;
using Library_DataAccess;

namespace The_Story_Corner_Project.Global_Classes
{
    public static class clsConnectionHandler
    {
        public static bool IsDatabaseAccessible()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
