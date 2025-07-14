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
        public frmManageBorrows()
        {
            InitializeComponent();
        }
        void _ResetDefaultValues()
        {
            dtpEndDate.MaxDate = DateTime.Now.AddMinutes(1);
            dtpEndDate.Value = DateTime.Now;
            dtpStartDate.Value = DateTime.Now.AddMonths(-5);
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
                    return clsBorrow.enBorrowStatus.BorrowedOnTime;
                case 1:
                    return clsBorrow.enBorrowStatus.BorrowedOverdue;
                    case 2:
                    return clsBorrow.enBorrowStatus.ReturnedOnTime;
                case 3:
                    return clsBorrow.enBorrowStatus.ReturnedOverdue;
                default:
                    return clsBorrow.enBorrowStatus.Unknown;
            }
        }
        private async Task<DataTable> GetDataFromDatabaseAsync(string Language)
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsBorrow.GetAllBorrows(GetBorrowStatus(),dtpStartDate.Value,dtpEndDate.Value);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvBorrows.DataSource = null;
            cbFilterBy.SelectedIndex = 0;
            
            try
            {
                // Disable the DataGridView while loading data
                dgvBorrows.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtBorrows = await GetDataFromDatabaseAsync(cbStatus.Text);

                // Bind the data to the DataGridView
                dgvBorrows.DataSource = _dtBorrows;
                if (dgvBorrows.RowCount > 0)
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
                dgvBorrows.Enabled = true;
            }
            lblRecordsCount.Text = dgvBorrows.RowCount.ToString();
            StyleColumns();
        }
        private void StyleColumns()
        {
            dgvBorrows.Columns["BorrowID"].HeaderText = "Borrow ID";
            dgvBorrows.Columns["BorrowID"].Width = 120;
            dgvBorrows.Columns["FullName"].HeaderText = "Full Name";
            dgvBorrows.Columns["FullName"].Width = 170;

            dgvBorrows.Columns["AccountNumber"].HeaderText = "Account Number";
            dgvBorrows.Columns["AccountNumber"].Width = 140;

            dgvBorrows.Columns["Title"].Width = 150;

            dgvBorrows.Columns["SerialNumber"].HeaderText = "Serial Number";
            dgvBorrows.Columns["SerialNumber"].Width = 150;

            dgvBorrows.Columns["LanguageName"].HeaderText = "Language";
            


            dgvBorrows.Columns["BorrowDate"].HeaderText = "Borrow Date";
            dgvBorrows.Columns["BorrowDate"].Width = 120;

            dgvBorrows.Columns["DueDate"].HeaderText = "Due Date";
            dgvBorrows.Columns["DueDate"].Width = 120;


            dgvBorrows.Columns["ActualReturnDate"].HeaderText = "Return date";
            dgvBorrows.Columns["ActualReturnDate"].Width = 130;

            dgvBorrows.Columns["Status"].Width = 140;

            dgvBorrows.Columns["DidExtended"].HeaderText = "Did extended";
            
            dgvBorrows.Columns["CreatedByUser"].HeaderText = "Created by user";
            dgvBorrows.Columns["CreatedByUser"].Width = 130;

            //When the borrow is not yet returned hide the Actual return date and show due date
            if(cbStatus.SelectedIndex == 0 || cbStatus.SelectedIndex == 1)
            {
                dgvBorrows.Columns["ActualReturnDate"].Visible = false;
            }
            //When the borrow is returned show actual return date and hide due date
            else
            {
                dgvBorrows.Columns["DueDate"].Visible = false;
            }


            if (dgvBorrows.Columns.Contains("StatusIndex"))
            {
                dgvBorrows.Columns["StatusIndex"].Visible = false;
            }
        }

        

        
        private void txtFilterValue_TextChanged_1(object sender, EventArgs e)
        {
            
            string filterColumn = cbFilterBy.Text.Trim();
            string filterValue = txtFilterValue.Text.Trim();

            if (filterColumn == "None" || filterValue == "")
            {
                _dtBorrows.DefaultView.RowFilter = "";

            }

            if (cbFilterBy.Text.Trim() == "Reader account number")
                filterColumn = "AccountNumber";
            if (cbFilterBy.Text.Trim() == "Book language")
                filterColumn = "LanguageName";
            if (cbFilterBy.Text.Trim() == "Book title")
                filterColumn = "Title";

            if (filterValue != "")
                _dtBorrows.DefaultView.RowFilter = String.Format($" {filterColumn} like '{filterValue}%' ");


            lblRecordsCount.Text = dgvBorrows.Rows.Count.ToString();

            if (dgvBorrows.Rows.Count == 0)
            {
                pctrLoading.Visible = true;
            }
            else
            {
                pctrLoading.Visible = false;
            }
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsGlobal.CurrentUser.Permissions != clsUser.enPermissions.FullAccess)
            {
                MessageBox.Show("You are not allowed to delete a borrow , please contact your admin!","Not allowed to delete",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this borrow?", "Delete permanently", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                return;
            }


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
    }
}
