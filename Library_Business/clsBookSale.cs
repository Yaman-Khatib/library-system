using Library_Data;
using System;
using System.Data;

namespace Library_Business
{
    public class clsBookSale
    {
        public int? SaleID { get; set; }
        public int? BookID { get; set; }
        public clsBook BookInfo { get; set; }
        public int? ReaderID { get; set; }
        public clsReader ReaderInfo { get; set; }
        public int? PaymentID { get; set; }
        public clsPayment PaymentInfo { get; set; }
        public string Notes { get; set; }
        public double? PaidAmount { get; set; }
        public int? SoldByUserID { get; set; } // New column
        public clsUser SoldByUserInfo { get; set; }

        private enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode;

        public clsBookSale()
        {
            SaleID = null;
            BookID = null;
            BookInfo = new clsBook();
            ReaderID = null;
            ReaderInfo = new clsReader();
            PaymentID = null;
            PaymentInfo = new clsPayment();
            Notes = null;
            PaidAmount = 0;
            SoldByUserID = 0;
            SoldByUserInfo = new clsUser();
            _Mode = enMode.AddNew;
        }

        private clsBookSale(int saleID, int bookID, int readerID, int paymentID, int soldByUserID, string notes)
        {
            SaleID = saleID;
            BookID = bookID;
            BookInfo = clsBook.FindByID(bookID);
            ReaderID = readerID;
            ReaderInfo = clsReader.FindByReaderID(readerID);
            PaymentID = paymentID;
            PaymentInfo = clsPayment.Find(paymentID);
            Notes = notes;
            PaidAmount = PaymentInfo.PaymentAmount.Value;
            SoldByUserID = soldByUserID;
            SoldByUserInfo = clsUser.FindByUserID(soldByUserID);
            _Mode = enMode.Update;
        }

        private bool AddBookSale()
        {
            if (BookID == null || ReaderID == null || SoldByUserID == null || PaidAmount == null)
            {
                return false;
            }

            clsBook Book = clsBook.FindByID(BookID.Value);
            if (Book == null || Book.CopiesCount <= 0)
                return false;

            int newCopiesCount = Book.CopiesCount.Value - 1;

            Book.UpdateCopiesCount(newCopiesCount);

            var payment = new clsPayment
            {
                PaymentAmount = PaidAmount,
                ReaderID = ReaderID,
                CreatedByUserID = SoldByUserID,
                PaymentDate = DateTime.Now,
                PaymentTypeID = (int)clsPaymentType.enPaymentType.BuyingBook, // Adjust this based on your payment types
                
            };

            if (payment.Save())
            {
                this.PaymentID = payment.PaymentID;
                this.SaleID = clsBookSaleData.AddNewSale(BookID.Value, ReaderID.Value, PaymentID.Value, SoldByUserID.Value, Notes);
                return SaleID != null;
            }
            return false;
        }

        private bool UpdateBookSale()
        {
            if (BookID == null || ReaderID == null || SoldByUserID == null || PaidAmount == null)
            {
                return false;
            }
            return clsBookSaleData.UpdateSale(SaleID.Value, BookID.Value, ReaderID.Value, PaymentID.Value, SoldByUserID.Value, Notes);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                return AddBookSale();
            }
            else if (_Mode == enMode.Update)
            {
                return UpdateBookSale();
            }
            return false;
        }

        public static DataTable GetAllBookSales()
        {
            return clsBookSaleData.GetAllSales();
        }

        public static bool DeleteBookSale(int saleID)
        {
            return clsBookSaleData.DeleteSale(saleID);
        }

        public static clsBookSale Find(int saleID)
        {
            int? readerID = null, bookID = null, paymentID = null, soldByUserID = null;
            string notes = null;

            if (clsBookSaleData.FindSale(saleID, ref bookID, ref readerID, ref paymentID, ref soldByUserID, ref notes))
            {
                return new clsBookSale(
                    saleID, bookID.Value, readerID.Value, paymentID.Value, soldByUserID.Value, notes
                );
            }
            return null;
        }
    }
}
