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
        public frmManagePayments()
        {
            InitializeComponent();
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

        private async Task<DataTable> GetDataFromDatabaseAsync(string Language)
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsPayment.GetAllPayments(GetPaymentTypeID(), dtpStartDate.Value, dtpEndDate.Value);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvPayments.DataSource = null;
            cbFilterBy.SelectedIndex = 0;

            try
            {
                // Disable the DataGridView while loading data
                dgvPayments.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtPayments = await GetDataFromDatabaseAsync(cbPaymentType.Text);

                // Bind the data to the DataGridView
                dgvPayments.DataSource = _dtPayments;
                if (dgvPayments.RowCount > 0)
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
                dgvPayments.Enabled = true;
            }
            lblRecordsCount.Text = dgvPayments.RowCount.ToString();
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

            string filterColumn = cbFilterBy.Text.Trim();
            string filterValue = txtFilterValue.Text.Trim();

            if (filterColumn == "None" || filterValue == "")
            {
                _dtPayments.DefaultView.RowFilter = "";

            }


            if (cbFilterBy.Text.Trim() == "Reader name")
                filterColumn = "FullName";

            if (cbFilterBy.Text.Trim() == "Reader account number")
                filterColumn = "AccountNumber";
            if (cbFilterBy.Text.Trim() == "Paid to user")
                filterColumn = "UserName";
            

            if (filterValue != "")
                _dtPayments.DefaultView.RowFilter = String.Format($" {filterColumn} like '{filterValue}%' ");


            lblRecordsCount.Text = dgvPayments.Rows.Count.ToString();
            UpdateTotalPaymentLabel();
            if (dgvPayments.Rows.Count == 0)
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
    }
}
