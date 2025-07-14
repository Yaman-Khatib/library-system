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
namespace The_Story_Corner_Project.Readers
{
    public partial class frmManageReaderSubscriptions : KryptonForm
    {

        int ReaderID;
        DataTable _dtSubscriptions;
        public frmManageReaderSubscriptions()
        {
            InitializeComponent();
            ctrlReaderInfoWithFilter1.OnReaderSelected += OnReaderSelected;
        }
        public frmManageReaderSubscriptions(int ReaderID)
        {
            InitializeComponent();
            this.ReaderID = ReaderID;
            ctrlReaderInfoWithFilter1.SelectReader(ReaderID);
            ctrlReaderInfoWithFilter1.FilterEnabled(false);

        }

        private void frmManageReaderSubscriptions_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }
        
        

        void OnReaderSelected(int? readerID)
        {
            if (readerID != null)
            {
                ReaderID = readerID.Value;
                LoadDataGridViewAsync();
            }
            else
            {
                _dtSubscriptions.Clear();                
            }
        }


        

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsSubscription.GetSubscriptionsForReader(ReaderID);
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
                _dtSubscriptions = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvPayments.DataSource = _dtSubscriptions;
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
            
            StyleColumns();
        }
        private void StyleColumns()
        {
            dgvPayments.Columns["SubscriptionID"].HeaderText = "Subscription ID";
            dgvPayments.Columns["SubscriptionID"].Width = 120;

            //dgvEnrollments.Columns["ReaderID"].HeaderText = "Reader ID";
            //dgvEnrollments.Columns["ReaderID"].Width = 140;



            //dgvEnrollments.Columns["FullName"].HeaderText = "Full Name";
            //dgvEnrollments.Columns["FullName"].Width = 190;
            dgvPayments.Columns["SubscriptionTime"].HeaderText = "Subscription Time";
            //dgvEnrollments.Columns["SubscriptionTime"].Width = 10;


            dgvPayments.Columns["SubscriptionDate"].HeaderText = "Subscription Date";
            //dgvEnrollments.Columns["SubscriptionDate"].Width = 140;

            dgvPayments.Columns["ExpirationDate"].HeaderText = "Expiration Date";
            //dgvEnrollments.Columns["ExpirationDate"].Width = 140;


            dgvPayments.Columns["SubscriptionTime"].HeaderText = "Subscription Time";
            dgvPayments.Columns["SubscriptionTime"].Width = 140;


            dgvPayments.Columns["SubscriptionTypeName"].HeaderText = "Subscription Type";
            dgvPayments.Columns["SubscriptionTypeName"].Width = 140;


            dgvPayments.Columns["PaymentAmount"].HeaderText = "Paid Fees";
            dgvPayments.Columns["PaymentAmount"].Width = 120;
           


            dgvPayments.Columns["CreatedByUser"].HeaderText = "Created by User";
            dgvPayments.Columns["CreatedByUser"].Width = 140;


            //dgvEnrollments.Columns["Description"].Width = 220;

            //// Hide unnecessary columns


            //if (dgvEnrollments.Columns.Contains("PaymentTypeID"))
            //{
            //    dgvEnrollments.Columns["PaymentTypeID"].Visible = false;
            //}

        }

        private void btnAddSubscription_Click(object sender, EventArgs e)
        {
            clsSubscription LastSubscription = clsSubscription.FindLastSubscription(ReaderID);
            bool ReaderHasActiveSubscription = LastSubscription.ExpirationDate > DateTime.Now;
            if (ReaderHasActiveSubscription)
            {
                MessageBox.Show("Reader already have an active subscription", "Cannot renew subscription", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmRenewSubscription frm = new frmRenewSubscription(this.ReaderID);
            frm.ShowDialog();

            LoadDataGridViewAsync();
        }
    }
}
