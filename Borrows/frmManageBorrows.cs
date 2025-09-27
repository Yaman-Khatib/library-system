using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;

namespace The_Story_Corner_Project.Borrows
{

    public partial class frmManageBorrows : KryptonForm
    {
        DataTable _dtBorrows;
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 0;
        private int _totalRecords = 0;
        private clsBorrow.enBorrowStatus _currentStatus;
        private DateTime _currentStartDate;
        private DateTime _currentEndDate;
        public frmManageBorrows()
        {
            InitializeComponent();
        }
        void _ResetDefaultValues()
        {
            
            dtpEndDate.Value = DateTime.Now.AddYears(1);
            dtpStartDate.Value = DateTime.Now.AddYears(-1);
            cbStatus.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;

        }
        private void frmManageBorrows_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (cbStatus.Items.Count > 0)
            {
                cbStatus.SelectedIndex = 0;
            }
            cbStatus.SelectedIndex = 0;
            LoadDataGridViewAsync();
        }



        clsBorrow.enBorrowStatus GetBorrowStatus()
        {
            switch(cbStatus.SelectedIndex)
            {
                case 0:
                    return clsBorrow.enBorrowStatus.All;
                case 1:
                    return clsBorrow.enBorrowStatus.BorrowedOnTime;
                case 2:
                    return clsBorrow.enBorrowStatus.BorrowedOverdue;
                    case 3:
                    return clsBorrow.enBorrowStatus.ReturnedOnTime;
                case 4:
                    return clsBorrow.enBorrowStatus.ReturnedOverdue;
                default:
                    return clsBorrow.enBorrowStatus.Unknown;
            }
        }
        private async Task<DataTable> GetDataFromDatabaseAsync(int pageNumber, int pageSize)
        {
            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            
            // Get filter values
            string filterColumn = GetFilterColumn();
            string filterValue = txtFilterValue.Text.Trim();
            
            return clsBorrow.GetAllBorrowsPaged(_currentStatus, _currentStartDate, _currentEndDate, filterColumn, filterValue, pageNumber, pageSize, out _totalRecords);
        }

        private string GetFilterColumn()
        {
            switch (cbFilterBy.Text)
            {
                case "Borrow ID":
                    return "BorrowID";
                case "Reader account number":
                    return "AccountNumber";
                case "Book title":
                    return "Title";
                case "Book language":
                    return "LanguageName";
                case "Full Name":
                    return "FullName";
                case "Serial Number":
                    return "SerialNumber";
                default:
                    return "";
            }
        }

        private async void LoadDataGridViewAsync()
        {
            dgvBorrows.DataSource = null;
            cbFilterBy.SelectedIndex = 0;
            
            // Set current filter parameters
            _currentStatus = GetBorrowStatus();
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
                dgvBorrows.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get paged data from the database asynchronously
                _dtBorrows = await GetDataFromDatabaseAsync(_currentPage, _pageSize);
                
                // Calculate total pages
                _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                // Bind the data to the DataGridView
                dgvBorrows.DataSource = _dtBorrows;
                if (dgvBorrows.RowCount > 0)
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
                dgvBorrows.Enabled = true;
            }
            
            UpdatePaginationInfo();
            StyleColumns();
        }
        private void StyleColumns()
        {
            // Check if DataGridView and columns exist before styling
            if (dgvBorrows?.Columns == null) return;

            // Hide ID and IsDeleted columns
            if (dgvBorrows.Columns.Contains("BorrowID"))
                dgvBorrows.Columns["BorrowID"].Visible = false;
            if (dgvBorrows.Columns.Contains("IsDeleted"))
                dgvBorrows.Columns["IsDeleted"].Visible = false;

            if (dgvBorrows.Columns.Contains("FullName"))
            {
                dgvBorrows.Columns["FullName"].HeaderText = "Full Name";
                dgvBorrows.Columns["FullName"].Width = 170;
            }

            if (dgvBorrows.Columns.Contains("AccountNumber"))
            {
                dgvBorrows.Columns["AccountNumber"].HeaderText = "Account Number";
                dgvBorrows.Columns["AccountNumber"].Width = 140;
            }

            if (dgvBorrows.Columns.Contains("Title"))
            {
                dgvBorrows.Columns["Title"].Width = 150;
            }

            if (dgvBorrows.Columns.Contains("SerialNumber"))
            {
                dgvBorrows.Columns["SerialNumber"].HeaderText = "Serial Number";
                dgvBorrows.Columns["SerialNumber"].Width = 150;
            }

            if (dgvBorrows.Columns.Contains("LanguageName"))
            {
                dgvBorrows.Columns["LanguageName"].HeaderText = "Language";
            }

            if (dgvBorrows.Columns.Contains("BorrowDate"))
            {
                dgvBorrows.Columns["BorrowDate"].HeaderText = "Borrow Date";
                dgvBorrows.Columns["BorrowDate"].Width = 120;
            }

            if (dgvBorrows.Columns.Contains("DueDate"))
            {
                dgvBorrows.Columns["DueDate"].HeaderText = "Due Date";
                dgvBorrows.Columns["DueDate"].Width = 120;
            }

            if (dgvBorrows.Columns.Contains("ActualReturnDate"))
            {
                dgvBorrows.Columns["ActualReturnDate"].HeaderText = "Return date";
                dgvBorrows.Columns["ActualReturnDate"].Width = 130;
            }

            if (dgvBorrows.Columns.Contains("Status"))
            {
                dgvBorrows.Columns["Status"].Width = 140;
            }

            if (dgvBorrows.Columns.Contains("DidExtended"))
            {
                dgvBorrows.Columns["DidExtended"].HeaderText = "Did extended";
            }
            
            if (dgvBorrows.Columns.Contains("CreatedByUser"))
            {
                dgvBorrows.Columns["CreatedByUser"].HeaderText = "Created by user";
                dgvBorrows.Columns["CreatedByUser"].Width = 130;
            }

            // Make the last column responsive to fill remaining width
            // Find the last visible column and make it fill the remaining space
            var visibleColumns = dgvBorrows.Columns.Cast<DataGridViewColumn>()
                .Where(col => col.Visible && col.Name != "StatusIndex")
                .ToList();
            
            if (visibleColumns.Any())
            {
                var lastColumn = visibleColumns.Last();
                lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            //When showing "All" borrows, show both columns
            if(cbStatus.SelectedIndex == 0) // All
            {
                dgvBorrows.Columns["ActualReturnDate"].Visible = true;
                dgvBorrows.Columns["DueDate"].Visible = true;
            }
            //When the borrow is not yet returned hide the Actual return date and show due date
            else if(cbStatus.SelectedIndex == 1 || cbStatus.SelectedIndex == 2) // BorrowedOnTime or BorrowedOverdue
            {
                dgvBorrows.Columns["ActualReturnDate"].Visible = false;
                dgvBorrows.Columns["DueDate"].Visible = true;
            }
            //When the borrow is returned show actual return date and hide due date
            else // ReturnedOnTime or ReturnedOverdue
            {
                dgvBorrows.Columns["DueDate"].Visible = false;
                dgvBorrows.Columns["ActualReturnDate"].Visible = true;
            }


            if (dgvBorrows.Columns.Contains("StatusIndex"))
            {
                dgvBorrows.Columns["StatusIndex"].Visible = false;
            }
        }

        

        
        private async void txtFilterValue_TextChanged_1(object sender, EventArgs e)
        {
            // Reset to first page when filter changes
            _currentPage = 1;
            await LoadCurrentPageAsync();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = true;
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dgvBooks_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            pctrLoading.Invalidate();

        }

        private void btnAddBorrow_Click_1(object sender, EventArgs e)
        {
            frmAddUpdateBorrow frm = new frmAddUpdateBorrow();
            frm.ShowDialog();

            _ResetDefaultValues();
            if (cbStatus.Items.Count > 0)
            {
                cbStatus.SelectedIndex = 0;
            }
            cbStatus.SelectedIndex = 0;
            LoadDataGridViewAsync();

        }

        private void toolStripMenuItemAddBorrow_Click(object sender, EventArgs e)
        {
            frmAddUpdateBorrow frm = new frmAddUpdateBorrow();
            frm.ShowDialog();

            _ResetDefaultValues();
            if (cbStatus.Items.Count > 0)
            {
                cbStatus.SelectedIndex = 0;
            }
            cbStatus.SelectedIndex = 0;
            LoadDataGridViewAsync();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBorrows.SelectedRows.Count > 0)
            {
                int BorrowID = Convert.ToInt16(dgvBorrows.SelectedRows[0].Cells["BorrowID"].Value);
                frmShowBorrowInfo frm = new frmShowBorrowInfo(BorrowID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please choose a borrow record");
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBorrows.SelectedRows.Count > 0)
            {
                int BorrowID = Convert.ToInt16(dgvBorrows.SelectedRows[0].Cells["BorrowID"].Value);
                frmAddUpdateBorrow frm = new frmAddUpdateBorrow(BorrowID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please choose the borrow record you want to edit!");
            }
        }

        private void toolStripMenuItemExtendBorrow_Click(object sender, EventArgs e)
        {
            if (dgvBorrows.SelectedRows.Count > 0)
            {
                int BorrowID = Convert.ToInt16(dgvBorrows.SelectedRows[0].Cells["BorrowID"].Value);
                clsBorrow borrow = clsBorrow.Find(BorrowID);

                if(!borrow.CanBorrowBeExtended())
                {
                    MessageBox.Show($"Cannot extend borrow date , reader have already extended due date by {borrow.ExtendedDays} days.", "Cannot extend borrow", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                switch(borrow.StatusIndex)
                {
                    case clsBorrow.enBorrowStatus.BorrowedOnTime:
                        if(MessageBox.Show("This borrow has not yet expired\n do you still want to extend due date?","Borrow isn't overdue",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if(borrow.ExtendBorrow(clsLibrarySettings.GetDefaultExtendDays()))
                            {
                                MessageBox.Show("Borrow extended successfully", "Extended", MessageBoxButtons.OK,MessageBoxIcon.Information);
                                LoadDataGridViewAsync();
                            }
                            else
                            {
                                MessageBox.Show("An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        return;
                    case clsBorrow.enBorrowStatus.BorrowedOverdue:
                        if (borrow.ExtendBorrow(clsLibrarySettings.GetDefaultExtendDays()))
                        {
                            MessageBox.Show("Borrow extended successfully", "Extended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataGridViewAsync();
                        }
                        else
                        {
                            MessageBox.Show("An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;

                    case clsBorrow.enBorrowStatus.ReturnedOnTime:
                        MessageBox.Show("The book is already returned, Cannot extend returned books!", "Cannot extend returned books", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    case clsBorrow.enBorrowStatus.ReturnedOverdue:
                        MessageBox.Show("The book is already returned, Cannot extend returned books!", "Cannot extend returned books", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                }
            }
            else
            {
                MessageBox.Show("Please choose the borrow record you want to return!");
            }
        }

        private void toolStripMenuItemReturnBook_Click(object sender, EventArgs e)
        {
            if (dgvBorrows.SelectedRows.Count > 0)
            {
                int BorrowID = Convert.ToInt16(dgvBorrows.SelectedRows[0].Cells["BorrowID"].Value);
                clsBorrow borrow = clsBorrow.Find(BorrowID);
                if(borrow.StatusIndex == clsBorrow.enBorrowStatus.ReturnedOnTime || borrow.StatusIndex == clsBorrow.enBorrowStatus.ReturnedOnTime)
                {
                    MessageBox.Show("The book is already returned!", "Already returned book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(borrow.StatusIndex == clsBorrow.enBorrowStatus.BorrowedOnTime)
                {
                    if (MessageBox.Show($"Are you sure you want to return book\nBook number: {borrow.BookInfo.SerialNumber}\n" +
                        $"Reader account number: {borrow.ReaderInfo.AccountNumber}", "Confirm returning book",MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    if(borrow.ReturnBook())
                    {
                        MessageBox.Show("Book returned successfully", "Returned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridViewAsync();
                    }
                    else
                    {
                        MessageBox.Show("An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
                else if(borrow.StatusIndex == clsBorrow.enBorrowStatus.BorrowedOverdue)
                {
                    int FinesAmountPerWeek = clsLibrarySettings.GetLateReturnFineAmount();
                    int LateReturnWeeksCount = borrow.CalculateOverdueWeeks();

                    if (MessageBox.Show($"This book has exceeded the allowed return period by {borrow.CalculateOverueDays()} days.\n Reader should pay {FinesAmountPerWeek * borrow.CalculateOverdueWeeks()} S.P as late return fines.", "This book is overdue",MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        clsPayment payment = new clsPayment();
                        payment.PaymentDate = DateTime.Now;
                        payment.ReaderID = borrow.ReaderID;
                        payment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                        payment.PaymentAmount = FinesAmountPerWeek * LateReturnWeeksCount;
                        payment.PaymentTypeID = (int)clsPaymentType.enPaymentType.LateReturnFee;
                        if (payment.Save())
                        {
                            if (borrow.ReturnBook())
                            {
                                MessageBox.Show("Returned successfully!");
                                LoadDataGridViewAsync();
                            }
                            else
                                MessageBox.Show("An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose the borrow record you want to return!");
            }
        }
      

        private void btnDeleteSelectedBorrowRecord_Click(object sender, EventArgs e)
        {
            if (clsGlobal.CurrentUser.Permissions != clsUser.enPermissions.FullAccess)
            {
                MessageBox.Show("You are not allowed to delete a borrow , please contact your admin!", "Not allowed to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvBorrows.SelectedRows.Count > 0)
            {
                int BorrowID = Convert.ToInt16(dgvBorrows.SelectedRows[0].Cells["BorrowID"].Value);

                if (MessageBox.Show("Are you sure you want to delete this borrow record?\n\nThis will soft delete the borrow record.", "Delete Borrow Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    if (clsBorrow.DeleteBorrow(BorrowID))
                    {
                        MessageBox.Show("Borrow record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridViewAsync();
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while deleting the borrow record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a borrow record to delete.");
            }
        }

        private void cmsBorrow_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void dgvBorrows_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvBorrows.ClearSelection(); // Clear any previous selections
                dgvBorrows.Rows[e.RowIndex].Selected = true; // Select the row
            }
        }

        private void btnAddBorrow_Click(object sender, EventArgs e)
        {

        }

        #region Pagination Methods

        private void UpdatePaginationInfo()
        {
            lblRecordsCount.Text = $"Page {_currentPage} of {_totalPages} ({_totalRecords} total records)";
            
            // Enable/disable navigation buttons (if they exist)
            // Note: Pagination buttons need to be added to the form designer
            // For now, we'll just update the label with pagination info
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

        #endregion

        #region Pagination Event Handlers

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        #endregion

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }
    }
}
