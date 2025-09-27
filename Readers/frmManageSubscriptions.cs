using ComponentFactory.Krypton.Toolkit;
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
using The_Story_Corner_Project.Global_Classes;

namespace The_Story_Corner_Project.Readers
{
    public partial class frmManageSubscriptions: KryptonForm
    {
        DataTable _dtSubscriptions;
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 0;
        private int _totalRecords = 0;
        private DateTime _currentStartDate;
        private DateTime _currentEndDate;

        public frmManageSubscriptions()
        {
            InitializeComponent();
        }

        private async Task<DataTable> GetDataFromDatabaseAsync(int pageNumber, int pageSize)
        {
            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsSubscription.GetAllSubscriptionsPaged(_currentStartDate, _currentEndDate, pageNumber, pageSize, out _totalRecords);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvSubscriptions.DataSource = null;
            
            // Set current filter parameters
            _currentStartDate = dtpStartDate.Value;
            _currentEndDate = dtpEndDate.Value;
            _currentPage = 1; // Reset to first page
            
            await LoadCurrentPageAsync();
        }

        private async Task LoadCurrentPageAsync()
        {
            try
            {
                // Disable the DataGridView while loading data
                dgvSubscriptions.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get paged data from the database asynchronously
                _dtSubscriptions = await GetDataFromDatabaseAsync(_currentPage, _pageSize);
                
                // Calculate total pages
                _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                // Bind the data to the DataGridView
                dgvSubscriptions.DataSource = _dtSubscriptions;

                if (dgvSubscriptions.RowCount > 0)
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
                dgvSubscriptions.Enabled = true;
            }
            
            UpdatePaginationInfo();
            StyleColumns();
        }

        private void UpdatePaginationInfo()
        {
            lblRecordsCount.Text = $"Page {_currentPage} of {_totalPages} ({_totalRecords} total records)";
        }

        private async void GoToNextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                await LoadCurrentPageAsync();
            }
        }

        private async void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                await LoadCurrentPageAsync();
            }
        }

        private void StyleColumns()
        {
            if (dgvSubscriptions.Rows.Count == 0)
                return;

            // Hide IsDeleted column but keep SubscriptionID for delete operations
            if (dgvSubscriptions.Columns.Contains("IsDeleted"))
                dgvSubscriptions.Columns["IsDeleted"].Visible = false;
            if (dgvSubscriptions.Columns.Contains("SubscriptionID"))
                dgvSubscriptions.Columns["SubscriptionID"].Visible = false;

            // Style other columns
            if (dgvSubscriptions.Columns.Contains("FullName"))
            {
                dgvSubscriptions.Columns["FullName"].HeaderText = "Full Name";
                dgvSubscriptions.Columns["FullName"].Width = 200;
            }

            if (dgvSubscriptions.Columns.Contains("AccountNumber"))
            {
                dgvSubscriptions.Columns["AccountNumber"].HeaderText = "Account Number";
                dgvSubscriptions.Columns["AccountNumber"].Width = 150;
            }

            if (dgvSubscriptions.Columns.Contains("SubscriptionTime"))
            {
                dgvSubscriptions.Columns["SubscriptionTime"].HeaderText = "Subscription Time";
                dgvSubscriptions.Columns["SubscriptionTime"].Width = 140;
            }

            if (dgvSubscriptions.Columns.Contains("SubscriptionDate"))
            {
                dgvSubscriptions.Columns["SubscriptionDate"].HeaderText = "Subscription Date";
                dgvSubscriptions.Columns["SubscriptionDate"].Width = 140;
            }

            if (dgvSubscriptions.Columns.Contains("ExpirationDate"))
            {
                dgvSubscriptions.Columns["ExpirationDate"].HeaderText = "Expiration Date";
                dgvSubscriptions.Columns["ExpirationDate"].Width = 140;
            }

            if (dgvSubscriptions.Columns.Contains("SubscriptionTypeName"))
            {
                dgvSubscriptions.Columns["SubscriptionTypeName"].HeaderText = "Subscription Type";
                dgvSubscriptions.Columns["SubscriptionTypeName"].Width = 150;
            }

            if (dgvSubscriptions.Columns.Contains("PaymentAmount"))
            {
                dgvSubscriptions.Columns["PaymentAmount"].HeaderText = "Paid Fees";
                dgvSubscriptions.Columns["PaymentAmount"].Width = 120;
            }

            if (dgvSubscriptions.Columns.Contains("Discount"))
            {
                dgvSubscriptions.Columns["Discount"].HeaderText = "Discount";
                dgvSubscriptions.Columns["Discount"].Width = 100;
            }

            if (dgvSubscriptions.Columns.Contains("CreatedByUser"))
            {
                dgvSubscriptions.Columns["CreatedByUser"].HeaderText = "Created by User";
                dgvSubscriptions.Columns["CreatedByUser"].Width = 140;
            }

            // Make the last column responsive to fill remaining width
            var visibleColumns = dgvSubscriptions.Columns.Cast<DataGridViewColumn>()
                .Where(col => col.Visible)
                .ToList();
            
            if (visibleColumns.Any())
            {
                var lastColumn = visibleColumns.Last();
                lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void btnAddNewSubscription_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.ReadersManagement))
            {
                MessageBox.Show("You don't have permissions to manage reader subscriptions, contact your admin!", "No permissions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var frm = new frmManageReaderSubscriptions();

            frm.ShowDialog();
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void dgvSubscriptions_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvSubscriptions.ClearSelection(); // Clear any previous selections
                dgvSubscriptions.Rows[e.RowIndex].Selected = true; // Select the row
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // Check if a row is selected
            if (dgvSubscriptions.SelectedRows.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            // Get the selected subscription ID
            int subscriptionID = GetSelectedSubscriptionID();
            if (subscriptionID == -1)
            {
                e.Cancel = true;
                return;
            }

            // Check if subscription is expired
            bool isExpired = clsSubscription.IsSubscriptionExpired(subscriptionID);
            
            // Enable/disable delete option based on expiration status
            deleteToolStripMenuItem.Enabled = !isExpired;
            
            if (isExpired)
            {
                deleteToolStripMenuItem.Text = "Cannot Delete (Expired)";
            }
            else
            {
                deleteToolStripMenuItem.Text = "Delete Subscription";
            }
        }

        private int GetSelectedSubscriptionID()
        {
            if (dgvSubscriptions.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvSubscriptions.SelectedRows[0];
                if (selectedRow.Cells["SubscriptionID"].Value != null)
                {
                    return Convert.ToInt32(selectedRow.Cells["SubscriptionID"].Value);
                }
            }
            return -1;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int subscriptionID = GetSelectedSubscriptionID();
            if (subscriptionID == -1)
            {
                MessageBox.Show("Please select a subscription to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if subscription is expired
            if (clsSubscription.IsSubscriptionExpired(subscriptionID))
            {
                MessageBox.Show("Cannot delete an expired subscription.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deletion
            if (MessageBox.Show("Are you sure you want to delete this subscription?\n\nThis will soft delete the subscription record.", 
                               "Delete Subscription", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsSubscription.SoftDeleteSubscription(subscriptionID))
                {
                    MessageBox.Show("Subscription deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridViewAsync();
                }
                else
                {
                    MessageBox.Show("Failed to delete subscription. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmManageSubscriptions_Load(object sender, EventArgs e)
        {
            // Set default date values
            dtpStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpEndDate.Value = DateTime.Now;

            LoadDataGridViewAsync();
        }

        private void dgvSubscriptions_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right) // Check for right-click and valid row
            //{
            //    // Select the entire row on right-click
            //    dgvSubscriptions.ClearSelection(); // Clear any previous selections
            //    dgvSubscriptions.Rows[e.].Selected = true; // Select the row
            //}
        }

        private void dgvSubscriptions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure that the click is on a valid row
            {
                dgvSubscriptions.Rows[e.RowIndex].Selected = true; // Select the clicked row
            }
        }
    }
}