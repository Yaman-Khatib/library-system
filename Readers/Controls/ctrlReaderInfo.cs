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
using The_Story_Corner_Project.Users;
using System.IO;
namespace The_Story_Corner_Project.Readers.Controls
{
    public partial class ctrlReaderInfo : UserControl
    {
        clsReader reader;
        public ctrlReaderInfo()
        {
            InitializeComponent();

        }

        public void SetInfo(int ReaderID)
        {
            if (clsReader.IsDeleted(ReaderID))
            {
                MessageBox.Show("This reader has been deleted!\nCannot select deleted reader", "Reader was deleted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            reader = clsReader.FindByReaderID(ReaderID);
            if(reader == null )
            {
                MessageBox.Show("Reader was not found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Enabled = false;
                return;
            }

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

            if(reader.PersonInfo.ImagePath != null && File.Exists(reader.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = reader.PersonInfo.ImagePath;
            }

            //Emergency contact 1

            lblEContact1Name.Text = "N/A";
            lblEContact1Relation.Text = "N/A";
            lblEContact1Mobile.Text = "N/A";
            if (reader.PersonInfo.EmergencyContact1ID != null)
            {
                lblEContact1Name.Text = reader.PersonInfo.EmergencyContact1.Name ?? "N/A";
                lblEContact1Relation.Text = reader.PersonInfo.EmergencyContact1.Relation ?? "N/A";
                lblEContact1Mobile.Text = reader.PersonInfo.EmergencyContact1.PhoneNumber ?? "N/A";
            }

                //Emergency contact 2
            lblEContact2Name.Text = "N/A";
            lblEContact2Relation.Text = "N/A";
            lblEContact2Mobile.Text = "N/A";
            if (reader.PersonInfo.EmergencyContact2ID != null)
            {
                lblEContact2Name.Text = reader.PersonInfo.EmergencyContact2.Name ?? "N/A";
                lblEContact2Relation.Text = reader.PersonInfo.EmergencyContact2.Relation ?? "N/A";
                lblEContact2Mobile.Text = reader.PersonInfo.EmergencyContact2.PhoneNumber ?? "N/A";
            }


        }

        

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            frmAddUpdateReader frm = new frmAddUpdateReader(reader.ReaderID.Value);
            this.Parent.Visible = false;
            frm.ShowDialog();
            this.Parent.Visible = true;

            SetInfo(reader.ReaderID.Value);
        }

        
    }
}
