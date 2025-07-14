using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;
namespace The_Story_Corner_Project.Courses
{
    public partial class frmEnrolleReaderInCourse : KryptonForm
    {
        clsCourse course;
        int ReaderID;
        clsCourseEnrolment courseEnrolment = new clsCourseEnrolment();
        public frmEnrolleReaderInCourse(int CourseID)
        {
            InitializeComponent();
            course = clsCourse.Find(CourseID);
            if( course == null ) {
                this.Close();
            }
            btnSave.Enabled = false;
            if(ctrlCourseInfo1.SetInfo(course.CourseID.Value) == false)
            {
                //The error message will be shown by the user control
                this.Close();
            }
            ctrlReaderInfoWithFilter1.OnReaderSelected += OnReader_Selected;
            ctrlCourseInfo1.OnCourseInfoSet += OnCourseInfo_Set;
        }

        void OnCourseInfo_Set(int CourseID)
        {
            //Refresh the course enrollment info
            course = clsCourse.Find(CourseID);
        }

        private void OnReader_Selected(int? ReaderID)
        {
            // Check if the maximum participants have been reached
            if (course.EnrolledParticipants >= course.MaxParticipants)
            {
                DialogResult result = MessageBox.Show("The maximum number of participants for this course has been reached. do you want to edit course info to add more participants?.",
                                "Enrollment Limit Reached",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    frmAddUpdateCourse frm = new frmAddUpdateCourse(course.CourseID.Value);
                    frm.ShowDialog();
                    bool CanEnroll = course.EnrolledParticipants >= course.MaxParticipants;
                    //The reader wanted to modify the max participants but still not modified so quit
                    if (!CanEnroll)
                    {
                        btnSave.Enabled = false;
                        return;
                    }

                }
                
                btnSave.Enabled = false;
                return;
            }

            if (ReaderID == null)
            {
                btnSave.Enabled = false;
                return;
            }
            courseEnrolment.CourseInfo = ctrlCourseInfo1.course;
            this.ReaderID = ReaderID.Value;
            if(clsCourseEnrolment.IsReaderEnrolled(ReaderID.Value,course.CourseID.Value))
            {
                MessageBox.Show("Reader has already enrolled in this course, cannot enroll him again!", "Reader is already enrolled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnSave.Enabled = false;
                return;
            }
            btnSave.Enabled = true;
        }

        private void frmEnrolleReaderInCourse_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            courseEnrolment.CourseEnrolmentDate = DateTime.Now;
            courseEnrolment.ReaderID = ReaderID;
            courseEnrolment.CourseID = course.CourseID;
            courseEnrolment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            string Notes = txtNotes.Text.Trim();
            courseEnrolment.Notes = Notes != String.Empty ? Notes : null;

            if (courseEnrolment.Save())
            {
                MessageBox.Show(
                    "The reader has been successfully enrolled in the course!",
                    "Enrolment Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show(
                    "An error occurred while trying to save the enrolment. Please try again later.",
                    "Error Saving Enrolment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
