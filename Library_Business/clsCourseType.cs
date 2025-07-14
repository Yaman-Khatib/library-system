using Library_DataAccess;
using System;
using System.Data;

namespace Library_Business
{
    public class clsCourseType
    {
        public int? CourseTypeID { get; set; }
        public string CourseTypeName { get; set; }
        public string Description { get; set; }
        private enum enMode { AddNew = 1, Update =2};
        enMode _Mode;
        public clsCourseType()
        {
            CourseTypeID = null;
            CourseTypeName = null;
            Description = null;
            _Mode = enMode.AddNew;
        }

        public clsCourseType(int? courseTypeID, string courseTypeName, string description)
        {
            CourseTypeID = courseTypeID;
            CourseTypeName = courseTypeName;
            Description = description;
            _Mode = enMode.Update;
        }

        private bool _AddNewCourseType()
        {
            CourseTypeID = clsCourseTypesData.AddNewCourseType(CourseTypeName, Description);
            return CourseTypeID != null;
        }

        private bool _UpdateCourseType()
        {
            return clsCourseTypesData.UpdateCourseType(CourseTypeID.Value, CourseTypeName, Description);
        }

        public static clsCourseType Find(int courseTypeID)
        {
            string courseTypeName = null;
            string description = null;
            if (clsCourseTypesData.FindCourseType(courseTypeID, ref courseTypeName, ref description))
            {
                return new clsCourseType(courseTypeID, courseTypeName, description);
            }
            else
                return null;
        }
        public static clsCourseType Find(String courseTypeName)
        {
            int? courseTypeID = null;
            string description = null;
            if (clsCourseTypesData.FindCourseTypeByName(ref courseTypeID,  courseTypeName, ref description))
            {
                return new clsCourseType(courseTypeID, courseTypeName, description);
            }
            else
                return null;
        }

        public static int? GetIDbyName(string TypeName)
        {
            return clsCourseTypesData.GetIDbyName(TypeName);
        }

        public static DataTable GetAllCourseTypes()
        {
            return clsCourseTypesData.GetAllCourseTypes();
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCourseType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                    case enMode.Update:
                    return _UpdateCourseType();
            }
                    return false;
        }

        public static bool DeleteCourseType(int courseTypeID)
        {
            return clsCourseTypesData.DeleteCourseType(courseTypeID);
        }
    }
}
