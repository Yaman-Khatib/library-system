using Library_DataAccess;
using System;
using System.Data;

namespace Library_Business
{
    public class clsPayment
    {
        public int? PaymentID { get; set; }
        public double? PaymentAmount { get; set; }
        public int? ReaderID { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentTypeID { get; set; }

        private enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode;

        public clsPayment()
        {
            PaymentID = null;
            PaymentAmount = null;
            ReaderID = null;
            CreatedByUserID = null;
            PaymentDate = null;
            PaymentTypeID = null;
            _Mode = enMode.AddNew;
        }

        private clsPayment(int paymentID, double paymentAmount, int readerID, int createdByUserID, DateTime paymentDate, int paymentTypeID)
        {
            PaymentID = paymentID;
            PaymentAmount = paymentAmount;
            ReaderID = readerID;
            CreatedByUserID = createdByUserID;
            PaymentDate = paymentDate;
            PaymentTypeID = paymentTypeID;
            _Mode = enMode.Update;
        }

        // Add a new payment
        private bool _AddNewPayment()
        {
            this.PaymentID = clsPaymentsData.AddNewPayment(PaymentAmount.Value, ReaderID.Value, CreatedByUserID.Value, PaymentDate.Value, PaymentTypeID.Value);
            return PaymentID != null;
        }

        // Update an existing payment
        private bool _UpdatePayment()
        {
            return clsPaymentsData.UpdatePayment(PaymentID.Value, PaymentAmount.Value, ReaderID.Value, CreatedByUserID.Value, PaymentDate.Value, PaymentTypeID.Value);
        }

        // Save the payment (add or update)
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPayment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdatePayment();
            }
            return false;
        }

        // Find a payment by ID
        public static clsPayment Find(int paymentID)
        {
            double? paymentAmount = null;
            int? readerID = null;
            int? createdByUserID = null;
            DateTime? paymentDate = null;
            int? paymentTypeID = null;

            if (clsPaymentsData.FindPaymentInfo(paymentID, ref paymentAmount, ref readerID, ref createdByUserID, ref paymentDate, ref paymentTypeID))
            {
                return new clsPayment(paymentID, paymentAmount.Value, readerID.Value, createdByUserID.Value, paymentDate.Value, paymentTypeID.Value);
            }
            return null;
        }

        // Get all payments for a specific period and type
        public static DataTable GetAllPayments(int paymentTypeID, DateTime startDate, DateTime endDate)
        {
            return clsPaymentsData.GetAllPayments(paymentTypeID, startDate, endDate);
        }

        // Update payment
        public static bool UpdatePayment(int paymentID, double paymentAmount, int readerID, int createdByUserID, DateTime paymentDate, int paymentTypeID)
        {
            return clsPaymentsData.UpdatePayment(paymentID, paymentAmount, readerID, createdByUserID, paymentDate, paymentTypeID);
        }

        // Delete a payment
        public static bool DeletePayment(int paymentID)
        {
            return clsPaymentsData.DeletePayment(paymentID);
        }

        // Get payments for a specific reader
        public static DataTable GetPaymentsForReader(int readerID)
        {
            return clsPaymentsData.GetPaymentsForReader(readerID);
        }
    }
}
