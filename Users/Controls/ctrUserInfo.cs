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

namespace The_Story_Corner_Project.Users.Controls
{
    public partial class ctrUserInfo : UserControl
    {
            clsUser _User = new clsUser();
        public ctrUserInfo()
        {
            InitializeComponent();
        }

        private string GetPermissionsString(int permissions)
        {
            if (permissions == -1) return "Full Access";
            if (permissions == (int)clsUser.enPermissions.NoPermissions) return "No Permissions";

            List<string> permissionsList = new List<string>();

            if ((permissions & (int)clsUser.enPermissions.ReadersManagement) == (int)clsUser.enPermissions.ReadersManagement)
                permissionsList.Add("Readers Management");

            if ((permissions & (int)clsUser.enPermissions.BooksManagement) == (int)clsUser.enPermissions.BooksManagement)
                permissionsList.Add("Books Management");

            if ((permissions & (int)clsUser.enPermissions.CoursesManagement) == (int)clsUser.enPermissions.CoursesManagement)
                permissionsList.Add("Courses Management");

            if ((permissions & (int)clsUser.enPermissions.UsersManagement) == (int)clsUser.enPermissions.UsersManagement)
                permissionsList.Add("Users Management");

            if ((permissions & (int)clsUser.enPermissions.PaymentsManagement) == (int)clsUser.enPermissions.PaymentsManagement)
                permissionsList.Add("Payments Management");

            // Join permissions with commas
            return string.Join(", ", permissionsList);
        }

        public void SetInfo(int UserID)
        {
            _User = clsUser.FindByUserID(UserID);
            if (_User == null)
            {
                MessageBox.Show("User was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = false;
                return;
            }

            lblFullName.Text = _User.PersonInfo.FirstName + " " + _User.PersonInfo.SecondName + " " + _User.PersonInfo.LastName;
            lblMother.Text = _User.PersonInfo.MotherName;

            if (_User.PersonInfo.Gender == false)
            {
                pbGender.Image = Resources.Man_32;
                lblGender.Text = "Male";
            }
            else
            {
                pbGender.Image = Resources.Woman_32;
                lblGender.Text = "Female";
            }
            lblAge.Text = _User.PersonInfo.Age.ToString();
            lblDateOfBirth.Text = _User.PersonInfo.DateOfBirth.Value.ToShortDateString();
            lblSchool.Text = _User.PersonInfo.School ?? "N/A";
            lblMobile.Text = _User.PersonInfo.Mobile ?? "N/A";
            lblPhone.Text = _User.PersonInfo.Phone ?? "N/A";
            lblFacebook.Text = _User.PersonInfo.FacebookUserName ?? "N/A";
            lblInstagram.Text = _User.PersonInfo.InstagramUserName ?? "N/A";
            lblAddress.Text = _User.PersonInfo.Address ?? "N/A";
            lblEmail.Text = _User.PersonInfo.Email ?? "N/A";

            if (_User.PersonInfo.ImagePath != null && File.Exists(_User.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = _User.PersonInfo.ImagePath;
            }

            //Emergency contact 
            lblEContactName.Text = "N/A";
            lblEContactRelation.Text = "N/A";
            lblEContactMobile.Text = "N/A";
            if (_User.PersonInfo.EmergencyContact1ID != null)
            {
                lblEContactName.Text = _User.PersonInfo.EmergencyContact1.Name ?? "N/A";
                lblEContactRelation.Text = _User.PersonInfo.EmergencyContact1.Relation ?? "N/A";
                lblEContactMobile.Text = _User.PersonInfo.EmergencyContact1.PhoneNumber ?? "N/A";
            }

            lblUserName.Text = _User.UserName;
            //Permissions 
            lblPermissions.Text = GetPermissionsString((int)_User.Permissions);

        }


        private void ctrUserInfo_Load(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser(_User.UserID.Value);
            this.Parent.Visible = false;
            frm.ShowDialog();
            this.SetInfo(_User.UserID.Value);
            this.Parent.Visible = true;
        }
    }
}
