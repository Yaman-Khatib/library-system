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
using The_Story_Corner_Project.Books.Controls;

namespace The_Story_Corner_Project.Readers.Controls
{
    public partial class ctrlReaderInfoWithFilter : UserControl
    {
        public Action<int?> OnReaderSelected;
        public int? ReaderID { get; set; }
        public clsReader Reader { get; set; }
        public ctrlReaderInfoWithFilter()
        {
            InitializeComponent();
            ReaderID = null;
            
        }

        public void FilterEnabled(bool Enabled)
        {
            gbFilter.Enabled = Enabled;
        }

        public async void SelectReader(string ReaderAccountNumber)
        {
            txtFilterValue.Text = ReaderAccountNumber;
            ctrlReaderBasicInfo1.SetInfo(ReaderAccountNumber);
            this.Reader = ctrlReaderBasicInfo1.reader;
            ReaderID = ctrlReaderBasicInfo1.ReaderID;
            OnReaderSelected?.Invoke(ctrlReaderBasicInfo1.ReaderID);
        }

        public void SelectReader(int ReaderID)
        {
            clsReader Reader = clsReader.FindByReaderID(ReaderID);
            if (Reader != null)
            {
                txtFilterValue.Text = Reader.AccountNumber;
                ctrlReaderBasicInfo1.SetInfo(ReaderID);
                this.ReaderID = Reader.ReaderID;
            }
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus(); 
        }
        public void ResetDefaultValues()
        {
            ctrlReaderBasicInfo1.ResetDefaultValues();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            string accountNumber = txtFilterValue.Text.Trim();
            if (String.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show("Please enter the account number");
                txtFilterValue.Focus();
                return;
            }

            ctrlReaderBasicInfo1.SetInfo(accountNumber);
            if(OnReaderSelected != null)
            {
                OnReaderSelected(ctrlReaderBasicInfo1.ReaderID);
            }
            ReaderID = ctrlReaderBasicInfo1.ReaderID;
        }

        //private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    frmAddUpdateReader frm = new frmAddUpdateReader();
        //    frm.ShowDialog();
        //}

        void OnReader_Added(int? ReaderID)
        {
            Reader = clsReader.FindByReaderID(ReaderID.Value);
            if (Reader != null)
            {
                txtFilterValue.Text = Reader.AccountNumber;
                ctrlReaderBasicInfo1.SetInfo(ReaderID.Value);
                this.ReaderID = Reader.ReaderID;
            OnReaderSelected?.Invoke(ctrlReaderBasicInfo1.ReaderID);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            frmAddUpdateReader frm = new frmAddUpdateReader();
            frm.OnReaderSaved += OnReader_Added;
            frm.ShowDialog();
        }

        private void ctrlReaderInfoWithFilter_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
