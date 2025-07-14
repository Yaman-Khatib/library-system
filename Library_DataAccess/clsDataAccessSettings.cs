using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Library_DataAccess
{
    public class clsDataAccessSettings
    {
        //public static string connectionString = "Server=.;DataBase=LibraryManagement;User ID=sa;password=sa123456;";
        public static string connectionString = ConfigurationManager.ConnectionStrings["LibraryConnectionString"].ConnectionString;
        public static string masterConnectionString = ConfigurationManager.ConnectionStrings["MasterConnectionString"].ConnectionString;

    }
}
