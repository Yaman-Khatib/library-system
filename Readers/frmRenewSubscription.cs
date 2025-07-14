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
namespace The_Story_Corner_Project.Readers
{
    public partial class frmRenewSubscription : KryptonForm
    {
        int ReaderID;
        public frmRenewSubscription()
        {
            InitializeComponent();
            ctrlReaderInfoWithFilter1.OnReaderSelected += OnReaderSelected;
        }
        void OnReaderSelected(int? readerID)
        {
            ReaderID = readerID ?? -1;
            if(ReaderID != -1)
                btnRenewSubscription.Enabled = true;
        }

        public frmRenewSubscription(int readerID)
        {
            InitializeComponent();
            ctrlReaderInfoWithFilter1.SelectReader(readerID);
            ctrlReaderInfoWithFilter1.FilterEnabled(false);
            this.ReaderID = ctrlReaderInfoWithFilter1.ReaderID??-1;
            if (this.ReaderID != -1)
                btnRenewSubscription.Enabled = true;
        }
        private void _FillSubscriptionTypesComboBox()
        {
            DataTable dt = clsSubscriptionType.GetAllSubscriptionTypes();
            foreach (DataRow row in dt.Rows)
            {
                cbSubscriptionType.Items.Add(row["SubscriptionTypeName"].ToString());
            }
        }

        private double GetFees()
        {
            double SubscriptionFees = (double)clsSubscriptionType.Find(cbSubscriptionType.SelectedIndex + 1).SubscriptionTypeFees.Value;
            return SubscriptionFees * (1 - (double)numericDiscount.Value / 100);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _FillSubscriptionTypesComboBox();
            dtpSubscriptionDate.Value = DateTime.Now;
        }

        private void ValidateEmptyKryptonTextBox(object sender, CancelEventArgs e)
        {

        }

        private void rbYesSchool_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void cbSubscriptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubscriptionType.SelectedIndex >= 0)
                lblFees.Text = GetFees().ToString("N0") + " S.P";
        }

        private void kryptonNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (cbSubscriptionType.SelectedIndex >= 0)
                lblFees.Text = GetFees().ToString("N0") + " S.P";
        }

        private void btnRenewSubscription_Click(object sender, EventArgs e)
        {
            if(cbSubscriptionType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select  subscription type!");
                return;
            }

            clsSubscription subscription = new clsSubscription();

            clsSubscriptionType subscriptionType = clsSubscriptionType.Find((cbSubscriptionType.SelectedIndex + 1));

            subscription = new clsSubscription();
            subscription.ReaderID = this.ReaderID;
            subscription.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            subscription.SubscriptionTypeID = subscriptionType.SubscriptionTypeID;
            subscription.StartDate = dtpSubscriptionDate.Value;
            subscription.ExpirationDate = dtpSubscriptionDate.Value.AddMonths(subscriptionType.SubscriptionTypeMonths.Value);
            subscription.SubscriptionReason = clsSubscription.enSubscriptionReason.FirstTime;
            subscription.Discount = (int)numericDiscount.Value;


            if(subscription.Save())
            {
                MessageBox.Show("New subscription was added successfully","Added successfully",MessageBoxButtons.OK,MessageBoxIcon.Information);
                btnRenewSubscription.Enabled = false;
                return;
            }
            else
            {
                MessageBox.Show("An error occurred , subscription wasn't renewed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
