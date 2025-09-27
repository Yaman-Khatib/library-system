using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;
using The_Story_Corner_Project.Payments;
namespace The_Story_Corner_Project.Readers
{
    public partial class frmManageReaders : KryptonForm
    {
        private DataTable _dtReaders;
        
        // Pagination fields
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 0;
        private int _totalRecords = 0;
        private string _currentFilterColumn = "";
        private string _currentFilterValue = "";
        
        public frmManageReaders()
        {
            InitializeComponent();
        }
        void _ResetFilters()
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;
        }

        private void frmManageReaders_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
            _ResetFilters();

        }

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsReader.GetAllReaders();
        }

        private async Task<DataTable> GetPagedDataFromDatabaseAsync()
        {
            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsReader.GetAllReadersPaged(_currentFilterColumn, _currentFilterValue, _currentPage, _pageSize, out _totalRecords);
        }

        private async void LoadDataGridViewAsync()
        {
            cbFilterBy.SelectedIndex = 0;
            await LoadCurrentPageAsync();
        }

        private async Task LoadCurrentPageAsync()
        {
            try
            {
                // Disable the DataGridView while loading data
                dgvReaders.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the paged data from the database asynchronously
                _dtReaders = await GetPagedDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvReaders.DataSource = _dtReaders;
                
                if(dgvReaders.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { 
                    pctrLoading.Visible = true; 
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // Re-enable the DataGridView
                dgvReaders.Enabled = true;                
            }
            
            UpdatePaginationInfo();
            StyleColumns();
        }

        

        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void StyleColumns()
        {
            // Hide ID and IsDeleted columns
            if (dgvReaders.Columns.Contains("ReaderID"))
                dgvReaders.Columns["ReaderID"].Visible = false;
            if (dgvReaders.Columns.Contains("IsDeleted"))
                dgvReaders.Columns["IsDeleted"].Visible = false;

            if (dgvReaders.Columns.Contains("AccountNumber"))
            {
                dgvReaders.Columns["AccountNumber"].HeaderText = "Account Number";
                dgvReaders.Columns["AccountNumber"].Width = 160;
            }

            if (dgvReaders.Columns.Contains("FullName"))
            {
                dgvReaders.Columns["FullName"].HeaderText = "Full Name";
                dgvReaders.Columns["FullName"].Width = 250;
            }

            if (dgvReaders.Columns.Contains("MobileNumber"))
            {
                dgvReaders.Columns["MobileNumber"].HeaderText = "Mobile Number";
                dgvReaders.Columns["MobileNumber"].Width = 150;
            }

            if (dgvReaders.Columns.Contains("Phone"))
            {
                dgvReaders.Columns["Phone"].Width = 150;
            }

            if (dgvReaders.Columns.Contains("Address"))
            {
                dgvReaders.Columns["Address"].Width = 260;
            }

            if (dgvReaders.Columns.Contains("SubscriptionStatus"))
            {
                dgvReaders.Columns["SubscriptionStatus"].HeaderText = "Subscription Status";
                dgvReaders.Columns["SubscriptionStatus"].Width = 160;
            }

            // Make the last column responsive to fill remaining width
            // Find the last visible column and make it fill the remaining space
            var visibleColumns = dgvReaders.Columns.Cast<DataGridViewColumn>()
                .Where(col => col.Visible)
                .ToList();
            
            if (visibleColumns.Any())
            {
                var lastColumn = visibleColumns.Last();
                lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void UpdatePaginationInfo()
        {
            _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
            lblRecordsCount.Text = $"Page {_currentPage} of {_totalPages} ({_totalRecords} total records)";
        }

        private void GoToNextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadCurrentPageAsync();
            }
        }

        private void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCurrentPageAsync();
            }
        }



        void _UpdateDataGridView()
        {
            _currentPage = 1; // Reset to first page
            LoadCurrentPageAsync();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";

            if (cbFilterBy.SelectedIndex == 0)
            {
                txtFilterValue.Visible = false;
                pnlGender.Visible = false;
            }
            else if (cbFilterBy.SelectedIndex == 3)
            {
                pnlGender.Visible = true;
                txtFilterValue.Visible = false;
            }
            else
            {
                pnlGender.Visible = false;
                txtFilterValue.Visible = true;
            }

        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            String FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Reader ID":
                    FilterColumn = "ReaderID";
                    break;
                case "Account Number":
                    FilterColumn = "AccountNumber";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Gender":
                    FilterColumn = "Gender";
                    break;                
                default:
                    FilterColumn = "";
                    break;
            }           

            // Update current filter values
            _currentFilterColumn = FilterColumn;
            _currentFilterValue = txtFilterValue.Text.Trim();
            
            // Reset to first page and reload data
            _currentPage = 1;
            LoadCurrentPageAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "Reader ID")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar);
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateReader frm = new frmAddUpdateReader();
            frm.ShowDialog();
            _UpdateDataGridView();
           
        }

        private void dgvReaders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure that the click is on a valid row
            {
                dgvReaders.Rows[e.RowIndex].Selected = true; // Select the clicked row
            }
        }

        
        private void dgvReaders_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvReaders.ClearSelection(); // Clear any previous selections
                dgvReaders.Rows[e.RowIndex].Selected = true; // Select the row
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented!");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
            {
                MessageBox.Show("Only admins have permission to delete readers.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (MessageBox.Show("Are you sure you want to delete reader?", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (dgvReaders.SelectedRows.Count > 0)
                {
                    if (clsReader.DeleteReader(Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value)))
                    {
                        MessageBox.Show("Reader was deleted successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _UpdateDataGridView();
                        return;
                    }
                    else
                    {
                        MessageBox.Show(" Cannot delete reader!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Please select a reader to delete!", "Select reader first", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvReaders.SelectedRows.Count > 0)
            {
                int ReaderID = Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value);
                frmAddUpdateReader frm = new frmAddUpdateReader(ReaderID);
                frm.ShowDialog();
                _UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a reader to edit!", "Select reader first", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvReaders.SelectedRows.Count > 0)
            {
                int ReaderID = Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value);
                frmShowReaderInfo frm = new frmShowReaderInfo(ReaderID);
                frm.ShowDialog();
                _UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a reader first!", "Select reader first", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (dgvReaders.Rows.Count > 0)
            {
                int readerID = Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value);
                frmReaderPayments frm = new frmReaderPayments(readerID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a reader to view his payments!", "Select desired payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
            if (dgvReaders.Rows.Count > 0)
            {
                int readerID = Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value);
                frmReaderPayments frm = new frmReaderPayments(readerID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a reader to view his payments!", "Select desired payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(rbMale.Checked)
            {
                _currentFilterColumn = "Gender";
                _currentFilterValue = "Male";
            }
            else if(rbFemale.Checked)
            {
                _currentFilterColumn = "Gender";
                _currentFilterValue = "Female";
            }
            else
            {
                _currentFilterColumn = "";
                _currentFilterValue = "";
            }
            
            // Reset to first page and reload data
            _currentPage = 1;
            LoadCurrentPageAsync();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dgvReaders.Rows.Count > 0)
            {
                int readerID = Convert.ToInt16(dgvReaders.SelectedRows[0].Cells["ReaderID"].Value);
                frmManageReaderSubscriptions frm = new frmManageReaderSubscriptions(readerID);
                frm.ShowDialog();
                LoadDataGridViewAsync();
            }
            else
            {
                MessageBox.Show("Please select a reader to view his subscription history!", "Select a reader first", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateReader frm = new frmAddUpdateReader();
            frm.ShowDialog();
            _UpdateDataGridView();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }
    }
    }

