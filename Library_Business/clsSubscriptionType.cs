using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsSubscriptionType
    {
        public int? SubscriptionTypeID { get; set; }
        public int? SubscriptionTypeMonths { get; set; }
        public double? SubscriptionTypeFees { get; set; }
        public string SubscriptionTypeName { get; set; }
        private enum enMode { AddNew = 1, Update = 2 }
        private enMode Mode { get; set; }

        clsSubscriptionType()
        {
            SubscriptionTypeID = null;
            SubscriptionTypeMonths = null;
            SubscriptionTypeFees = null;
            SubscriptionTypeName = null;
        }
        clsSubscriptionType(int? subscriptionTypeID, int? subscriptionTypeMonths, double? subscriptionTypeFees,string subscriptionTypeName)
        {
            SubscriptionTypeID = subscriptionTypeID;
            SubscriptionTypeMonths = subscriptionTypeMonths;
            SubscriptionTypeFees = subscriptionTypeFees;
            SubscriptionTypeName = subscriptionTypeName;
        }


        private bool _AddNew()
        {
            SubscriptionTypeID = clsSubscriptionTypesData.AddNewType(SubscriptionTypeMonths.Value, SubscriptionTypeFees.Value,SubscriptionTypeName);
            return SubscriptionTypeID != null;
        }

        private bool _Update()
        {
            return clsSubscriptionTypesData.UpdateType(SubscriptionTypeID.Value, SubscriptionTypeMonths.Value, SubscriptionTypeFees.Value, SubscriptionTypeName);
        }

        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (_AddNew())
                {
                    Mode = enMode.Update;
                    return true;
                }
                return false;
            }
            else
            {
                return _Update();
            }
        }

        public static clsSubscriptionType Find(int subscriptionTypeID)
        {
            int? subscriptionMonths = null;
            double? subscriptionTypeFees = null;
            string subscriptionTypeName = null;
            bool isFound = clsSubscriptionTypesData.FindTypeInfo(subscriptionTypeID, ref subscriptionMonths, ref subscriptionTypeFees,ref subscriptionTypeName);

            if (isFound)
            {
                return new clsSubscriptionType(subscriptionTypeID, subscriptionMonths, subscriptionTypeFees, subscriptionTypeName);
            }
            return null;
        }

        public bool Delete()
        {
            if (SubscriptionTypeID.HasValue)
            {
                return clsSubscriptionTypesData.DeleteType(SubscriptionTypeID.Value);
            }
            return false;
        }

        public static DataTable GetAllSubscriptionTypes()
        {
            return clsSubscriptionTypesData.GetAllSubscriptionTypes();
        }

        public static bool UpdateFees(int SubscriptionTypeID, double newFees)
        {            
            return clsSubscriptionTypesData.UpdateTypeFees(SubscriptionTypeID, newFees);                       
        }

        public static int? FindTypeIDByMonthsCount(byte subscriptionMonths)
        {
            return clsSubscriptionTypesData.FindTypeID(subscriptionMonths);
        }       

    }
}
