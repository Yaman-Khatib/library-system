using Library_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

public class clsLibrarySettingsData
{
    private static string connectionString = clsDataAccessSettings.connectionString;

    // Get a setting by ID
    public static DataRow FindSetting(int settingID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM LibrarySettings WHERE SettingID = @SettingID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SettingID", settingID);
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }
    }

    // Update setting value and last updated date
    public static bool UpdateSetting(int settingID, int settingValue)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE LibrarySettings SET SettingValue = @SettingValue, LastUpdated = @LastUpdated WHERE SettingID = @SettingID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SettingValue", settingValue);
                command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
                command.Parameters.AddWithValue("@SettingID", settingID);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    // Get specific setting values for different use cases
    public static int GetSettingValue(int settingID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT SettingValue FROM LibrarySettings WHERE SettingID = @SettingID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SettingID", settingID);
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
