using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;
using The_Story_Corner_Project.Properties;
using System.IO;
namespace The_Story_Corner_Project.Users
{
    
    public partial class frmAddUpdateUser : KryptonForm
    {
        enum enMode { AddNew = 1, Update = 2 }
        enMode _Mode = enMode.AddNew;
        clsUser _User;
        int _UserID;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = UserID;
        }
        void _LoadPermissions()
        {
            cbFullAccess.Checked = (_User.Permissions == clsUser.enPermissions.FullAccess);
            cbManageBooks.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.BooksManagement) == (int)clsUser.enPermissions.BooksManagement);
            cbManageCourses.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.CoursesManagement) == (int)clsUser.enPermissions.CoursesManagement);
            cbManagePayments.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.PaymentsManagement) == (int)clsUser.enPermissions.PaymentsManagement);
            cbManageReaders.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.ReadersManagement) == (int)clsUser.enPermissions.ReadersManagement);
            cbManageUsers.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.UsersManagement) == (int)clsUser.enPermissions.UsersManagement);
            cbSettingsAccess.Checked = (((int)_User.Permissions & (int)clsUser.enPermissions.ManagementSettings) == (int)clsUser.enPermissions.ManagementSettings);

        }

        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_User.PersonInfo.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_User.PersonInfo.ImagePath != null)
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_User.PersonInfo.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    //then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }
        int _GetPermissionsFromCheckBoxes()
        {
            
            int permission = 0;
            if(cbFullAccess.Checked)
            {
                
                return (int)clsUser.enPermissions.FullAccess;
            }
            permission += (cbManageBooks.Checked) ? (int)clsUser.enPermissions.BooksManagement : 0;
            permission += (cbManageCourses.Checked) ? (int)clsUser.enPermissions.CoursesManagement : 0;
            permission += (cbManagePayments.Checked) ? (int)clsUser.enPermissions.PaymentsManagement : 0;
            permission += (cbManageReaders.Checked) ? (int)clsUser.enPermissions.ReadersManagement : 0;
            permission += (cbManageUsers.Checked) ? (int)clsUser.enPermissions.UsersManagement : 0;
            permission += (cbSettingsAccess.Checked) ? (int)clsUser.enPermissions.ManagementSettings : 0;
            return permission;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                MessageBox.Show("Error", "User was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            txtFirstName.Text = _User.PersonInfo.FirstName;
            txtFather.Text = _User.PersonInfo.SecondName;
            txtLast.Text = _User.PersonInfo.LastName;
            txtMother.Text = _User.PersonInfo.MotherName;
            
            lblUserID.Text = _User.UserID?.ToString();

            if (_User.PersonInfo.Gender == false)
            {
                rbMale.Checked = true;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                rbFemale.Checked = true;
                pbPersonImage.Image = Resources.Female_512;
            }
            dtpDateOfBirth.Value = _User.PersonInfo.DateOfBirth.Value;

           

            //contact info
            txtPhone.Text = _User.PersonInfo.Phone ?? String.Empty;
            txtEmail.Text = _User.PersonInfo.Email ?? String.Empty;
            txtMobile.Text = _User.PersonInfo.Mobile;//this is not nullable
            
            txtInstagram.Text = _User.PersonInfo.InstagramUserName ?? string.Empty;
            if (_User.PersonInfo.EmergencyContact1ID != null)
            {
                txtEmergencyName.Text = _User.PersonInfo.EmergencyContact1.Name ?? string.Empty;
                txtEmergencyRelation.Text = _User.PersonInfo.EmergencyContact1.Relation ?? string.Empty;
                txtEmergencyMobile.Text = _User.PersonInfo.EmergencyContact1.PhoneNumber ?? string.Empty;
            }

            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtPasswordConfirmation.Text = _User.Password;
            txtAddress.Text = _User.PersonInfo.Address;
            _LoadPermissions();

            //load person image incase it was set.
            if (_User.PersonInfo.ImagePath != null && File.Exists(_User.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = _User.PersonInfo.ImagePath;

            }

            //hide/show the remove linke incase there is no image for the person.
            llRemoveImage.Visible = (_User.PersonInfo.ImagePath != null);
        }
        private void _ResetDefaultValues()
        {
           
            if (_Mode == enMode.AddNew)
            {
                lblModeTitle.Text = "Add New User";
                lblUserID.Text = "N/A";
                this.Text = "Add New User";
                _User = new clsUser();
                this.ActiveControl = txtFirstName;
            }
            else
            {
                lblModeTitle.Text = "Update User Info";
                this.Text = "Update User";
            }
            //What is the minimum allowed age?
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-3);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            txtFirstName.Text = 
            
            lblUserID.Text = string.Empty;

            rbMale.Checked = true;
            pbPersonImage.Image = Resources.Male_512;

            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
                        


            //contact info
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;//this is not nullable            
            txtInstagram.Text = string.Empty;

            txtEmergencyName.Text = string.Empty;
            txtEmergencyRelation.Text = string.Empty;
            txtEmergencyMobile.Text = string.Empty;

            
            txtAddress.Text = string.Empty;

            cbFullAccess.Checked = false;
            cbManageBooks.Checked = false;
            cbManageCourses.Checked = false;
            cbManagePayments.Checked = false;
            cbManageReaders.Checked = false;
            cbManageUsers.Checked = false;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if(_Mode == enMode.Update)
                _LoadData();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblModeTitle_Click(object sender, EventArgs e)
        {

        }
        private void _SaveEmergencyContactsInfo()
        {
            if (_Mode == enMode.AddNew)
            {
                if (txtEmergencyName.Text != String.Empty)
                {
                    clsEmergencyContact E1 = new clsEmergencyContact();
                    E1.Name = txtEmergencyName.Text;
                    E1.Relation = txtEmergencyRelation.Text;
                    E1.PhoneNumber = txtEmergencyMobile.Text;
                    E1.Save();
                    _User.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                else
                {
                    _User.PersonInfo.EmergencyContact1ID = null;
                }
                
            }
            if (_Mode == enMode.Update && _User.PersonInfo.EmergencyContact1ID != null)
            {
                if (txtEmergencyName.Text != String.Empty)
                {
                    clsEmergencyContact E1 = new clsEmergencyContact();
                    E1.Name = txtEmergencyName.Text;
                    E1.Relation = txtEmergencyRelation.Text;
                    E1.PhoneNumber = txtPhone.Text;
                    E1.Save();
                    _User.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                else
                {
                    //Find a way to delete the Emergency contact 
                    //Right now we just set EmergencyContact1ID to null
                    _User.PersonInfo.EmergencyContact1ID = null;

                }
            }
  
            }








        private bool _SavePersonInfo()
        {
            if(!_HandlePersonImage())
            { return false; }

            _SaveEmergencyContactsInfo();
            if (_Mode == enMode.AddNew)
            {
                clsPerson personInfo = new clsPerson();
                personInfo.FirstName = txtFirstName.Text;
                personInfo.LastName = txtLast.Text;
                personInfo.SecondName = txtLast.Text;
                personInfo.MotherName = txtMother.Text;
                personInfo.DateOfBirth = dtpDateOfBirth.Value;
                
                if (rbMale.Checked)
                    personInfo.Gender = false;
                else
                    personInfo.Gender = true;
                personInfo.Phone = txtPhone.Text;
                personInfo.Email = txtEmail.Text;
                personInfo.Mobile = txtMobile.Text;
                
                personInfo.Address = txtAddress.Text;
                personInfo.InstagramUserName = txtInstagram.Text;
                personInfo.EmergencyContact1ID = _User.PersonInfo.EmergencyContact1ID;
                if (pbPersonImage.ImageLocation != null)
                    personInfo.ImagePath = pbPersonImage.ImageLocation;
                else
                    personInfo.ImagePath = null;

                if (personInfo.Save())
                {
                    _User.PersonID = personInfo.PersonID;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                clsPerson person = clsPerson.Find(_User.PersonID.Value);
                if (person == null)
                {
                    MessageBox.Show("Error", "User was not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return false;
                }

                person.FirstName = txtFirstName.Text;
                person.LastName = txtLast.Text;
                person.SecondName = txtFather.Text;
                person.MotherName = txtMother.Text;
                person.DateOfBirth = dtpDateOfBirth.Value;
                
                if (rbMale.Checked)
                    person.Gender = false;
                else
                    person.Gender = true;
                person.Phone = txtPhone.Text;
                person.Email = txtEmail.Text;
                person.Mobile = txtMobile.Text;                
                person.Address = txtAddress.Text;
                person.InstagramUserName = txtInstagram.Text;
                person.EmergencyContact1ID = _User.PersonInfo.EmergencyContact1ID;
                if (pbPersonImage.ImageLocation != null)
                    person.ImagePath = pbPersonImage.ImageLocation;
                else
                    person.ImagePath = null;

                //There is no business need for another Emergency contact
                return (person.Save());
               
            }
            
        }
        
        private bool _SaveUserInfo()
        {
            if(!_SavePersonInfo())
            {
                return false;
            }
            
            
            //Don't worry about validation it will be handled before executing this method
            _User.UserName = txtUserName.Text;            
            _User.Password = txtPassword.Text;
            _User.Permissions = (clsUser.enPermissions)_GetPermissionsFromCheckBoxes();
            
            return _User.Save();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            if (_SaveUserInfo())
            {
                lblUserID.Text = _User.UserID.ToString();
                MessageBox.Show("User has been added successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("An error occurred, unable to save user!", "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Validations

        private bool AreAllFieldsValid()
        {
            this.ValidateChildren();
            foreach (Control control in this.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(control)))
                {
                    return false; // At least one control has an error
                }
            }
            return true; // All fields are valid
        }
        
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (txtEmail.Text.Trim() == "")
                return;

            //validate email format
            if (!clsValidating.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            };

        }
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void ValidateEmptyKryptonTextBox(object sender, CancelEventArgs e)
        {
            KryptonTextBox Temp = (KryptonTextBox)sender;
            if (Temp.Text.Trim() == String.Empty)
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void ValidateEmergencyContact(object sender, CancelEventArgs e)
        {
            // Trimmed text for easier readability
            string name1 = txtEmergencyName.Text.Trim();
            string mobile1 = txtEmergencyMobile.Text.Trim();
            string relation1 = txtEmergencyRelation.Text.Trim();

            // All fields are empty (it's ok)
            if (string.IsNullOrEmpty(name1) && string.IsNullOrEmpty(mobile1) && string.IsNullOrEmpty(relation1))
            {
                errorProvider1.SetError(txtEmergencyName, null);
                errorProvider1.SetError(txtEmergencyRelation, null);
                errorProvider1.SetError(txtEmergencyMobile, null);
                return;
            }

            // All fields are filled (it's ok)
            if (!string.IsNullOrEmpty(name1) && !string.IsNullOrEmpty(mobile1) && !string.IsNullOrEmpty(relation1))
            {
                errorProvider1.SetError(txtEmergencyName, null);
                errorProvider1.SetError(txtEmergencyRelation, null);
                errorProvider1.SetError(txtEmergencyMobile, null);
                return;
            }

            // If not all fields are filled (validation error)
            e.Cancel = true;
            errorProvider1.SetError(txtEmergencyName, string.IsNullOrEmpty(name1) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyRelation, string.IsNullOrEmpty(relation1) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyMobile, string.IsNullOrEmpty(mobile1) ? "This field is required!" : null);
        }
        private void ValidatePasswordConfirmation(object sender, CancelEventArgs e)
        {
            ValidateEmptyKryptonTextBox(sender, e);
            if(txtPasswordConfirmation.Text != txtPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPasswordConfirmation, "Password confirmation does not match");
            }
            else
            {
                errorProvider1.SetError(txtPasswordConfirmation, null);
            }
        }

        

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                // ...
            }
        }

        private void cbFullAccess_CheckedChanged(object sender, EventArgs e)
        {
            if(cbFullAccess.Checked)
            {
                cbManageBooks.Checked = true;
                cbManageCourses.Checked = true;
                cbManagePayments.Checked = true;
                cbManageReaders.Checked = true;
                cbManageUsers.Checked = true;
                cbSettingsAccess.Checked = true;
            }
            else
            {
                cbManageBooks.Checked = false;
                cbManageCourses.Checked = false;
                cbManagePayments.Checked = false;
                cbManageReaders.Checked = false;
                cbManageUsers.Checked = false;
                cbSettingsAccess.Checked = false;
            }
        }

        private void cbManageReaders_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbManageReaders.Checked)
                cbFullAccess.Checked = false;

        }

        private void cbManageBooks_CheckedChanged(object sender, EventArgs e)
        {
            if(!cbManageBooks.Checked)
            cbFullAccess.Checked = false;
        }

        private void cbManageCourses_CheckedChanged(object sender, EventArgs e)
        {
            if(! cbManageCourses.Checked)
                cbFullAccess.Checked = false;
        }

        private void cbManageUsers_CheckedChanged(object sender, EventArgs e)
        {
            if(!cbManageUsers.Checked)
                cbFullAccess.Checked= false;
        }

        private void cbManagePayments_CheckedChanged(object sender, EventArgs e)
        {
            if(!cbManagePayments.Checked)
                cbFullAccess.Checked= false;
        }

        private void Validate_Permissions(object sender, CancelEventArgs e)
        {
            if (!(cbFullAccess.Checked || cbManageBooks.Checked || cbManageCourses.Checked || cbManagePayments.Checked || cbManageUsers.Checked || cbManageReaders.Checked))
            {
                e.Cancel = true;
                errorProvider1.SetError(cbFullAccess, "Please choose permissions");

            }
            else
                errorProvider1.SetError(cbFullAccess, null);
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image= Resources.Female_512;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
