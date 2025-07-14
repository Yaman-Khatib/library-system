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

namespace The_Story_Corner_Project.Users
{
    public partial class frmManageUsers : KryptonForm
    {
        DataTable _dtUsers;
        public frmManageUsers()
        {
            InitializeComponent();
        }
        void _ResetFilters()
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
            
        }


        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
            _ResetFilters();
        }
        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(10);
            return clsUser.GetAllUsers();
        }

        private async void LoadDataGridViewAsync()
        {
            cbFilterBy.SelectedIndex = 0;
            try
            {
                // Disable the DataGridView while loading data
                dgvUsers.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtUsers = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvUsers.DataSource = _dtUsers;
                if (dgvUsers.RowCount > 0)
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
            }
            finally
            {
                // Re-enable the DataGridView
                dgvUsers.Enabled = true;
            }
            lblRecordsCount.Text = dgvUsers.RowCount.ToString();
            StyleColumns();
        }
        private void StyleColumns()
        {
            if (dgvUsers != null && dgvUsers.RowCount > 0)
            {

                dgvUsers.Columns["UserID"].HeaderText = "User ID";
                dgvUsers.Columns["UserID"].Width = 115;

                dgvUsers.Columns["UserName"].HeaderText = "User Name";
                dgvUsers.Columns["UserName"].Width = 160;

                dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                dgvUsers.Columns["FullName"].Width = 260;

                dgvUsers.Columns["MobileNumber"].HeaderText = "Mobile Number";
                dgvUsers.Columns["MobileNumber"].Width = 180;

                dgvUsers.Columns["Email"].Width = 200;

                dgvUsers.Columns["Address"].Width = 240;

                dgvUsers.Columns["IsDeleted"].Visible = false;

            }

        }

        void _UpdateDataGridView()
        {
            LoadDataGridViewAsync();
            lblRecordsCount.Text = dgvUsers.RowCount.ToString();
        }
        private void dgvReaders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _UpdateDataGridView();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";

            if (cbFilterBy.SelectedIndex == 0)
                txtFilterValue.Visible = false;
            else
                txtFilterValue.Visible = true;
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            String FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "User Name":
                    FilterColumn = "Username";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Gender":
                    FilterColumn = "Gender";
                    break;                    
                default:
                    FilterColumn = "None";
                    break;
            }

            if (FilterColumn == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                if (dgvUsers.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { pctrLoading.Visible = true; }
                return;
            }

            if (FilterColumn == "ReaderID")
            {
                _dtUsers.DefaultView.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtUsers.DefaultView.RowFilter = String.Format("{0} like '{1}%' ", FilterColumn, txtFilterValue.Text.ToString().Trim());
            }
            if (dgvUsers.RowCount > 0)
            {
                pctrLoading.Visible = false;
            }
            else
            { pctrLoading.Visible = true; }


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "_Reader ID")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar);
            }        
        }

        

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure that the click is on a valid row
            {
                dgvUsers.ClearSelection();
                dgvUsers.Rows[e.RowIndex].Selected = true; // Select the clicked row
            }
        }

        private void dgvUsers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvUsers.ClearSelection(); // Clear any previous selections
                dgvUsers.Rows[e.RowIndex].Selected = true; // Select the row
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
            {
                MessageBox.Show("Only admins have permission to delete users.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete user?", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {



                if (dgvUsers.SelectedRows.Count > 0)
                {
                    int userID = Convert.ToInt16(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                    if (userID == clsGlobal.CurrentUser.UserID)
                    {
                        MessageBox.Show("Cannot delete current user, please delete it using another account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (clsUser.DeleteUser(userID))
                    {
                        MessageBox.Show("User was deleted successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _UpdateDataGridView();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Please select a user to delete!", "Select user first", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _UpdateDataGridView();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = Convert.ToInt16(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
            frmAddUpdateUser frm = new frmAddUpdateUser(UserID);
            frm.ShowDialog();
            _UpdateDataGridView();

        }

        private void cmsReaders_Opening(object sender, CancelEventArgs e)
        {

        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented!");
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                frmShowUserInfo frm = new frmShowUserInfo(Convert.ToInt16(dgvUsers.SelectedRows[0].Cells["UserID"].Value));
                frm.ShowDialog();
                _UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a user first!", "Select user first", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
