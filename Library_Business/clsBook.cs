using DVLD.Global_Classes;
using Library_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library_Business
{
    public class clsBook
    {
        public int? BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string SerialNumber { get; set; }
        public int? GenreID { get; set; }
        public clsBookGenres BookGenreInfo;
        public clsLanguage LanguageInfo;
        public int? LanguageID { get; set; }
        public enum enStatus { Available = 1, Borrowed = 2, Reserved = 3 }
        public enStatus? Status { get; set; }
        public int? CopiesCount { get; set; }
        public string Description { get; set; }
        private enum enMode { AddNew = 1, Update = 2 }
        private enMode Mode { get; set; }

        // Default constructor
        public clsBook()
        {
            BookID = null;
            Title = null;
            Author = null;
            ISBN = null;
            GenreID = null;
            LanguageID = null;
            Status = enStatus.Available;
            CopiesCount = null;
            Description = null;
            SerialNumber = null;
            Mode = enMode.AddNew;
        }

        // Private constructor for internal use
        private clsBook(int bookID, string title, string author, string isbn, string serialNumber, int genreID, int languageID,  int copiesCount, string description)
        {
            BookID = bookID;
            Title = title;
            Author = author;
            ISBN = isbn;
            GenreID = genreID;
            LanguageID = languageID;
            Status = clsBook.GetBookStatus(bookID,copiesCount); //The status will be calculated from data base if the count of borrowed records for this books is equal to the copies count 
            CopiesCount = copiesCount;
            Description = description;
            SerialNumber = serialNumber;
            BookGenreInfo = clsBookGenres.Find(genreID);
            LanguageInfo = clsLanguage.Find(languageID);


            Mode = enMode.Update;
        }


        public static  enStatus GetBookStatus(int bookID,int copiesCount)
        {
            int AvaialableCoppies = copiesCount - clsBorrow.GetActiveBorrowsCopyCountForBook(bookID);
            return (AvaialableCoppies == 0) ? enStatus.Borrowed : enStatus.Available;
        }
        public bool UpdateCopiesCount(int newCopiesCount)
        {
            if (BookID == null)
            {
                return false; // BookID is required
            }

            // Call the data access layer to update the copies count
            return clsBooksData.UpdateCopiesCount(BookID.Value, newCopiesCount);
        }        
                

        // Method to get all books
        public static DataTable GetAllBooks(string Language)
        {
            // Get all books using the data access layer
            return clsBooksData.GetAllBooks(Language);
        }

        // Method to add a new book            
            private bool _AddNewBook()
            {
                if (Title == null || Author == null  || SerialNumber == null || GenreID == null || LanguageID == null || Status == null || CopiesCount == null)
                {
                    return false; // Required fields are missing
                }

                // Call the data access layer to add the new book
                BookID = clsBooksData.AddNew(Title, Author, ISBN, SerialNumber, GenreID.Value, LanguageID.Value,  CopiesCount.Value, Description);
                return BookID != null;
            }

            // Method to update an existing book
            private bool _UpdateBook()
            {
                if (BookID == null || Title == null || Author == null || SerialNumber == null || GenreID == null || LanguageID == null || Status == null || CopiesCount == null)
                {
                    return false; // Required fields are missing
                }

                // Call the data access layer to update the book
                return clsBooksData.Update(BookID.Value, Title, Author, ISBN, SerialNumber, GenreID.Value, LanguageID.Value, CopiesCount.Value, Description);
            }

        // Method to save the book (add new or update)
        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (_AddNewBook())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                return false;
            }
            else
            {
                return _UpdateBook();
            }
        }    



            // Method to delete a book by its ID
            public static bool DeleteBook(int bookID)
            {
                // Call the data access layer to delete the book
                return clsBooksData.Delete(bookID);
            }


        public static bool IsDeleted(String BookNumber)
        {
            return clsBooksData.IsBookDeleted(BookNumber);
        }

        public static bool IsDeleted(int BookID)
        {
            return clsBooksData.IsBookDeleted(BookID);
        }


        public static bool RestoreDeletedBook(int bookID)
        {
            return clsBooksData.RestoreDeletedBook(bookID);
        }

        public static bool RestoreDeletedBook(String bookNumber)
        {
            return clsBooksData.RestoreDeletedBook(bookNumber);
        }

        // Method to find a book by its ID
        public static clsBook FindByID(int bookID)
            {
                string title = null, author = null, isbn = null, serialNumber = null, description = null;
                int? genreID = null, languageID = null, status = null, copiesCount = null;

                // Find the book using the data access layer
                bool isFound = clsBooksData.Find(bookID, ref title, ref author, ref isbn, ref serialNumber, ref genreID, ref languageID,  ref copiesCount, ref description);
                if (isFound)
                {
                    return new clsBook(bookID, title, author, isbn, serialNumber, genreID.Value, languageID.Value,  copiesCount.Value, description);
                }

                // If not found, return null
                return null;
            }

            // Method to find a book by its ISBN
            //public static clsBook FindByISBN(string isbn)
            //{
            //    int? bookID = null, genreID = null, languageID = null, status = null, copiesCount = null;
            //    string title = null, author = null, serialNumber = null, description = null;

            //    // Find the book by ISBN using the data access layer
            //    bool isFound = clsBooksData.FindByISBN(isbn, ref bookID, ref title, ref author, ref serialNumber, ref genreID, ref languageID,  ref copiesCount, ref description);
            //    if (isFound)
            //    {
            //        return new clsBook(bookID.Value, title, author, isbn, serialNumber, genreID.Value, languageID.Value, copiesCount.Value, description);
            //    }

            //    // If not found, return null
            //    return null;
            //}

        // Method to find a book by its ISBN
        public static clsBook FindByTitle(string Title)
        {
            int? bookID = null, genreID = null, languageID = null, status = null, copiesCount = null;
            string ISBN = null, author = null, serialNumber = null, description = null;

            // Find the book by ISBN using the data access layer
            bool isFound = clsBooksData.FindByTitle(Title, ref bookID,  ref author, ref ISBN, ref serialNumber, ref genreID, ref languageID,  ref copiesCount, ref description);
            if (isFound)
            {
                return new clsBook(bookID.Value, Title, author, ISBN,  serialNumber, genreID.Value, languageID.Value,  copiesCount.Value, description);
            }

            // If not found, return null
            return null;
        }

        public static clsBook FindBySerialNumber(string serialNumber)
        {
            int? bookID = null, genreID = null, languageID = null, status = null, copiesCount = null;
            string title = null, author = null, isbn = null, description = null;

            // Find the book by ISBN using the data access layer
            bool isFound = clsBooksData.FindBySerialNumber(serialNumber, ref bookID, ref title, ref author, ref isbn, ref genreID, ref languageID, ref copiesCount, ref description);
            if (isFound)
            {
                return new clsBook(bookID.Value, title, author, isbn, serialNumber, genreID.Value, languageID.Value, copiesCount.Value, description);
            }

            // If not found, return null
            return null;
        }

        public static DataTable GetAllAuthors()
        {
            return clsBooksData.GetAllAuthors();
        }
    }
    }



