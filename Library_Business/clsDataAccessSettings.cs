using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsConnectionString
    {
        public static String ConnectionString = clsDataAccessSettings.connectionString;
        public static String MasterConnectionString = clsDataAccessSettings.masterConnectionString;

    }
}
