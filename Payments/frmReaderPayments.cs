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
using static Library_Business.clsPaymentType;
namespace The_Story_Corner_Project.Payments
{
    public partial class frmReaderPayments : KryptonForm
    {
        int ReaderID;
        DataTable _dtPayments;
        public frmReaderPayments(int ReaderID)
        {
            InitializeComponent();
            this.ReaderID = ReaderID;
            ctrlReaderInfoWithFilter1.SelectReader(ReaderID);
            ctrlReaderInfoWithFilter1.FilterEnabled(false);
        }
        public frmReaderPayments()
        {
            InitializeComponent();
            ctrlReaderInfoWithFilter1.OnReaderSelected += OnReaderSelected;
        }

        void OnReaderSelected(int? readerID)
        {
            if(readerID != null)
            {
                ReaderID = readerID.Value;
                LoadDataGridViewAsync();
            }            
            else
            {
                _dtPayments.Clear();
                lblTotalPaymentsFor.Text = "No payments";
            }
        }

        private void frmReaderPayments_Load(object sender, EventArgs e)
        {
            
            LoadDataGridViewAsync();
        }

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsPayment.GetPaymentsForReader(ReaderID);
        }

        private async void LoadDataGridViewAsync()
        {
            dgvPayments.DataSource = null;
            

            try
            {
                // Disable the DataGridView while loading data
                dgvPayments.Enabled = false;

                // Show a loading message or spinner
                //pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtPayments = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvPayments.DataSource = _dtPayments;
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
                dgvPayments.Enabled = true;
            }
            lblRecordsCount.Text = dgvPayments.RowCount.ToString();
            UpdateTotalPaymentLabel();
            StyleColumns();
        }
        private void StyleColumns()
        {
            //dgvEnrollments.Columns["PaymentID"].HeaderText = "Payment ID";
            //dgvEnrollments.Columns["PaymentID"].Width = 140;

            //dgvEnrollments.Columns["ReaderID"].HeaderText = "Reader ID";
            //dgvEnrollments.Columns["ReaderID"].Width = 140;



            //dgvEnrollments.Columns["FullName"].HeaderText = "Full Name";
            //dgvEnrollments.Columns["FullName"].Width = 190;


            dgvPayments.Columns["PaymentDate"].HeaderText = "Payment Date";
            dgvPayments.Columns["PaymentDate"].Width = 130;

            dgvPayments.Columns["PaymentTime"].HeaderText = "Payment Time";
            dgvPayments.Columns["PaymentTime"].Width = 130;



            dgvPayments.Columns["PaymentAmount"].HeaderText = "Payment Amount";
            dgvPayments.Columns["PaymentAmount"].Width = 160;


            dgvPayments.Columns["PaymentType"].HeaderText = "Payment Type";
            dgvPayments.Columns["PaymentType"].Width = 130;


            dgvPayments.Columns["CreatedBy"].HeaderText = "Paid To User";
            dgvPayments.Columns["CreatedBy"].Width = 130;


            //dgvEnrollments.Columns["Description"].Width = 220;

            //// Hide unnecessary columns


            //if (dgvEnrollments.Columns.Contains("PaymentTypeID"))
            //{
            //    dgvEnrollments.Columns["PaymentTypeID"].Visible = false;
            //}

        }

        private void UpdateTotalPaymentLabel()
        {
            long TotalAmount = 0;

            // Safely calculate the total payment amount, handling possible null values
            TotalAmount = _dtPayments.DefaultView.Cast<DataRowView>().Where(rowView => rowView["PaymentAmount"] != DBNull.Value)
                .Sum(rowView => Convert.ToInt64(rowView["PaymentAmount"]));

            
            string formattedNumber = TotalAmount.ToString("N0");
            lblTotalPaymentsFor.Text = $"Total payments: {formattedNumber} S.P";
        }

        
    }
}
