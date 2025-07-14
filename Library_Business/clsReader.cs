using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsReader
    {
        public enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode;
        public int? ReaderID { get; set; }
        public string AccountNumber { get; set; }
        public int? PersonID { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime? JoinDate { get; set; }
        public clsPerson PersonInfo { get; set; }
        
        public clsUser CreatedByUserInfo { get; set; }
        public clsSubscription LastSubscriptionInfo;

        public clsReader()
        {
            ReaderID = null;
            AccountNumber = null;
            PersonID = null;
            PersonInfo = new clsPerson();
            CreatedByUserID = null;
            JoinDate = null;            
            CreatedByUserInfo = null;
            LastSubscriptionInfo = null;
            _Mode = enMode.AddNew;
        }

        public static string PredictNewAccountNumber()
        {
            String PreviousAccountNumber = clsReadersData.GetLastAccountNumber();
            String CurrentYear = DateTime.Now.Year.ToString();

            string[] Parts = PreviousAccountNumber.Split('-');
            if (Parts.Length == 2)
            {
                if (Parts[0] == CurrentYear)
                {
                    int prevNum = int.Parse(Parts[1]) + 1;
                    return CurrentYear + "-" + prevNum.ToString();
                }
                else
                {
                    return CurrentYear + "-1";
                }
            }
            else //If there is no reader in the system return ex: 2025-1
                return CurrentYear + "-1";
        }
        public static string GetLastAddedAccountNumber()
        {
            return clsReadersData.GetLastAccountNumber();
        }
        private clsReader(int readerID, string accountNumber, int personID, int createdByUSerID, DateTime joinDate)
        {
            ReaderID = readerID;
            AccountNumber = accountNumber;
            PersonID = personID;
            CreatedByUserID = createdByUSerID;
            JoinDate = joinDate;
            PersonInfo = clsPerson.Find(personID);
            CreatedByUserInfo = clsUser.FindByUserID(createdByUSerID);
            LastSubscriptionInfo = clsSubscription.FindLastSubscription(readerID);
            _Mode = enMode.Update;
        }


        private bool _AddNewReader()
        {
            // Check for required values without throwing exceptions
            if (AccountNumber == null || PersonID == null || CreatedByUserID == null || JoinDate == null)
            {
                // Return false to indicate failure due to missing values
                return false;
            }

            // Add the new reader using the data access layer and get the inserted ReaderID
            ReaderID = clsReadersData.AddNewReader(AccountNumber, PersonID.Value, CreatedByUserID.Value, JoinDate.Value);
            return ReaderID != null;
        }

        private bool _UpdateReader()
        {
            if (ReaderID == null || AccountNumber == null || PersonID == null || CreatedByUserID == null )
            {
                return false;
            }

            // Update the reader using the data access layer
            return clsReadersData.UpdateReader(ReaderID.Value, AccountNumber, PersonID.Value, CreatedByUserID.Value);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewReader() == true)
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;


                case enMode.Update:
                    return _UpdateReader();

            }

            return false;
        }



        public static clsReader FindByReaderID(int readerID)
        {
            string accountNumber = null;
            int? personID = null;
            int? createdByUSerID = null;
            DateTime? joinDate = null;

            // Find the reader using the data access layer
            bool isFound = clsReadersData.FindByReaderID(readerID, ref accountNumber, ref personID, ref createdByUSerID, ref joinDate);
            if (isFound)
            {
                return new clsReader(readerID, accountNumber, personID.Value, createdByUSerID.Value, joinDate.Value);
            }

            // If not found, return null
            return null;
        }

        public static clsReader FindByAccountNumber(string accountNumber)
        {
            int? readerID = null;
            int? personID = null;
            int? createdByUSerID = null;
            DateTime? joinDate = null;

            // Find the reader by AccountNumber using the data access layer
            bool isFound = clsReadersData.FindByAccountNumber(accountNumber, ref readerID, ref personID, ref createdByUSerID, ref joinDate);
            if (isFound)
            {
                return new clsReader(readerID.Value, accountNumber, personID.Value, createdByUSerID.Value, joinDate.Value);
            }

            // If not found, return null
            return null;
        }

        public static clsReader FindReaderByPersonID(int personID)
        {
            int? readerID = null;
            string accountNumber = null;
            int? createdByUSerID = null;
            DateTime? joinDate = null;

            // Find the reader by PersonID using the data access layer
            bool isFound = clsReadersData.FindReaderByPersonID(personID, ref readerID, ref accountNumber, ref createdByUSerID, ref joinDate);
            if (isFound)
            {
                return new clsReader(readerID.Value, accountNumber, personID, createdByUSerID.Value, joinDate.Value);
            }

            // If not found, return null
            return null;
        }

        public static DataTable GetAllReaders()
        {
            // Get all readers using the data access layer
            return clsReadersData.GetAllReaders();
        }

        public static bool IsReaderExists(int readerID)
        {
            // Check if the reader exists using the data access layer
            return clsReadersData.IsReaderExists(readerID);
        }

        public static bool IsReaderExists(string accountNumber)
        {
            // Check if the reader exists using the data access layer
            return clsReadersData.IsReaderExists(accountNumber);
        }

        public static bool IsDeleted(int ReaderID)
        {
            return clsReadersData.IsReaderDeleted(ReaderID);
        }
        public static bool IsDeleted(String AccountNumber)
        {
            return clsReadersData.IsReaderDeleted(AccountNumber);
        }

        public static bool IsPersonAReader(int personID)
        {
            return clsReadersData.IsPersonAReader(personID);
        }

        public static bool DeleteReader(int readerID)
        {
            //We will delete a reader soft delete not physical delete
            return clsReadersData.SoftDelete(readerID);
        }
        public static bool RestoreDeletedReader(int ReaderID)
        {
            return clsReadersData.RestoreDeletedReader(ReaderID);
        }

        public static bool DeleteReader(string accountNumber)
        {
            // Delete the reader using the data access layer
            return clsReadersData.SoftDelete(accountNumber);
        }
        public static bool RestoreDeletedReader(String AccountNumber)
        {
            return clsReadersData.RestoreDeletedReader(AccountNumber);
        }
    }
}
