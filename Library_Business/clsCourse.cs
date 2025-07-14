using Library_DataAccess;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Library_Business
{
    public class clsCourse
    {
        public int? CourseID { get; set; }
        public int? CourseTypeID { get; set; }
        public clsCourseType CourseTypInfo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Fees { get; set; }
        public int? MaxParticipants { get; set; }
        public int? EnrolledParticipants { get; set; }
        public int? TutorID { get; set; }
        public clsTutor TutorInfo { get; set; }
        public string Description { get; set; }
        public enum enStatus
        {
            Upcoming = 1,
            OnGoing = 2,
            Expired = 3
        }
        public enStatus? Status { get; set; }
        enum enMode { AddNew = 1, Update = 2 }
        enMode _Mode;
        public clsCourse()
        {
            CourseID = null;
            CourseTypeID = null;
            StartDate = null;
            EndDate = null;
            Fees = null;
            MaxParticipants = null;
            Description = null;
            EnrolledParticipants = null;
            Status = null;
            _Mode = enMode.AddNew;
        }

        public clsCourse(int? courseID, int? courseTypeID, DateTime? startDate, DateTime? endDate, decimal? fees, int? maxParticipants,int? tutorID, string notes)
        {
            CourseID = courseID;
            CourseTypeID = courseTypeID;
            CourseTypInfo = clsCourseType.Find(courseTypeID.Value);
            StartDate = startDate;
            EndDate = endDate;
            Fees = fees;
            MaxParticipants = maxParticipants;
            EnrolledParticipants = clsCourseEnrolment.GetEnrollmentsCount(this.CourseID.Value);
            TutorInfo = clsTutor.Find(tutorID.Value);
            Description = notes;
            _Mode = enMode.Update;

            if (this.IsUpcoming())
                Status = enStatus.Upcoming;
            else if (this.IsOngoing())
                Status = enStatus.OnGoing;
            else
                Status = enStatus.Expired;
        }

        private bool _AddNewCourse()
        {
            CourseID = clsCoursesData.AddNewCourse(CourseTypeID.Value, StartDate.Value, EndDate.Value, Fees.Value, MaxParticipants, TutorID.Value, Description);
            return CourseID != null;
        }

        private bool _UpdateCourse()
        {
            return clsCoursesData.UpdateCourse(CourseID.Value, CourseTypeID.Value, StartDate.Value, EndDate.Value, Fees.Value, MaxParticipants, TutorID.Value, Description);
        }

        public static clsCourse Find(int courseID)
        {
            int? courseTypeID = null;
            DateTime? startDate = null, endDate = null;
            decimal? fees = null;
            int? maxParticipants = null;
            string notes = null;
            int? tutorID = null;
            if (clsCoursesData.FindCourse(courseID, ref courseTypeID, ref startDate, ref endDate, ref fees, ref maxParticipants,ref tutorID, ref notes))
            {
                return new clsCourse(courseID, courseTypeID, startDate, endDate, fees, maxParticipants,tutorID ,notes);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCourse())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateCourse();
                default:
                    return false;
            }
        }

        public static bool DeleteCourse(int courseID)
        {
            return clsCoursesData.DeleteCourse(courseID);
        }

        public static DataTable GetAllCoursesForAPeriod(DateTime StartDate,DateTime EndDate)
        {
            return clsCoursesData.GetAllCourses(StartDate,EndDate);
        }
        public bool IsOngoing()
        {
            return this.StartDate.HasValue && this.EndDate.HasValue &&
                   this.StartDate.Value.Date <= DateTime.Now.Date && this.EndDate.Value.Date >= DateTime.Now.Date;
        }

        public bool IsUpcoming()
        {
            return this.StartDate.HasValue &&
                   this.StartDate.Value.Date > DateTime.Now.Date;
        }        

        public static bool IsDeleted(int CourseID)
        {
            return clsCourse.IsDeleted(CourseID);
        }

        public bool IsExpired()
        {
            return this.EndDate.HasValue &&
                   DateTime.Now.Date > this.EndDate.Value.Date;
        }

    }
}
