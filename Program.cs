using Library_Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Story_Corner_Project.Global_Classes;
using The_Story_Corner_Project.Login;

namespace The_Story_Corner_Project
{
    internal static class Program
    {
    
    

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]


        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool DoesDBExists;
            using (SqlConnection MasterConnection = new SqlConnection(clsConnectionString.MasterConnectionString))
            {
                MasterConnection.Open();

                DoesDBExists = clsGlobal.DatabaseExists(MasterConnection, "LibrarySystemDB");
            }
            //Application.Run(new CheckerForm());
            if(DoesDBExists)
            Application.Run(new frmLogin());
            else
            {
                Application.Run(new frmMainMenu(false));
            }

        }
    }
}
