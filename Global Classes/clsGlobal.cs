using Library_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Story_Corner_Project.Global_Classes
{
    public class clsGlobal
    {

        public static clsUser CurrentUser;

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                // Get the path to the AppData folder
                string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appDirectory = Path.Combine(appDataDirectory, "The_Story_Corner_Project");

                // Path for the file that contains the credential
                string filePath = Path.Combine(appDirectory, "loginData.txt");

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);
                            if (result[0] != "")
                            {
                                Username = result[0];
                                Password = result[1];
                                return true;
                            }
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool RememberUserNameAndPassword(string Username, string Password)
        {
            try
            {
                // Get the path to the AppData folder
                string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appDirectory = Path.Combine(appDataDirectory, "The_Story_Corner_Project");

                // Ensure the directory exists
                if (!Directory.Exists(appDirectory))
                {
                    Directory.CreateDirectory(appDirectory);
                }

                // Define the path to the text file where you want to save the data
                string filePath = Path.Combine(appDirectory, "loginData.txt");

                // In case the username is empty, delete the file
                if (Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                // Concatenate username and password with separator
                string dataToSave = Username + "#//#" + Password;

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the file
                    writer.WriteLine(dataToSave);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool DoesCurrentUserHavePermission(clsUser.enPermissions permissions)
        {
            return ((CurrentUser.Permissions & permissions) == permissions);
        }
        //public static bool GetStoredCredential(ref String UserName, ref String Password)
        //{
        //    String keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\DVLD\CREDENTIALS";

        //    try
        //    {
        //        string UserNameValue = Registry.GetValue(keyPath, "User", null) as String;
        //        string PasswordValue = Registry.GetValue(keyPath, "Password", null) as String;
        //        if (!string.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
        //        {
        //            UserName = UserNameValue;
        //            Password = PasswordValue;
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}");
        //        return false;
        //    }
        //}


        public static bool DatabaseExists(SqlConnection connection, string databaseName)
        {
            string query = $@"
        IF EXISTS (SELECT name 
                   FROM master.dbo.sysdatabases 
                   WHERE name = '{databaseName}')
        SELECT 1
        ELSE
        SELECT 0";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (int)command.ExecuteScalar() == 1;
            }
        }

    }
}
