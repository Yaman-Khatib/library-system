using Library_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Business
{
    public class clsBorrow
    {
        public int? BorrowID { get; set; }
        public int? ReaderID { get;
            
            set
                ; }        
        public clsReader ReaderInfo;
        public int? BookID { get; set; }
        public clsBook BookInfo;
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public int? ExtendedDays { get; set; }
        public enum enBorrowStatus
        {
            BorrowedOnTime = 1,
            BorrowedOverdue = 2,
            ReturnedOnTime = 3,
            ReturnedOverdue = 4,
            Unknown = -1
        }
        /// <summary>
        /// This field is calculated in data base according to the actual return date
        /// </summary>
        public enBorrowStatus? StatusIndex { get; set; }
        
        public int? CreatedByUserID { get; set; }
        public clsUser UserInfo;
        public string Notes { get; set; }
        
        private enum enMode { AddNew = 1, Update = 2 };
        private enMode Mode { get; set; }

        // Constructors
        public clsBorrow()
        {
            BorrowID = null;
            ReaderID = null;
            ReaderInfo = null;
            BookID = null;
            BookInfo = null;
            BorrowDate = null;
            DueDate = null;
            ActualReturnDate = null;
            ExtendedDays = null;
            CreatedByUserID = null;
            UserInfo = null;
            Notes = null;

            StatusIndex = enBorrowStatus.BorrowedOnTime;
            Mode = enMode.AddNew;
        }

        private clsBorrow(int? borrowID, int? readerID, int? bookID, DateTime? borrowDate, DateTime? dueDate, DateTime? actualReturnDate,enBorrowStatus? statusIndex, int? extendedDays, int? createdByUserID, string notes)
        {
            BorrowID = borrowID;
            ReaderID = readerID;
            ReaderInfo = clsReader.FindByReaderID(readerID.Value);
            BookID = bookID;
            BookInfo = clsBook.FindByID(bookID.Value);
            BorrowDate = borrowDate;
            DueDate = dueDate;
            ActualReturnDate = actualReturnDate;
            ExtendedDays = extendedDays;
            CreatedByUserID = createdByUserID;
            UserInfo = clsUser.FindByUserID(createdByUserID.Value);
            Notes = notes;
            StatusIndex = CalculateStatusIndex(this.DueDate,this.ActualReturnDate);
            Mode = enMode.Update;
        }

        // Private methods to add or update borrow records
        private bool _AddNewBorrow()
        {
            if (ReaderID == null || BookID == null || BorrowDate == null || DueDate == null || CreatedByUserID == null)
                return false; // Required fields are missing
            
            if (ReaderInfo != null)
            {
                BorrowID = clsBorrowsData.AddNewBorrow(ReaderID.Value, BookID.Value, BorrowDate.Value, DueDate.Value, CreatedByUserID.Value, Notes);
                return BorrowID != null;
            }
            return false;
        }




        public static int GetActiveBorrowsCopyCountForBook(int bookID)
        {
            return clsBorrowsData.GetActiveBorrowsCopyCountForBook(bookID);
        }

        public int CalculateOverueDays()
        {

            // If DueDate is null or the current date is not overdue, return 0
            if (DueDate == null || DateTime.Now <= DueDate.Value)
            {
                return 0;
            }

            return (DateTime.Now - DueDate.Value).Days;
        }

            public int CalculateOverdueWeeks()
        {

            // If DueDate is null or the current date is not overdue, return 0
            if (DueDate == null || DateTime.Now <= DueDate.Value)
            {
                return 0;
            }

            int daysOverdue = (DateTime.Now - DueDate.Value).Days;

            return (int)Math.Ceiling(daysOverdue / 7.0);
            
        }
        private bool _UpdateBorrow()
        {
            if (BorrowID == null || ReaderID == null || BookID == null || BorrowDate == null || DueDate == null || CreatedByUserID == null)
                return false; // Required fields are missing

            return clsBorrowsData.UpdateBorrow(BorrowID.Value, ReaderID.Value,  BookID.Value, BorrowDate.Value, DueDate.Value, ActualReturnDate, ExtendedDays ?? 0, CreatedByUserID.Value, Notes);
        }


        public bool CanBorrowBeExtended()
        {
            return (this.ExtendedDays < clsLibrarySettings.GetDefaultExtendDays() *  clsLibrarySettings.GetDefaultExtendTimesPerBorrow());
        }


        public static enBorrowStatus CalculateStatusIndex(DateTime? DueDate, DateTime? ActualReturnDate)
        {
            if (!DueDate.HasValue) return enBorrowStatus.Unknown;

            DateTime now = DateTime.Now;

            // Case 1: Book is currently borrowed and on time
            if (ActualReturnDate == null && DueDate >= now)
            {
                return enBorrowStatus.BorrowedOnTime;
            }
            // Case 2: Book is returned on time
            else if (ActualReturnDate.HasValue && ActualReturnDate <= DueDate)
            {
                return enBorrowStatus.ReturnedOnTime;
            }
            // Case 3: Book is currently borrowed and overdue
            else if (ActualReturnDate == null && DueDate < now)
            {
                return enBorrowStatus.BorrowedOverdue;
            }
            // Case 4: Book is returned but overdue
            else if (ActualReturnDate.HasValue && ActualReturnDate > DueDate)
            {
                return enBorrowStatus.ReturnedOverdue;
            }

            return enBorrowStatus.Unknown;
        }

        // Public Save method to determine whether to add or update
        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (_AddNewBorrow())
                {
                    Mode = enMode.Update;
                    return true;
                }
                return false;
            }
            else
            {
                if( _UpdateBorrow())
                {
                    this.StatusIndex = CalculateStatusIndex(this.DueDate, this.ActualReturnDate);
                    return true;
                }
                return false;
            }
        }

        // Static method to find a borrow by BorrowID
        public static clsBorrow Find(int borrowID)
        {
            // Initialize non-nullable local variables with default values
            int readerID = 0;
            int bookID = 0;
            int extendedDaysCount = 0;
            int createdByUserID = 0;
            DateTime borrowDate = default(DateTime);
            DateTime dueDate = default(DateTime);
            DateTime? actualReturnDate = null;
            
            string notes = null;

            // Call the data access method
            bool isFound = clsBorrowsData.FindBorrow(borrowID, ref readerID,  ref bookID,
                                                     ref borrowDate, ref dueDate, ref extendedDaysCount,
                                                     ref actualReturnDate, ref createdByUserID, ref notes);

            if (isFound)
            {
                // Return a new instance of clsBorrow with the retrieved values
                return new clsBorrow(borrowID, readerID, bookID, borrowDate, dueDate, actualReturnDate,CalculateStatusIndex(dueDate,actualReturnDate),
                                     extendedDaysCount, createdByUserID, notes);
            }

            // If not found, return null
            return null;
        }

        // Method to delete a borrow by BorrowID
        public static bool DeleteBorrow(int borrowID)
        {
            return clsBorrowsData.DeleteBorrowing(borrowID);
        }

        // Method to return a book (set actual return date)
        public bool ReturnBook()
        {
            if (BorrowID == null)
                return false;

            ActualReturnDate = DateTime.Now;
            bool Result =  clsBorrowsData.ReturnBook(BorrowID.Value, Notes);
            if(Result)
            {
                this.StatusIndex = CalculateStatusIndex(DueDate,ActualReturnDate);
                return true;
            }
            return false;
        }

        // Method to extend a borrowing period
        public bool ExtendBorrow(int extendDays)
        {
            if (BorrowID == null)
                return false;

            if (clsBorrowsData.ExtendBorrowing(BorrowID.Value, extendDays))
            {
                ExtendedDays = (ExtendedDays ?? 0) + extendDays;
                DueDate = DueDate?.AddDays(extendDays);
                return true;
            }
            return false;
        }

        // Static methods to retrieve borrows for a specific reader or all borrows
        public static DataTable GetAllBorrowsForReader(int readerID)
        {
            return clsBorrowsData.GetAllBorrowsForReader(readerID);
        }

        public static int GetNumberOfActiveBorrowedCopies(int bookID)
        {
            return clsBorrowsData.GetActiveBorrowsCopyCountForBook(bookID);
        }

        public static DataTable GetAllBorrows(enBorrowStatus statusIndex,DateTime startDate,DateTime endDate)
        {
            return clsBorrowsData.GetAllBorrows((int)statusIndex, startDate, endDate);

        }

        public static bool ReturnBook(int borrowID, string Notes)
        {
            return clsBorrowsData.ReturnBook(borrowID, Notes);
        }


        public static int GetNumberOfCurrentlyBorrowedMagazinesForReader(int readerID)
        {
            return clsBorrowsData.GetNumberOfCurrentlyBorrowedMagazinesByReader(readerID);
        }


        public static int GetNumberOfCurrentlyBorrowedClassicBooksToReader(int readerID)
        {
            return clsBorrowsData.GetNumberOfCurrentlyBorrowedClassicBooksByReader(readerID);
        }

    }
}
