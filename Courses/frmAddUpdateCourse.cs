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
using The_Story_Corner_Project.Tutors;

namespace The_Story_Corner_Project.Courses
{
    public partial class frmAddUpdateCourse : KryptonForm
    {
        enum enMode {New = 1, Update = 2 };
        enMode _Mode;
        clsCourse course;
        int CourseID;
        public frmAddUpdateCourse()
        {
            InitializeComponent();
            _Mode = enMode.New;
        }

        public frmAddUpdateCourse(int CourseID)
        {
            InitializeComponent();
            this.CourseID = CourseID;
            _Mode = enMode.Update;
        }
        private void FillTutorNames()
        {
            DataTable _dtTutors = clsTutor.GetAllTutors();
            if (_dtTutors != null)
            {

                cbTutorName.Items.Clear();
                foreach (DataRow row in _dtTutors.Rows)
                {
                    cbTutorName.Items.Add(row["TutorName"].ToString());
                }
            }
        }
        private void FillCourseTypes()
        {
            DataTable _dtTutors = clsCourseType.GetAllCourseTypes();
            if (_dtTutors != null)
            {

                cbCourseName.Items.Clear();
                foreach (DataRow row in _dtTutors.Rows)
                {
                    cbCourseName.Items.Add(row["CourseTypeName"].ToString());
                }
            }
        }
        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.New)
            {
                lblModeTitle.Text = "Launch new course";
                this.Text = lblModeTitle.Text;
            }
            else
            {
                lblModeTitle.Text = "Update course info";
                this.Text = lblModeTitle.Text;
            }
            dtpStartDate.MinDate = DateTime.Now; 
            dtpEndDate.MinDate = DateTime.Now;
            FillCourseTypes();
            FillTutorNames();
        }
        void LoadData()
        {
            course = clsCourse.Find(CourseID);
            if(course != null) 
            {
                if(cbTutorName.Items.Count > 0) 
                cbTutorName.SelectedIndex = cbTutorName.Items.IndexOf(course.TutorInfo.PersonInfo.FullNameFirstAndLast);

                if (cbCourseName.Items.Count > 0)
                    cbCourseName.SelectedIndex = cbCourseName.Items.IndexOf(course.CourseTypInfo.CourseTypeName);

                numericFees.Value = course.Fees.Value;
                numericMaxParticipants.Value = course.MaxParticipants.Value;

                txtDescription.Text = course.Description ?? "";
            }
            else
            {
                MessageBox.Show("course info wasn't found","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            
            
        }

        private void frmLaunchUpdateCourse_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if(_Mode == enMode.Update)
            {
                LoadData();
            }
            else
            {
                course = new clsCourse();
            }
            frmAddUpdateTutors.OnTutorAdded += OnTutor_Added;
            frmAddUpdateCourseType.OnCourseTypeAdded += OnCourseTypeAdded;
        }

        private void OnTutor_Added(int? TutorID)
        {
            FillTutorNames();

            cbTutorName.SelectedIndex = cbTutorName.Items.IndexOf(clsTutor.Find(TutorID.Value).PersonInfo.FullNameFirstAndLast);
        }
        private void OnCourseTypeAdded(int CourseTypeID)
        {
            FillCourseTypes();
            cbCourseName.SelectedIndex = cbCourseName.Items.IndexOf(clsCourseType.Find(CourseTypeID).CourseTypeName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            course.StartDate = dtpStartDate.Value;
            course.EndDate = dtpEndDate.Value;
            course.MaxParticipants = (int)numericMaxParticipants.Value;
            course.Fees = (int)numericFees.Value;
            course.TutorID = clsTutor.GetIDByTutorName(cbTutorName.Text);
            course.CourseTypeID = clsCourseType.GetIDbyName(cbCourseName.Text);
            course.Description = txtDescription.Text;
            if(course.Save())
            {
                if(_Mode == enMode.New)
                {
                    MessageBox.Show("Course was added successfully!", "Launched successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Course was updated successfully!", "Updated successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                _Mode = enMode.Update;
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Couldn't save course info!", "An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBox_Validating(object sender, CancelEventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            if(temp.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "This field cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void numericFees_Validating(object sender, CancelEventArgs e)
        {
            NumericUpDown temp = (NumericUpDown)sender;
            if(temp.Value <= 0)
            {
                e.Cancel= true;
                errorProvider1.SetError(temp, "Please enter a valid fees amount!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void numericMaxParticipants_Validating(object sender, CancelEventArgs e)
        {
            NumericUpDown temp = (NumericUpDown)sender;
            if (temp.Value <= 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "Please enter a valid max participants count!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            frmAddUpdateTutors frm = new frmAddUpdateTutors();
            frm.ShowDialog();
            
        }

        private void btnAddNewCourseType_Click(object sender, EventArgs e)
        {
            frmAddUpdateCourseType frm = new frmAddUpdateCourseType();
            frm.ShowDialog();
            
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void dtpEndDate_Validating(object sender, CancelEventArgs e)
        {
            if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)  // Compare end date with start date
            {
                e.Cancel = true;
                errorProvider1.SetError(dtpEndDate, "End date must be later than the start date.");
            }
            else
            {
                errorProvider1.SetError(dtpEndDate, null);  // Clear the error if the condition is satisfied
            }
        }

    }
}
