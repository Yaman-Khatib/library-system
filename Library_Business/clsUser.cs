using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library_Business
{
    public class clsUser
    {
        public enum enMode { AddNew =1, Update=2}
        private enMode _Mode;
        public enum enPermissions { FullAccess = -1,NoPermissions = 0, ReadersManagement = 1, BooksManagement = 2, CoursesManagement = 4, UsersManagement = 8, PaymentsManagement = 16 ,ManagementSettings = 32}
        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public enPermissions? Permissions { get; set; }
        public clsPerson PersonInfo { get; set; }

        public clsUser()
        {

            UserID = null;
            PersonID = null;
            UserName = null; 
            Password = null;
            Permissions = null;
            PersonInfo = new clsPerson();
            IsDeleted = false;
            _Mode = enMode.AddNew;
        }

        private clsUser(int? userID,int? personID, string userName, string password, enPermissions? permissions,bool isDeleted)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            Permissions = permissions;
            IsDeleted = isDeleted;
            PersonInfo = clsPerson.Find(personID.Value);
            _Mode = enMode.Update;
        }


        private bool _AddNewUser()
        {
            UserID = clsUsersData.AddNewUser(PersonID.Value, UserName, Password, (int)Permissions.Value);
            return UserID != null;
        }

        private bool _UpdateUser()
        {
            // Update the user using the data access layer
            return clsUsersData.UpdateUser(UserID.Value, UserName, Password, PersonID.Value, (int)Permissions.Value);
        }


        public static clsUser FindByUserID(int userID)
        {
            string userName = null;
            string password = null;
            int? personID = null;
            int? permissions = null;
            bool isDeleted = false;
            // Find the user using the data access layer
            bool isFound = clsUsersData.FindByUserID(userID, ref userName, ref password, ref personID, ref permissions,ref isDeleted);
            if (isFound)
            {
                return new clsUser(userID, personID, userName, password, (enPermissions?)permissions, isDeleted);
            }

            // If not found, return null
            return null;
        }

        public static clsUser FindByPersonID(int personID)
        {
            int? userID = null;
            string userName = null;
            string password = null;
            int? permissions = null;
            bool isDeleted = false;
            // Find the user by PersonID using the data access layer
            bool isFound = clsUsersData.FindByPersonID(personID, ref userID, ref userName, ref password, ref permissions,ref isDeleted);
            if (isFound)
            {
                return new clsUser(userID, personID, userName, password, (enPermissions?)permissions, isDeleted);
            }

            // If not found, return null
            return null;
        }

        public static clsUser GetUserInfoByUserNameAndPassword(string userName, string password)
        {
            int? userID = null;
            int? personID = null;
            string accountNumber = null;
            int? permissions = null;
            bool IsDeleted = false;
            // Authenticate the user using the data access layer
            bool isAuthenticated = clsUsersData.GetUserInfoByUserNameAndPassword(userName, password, ref userID, ref personID, ref accountNumber, ref permissions, ref IsDeleted);
            if (isAuthenticated)
            {
                return new clsUser(userID, personID, userName, password, (enPermissions?)permissions, IsDeleted);
            }

            // If not authenticated, return null
            return null;
        }

        public static DataTable GetAllUsers()
        {
            // Get all users using the data access layer
            return clsUsersData.GetAllUsers();
        }

        public static bool ChangePassword(int? UserID,string newPassword)
        {
            
            // Change the password using the data access layer
            return clsUsersData.ChangePassword(UserID.Value, newPassword);
        }

        public bool UpdateUserInfo(string NewUserName, string NewPassword, enPermissions? NewPermissions)
        {
            UserName = NewUserName;
            Password = NewPassword;
            Permissions = NewPermissions;
            return Save();
        }

        public static bool IsUserExistsByUserID(int userID)
        {
            // Check if the user exists using the data access layer
            return clsUsersData.IsUserExistsByUserID(userID);
        }

        public static bool IsUserExistsByPersonID(int personID)
        {
            return clsUsersData.IsUserExistsByPersonID(personID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser() == true)
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;


                case enMode.Update:
                    return _UpdateUser();

            }

            return false;
        }

        public static bool DeleteUser(int userID)
        {
            // Delete the user using the data access layer
            return clsUsersData.DeleteUser(userID);
        }


    }
}
