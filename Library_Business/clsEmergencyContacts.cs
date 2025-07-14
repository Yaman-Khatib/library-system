using Library_DataAccess;
using System;
using System.Data;
using System.Xml.Linq;

namespace Library_Business
{
    public class clsEmergencyContact
    {
        public int? ContactID { get; set; }        
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Relation { get; set; }

        enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;

        public clsEmergencyContact()
        {
            ContactID = null;
            Name = null;
            PhoneNumber = null;
            Relation = null;
            _Mode = enMode.AddNew;
        }

        private clsEmergencyContact(int? contactID, string Name, string phoneNumber, string relation)
        {
            ContactID = contactID;
            this.Name = Name;
            PhoneNumber = phoneNumber;
            Relation = relation;
            _Mode = enMode.Update;
        }

        private bool _AddNewEmergencyContact()
        {
            this.ContactID = clsEmergencyContactData.AddNewEmergencyContact(Name, PhoneNumber, Relation);
            return ContactID != null;
        }

        private bool _UpdateEmergencyContact()
        {
            return clsEmergencyContactData.UpdateEmergencyContact(ContactID.Value, Name, PhoneNumber, Relation);
        }


        public static clsEmergencyContact Find(int contactID)
        {
            // Variables to hold the data retrieved from the data access method
            string name = null;
            string phoneNumber = null;
            string relation = null;

            // Call the data access Find method to populate the variables
            bool found = clsEmergencyContactData.Find(contactID, ref name, ref phoneNumber, ref relation);

            // If a record was found, create a new instance of clsEmergencyContact using the retrieved data
            if (found)
            {
                return new clsEmergencyContact(contactID, name, phoneNumber, relation);
            }

            // If no record was found, return null
            return null;
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewEmergencyContact())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateEmergencyContact();
            }

            return false;
        }

        

        public static bool DeleteEmergencyContact(int contactID)
        {
            return clsEmergencyContactData.DeleteEmergencyContact(contactID);
        }
        

        public static DataTable GetEmergencyContactsByPerson(int personID)
        {
            return clsEmergencyContactData.GetEmergencyContactsByPerson(personID);
        }
    }
}

