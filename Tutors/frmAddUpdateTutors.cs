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
using The_Story_Corner_Project.Properties;
using System.IO;
using The_Story_Corner_Project.Global_Classes;
namespace The_Story_Corner_Project.Tutors
{
    public partial class frmAddUpdateTutors : KryptonForm
    {

        enum enMode { AddNew = 1, Update = 2 }
        enMode _Mode;
        clsTutor _Tutor;
        int _TutorID;
        public static Action<int?> OnTutorAdded;
        string PredicatedAccountNumber;
        string PredicatedAccountExample;
        public frmAddUpdateTutors()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _Tutor = new clsTutor();
            lblModeTitle.Text = "Add new tutor";
            this.Text = lblModeTitle.Text;
        }

        public frmAddUpdateTutors(int ReaderID)
        {
            InitializeComponent();            
            _Mode = enMode.Update;
            _TutorID = ReaderID;
            lblModeTitle.Text = "Update tutor";
            this.Text = lblModeTitle.Text;
        }


        private void frmAddUpdateTutors_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        
        private void _LoadData()
        {
             _Tutor = clsTutor.Find(_TutorID);

            if (_Tutor == null)
            {
                MessageBox.Show("Error", "Tutor was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            txtFirstName.Text = _Tutor.PersonInfo.FirstName;
            txtFatherName.Text = _Tutor.PersonInfo.SecondName;
            txtLastName.Text = _Tutor.PersonInfo.LastName;
            txtMotherName.Text = _Tutor.PersonInfo.MotherName;

            if (_Tutor.PersonInfo.Gender == false)
            {
                rbMale.Checked = true;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                rbFemale.Checked = true;
                pbPersonImage.Image = Resources.Female_512;
            }
            dtpDateOfBirth.Value = _Tutor.PersonInfo.DateOfBirth.Value;

            //This field is nullable
            
            txtNotes.Text = _Tutor.PersonInfo?.Notes ?? String.Empty;


            //contact info
            txtPhone.Text = _Tutor.PersonInfo.Phone ?? String.Empty;
            txtEmail.Text = _Tutor.PersonInfo.Email ?? String.Empty;
            txtMobile.Text = _Tutor.PersonInfo.Mobile;//this is not nullable
            txtFacebookUserName.Text = _Tutor.PersonInfo.FacebookUserName ?? string.Empty;
            txtInstagramUserName.Text = _Tutor.PersonInfo.InstagramUserName ?? string.Empty;
            if (_Tutor.PersonInfo.EmergencyContact1ID != null)
            {
                txtEmergencyCoName1.Text = _Tutor.PersonInfo.EmergencyContact1.Name ?? string.Empty;
                txtEmergencyCoRelation1.Text = _Tutor.PersonInfo.EmergencyContact1.Relation ?? string.Empty;
                txtEmergencyMobile1.Text = _Tutor.PersonInfo.EmergencyContact1.PhoneNumber ?? string.Empty;
            }

            if (_Tutor.PersonInfo.EmergencyContact2ID != null)
            {
                txtEmergencyCoName2.Text = _Tutor.PersonInfo.EmergencyContact2.Name ?? string.Empty;
                txtEmergencyCoRelation2.Text = _Tutor.PersonInfo.EmergencyContact2.Relation ?? string.Empty;
                txtEmergencyMobile2.Text = _Tutor.PersonInfo.EmergencyContact2.PhoneNumber ?? string.Empty;
            }
            txtAddress.Text = _Tutor.PersonInfo.Address;

            //load person image incase it was set.
            if (_Tutor.PersonInfo.ImagePath != null && File.Exists(_Tutor.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = _Tutor.PersonInfo.ImagePath;
                llRemoveImage.Visible = true;
            }
            //    int SubscriptionTypeIndex = cbSubscriptionType.Items.IndexOf(_Reader.LastSubscriptionInfo.SubscriptionTypeInfo.SubscriptionTypeName);            
            //    cbSubscriptionType.SelectedIndex = SubscriptionTypeIndex;
            //    numericDiscount.Value = _Reader.LastSubscriptionInfo.Discount;
            //    cbSubscriptionType.Enabled = false;
        }

        private void SaveEmergencyContactsInfo()
        {
            if (_Mode == enMode.AddNew)
            {
                if (txtEmergencyCoName1.Text != String.Empty)
                {
                    clsEmergencyContact E1 = new clsEmergencyContact();
                    E1.Name = txtEmergencyCoName1.Text;
                    E1.Relation = txtEmergencyCoRelation1.Text;
                    E1.PhoneNumber = txtEmergencyMobile1.Text;
                    E1.Save();
                    _Tutor.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                if (txtEmergencyCoName2.Text != String.Empty)
                {
                    clsEmergencyContact E2 = new clsEmergencyContact();
                    E2.Name = txtEmergencyCoName2.Text;
                    E2.Relation = txtEmergencyCoRelation2.Text;
                    E2.PhoneNumber = txtEmergencyMobile2.Text;
                    E2.Save();
                    _Tutor.PersonInfo.EmergencyContact2ID = E2.ContactID.Value;
                }
            }
            if (_Mode == enMode.Update && _Tutor.PersonInfo.EmergencyContact1ID == null) //If reader had no eContact1 previously
            {
                //if the eContact 1 info was field then create a new eContact record
                if (txtEmergencyCoName1.Text != String.Empty)
                {
                    clsEmergencyContact E1 = new clsEmergencyContact();
                    E1.Name = txtEmergencyCoName1.Text;
                    E1.Relation = txtEmergencyCoRelation1.Text;
                    E1.PhoneNumber = txtEmergencyMobile1.Text;
                    E1.Save();
                    _Tutor.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                else
                {
                    _Tutor.PersonInfo.EmergencyContact1ID = null;
                }
            }
            else if (_Mode == enMode.Update && _Tutor.PersonInfo.EmergencyContact1ID != null) //The user already has a contact1 but we will update it
            {
                int EmergencyContactID = _Tutor.PersonInfo.EmergencyContact1ID ?? -1;
                if (txtEmergencyCoName1.Text != String.Empty)
                {
                    clsEmergencyContact E1 = clsEmergencyContact.Find(EmergencyContactID);
                    E1.Name = txtEmergencyCoName1.Text;
                    E1.Relation = txtEmergencyCoRelation1.Text;
                    E1.PhoneNumber = txtEmergencyMobile1.Text;
                    E1.Save();

                }
                else //if the contact info fields were empty then the user wants to delete contact info
                {
                    _Tutor.PersonInfo.EmergencyContact1ID = null;
                    //And then we will delete the Contact record
                    clsEmergencyContact.DeleteEmergencyContact(EmergencyContactID);
                }
            }

            if (_Mode == enMode.Update && _Tutor.PersonInfo.EmergencyContact2ID == null)//if the reader had no EContact2 previously
            {
                //if the user filled contact info add a new contact
                if (txtEmergencyCoName2.Text != String.Empty)
                {
                    clsEmergencyContact E2 = new clsEmergencyContact();
                    E2.Name = txtEmergencyCoName2.Text;
                    E2.Relation = txtEmergencyCoRelation2.Text;
                    E2.PhoneNumber = txtEmergencyMobile2.Text;
                    E2.Save();
                    _Tutor.PersonInfo.EmergencyContact1ID = E2.ContactID.Value;
                }
                else
                {
                    _Tutor.PersonInfo.EmergencyContact2ID = null;
                }
            }
            else if (_Mode == enMode.Update && _Tutor.PersonInfo.EmergencyContact2ID != null) //The user already has a contact2 but we will update it
            {
                int EmergencyContactID = _Tutor.PersonInfo.EmergencyContact2ID ?? -1;
                if (txtEmergencyCoName1.Text != String.Empty)//Update it
                {
                    clsEmergencyContact E1 = clsEmergencyContact.Find(EmergencyContactID);
                    E1.Name = txtEmergencyCoName1.Text;
                    E1.Relation = txtEmergencyCoRelation1.Text;
                    E1.PhoneNumber = txtEmergencyMobile1.Text;
                    E1.Save();

                }
                else //if the contact info fields were empty then the user wants to delete contact info
                {
                    _Tutor.PersonInfo.EmergencyContact1ID = null;
                    //And then we will delete the Contact record
                    clsEmergencyContact.DeleteEmergencyContact(EmergencyContactID);
                }
            }




        }

        private bool SavePersonInfo()
        {
            if (!_HandlePersonImage())
                return false;
            SaveEmergencyContactsInfo();
            if (_Mode == enMode.AddNew)
            {
                clsPerson personInfo = new clsPerson();
                personInfo.FirstName = txtFirstName.Text;
                personInfo.LastName = txtLastName.Text;
                personInfo.SecondName = txtFatherName.Text;
                personInfo.MotherName = txtMotherName.Text;
                personInfo.DateOfBirth = dtpDateOfBirth.Value;
                
                if (rbMale.Checked)
                    personInfo.Gender = false;
                else
                    personInfo.Gender = true;
                personInfo.Phone = txtPhone.Text;
                personInfo.Email = txtEmail.Text;
                personInfo.Mobile = txtMobile.Text;
                personInfo.FacebookUserName = txtFacebookUserName.Text;
                personInfo.Address = txtAddress.Text;
                personInfo.InstagramUserName = txtInstagramUserName.Text;
                personInfo.EmergencyContact1ID = _Tutor.PersonInfo.EmergencyContact1ID;
                personInfo.EmergencyContact2ID = _Tutor.PersonInfo.EmergencyContact2ID;
                if (pbPersonImage.ImageLocation != null)
                    personInfo.ImagePath = pbPersonImage.ImageLocation;
                else
                    personInfo.ImagePath = null;

                if (personInfo.Save())
                {
                    _Tutor.PersonID = personInfo.PersonID;
                    return true;
                }
                else { return false; }
            }
            else
            {
                clsPerson person = clsPerson.Find(_Tutor.PersonID.Value);
                if (person == null)
                {
                    MessageBox.Show("Person was not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

                person.FirstName = txtFirstName.Text;
                person.LastName = txtLastName.Text;
                person.SecondName = txtFatherName.Text;
                person.MotherName = txtMotherName.Text;
                person.DateOfBirth = dtpDateOfBirth.Value;                
                if (rbMale.Checked)
                    person.Gender = false;
                else
                    person.Gender = true;
                person.Phone = txtPhone.Text;
                person.Email = txtEmail.Text;
                person.Mobile = txtMobile.Text;
                person.FacebookUserName = txtFacebookUserName.Text;
                person.Address = txtAddress.Text;
                person.InstagramUserName = txtInstagramUserName.Text;

                person.EmergencyContact1ID = _Tutor.PersonInfo.EmergencyContact1ID;
                person.EmergencyContact2ID = _Tutor.PersonInfo.EmergencyContact2ID;
                if (pbPersonImage.ImageLocation != null)
                    person.ImagePath = pbPersonImage.ImageLocation;
                else
                    person.ImagePath = null;


                return person.Save();
            }

        }
        private void _ResetDefaultValues()
        {
            

            if (_Mode == enMode.AddNew)
            {
                lblModeTitle.Text = "Add New Tutor";
                this.Text = "Add New Tutor";
            
                PredicatedAccountNumber = clsReader.PredictNewAccountNumber();
                PredicatedAccountExample = $"e.g: {PredicatedAccountNumber}";            
            }
            else
            {
                
                lblModeTitle.Text = "Update Tutor Info";
                
                this.Text = "Update Reader";                
            }
            //What is the minimum allowed age?
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-3);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            

            txtFirstName.Text = string.Empty;
            txtFatherName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMotherName.Text = string.Empty;
            

            rbMale.Checked = true;
            pbPersonImage.Image = Resources.Male_512;
            llRemoveImage.Visible = false;
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //This field is nullable
            txtNotes.Text = string.Empty;


            //contact info
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;//this is not nullable
            txtFacebookUserName.Text = string.Empty; ;
            txtInstagramUserName.Text = string.Empty;

            txtEmergencyCoName1.Text = string.Empty;
            txtEmergencyCoRelation1.Text = string.Empty;
            txtEmergencyMobile1.Text = string.Empty;

            txtEmergencyCoName2.Text = string.Empty;
            txtEmergencyCoRelation2.Text = string.Empty;
            txtEmergencyMobile2.Text = string.Empty;

            txtAddress.Text = string.Empty;
        }


        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private bool _HandlePersonImage()   
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Tutor.PersonInfo.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Tutor.PersonInfo.ImagePath != null)
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_Tutor.PersonInfo.ImagePath);
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

     

  

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SavePersonInfo();

            if (_Mode == enMode.AddNew)
            {
                _Tutor.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                
                if (_Tutor.Save())
                {
                    
                    MessageBox.Show("Tutor has been added successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    OnTutorAdded?.Invoke(_Tutor.TutorID);
                }

            }
            else //if mode is update
            {
                if (_Tutor.Save())
                {
                    MessageBox.Show("Tutor has been updated successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                }
            }

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
            TextBox Temp = (TextBox)sender;
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
        private void ValidateEmergencyContact1(object sender, CancelEventArgs e)
        {
            // Trimmed text for easier readability
            string name1 = txtEmergencyCoName1.Text.Trim();
            string mobile1 = txtEmergencyMobile1.Text.Trim();
            string relation1 = txtEmergencyCoRelation1.Text.Trim();

            // All fields are empty (it's ok)
            if (string.IsNullOrEmpty(name1) && string.IsNullOrEmpty(mobile1) && string.IsNullOrEmpty(relation1))
            {

                errorProvider1.SetError(txtEmergencyCoName1, null);
                errorProvider1.SetError(txtEmergencyCoRelation1, null);
                errorProvider1.SetError(txtEmergencyMobile1, null);
                return;
            }

            // All fields are filled (it's ok)
            if (!string.IsNullOrEmpty(name1) && !string.IsNullOrEmpty(mobile1) && !string.IsNullOrEmpty(relation1))
            {

                errorProvider1.SetError(txtEmergencyCoName1, null);
                errorProvider1.SetError(txtEmergencyCoRelation1, null);
                errorProvider1.SetError(txtEmergencyMobile1, null);
                return;
            }

            // If not all fields are filled (validation error)
            e.Cancel = true;
            errorProvider1.SetError(txtEmergencyCoName1, string.IsNullOrEmpty(name1) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyCoRelation1, string.IsNullOrEmpty(relation1) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyMobile1, string.IsNullOrEmpty(mobile1) ? "This field is required!" : null);
        }
        private void ValidateEmergencyContact2(object sender, CancelEventArgs e)
        {
            // Trimmed text for easier readability
            string name2 = txtEmergencyCoName2.Text.Trim();
            string mobile2 = txtEmergencyMobile2.Text.Trim();
            string relation2 = txtEmergencyCoRelation2.Text.Trim();

            // All fields are empty (it's ok)
            if (string.IsNullOrEmpty(name2) && string.IsNullOrEmpty(mobile2) && string.IsNullOrEmpty(relation2))
            {
                errorProvider1.SetError(txtEmergencyCoName2, null);
                errorProvider1.SetError(txtEmergencyCoRelation2, null);
                errorProvider1.SetError(txtEmergencyMobile2, null);
                return;
            }

            // All fields are filled (it's ok)
            if (!string.IsNullOrEmpty(name2) && !string.IsNullOrEmpty(mobile2) && !string.IsNullOrEmpty(relation2))
            {
                errorProvider1.SetError(txtEmergencyCoName2, null);
                errorProvider1.SetError(txtEmergencyCoRelation2, null);
                errorProvider1.SetError(txtEmergencyMobile2, null);
                return;
            }

            // If not all fields are filled (validation error)
            e.Cancel = true;
            errorProvider1.SetError(txtEmergencyCoName2, string.IsNullOrEmpty(name2) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyCoRelation2, string.IsNullOrEmpty(relation2) ? "This field is required!" : null);
            errorProvider1.SetError(txtEmergencyMobile2, string.IsNullOrEmpty(mobile2) ? "This field is required!" : null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            tcTutor.SelectedTab = tcTutor.TabPages[1];
        }

        private void rbMale_CheckedChanged_1(object sender, EventArgs e)
        {
            if(rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void llSetImage_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void llRemoveImage_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }
    }
}
