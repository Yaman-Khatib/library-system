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
using The_Story_Corner_Project.Properties;
using System.IO;
using System.Net;
namespace The_Story_Corner_Project.Readers.Controls
{
    public partial class ctrlReaderBasicInfo : UserControl
    {
        public clsReader reader;
        public int? ReaderID = null;
        public ctrlReaderBasicInfo()
        {
            InitializeComponent();
        }

        public void ResetDefaultValues()
        {
            reader = new clsReader();
            llEditPersonInfo.Enabled = false;
            lblFullName.Text = "N/A";
            lblAddress.Text = "N/A";
            lblAge.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblEmail.Text = "N/A";
            lblFacebook.Text = "N/A";
            lblGender.Text = "N/A";
            lblInstagram.Text = "N/A";
            lblMobile.Text = "N/A";
            lblMother.Text = "N/A";
            lblPhone.Text = "N/A";
            lblReaderID.Text = "N/A";
            lblSchool.Text = "N/A";
            pbPersonImage.Image = Resources.Male_512;
            pbGender.Image = Resources.Man_32;
            ReaderID = null;
        }

        private void FillInfo()
        {
            if (reader == null)
            {
                ResetDefaultValues();
                return;
            }
            else
            {
                lblFullName.Text = reader.PersonInfo.FirstName + " " + reader.PersonInfo.SecondName + " " + reader.PersonInfo.LastName;
                lblMother.Text = reader.PersonInfo.MotherName;

                if (reader.PersonInfo.Gender == false)
                {
                    pbGender.Image = Resources.Man_32;
                    lblGender.Text = "Male";
                }
                else
                {
                    pbGender.Image = Resources.Woman_32;
                    lblGender.Text = "Female";
                }
                lblAge.Text = reader.PersonInfo.Age.ToString();
                lblDateOfBirth.Text = reader.PersonInfo.DateOfBirth.Value.ToShortDateString();
                lblSchool.Text = reader.PersonInfo.School ?? "N/A";
                lblMobile.Text = reader.PersonInfo.Mobile ?? "N/A";
                lblPhone.Text = reader.PersonInfo.Phone ?? "N/A";
                lblFacebook.Text = reader.PersonInfo.FacebookUserName ?? "N/A";
                lblInstagram.Text = reader.PersonInfo.InstagramUserName ?? "N/A";
                lblAddress.Text = reader.PersonInfo.Address ?? "N/A";
                lblEmail.Text = reader.PersonInfo.Email ?? "N/A";

                if (reader.PersonInfo.ImagePath != null && File.Exists(reader.PersonInfo.ImagePath))
                {
                    pbPersonImage.ImageLocation = reader.PersonInfo.ImagePath;
                }

            }
        }

            public void SetInfo(int ReaderID)
        {

            if (clsReader.IsDeleted(ReaderID))
            {
                DialogResult result = MessageBox.Show("The selected reader has been deleted.\nDo you want to restore it?", "Reader was deleted", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (clsReader.RestoreDeletedReader(ReaderID))
                    {
                        MessageBox.Show("Book was restored successfully!", "Book was restored", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("An error occurred cannot restore book", "Book was not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetDefaultValues();
                        return;
                    }
                }
                else
                {
                    ResetDefaultValues();
                    return;
                }
            }
            reader = clsReader.FindByReaderID(ReaderID);
            if (reader == null)
            {
                MessageBox.Show("Reader was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaultValues();
                return;
            }
            reader.ReaderID = ReaderID;
            llEditPersonInfo.Enabled = true;
            FillInfo();
        }

        public void SetInfo(string AccountNumber)
        {
            if(clsReader.IsDeleted(AccountNumber))
            {                                
                    DialogResult result = MessageBox.Show("The selected reader has been deleted.\nDo you want to restore it?", "Reader was deleted", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        if (clsReader.RestoreDeletedReader(AccountNumber))
                        {
                            MessageBox.Show("Book was restored successfully!", "Book was restored", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("An error occurred cannot restore book", "Book was not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetDefaultValues();
                        return;
                        }
                    }
                    else
                    {
                ResetDefaultValues();
                        return;
                    }
            }
            reader = clsReader.FindByAccountNumber(AccountNumber);

            if (reader == null)
            {
                MessageBox.Show("Reader was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaultValues();
                return;
            }
            ReaderID = reader.ReaderID;
            FillInfo();
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmAddUpdateReader frm = new frmAddUpdateReader(reader.ReaderID.Value);            
            frm.ShowDialog();            
            SetInfo(reader.ReaderID.Value);
        }


    }
}
