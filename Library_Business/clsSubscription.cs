using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsSubscription
    {
        public int? SubscriptionID { get; set; }
        public int? ReaderID { get; set; }
        public DateTime?StartDate { get; set; }
        public DateTime?ExpirationDate { get; set; }
        public enum enSubscriptionReason { FirstTime, Renew = 2 }
        public enSubscriptionReason? SubscriptionReason { get; set; }
        public int? SubscriptionTypeID { get; set; }
        public clsSubscriptionType SubscriptionTypeInfo { get; set; }
        public int? PaymentID { get; set; }
        public int? CreatedByUserID { get; set; }
        public int Discount {  get; set; }//for 50% discount it will store 50
        enum enMode { AddNew = 1, Update=2}
        enMode _Mode;
        
        private clsSubscription(int? subscriptionID, int? readerID, DateTime? startDate, DateTime? expirationDate, enSubscriptionReason? subscriptionReason, int? subscriptionTypeID, int? paimentID,int? Discount ,int? createdByUserID)
        {
            SubscriptionID = subscriptionID;
            ReaderID = readerID;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            SubscriptionReason = subscriptionReason;
            SubscriptionTypeID = subscriptionTypeID;
            SubscriptionTypeInfo = clsSubscriptionType.Find(subscriptionTypeID.Value);
            PaymentID = paimentID;
            CreatedByUserID = createdByUserID;
            this.Discount = Discount ?? 0;
            _Mode = enMode.Update;
        }

        public clsSubscription()
        {
            SubscriptionID = null;
            ReaderID = null;
            StartDate = null;
            ExpirationDate = null;
            SubscriptionReason = null;
            SubscriptionTypeID = null;
            PaymentID = null;
            CreatedByUserID = null;
            Discount = 0;
            _Mode = enMode.AddNew;
        }

        private bool _AddNewSubscription()
        {
            clsSubscriptionType SubscriptionType = clsSubscriptionType.Find((int)this.SubscriptionTypeID);
            clsPayment Payment = new clsPayment();
            Payment.PaymentAmount = SubscriptionType.SubscriptionTypeFees - SubscriptionType.SubscriptionTypeFees*((double)this.Discount/100);
            Payment.PaymentDate = DateTime.Now;
            Payment.ReaderID = ReaderID;
            Payment.CreatedByUserID = CreatedByUserID;
            Payment.PaymentTypeID = (int)clsPaymentType.enPaymentType.Subscription;
            Payment.Save();
            this.PaymentID = Payment.PaymentID;

            if (Payment.PaymentID != null)
            {
                SubscriptionID = clsSubscriptionsData.AddNewSubscription(
                    ReaderID.Value, StartDate.Value, ExpirationDate.Value,
                    (int)SubscriptionReason.Value, SubscriptionTypeID.Value,
                    PaymentID.Value, Discount, CreatedByUserID.Value);
            }
            return SubscriptionID != null;
        }

        private bool _UpdateSubscription()
        {
            return clsSubscriptionsData.UpdateSubscription(
                SubscriptionID.Value, ReaderID.Value, StartDate.Value, ExpirationDate.Value,
                (int)SubscriptionReason.Value, SubscriptionTypeID.Value, PaymentID.Value, Discount, CreatedByUserID.Value);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewSubscription())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateSubscription();
                default:
                    return false;
            }
        }


        public static clsSubscription FindLastSubscription(int readerID)
        {
            int? subscriptionID = null;
            DateTime? startDate = null, expirationDate = null;
            int? subscriptionReason = null, subscriptionTypeID = null, createdByUserID = null;
            int? paymentID = null;
            int discount = 0;
            bool isFound = clsSubscriptionsData.FindLastSubscription(readerID, ref subscriptionID, ref startDate, ref expirationDate, ref subscriptionReason, ref subscriptionTypeID, ref discount, ref paymentID, ref createdByUserID);

            if (isFound)
            {
                return new clsSubscription(subscriptionID, readerID, startDate, expirationDate, (enSubscriptionReason)subscriptionReason, subscriptionTypeID, paymentID, discount, createdByUserID);
            }

            return null;
        }
        public static DataTable GetSubscriptionsForReader(int readerID)
        {
            return clsSubscriptionsData.GetSubscriptionsForReader(readerID);
        }

        public static DataTable GetSubscriptionsForReader(string AccountNumber)
        {
            return clsSubscriptionsData.GetSubscriptionsForReader(AccountNumber);
        }
        public static DataTable GetAllSubscriptions()
        {
            return clsSubscriptionsData.GetAllSubscriptions();
        }
        public static clsSubscription Find(int SubscriptionID)
        {
            int? subscriptionID = null;
            int? readerID=null; DateTime? startDate = null; DateTime? expirationDate = null; int? subscriptionReason = null; int? subscriptionTypeID = null;
            int? paymentID = null; int? createdByUserID = null;
            int discount = 0;
            if (clsSubscriptionsData.Find(subscriptionID.Value, ref readerID, ref startDate, ref expirationDate, ref subscriptionReason, ref subscriptionTypeID,ref discount, ref paymentID, ref createdByUserID))
            {
                return new clsSubscription(subscriptionID, readerID, startDate, expirationDate, (enSubscriptionReason)subscriptionReason, subscriptionTypeID, discount, paymentID, createdByUserID);
            }
            else
                return null;
        }

        public static bool DeleteSubscription(int subscriptionID)
        {
            var subscription = clsSubscription.Find(subscriptionID);
            if (subscription != null && clsPayment.DeletePayment(subscription.PaymentID.Value))
            {
                return clsSubscriptionsData.DeleteSubscription(subscriptionID);
            }
            else
            {
                return false;
            }
        }

        public static bool DoesReaderHaveAnActiveSubscription(int readerID)
        {
            return clsSubscriptionsData.DoesReaderHaveAnActiveSubscription(readerID);
        }
    }
}


//using Library_DataAccess;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Library_Business
//{
//    public class clsSubscription
//    {
//        public int? SubscriptionID { get; set; }
//        public int? ReaderID { get; set; }
//        public DateTime? StartDate { get; set; }
//        public DateTime? ExpirationDate { get; set; }
//        public enum enSubscriptionReason { FirstTime, Renew = 2 }
//        public enSubscriptionReason? SubscriptionReason { get; set; }
//        public int? SubscriptionTypeID { get; set; }
//        public int? PaymentID { get; set; }
//        public int? CreatedByUserID { get; set; }
//        public int Discount { get; set; }//for 50% discount it will store 50
//        enum enMode { AddNew = 1, Update = 2 }
//        enMode _Mode;

//        public clsSubscription()
//        {
//            SubscriptionID = null;
//            ReaderID = null;
//            StartDate = null;
//            ExpirationDate = null;
//            SubscriptionReason = null;
//            SubscriptionTypeID = null;
//            PaymentID = null;
//            CreatedByUserID = null;
//            _Mode = enMode.AddNew;
//        }
//        private clsSubscription(int? subscriptionID, int? readerID, DateTime? startDate, DateTime? expirationDate, enSubscriptionReason? subscriptionReason, int? subscriptionTypeID, int? paimentID, int? createdByUserID)
//        {
//            SubscriptionID = subscriptionID;
//            ReaderID = readerID;
//            StartDate = startDate;
//            ExpirationDate = expirationDate;
//            SubscriptionReason = subscriptionReason;
//            SubscriptionTypeID = subscriptionTypeID;
//            PaymentID = paimentID;
//            CreatedByUserID = createdByUserID;
//            _Mode = enMode.Update;
//        }


//        private bool _AddNewSubscription()
//        {
//            clsSubscriptionType SubscriptionType = clsSubscriptionType.Find((int)this.SubscriptionTypeID);
//            clsPayment Payment = new clsPayment();
//            Payment.PaymentAmount = SubscriptionType.SubscriptionTypeFees;
//            Payment.PaymentDate = DateTime.Now;
//            Payment.ReaderID = ReaderID;
//            Payment.CreatedByUserID = CreatedByUserID;
//            Payment.PaymentTypeID = (int)clsPaymentType.enPaymentType.Subscription;
//            Payment.Save();
//            if (Payment.PaymentID != null)
//            {
//                SubscriptionID = clsSubscriptionsData.AddNewSubscription(
//                ReaderID.Value, StartDate.Value, ExpirationDate.Value, (int)SubscriptionReason.Value, SubscriptionTypeID.Value, Payment.PaymentID.Value, CreatedByUserID.Value);
//            }
//            return SubscriptionID != null;
//        }

//        private bool _UpdateSubscription()
//        {
//            return clsSubscriptionsData.UpdateSubscription(
//                SubscriptionID.Value, ReaderID.Value, StartDate.Value, ExpirationDate.Value, (int)SubscriptionReason.Value, SubscriptionTypeID.Value, PaymentID.Value, CreatedByUserID.Value);
//        }

//        public static clsSubscription FindLastSubscription(int readerID)
//        {
//            int? subscriptionID = null;
//            DateTime? startDate = null, expirationDate = null;
//            int? subscriptionReason = null, subscriptionTypeID = null, createdByUserID = null;
//            int? paymentID = null;

//            bool isFound = clsSubscriptionsData.FindLastSubscription(readerID, ref subscriptionID, ref startDate, ref expirationDate, ref subscriptionReason, ref subscriptionTypeID, ref paymentID, ref createdByUserID);

//            if (isFound)
//            {
//                return new clsSubscription(subscriptionID, readerID, startDate, expirationDate, (enSubscriptionReason)subscriptionReason, subscriptionTypeID, paymentID, createdByUserID);
//            }

//            return null;
//        }

//        public bool Save()
//        {
//            switch (_Mode)
//            {
//                case enMode.AddNew:
//                    if (_AddNewSubscription())
//                    {
//                        _Mode = enMode.Update;
//                        return true;
//                    }
//                    else { return false; }


//                case enMode.Update:
//                    return _UpdateSubscription();
//            }
//            return false;
//        }

//        public static DataTable GetSubscriptionsForReader(int readerID)
//        {
//            return clsSubscriptionsData.GetSubscriptionsForReader(readerID);
//        }

//        public static DataTable GetSubscriptionsForReader(string AccountNumber)
//        {
//            return clsSubscriptionsData.GetSubscriptionsForReader(AccountNumber);
//        }
//        public static clsSubscription Find(int SubscriptionID)
//        {
//            int? subscriptionID = null;
//            int? readerID = null; DateTime? startDate = null; DateTime? expirationDate = null; int? subscriptionReason = null; int? subscriptionTypeID = null;
//            int? paymentID = null; int? createdByUserID = null;
//            if (clsSubscriptionsData.Find(subscriptionID.Value, ref readerID, ref startDate, ref expirationDate, ref subscriptionReason, ref subscriptionTypeID, ref paymentID, ref createdByUserID))
//            {
//                return new clsSubscription(subscriptionID, readerID, startDate, expirationDate, (enSubscriptionReason)subscriptionReason, subscriptionTypeID, paymentID, createdByUserID);
//            }
//            else
//                return null;
//        }
//        public static DataTable GetAllSubscriptions()
//        {
//            return clsSubscriptionsData.GetAllSubscriptions();
//        }

//        public static bool DeleteSubscription(int subscriptionID)
//        {
//            var subscription = clsSubscription.Find(subscriptionID);
//            if (subscription != null && clsPayment.DeletePayment(subscription.PaymentID.Value))
//            {
//                return clsSubscriptionsData.DeleteSubscription(subscriptionID);
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static bool DoesReaderHaveAnActiveSubscription(int readerID)
//        {
//            return clsSubscriptionsData.DoesReaderHaveAnActiveSubscription(readerID);
//        }
//    }
//}
