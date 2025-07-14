using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsPaymentType
    {
        public int? PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public enum enPaymentType { Subscription = 1, LateReturnFee = 2, CourseSubscription = 3, BuyingBook = 4 }
        public clsPaymentType()
        {
            PaymentTypeID = null;
            PaymentTypeName = null;
        }
        private clsPaymentType(int paymentTypeID, string paymentTypeName)
        {
            PaymentTypeID = paymentTypeID;
            PaymentTypeName = paymentTypeName;
        }

        // Method to add a new Payment Type
        public static int? AddNewPaymentType(string paymentTypeName)
        {
            return clsPaymentTypesData.AddNewPaymentType(paymentTypeName);
        }

        // Method to update an existing Payment Type
        public static bool UpdatePaymentType(int paymentTypeID, string paymentTypeName)
        {
            return clsPaymentTypesData.UpdatePaymentType(paymentTypeID, paymentTypeName);
        }

        // Method to delete a Payment Type
        public static bool DeletePaymentType(int paymentTypeID)
        {
            return clsPaymentTypesData.DeletePaymentType(paymentTypeID);
        }

        // Method to find a Payment Type by PaymentTypeID
        public static clsPaymentType GetPaymentTypeInfo(int paymentTypeID)
        {
            string paymentTypeName = null;
            if (clsPaymentTypesData.FindPaymentTypeInfo(paymentTypeID, ref paymentTypeName))
                return new clsPaymentType(paymentTypeID, paymentTypeName);
            else
                return null;
        }
        public static int GetPaymentTypeID(string paymentTypeName)
        {
            return clsPaymentType.GetPaymentTypeID(paymentTypeName);
        }
        // Method to get all Payment Types
        public static DataTable GetAllPaymentTypes()
        {
            return clsPaymentTypesData.GetAllPaymentTypes();
        }
    }
}
