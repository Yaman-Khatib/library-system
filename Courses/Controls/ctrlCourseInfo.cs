using Library_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Story_Corner_Project.Courses.Controls
{
    public partial class ctrlCourseInfo : UserControl
    {
        public clsCourse course = new clsCourse();
        public ctrlCourseInfo()
        {
            InitializeComponent();
        }
        public Action<int> OnCourseInfoSet;

        public void ResetValues()
        {
            lblCourseID.Text = "N/A";
            lblCourseName.Text = "N/A";
            lblEndDate.Text = "N/A";
            lblEnrolledParticipants.Text = "N/A";
            lblMaxParticipants.Text = "N/A";
            lblStartDate.Text = "N/A";
            lblStatus.Text = "N/A";
            lblTutorName.Text = "N/A";
            lblNotes.Text = "N/A";
            lblFees.Text = "N/A";
            
        }
        public bool SetInfo(int CourseID)
        {
            course = clsCourse.Find(CourseID);
            if(course == null)
            {
                MessageBox.Show("Course info was not found!","Not found",MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetValues();
                return false;
            }

            lblCourseID.Text = course.CourseID.ToString();
            lblCourseName.Text = course.CourseTypInfo.CourseTypeName;
            lblTutorName.Text = course.TutorInfo.PersonInfo.FullNameFirstAndLast;
            lblEndDate.Text = course.EndDate.Value.ToShortDateString();
            lblStartDate.Text = course.StartDate.Value.ToShortDateString();
            lblEnrolledParticipants.Text = course.EnrolledParticipants.ToString();
            lblMaxParticipants.Text = course.MaxParticipants.ToString();
            lblFees.Text = course.Fees.ToString() + " S.P";
            lblNotes.Text = course.Description ?? "N/A";
            switch(course.Status)
            {
                case clsCourse.enStatus.Upcoming:
                    lblStatus.Text = "Upcoming";
                    break;
                case clsCourse.enStatus.OnGoing:
                    lblStatus.Text = "Ongoing";
                    break;
                case clsCourse.enStatus.Expired:
                    lblStatus.Text = "Expired";
                    break;
            }
            OnCourseInfoSet?.Invoke(course.CourseID.Value);
            return true;
        }

        private void llEditCourseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateCourse frm = new frmAddUpdateCourse(course.CourseID.Value);
            frm.ShowDialog();
            SetInfo(this.course.CourseID.Value);
        }
    }
}
