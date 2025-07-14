using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DVLD.Global_Classes;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;
namespace The_Story_Corner_Project.Courses
{

    public partial class frmManageCourses : KryptonForm
    {
        DataTable _dtCourses;
        public frmManageCourses()
        {
            InitializeComponent();
        }

        private void frmManageCourses_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
            _ResetFilters();
        }

        void _ResetFilters()
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
            txtFilterValue.Text = "";
            pnlStatus.Visible = false;

            //Reset date time pickers

            dtpMinStartDate.MinDate = new DateTime(2023, 1, 1);
            dtpMaxDate.MaxDate = DateTime.Now.AddYears(1);

            dtpMaxDate.Value = DateTime.Now.AddMonths(3);
            dtpMinStartDate.Value = DateTime.Now.AddYears(-1);


        }

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsCourse.GetAllCoursesForAPeriod(dtpMinStartDate.Value, dtpMaxDate.Value);
        }

        private async void LoadDataGridViewAsync()
        {
            cbFilterBy.SelectedIndex = 0;
            try
            {
                // Disable the DataGridView while loading data
                dgvCourses.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtCourses = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvCourses.DataSource = _dtCourses;
                if (dgvCourses.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { pctrLoading.Visible = true; }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                MessageBox.Show("Error loading data: " + ex.Message);
                clsLogEvent.Log(ex);
            }
            finally
            {
                // Re-enable the DataGridView
                dgvCourses.Enabled = true;
            }
            lblRecordsCount.Text = dgvCourses.RowCount.ToString();
            StyleColumns();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = false;
                pnlStatus.Visible = false;
                if (_dtCourses != null)
                {
                    _dtCourses.DefaultView.RowFilter = "";
                    lblRecordsCount.Text = dgvCourses.RowCount.ToString();
                    if (dgvCourses.RowCount > 0)
                    {
                        pctrLoading.Visible = false;
                    }
                    else
                    { pctrLoading.Visible = true; }
                    return;
                }

            }
            else if (cbFilterBy.SelectedIndex == 1)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = false;
                pnlStatus.Visible = true;
                if (rbExpired.Checked == true)
                    _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Expired'");
                else if (rbActive.Checked)
                    _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Ongoing'");
                else
                    _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Upcoming'");

            }
            else
            {
                txtFilterValue.Text = "";
                pnlStatus.Visible = false;
                txtFilterValue.Visible = true;
            }
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }


        private void StyleColumns()
        {
            dgvCourses.Columns["CourseID"].HeaderText = "course ID";
            dgvCourses.Columns["CourseID"].Width = 120;


            dgvCourses.Columns["CourseName"].HeaderText = "course name";
            dgvCourses.Columns["CourseName"].Width = 150;

            dgvCourses.Columns["Description"].Width = 220;


            dgvCourses.Columns["Tutor"].HeaderText = "Tutor";
            dgvCourses.Columns["Tutor"].Width = 150;

            dgvCourses.Columns["StartDate"].HeaderText = "Start date";
            dgvCourses.Columns["StartDate"].Width = 150;


            dgvCourses.Columns["EndDate"].HeaderText = "End date";
            dgvCourses.Columns["EndDate"].Width = 150;


            dgvCourses.Columns["Enrollment Fees"].Width = 150;
            dgvCourses.Columns["Max Participants"].Width = 150;


        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            String FilterColumn = "";
            switch (cbFilterBy.Text)
            {

                case "course name":
                    FilterColumn = "CourseName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                case "Tutor name":
                    FilterColumn = "Tutor";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (FilterColumn == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtCourses.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvCourses.RowCount.ToString();
                if (dgvCourses.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { pctrLoading.Visible = true; }
                return;
            }
            String FilterValue = txtFilterValue.Text.Trim();
            if (FilterColumn == "Status")
            {
                if (rbActive.Checked)
                {
                    _dtCourses.DefaultView.RowFilter = String.Format("{0} = '{1}'", FilterColumn, "Active");
                }
                else
                {
                    _dtCourses.DefaultView.RowFilter = String.Format("{0} = '{1}'", FilterColumn, "Expired");
                }
            }
            else
            {
                _dtCourses.DefaultView.RowFilter = String.Format("{0} like '{1}%' ", FilterColumn, txtFilterValue.Text.ToString().Trim());
            }
            if (dgvCourses.RowCount > 0)
            {
                pctrLoading.Visible = false;
            }
            else
            { pctrLoading.Visible = true; }
        }

        private void btnAddBook_Click_1(object sender, EventArgs e)
        {
            frmAddUpdateCourse frm = new frmAddUpdateCourse();
            frm.ShowDialog();
            LoadDataGridViewAsync();
        }

        private void rbUpComing_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExpired.Checked == true)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Expired'");
            else if (rbActive.Checked)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Ongoing'");
            else
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Upcoming'");

        }

        private void rbActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExpired.Checked == true)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Expired'");
            else if (rbActive.Checked)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Ongoing'");
            else
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Upcoming'");

        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExpired.Checked == true)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Expired'");
            else if (rbActive.Checked)
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Ongoing'");
            else
                _dtCourses.DefaultView.RowFilter = String.Format("Status = 'Upcoming'");

        }

        private void EnroleParticipant_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                int CourseID = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells[0].Value);
                int maxParticipants = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells["Max Participants"].Value);
                int EnrolledParticipants = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells["Enrolled Participants"].Value);

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
                        
                        frmAddUpdateCourse frmCourse = new frmAddUpdateCourse(CourseID);
                        frmCourse.ShowDialog();
                        LoadDataGridViewAsync();
                        return;

                    }
                    else
                    {
                        return;
                    }
                }

                frmEnrolleReaderInCourse frm = new frmEnrolleReaderInCourse(CourseID);
                frm.ShowDialog();
                LoadDataGridViewAsync();
            }
            else
            {
                MessageBox.Show(
                    "No course has been selected. Please choose a course from the list before proceeding to enroll a reader.",
                    "Course Selection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvCourses.SelectedRows.Count > 0)
            {
                int CourseID = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells[0].Value);
                frmAddUpdateCourse frm = new frmAddUpdateCourse(CourseID);
                frm.ShowDialog();
                LoadDataGridViewAsync();
            }
            else
            {
                MessageBox.Show(
                   "No course has been selected. Please choose a course from the list so you can edit it.",
                   "Course Selection Required",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning
               );
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                int CourseID = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells[0].Value);
                frmCourseDetatils frm = new frmCourseDetatils(CourseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                   "No course has been selected. Please choose a course from the list so you can edit it.",
                   "Course Selection Required",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning
               );
            }
        }

        private void listParticipantsItem_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                int CourseID = Convert.ToInt16(dgvCourses.SelectedRows[0].Cells[0].Value);
                frmListEnrolledReadersInACourse frm = new frmListEnrolledReadersInACourse(CourseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                   "No course has been selected. Please choose a course from the list so you can view enrolled readers.",
                   "Course Selection Required",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning
               );
            }
        }

        private void dgvCourses_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                if(dgvCourses.CurrentRow != null)
                {
                    dgvCourses.CurrentRow.Selected = true;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
            {
                MessageBox.Show("Only admins have permission to delete courses.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
