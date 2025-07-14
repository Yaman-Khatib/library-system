using System;
using System.Data;

public class clsLibrarySettings
{
    public int SettingID { get; set; }
    public string SettingName { get; set; }
    public string SettingDescription { get; set; }
    public DateTime LastUpdated { get; set; }

    public enum enSettingType
    {
        ExtendBorrowDaysID = 1,
        LateReturnFeesAmountID = 2,
        ExtendPerBorrowCountID = 3,
        DefaultBorrowDays = 4
    }

    // Method to find a setting by enum ID
    public static clsLibrarySettings FindSetting(enSettingType settingID)
    {
        DataRow row = clsLibrarySettingsData.FindSetting((int)settingID);
        if (row != null)
        {
            return new clsLibrarySettings
            {
                SettingID = Convert.ToInt32(row["SettingID"]),
                SettingName = row["SettingName"].ToString(),
                SettingDescription = row["Description"].ToString(),
                LastUpdated = row["LastUpdated"] != DBNull.Value ? Convert.ToDateTime(row["LastUpdated"]) : DateTime.MinValue
            };
        }
        return null;
    }

    // Update a setting value
    public static bool UpdateSetting(enSettingType settingID, int settingValue)
    {
        return clsLibrarySettingsData.UpdateSetting((int)settingID, settingValue);
    }

    // Get specific settings using predefined IDs
    public static int GetDefaultExtendDays()
    {
        return clsLibrarySettingsData.GetSettingValue((int)enSettingType.ExtendBorrowDaysID);
    }

    public static int GetDefaultExtendTimesPerBorrow()
    {
        return clsLibrarySettingsData.GetSettingValue((int)enSettingType.ExtendPerBorrowCountID);
    }

    public static int GetLateReturnFineAmount()
    {
        return clsLibrarySettingsData.GetSettingValue((int)enSettingType.LateReturnFeesAmountID);
    }

    public static int GetDefaultBorrowingDays()
    {
        return clsLibrarySettingsData.GetSettingValue((int)enSettingType.DefaultBorrowDays);
    }
}
