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
namespace The_Story_Corner_Project.Courses
{
    public partial class frmListEnrolledReadersInACourse : KryptonForm
    {
        DataTable _dtEnrolledReaders;
        int courseID;
        public frmListEnrolledReadersInACourse(int CourseID)
        {
            InitializeComponent();
            this.courseID = CourseID;
            if(!ctrlCourseInfo1.SetInfo(CourseID))
            {
                this.Close();
            }
        }

        private void frmListEnrolledReadersInACourse_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
            ctrlCourseInfo1.OnCourseInfoSet += OnCourseInfoSet;
        }

        void OnCourseInfoSet(int ID)
        {
            courseID = ID;            
        }

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsCourseEnrolment.GetAllEnrollments(courseID);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvEnrollments.DataSource = null;


            try
            {
                // Disable the DataGridView while loading data
                dgvEnrollments.Enabled = false;

                // Show a loading message or spinner
                //pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtEnrolledReaders = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvEnrollments.DataSource = _dtEnrolledReaders;
                //if (dgvEnrollments.RowCount > 0)
                //{
                //    pctrLoading.Visible = false;
                //}
                //else
                //{ pctrLoading.Visible = true; }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // Re-enable the DataGridView
                dgvEnrollments.Enabled = true;
            }
            lblRecordsCount.Text = dgvEnrollments.RowCount.ToString();

            StyleColumns();
        }
        private void StyleColumns()
        {
            dgvEnrollments.Columns["AccountNumber"].HeaderText = "Account number";
            dgvEnrollments.Columns["AccountNumber"].Width = 130;

            //dgvEnrollments.Columns["ReaderID"].HeaderText = "Reader ID";
            //dgvEnrollments.Columns["ReaderID"].Width = 140;



            //dgvEnrollments.Columns["FullName"].HeaderText = "Full Name";
            dgvEnrollments.Columns["Person name"].Width = 190;                                               
            
            //dgvEnrollments.Columns["Enrolled by user"].Width = 120;


            //dgvEnrollments.Columns["Description"].Width = 220;

            //// Hide unnecessary columns


            //if (dgvEnrollments.Columns.Contains("PaymentTypeID"))
            //{
            //    dgvEnrollments.Columns["PaymentTypeID"].Visible = false;
            //}

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            int maxParticipants = ctrlCourseInfo1.course.MaxParticipants.Value;
            int EnrolledParticipants = ctrlCourseInfo1.course.EnrolledParticipants.Value;

            if (EnrolledParticipants >= maxParticipants)
            {
                DialogResult result = MessageBox.Show(
                    "The maximum number of participants for this course has already been reached.\n\n" +
                    "Would you like to edit the course details to allow more participants?",
                    "Maximum Participants Reached",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {

                    frmAddUpdateCourse frmCourse = new frmAddUpdateCourse(courseID);
                    frmCourse.ShowDialog();
                    ctrlCourseInfo1.SetInfo(courseID);
                    LoadDataGridViewAsync();
                    return;

                }
                else
                {
                    return;
                }
            }
            frmEnrolleReaderInCourse frm = new frmEnrolleReaderInCourse(courseID);            
            frm.ShowDialog();
            LoadDataGridViewAsync();
        }
    }
}
