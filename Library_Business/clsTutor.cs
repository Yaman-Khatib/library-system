using Library_DataAccess;
using System;
using System.Data;

namespace Library_Business
{
    public class clsTutor
    {
        public int? TutorID { get; set; }
        public int? PersonID { get; set; }
        public int? CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public clsPerson PersonInfo { get; set; }
        private enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode;

        // Constructor for a new Tutor
        public clsTutor()
        {
            TutorID = null;
            PersonID = null;
            PersonInfo = new clsPerson();
            CreatedByUserID = null;
            CreatedByUserInfo = null;
            _Mode = enMode.AddNew;
        }

        // Private constructor for an existing Tutor
        private clsTutor(int tutorID, int personID, int createdByUserID)
        {
            TutorID = tutorID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            PersonInfo = clsPerson.Find(personID);
            CreatedByUserInfo = clsUser.FindByUserID(createdByUserID);
            _Mode = enMode.Update;
        }

        // Add a new Tutor to the database
        private bool _AddNewTutor()
        {
            
                
                TutorID = clsTutorsData.AddNewTutor(PersonID.Value, CreatedByUserID.Value);
                return TutorID != null;
            
            
        }

        // Update an existing Tutor
        private bool _UpdateTutor()
        {
            if (PersonInfo.Save()) // Update the Person record
            {
                return clsTutorsData.UpdateTutor(TutorID.Value, PersonID.Value, CreatedByUserID.Value);
            }
            return false;
        }

        // Save the Tutor record (Add or Update)
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTutor())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTutor();
            }
            return false;
        }

        // Delete a Tutor
        public static bool DeleteTutor(int tutorID)
        {
            return clsTutorsData.DeleteTutor(tutorID);
        }

        // Find a Tutor by ID
        public static clsTutor Find(int tutorID)
        {
            int? personID = null;
            int? createdByUserID = null;

            if (clsTutorsData.FindTutor(tutorID, ref personID, ref createdByUserID))
            {
                return new clsTutor(tutorID, personID.Value, createdByUserID.Value);
            }
            return null;
        }

        // Get tutor ID by their name
        public static int? GetIDByTutorName(string name)
        {
            return clsTutorsData.GetIDByTutorName(name);
        }

        // Get all Tutors
        public static DataTable GetAllTutors()
        {
            return clsTutorsData.GetAllTutors();
        }
    }
}
