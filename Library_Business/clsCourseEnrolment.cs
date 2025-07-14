using Library_DataAccess;
using System;
using System.Data;

namespace Library_Business
{
    public class clsCourseEnrolment
    {
        public int? CourseEnrolmentID { get; set; }
        public int? CourseID { get; set; }
        public clsCourse CourseInfo { get; set; }
        public int? ReaderID { get; set; }
        public int? PaymentID { get; set; }
        public DateTime? CourseEnrolmentDate { get; set; }
        public string Notes { get; set; }
        public int? CreatedByUserID { get; set; }
        private enum enMode { AddNew = 1, Update = 2 }
        private enMode Mode;

        public clsCourseEnrolment()
        {
            CourseEnrolmentID = null;
            CourseID = null;
            ReaderID = null;
            PaymentID = null;
            CourseEnrolmentDate = null;
            Notes = null;
            CreatedByUserID = null;
            CourseInfo = new clsCourse();
            Mode = enMode.AddNew;
        }

        public clsCourseEnrolment(int courseEnrolmentID, int courseID, int readerID, int paymentID, DateTime courseEnrolmentDate, string notes, int? createdByUserID = null)
        {
            CourseEnrolmentID = courseEnrolmentID;
            CourseID = courseID;
            CourseInfo = clsCourse.Find(courseID);
            ReaderID = readerID;
            PaymentID = paymentID;
            CourseEnrolmentDate = courseEnrolmentDate;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            Mode = enMode.Update;
        }

        private bool _AddNewEnrolment()
        {
            clsPayment payment = new clsPayment();
            payment.ReaderID = this.ReaderID;
            payment.CreatedByUserID = this.CreatedByUserID;
            payment.PaymentDate = this.CourseEnrolmentDate;
            payment.PaymentTypeID = (int)clsPaymentType.enPaymentType.CourseSubscription;
            payment.PaymentAmount = (double)this.CourseInfo.Fees.Value;
            payment.ReaderID = this.ReaderID;
            
            if (!payment.Save())
            {
                return false;
            }
            this.PaymentID = payment.PaymentID;
            CourseEnrolmentID = clsCourseEnrolmentData.AddNewEnrolment(
                CourseID.Value, ReaderID.Value, PaymentID.Value, CourseEnrolmentDate.Value, Notes, CreatedByUserID);

            return CourseEnrolmentID != null;
        }

        private bool _UpdateEnrolment()
        {
            return clsCourseEnrolmentData.UpdateEnrolment(
                CourseEnrolmentID.Value, CourseID.Value, ReaderID.Value, PaymentID.Value, CourseEnrolmentDate.Value, Notes, CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewEnrolment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateEnrolment();
                default:
                    return false;
            }
        }

        public static bool DeleteEnrolment(int courseEnrolmentID)
        {
            return clsCourseEnrolmentData.DeleteEnrolment(courseEnrolmentID);
        }

        public static clsCourseEnrolment Find(int courseEnrolmentID)
        {
            int? courseID = null, readerID = null, paymentID = null;
            DateTime? courseEnrolmentDate = null;
            string notes = null;
            int? createdByUserID = null;

            bool isFound = clsCourseEnrolmentData.FindEnrolment(
                courseEnrolmentID, ref courseID, ref readerID, ref paymentID, ref courseEnrolmentDate, ref notes, ref createdByUserID);

            if (isFound)
            {
                return new clsCourseEnrolment(
                    courseEnrolmentID, courseID.Value, readerID.Value, paymentID.Value, courseEnrolmentDate.Value, notes, createdByUserID);
            }

            return null;
        }

        public static DataTable GetAllEnrollments()
        {
            return clsCourseEnrolmentData.GetAllEnrollments();
        }

        public static DataTable GetAllEnrollments(int courseID)
        {
            return clsCourseEnrolmentData.GetAllEnrollments(courseID);
        }

        public static int GetEnrollmentsCount(int courseID)
        {
            return clsCourseEnrolmentData.GetAllEnrollmentsCount(courseID);
        }

        public static bool IsReaderEnrolled(int readerID,int courseID)
        {
            return clsCourseEnrolmentData.IsReaderEnrolled(readerID, courseID);
        }

        public static DataTable GetAllEnrollmentsForReader(int readerID)
        {
            return clsCourseEnrolmentData.GetAllEnrollmentsForReader(readerID);
        }
    }
}
