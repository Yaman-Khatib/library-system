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
namespace The_Story_Corner_Project.Payments
{
    public partial class frmManagePayments : KryptonForm
    {
        DataTable _dtPayments;
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 0;
        private int _totalRecords = 0;
        private int _currentPaymentTypeID;
        private DateTime _currentStartDate;
        private DateTime _currentEndDate;
        private System.Windows.Forms.Timer _searchTimer;

        public frmManagePayments()
        {
            InitializeComponent();
            _searchTimer = new System.Windows.Forms.Timer();
            _searchTimer.Interval = 500;
            _searchTimer.Tick += _searchTimer_Tick;
        }

        private void frmManagePayments_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (cbPaymentType.Items.Count > 0)
            {
                cbPaymentType.SelectedIndex = 0;
            }
            cbPaymentType.SelectedIndex = 0;
            LoadDataGridViewAsync();
        }

        void _ResetDefaultValues()
        {
            dtpEndDate.MaxDate = DateTime.Now.AddMinutes(1);
            dtpEndDate.Value = DateTime.Now;
            dtpStartDate.Value = DateTime.Now.AddMonths(-1);
            cbPaymentType.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Visible = false;

        }

        int GetPaymentTypeID()
        {
            return cbPaymentType.SelectedIndex + 1;
        }

        private async Task<DataTable> GetDataFromDatabaseAsync(int pageNumber, int pageSize)
        {
            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsPayment.GetAllPaymentsPaged(_currentPaymentTypeID, _currentStartDate, _currentEndDate, pageNumber, pageSize, out _totalRecords);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvPayments.DataSource = null;
            cbFilterBy.SelectedIndex = 0;

            // Set current filter parameters
            _currentPaymentTypeID = GetPaymentTypeID();
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
                dgvPayments.Enabled = false;
                dgvPayments.DataSource = null;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get paged data from the database asynchronously
                _dtPayments = await GetDataFromDatabaseAsync(_currentPage, _pageSize);

                // Calculate total pages
                _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                // Bind the data to the DataGridView
                dgvPayments.DataSource = _dtPayments;
                if (dgvPayments.RowCount > 0)
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
                dgvPayments.Enabled = true;
            }

            UpdatePaginationInfo();
            UpdateTotalPaymentLabel();
            StyleColumns();
        }
        private void StyleColumns()
        {
            dgvPayments.Columns["PaymentID"].HeaderText = "Payment ID";
            dgvPayments.Columns["PaymentID"].Width = 140;

            dgvPayments.Columns["ReaderID"].HeaderText = "Reader ID";
            dgvPayments.Columns["ReaderID"].Width = 140;



            dgvPayments.Columns["FullName"].HeaderText = "Full Name";
            dgvPayments.Columns["FullName"].Width = 230;

            dgvPayments.Columns["AccountNumber"].HeaderText = "Account Number";
            dgvPayments.Columns["AccountNumber"].Width = 160;           

            dgvPayments.Columns["PaymentDate"].HeaderText = "Payment Date";
            dgvPayments.Columns["PaymentDate"].Width = 180;

            

            dgvPayments.Columns["PaymentAmount"].HeaderText = "Payment amount";
            dgvPayments.Columns["PaymentAmount"].Width = 150;


            dgvPayments.Columns["PaymentTypeName"].HeaderText = "Payment type";
            dgvPayments.Columns["PaymentTypeName"].Width = 165;

            
            dgvPayments.Columns["UserName"].HeaderText = "Paid to user";
            dgvPayments.Columns["UserName"].Width = 140;


            

            // Hide unnecessary columns
            

            if (dgvPayments.Columns.Contains("PaymentTypeID"))
            {
                dgvPayments.Columns["PaymentTypeID"].Visible = false;
            }

        }


        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            // Stop the timer if it's running (user is still typing)
            _searchTimer.Stop();

            // Restart the timer
            _searchTimer.Start();
        }

        private async void _searchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();

            // For now, we'll implement a simple approach where filtering resets to page 1
            // In a more advanced implementation, you could add filter parameters to the SQL query
            _currentPage = 1;
            await LoadCurrentPageAsync();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            if(cbFilterBy.SelectedIndex == 0)
            {
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true; 
            }
        }

        private void UpdateTotalPaymentLabel()
        {
            long TotalAmount = 0;

            // Safely calculate the total payment amount, handling possible null values
            TotalAmount = _dtPayments.DefaultView.Cast<DataRowView>().Where(rowView => rowView["PaymentAmount"] != DBNull.Value)
                .Sum(rowView => Convert.ToInt64(rowView["PaymentAmount"]));

            switch (cbPaymentType.SelectedIndex)
            {
                case 0:
                    lblTotalPaymentsFor.Text = "Total payments for subscriptions:";
                    break;

                case 1:
                    lblTotalPaymentsFor.Text = "Total paid overdue fines:";
                    break;

                case 2:
                    lblTotalPaymentsFor.Text = "Total course enrollment fees:";
                    break;

                case 3:
                    lblTotalPaymentsFor.Text = "Total payments for book sales:";
                    break;

            }    

            string formattedNumber = TotalAmount.ToString("N0");
            lblTotalPaymentsFor.Text += $"  {formattedNumber} S.P";
        }

        private void cbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dgvPayments_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvPayments.ClearSelection(); // Clear any previous selections
                dgvPayments.Rows[e.RowIndex].Selected = true; // Select the row
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

        private void toolStripMenuItemAddBorrow_Click(object sender, EventArgs e)
        {
            if (dgvPayments.Rows.Count > 0)
            {
                int readerID = Convert.ToInt16(dgvPayments.SelectedRows[0].Cells["ReaderID"].Value);
                frmReaderPayments frm = new frmReaderPayments(readerID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a reader to view his payments!", "Select desired payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        #endregion
    }
}
