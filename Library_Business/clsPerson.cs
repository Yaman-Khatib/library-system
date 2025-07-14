using Library_DataAccess;
using System;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace Library_Business
{
    public class clsPerson
    {
        public int? PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string MotherName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FacebookUserName { get; set; }
        public string InstagramUserName { get; set; }
        public string School { get; set; }
        public int? EmergencyContact1ID { get; set; } // New property
        public int? EmergencyContact2ID { get; set; } // New property
        public string ImagePath { get; set; }
        public string Notes { get; set; }
        public clsEmergencyContact EmergencyContact1 { get; set; }
        public clsEmergencyContact EmergencyContact2 { get; set; }


        public string FullName { get; set; }
        public string FullNameFirstAndLast { get; set; }

        enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;

        public clsPerson()
        {
            PersonID = null;
            FirstName = null;
            SecondName = null;
            LastName = null;
            MotherName = null;
            Gender = null;
            DateOfBirth = null;
            Age = 0;
            Mobile = null;
            Phone = null;
            Email = null;
            Address = null;
            FacebookUserName = null;
            InstagramUserName = null;
            School = null;
            EmergencyContact1ID = null; // Initialize new property
            EmergencyContact2ID = null; // Initialize new property
            ImagePath = null;
            Notes = null;
            EmergencyContact1 = new clsEmergencyContact();
            EmergencyContact2 = new clsEmergencyContact();
            _Mode = enMode.AddNew;
        }

        private clsPerson(int? personID, string firstName, string secondName, string lastName, string motherName,
                          bool? gender, DateTime? dateOfBirth, string mobile, string phone, string email,
                          string address, string facebookUserName, string instagramUserName, string school,
                          int? emergencyContact1ID, int? emergencyContact2ID, string imagePath, string notes)
        {
            PersonID = personID;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            MotherName = motherName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Age = (DateTime.Now.Year - DateOfBirth.Value.Year - ((DateTime.Now.Month < DateOfBirth.Value.Month) ? 1 : ((DateTime.Now.Month == DateOfBirth.Value.Month && DateTime.Now.Day < DateOfBirth.Value.Day) ? 1 : 0))); 
            Mobile = mobile;
            Phone = phone;
            Email = email;
            Address = address;
            FacebookUserName = facebookUserName;
            InstagramUserName = instagramUserName;
            School = school;
            EmergencyContact1ID = emergencyContact1ID; // Set new property
            EmergencyContact2ID = emergencyContact2ID; // Set new property
            ImagePath = imagePath;
            Notes = notes;
            if(emergencyContact1ID != null)
                EmergencyContact1 = clsEmergencyContact.Find(emergencyContact1ID.Value);
            if(emergencyContact2ID != null)
                EmergencyContact2 = clsEmergencyContact.Find(emergencyContact2ID.Value);
            _Mode = enMode.Update;

            FullName = FirstName + " " + SecondName + " " + LastName;
            FullNameFirstAndLast = FirstName + " " + LastName;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPeopleData.AddNewPerson(FirstName, SecondName, LastName, MotherName, Gender, DateOfBirth,
                                                       Mobile, Phone, Email, Address, FacebookUserName,
                                                       InstagramUserName, School, EmergencyContact1ID,
                                                       EmergencyContact2ID, ImagePath, Notes);
            return PersonID != null;
        }

        private bool _UpdatePerson()
        {
            return clsPeopleData.UpdatePerson(PersonID, FirstName, SecondName, LastName, MotherName, Gender, DateOfBirth,
                                              Mobile, Phone, Email, Address, FacebookUserName, InstagramUserName,
                                              School, EmergencyContact1ID, EmergencyContact2ID, ImagePath, Notes);
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static clsPerson Find(int personID)
        {
            // Variables to hold the data retrieved from the data access method
            string firstName = null;
            string secondName = null;
            string lastName = null;
            string motherName = null;
            bool? gender = null;
            DateTime? birthDate = null;
            string mobile = null;
            string phone = null;
            string email = null;
            string address = null;
            string facebookUserName = null;
            string instagramUserName = null;
            string school = null;
            int? emergencyContact1ID = null;
            int? emergencyContact2ID = null;
            string imagePath = null;
            string notes = null;

            // Call the data access Find method to populate the variables
            bool isFound = clsPeopleData.Find(personID, ref firstName, ref secondName, ref lastName, ref motherName,
                                              ref gender, ref birthDate, ref mobile, ref phone, ref email, ref address,
                                              ref facebookUserName, ref instagramUserName, ref school,
                                              ref emergencyContact1ID, ref emergencyContact2ID, ref imagePath, ref notes);

            // Create a new instance of clsPerson using the retrieved data
            if (isFound)
                return new clsPerson(personID, firstName, secondName, lastName, motherName, gender, birthDate, mobile,
                                     phone, email, address, facebookUserName, instagramUserName, school,
                                     emergencyContact1ID, emergencyContact2ID, imagePath, notes);
            else
                return null;
        }

        public static bool DeletePerson(int personID)
        {

            clsPerson person = clsPerson.Find(personID);

            bool DeleteResult =  clsPeopleData.DeletePerson(personID);

            if (person.EmergencyContact1ID != null)
                clsEmergencyContact.DeleteEmergencyContact(person.EmergencyContact1ID.Value);
            if (person.EmergencyContact2ID != null)
                clsEmergencyContact.DeleteEmergencyContact(person.EmergencyContact2ID.Value);
            return DeleteResult;   
        }

        public bool IsPersonExists(int personID)
        {
            return clsPeopleData.IsPersonExist(personID);
        }
    }
}
