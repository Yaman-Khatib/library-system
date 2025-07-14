using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using The_Story_Corner_Project.Global_Classes;
using Library_Business;
using The_Story_Corner_Project.Properties;

namespace The_Story_Corner_Project.Readers
{
    public partial class frmAddUpdateReader : KryptonForm
    {
        enum enMode { AddNew = 1, Update = 2 }
        enMode _Mode;
        clsReader _Reader = new clsReader();
        int _ReaderID;
        public Action<int?> OnReaderSaved;
        
        string PredicatedAccountNumber;
        string PredicatedAccountExample;
        public frmAddUpdateReader()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;

        }

        public frmAddUpdateReader(int ReaderID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _ReaderID = ReaderID;

        }
        private void _LoadData()
        {
            _Reader = clsReader.FindByReaderID(_ReaderID);

            if (_Reader == null)
            {
                MessageBox.Show( "Reader was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(clsReader.IsDeleted(_ReaderID))
            {
                MessageBox.Show("Cannot update reader info, reader has been deleted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            txtFirstName.Text = _Reader.PersonInfo.FirstName;
            txtFatherName.Text = _Reader.PersonInfo.SecondName;
            txtLastName.Text = _Reader.PersonInfo.LastName;
            txtMotherName.Text = _Reader.PersonInfo.MotherName;

            txtAccountNumber.Text = _Reader.AccountNumber;
            lblReaderID.Text = _Reader.ReaderID?.ToString();

            if (_Reader.PersonInfo.Gender == false)
            {
                rbMale.Checked = true;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                rbFemale.Checked = true;
                pbPersonImage.Image = Resources.Female_512;
            }
            dtpDateOfBirth.Value = _Reader.PersonInfo.DateOfBirth.Value;

            //This field is nullable
            txtSchoolName.Text = _Reader.PersonInfo.School ?? String.Empty;
            txtNotes.Text = _Reader.PersonInfo?.Notes ?? String.Empty;


            //contact info
            txtPhone.Text = _Reader.PersonInfo.Phone ?? String.Empty;
            txtEmail.Text = _Reader.PersonInfo.Email ?? String.Empty;
            txtMobile.Text = _Reader.PersonInfo.Mobile;//this is not nullable
            txtFacebookUserName.Text = _Reader.PersonInfo.FacebookUserName ?? string.Empty;
            txtInstagramUserName.Text = _Reader.PersonInfo.InstagramUserName ?? string.Empty;
            if (_Reader.PersonInfo.EmergencyContact1ID != null)
            {
                txtEmergencyCoName1.Text = _Reader.PersonInfo.EmergencyContact1.Name ?? string.Empty;
                txtEmergencyCoRelation1.Text = _Reader.PersonInfo.EmergencyContact1.Relation ?? string.Empty;
                txtEmergencyMobile1.Text = _Reader.PersonInfo.EmergencyContact1.PhoneNumber ?? string.Empty;
            }

            if (_Reader.PersonInfo.EmergencyContact2ID != null)
            {
                txtEmergencyCoName2.Text = _Reader.PersonInfo.EmergencyContact2.Name ?? string.Empty;
                txtEmergencyCoRelation2.Text = _Reader.PersonInfo.EmergencyContact2.Relation ?? string.Empty;
                txtEmergencyMobile2.Text = _Reader.PersonInfo.EmergencyContact2.PhoneNumber ?? string.Empty;
            }
            txtAddress.Text = _Reader.PersonInfo.Address;

            //load person image incase it was set.
            if (_Reader.PersonInfo.ImagePath != null && File.Exists(_Reader.PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = _Reader.PersonInfo.ImagePath;
                llRemoveImage.Visible = true;
            }
            //    int SubscriptionTypeIndex = cbSubscriptionType.Items.IndexOf(_Reader.LastSubscriptionInfo.SubscriptionTypeInfo.SubscriptionTypeName);            
            //    cbSubscriptionType.SelectedIndex = SubscriptionTypeIndex;
            //    numericDiscount.Value = _Reader.LastSubscriptionInfo.Discount;
            //    cbSubscriptionType.Enabled = false;
        }
        private void _ResetDefaultValues()
        {
            _FillSubscriptionTypesComboBox();
            dtpSubscriptionDate.Value = DateTime.Now;
            if (_Mode == enMode.AddNew)
            {
                lblModeTitle.Text = "Add New Reader";
                lblReaderID.Text = "N/A";
                lblSubscriptionType.Text = "Subscription Type : ";
                this.Text = "Add New Reader";
                txtAccountNumber.Enabled = true;
                PredicatedAccountNumber = clsReader.PredictNewAccountNumber();
                PredicatedAccountExample = $"e.g: {PredicatedAccountNumber}";
                SetPlaceholderTextForAccountNumber();
                pnlSubscriptionDate.Visible = true;
            }
            else
            {
                lblSubscriptionType.Text = "Last Subscription : ";
                lblModeTitle.Text = "Update Reader Info";
                pnlFees.Visible = false;
                this.Text = "Update Reader";
                txtAccountNumber.Enabled = false;
                pnlSubscriptionType.Visible = false;
                pnlSubscriptionDate.Visible = false;
            }
            //What is the minimum allowed age?
            dtpDateOfBirth.MaxDate = DateTime.Now;
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cbSubscriptionType.SelectedIndex = 0;

            txtFirstName.Text = string.Empty;
            txtFatherName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMotherName.Text = string.Empty;

            txtAccountNumber.Text = string.Empty;
            lblReaderID.Text = string.Empty;

            rbMale.Checked = true;
            pbPersonImage.Image = Resources.Male_512;
            llRemoveImage.Visible = false;
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //This field is nullable
            txtSchoolName.Text = string.Empty;
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
            double SubscriptionFees = (double)clsSubscriptionType.Find( cbSubscriptionType.SelectedIndex + 1).SubscriptionTypeFees.Value;
            return SubscriptionFees * (1 - (double)numericDiscount.Value/100);
        }
        private void frmAddUpdateReader_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
            SetPlaceholderTextForAccountNumber();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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
            if (_Reader.PersonInfo.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Reader.PersonInfo.ImagePath != null)
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_Reader.PersonInfo.ImagePath);
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

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }

        private void rbYesSchool_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoSchool.Checked)
            {
                txtSchoolName.Text = String.Empty;
                txtSchoolName.Enabled = false;
            }
            else
            {
                txtSchoolName.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    _Reader.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                if (txtEmergencyCoName2.Text != String.Empty)
                {
                    clsEmergencyContact E2 = new clsEmergencyContact();
                    E2.Name = txtEmergencyCoName2.Text;
                    E2.Relation = txtEmergencyCoRelation2.Text;
                    E2.PhoneNumber = txtEmergencyMobile2.Text;
                    E2.Save();
                    _Reader.PersonInfo.EmergencyContact2ID = E2.ContactID.Value;
                }
            }
            if (_Mode == enMode.Update && _Reader.PersonInfo.EmergencyContact1ID == null) //If reader had no eContact1 previously
            {
                //if the eContact 1 info was field then create a new eContact record
                if (txtEmergencyCoName1.Text != String.Empty)
                {
                    clsEmergencyContact E1 = new clsEmergencyContact();
                    E1.Name = txtEmergencyCoName1.Text;
                    E1.Relation = txtEmergencyCoRelation1.Text;
                    E1.PhoneNumber = txtEmergencyMobile1.Text;
                    E1.Save();
                    _Reader.PersonInfo.EmergencyContact1ID = E1.ContactID.Value;
                }
                else
                {
                    _Reader.PersonInfo.EmergencyContact1ID = null;
                }
            }
            else if (_Mode == enMode.Update && _Reader.PersonInfo.EmergencyContact1ID != null) //The user already has a contact1 but we will update it
            {
                int EmergencyContactID = _Reader.PersonInfo.EmergencyContact1ID ?? -1;
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
                    _Reader.PersonInfo.EmergencyContact1ID = null;
                    //And then we will delete the Contact record
                    clsEmergencyContact.DeleteEmergencyContact(EmergencyContactID);
                }
            }

            if (_Mode == enMode.Update && _Reader.PersonInfo.EmergencyContact2ID == null)//if the reader had no EContact2 previously
            {
                //if the user filled contact info add a new contact
                if (txtEmergencyCoName2.Text != String.Empty)
                {
                    clsEmergencyContact E2 = new clsEmergencyContact();
                    E2.Name = txtEmergencyCoName2.Text;
                    E2.Relation = txtEmergencyCoRelation2.Text;
                    E2.PhoneNumber = txtEmergencyMobile2.Text;
                    E2.Save();
                    _Reader.PersonInfo.EmergencyContact1ID = E2.ContactID.Value;
                }
                else
                {
                    _Reader.PersonInfo.EmergencyContact2ID = null;
                }
            }
            else if (_Mode == enMode.Update && _Reader.PersonInfo.EmergencyContact2ID != null) //The user already has a contact2 but we will update it
            {
                int EmergencyContactID = _Reader.PersonInfo.EmergencyContact2ID ?? -1;
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
                    _Reader.PersonInfo.EmergencyContact1ID = null;
                    //And then we will delete the Contact record
                    clsEmergencyContact.DeleteEmergencyContact(EmergencyContactID);
                }
            }




        }

        private void SaveSubscriptionInfo()
        {
            clsSubscription subscription;
            if (_Mode == enMode.AddNew)
            {
                clsSubscriptionType subscriptionType = clsSubscriptionType.Find((cbSubscriptionType.SelectedIndex + 1));

                subscription = new clsSubscription();
                subscription.ReaderID = _Reader.ReaderID;
                subscription.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                subscription.SubscriptionTypeID = subscriptionType.SubscriptionTypeID;
                subscription.StartDate = dtpSubscriptionDate.Value;
                subscription.ExpirationDate = dtpSubscriptionDate.Value.AddMonths(subscriptionType.SubscriptionTypeMonths.Value);
                subscription.SubscriptionReason = clsSubscription.enSubscriptionReason.FirstTime;
                subscription.Discount = (int)numericDiscount.Value;
                

                subscription.Save();
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
                if (txtSchoolName.Text != "")
                    personInfo.School = txtSchoolName.Text;
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
                personInfo.EmergencyContact1ID = _Reader.PersonInfo.EmergencyContact1ID;
                personInfo.EmergencyContact2ID = _Reader.PersonInfo.EmergencyContact2ID;
                if (pbPersonImage.ImageLocation != null)
                    personInfo.ImagePath = pbPersonImage.ImageLocation;
                else
                    personInfo.ImagePath = null;

                if (personInfo.Save())
                {
                    _Reader.PersonID = personInfo.PersonID;
                    return true;
                }
                else { return false; }
            }
            else
            {
                clsPerson person = clsPerson.Find(_Reader.PersonID.Value);
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
                if (txtSchoolName.Text != "")
                    person.School = txtSchoolName.Text;
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

                person.EmergencyContact1ID = _Reader.PersonInfo.EmergencyContact1ID;
                person.EmergencyContact2ID = _Reader.PersonInfo.EmergencyContact2ID;
                if (pbPersonImage.ImageLocation != null)
                    person.ImagePath = pbPersonImage.ImageLocation;
                else
                    person.ImagePath = null;

                return person.Save();
            }

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
                _Reader.AccountNumber = txtAccountNumber.Text;
                _Reader.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                _Reader.JoinDate = dtpSubscriptionDate.Value;
                if (_Reader.Save())
                {
                    if (!(cbSubscriptionType.SelectedIndex == 3)) //For limited subscription there is no need to add add a subscription
                    {
                        SaveSubscriptionInfo();
                    }
                    lblReaderID.Text = _Reader.ReaderID.ToString();
                    MessageBox.Show("Reader has been added successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    OnReaderSaved?.Invoke(_Reader.ReaderID);
                }
            }
            else //if mode is update
            {
                if (_Reader.Save())
                {
                    MessageBox.Show("Reader has been updated successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    
                }
            }



        }

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

        private void ValidateAccountNumber(object sender, CancelEventArgs e)
        {
            KryptonTextBox Temp = (KryptonTextBox)sender;
            if (clsReader.IsReaderExists(Temp.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This account number is already taken, Choose another one!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }


        private void AccountNumber_Validating(object sender, CancelEventArgs e)
        {
            if (_Mode == enMode.Update)
                return;

            KryptonTextBox Temp = (KryptonTextBox)sender;
            if (Temp.Text.Trim() == String.Empty || Temp.Text.Trim() == PredicatedAccountExample)
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }

            string accountNumber = Temp.Text.Trim();

            if (!clsValidating.IsValidAccountNumber(accountNumber))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, $"Invalid account number format. It should follow the format 'Year-Number' (e.g., {clsReader.PredictNewAccountNumber()}).\nOnly years up to the current year are allowed.");
                return;
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }

            //check if account number is available
            if (clsReader.IsReaderExists(accountNumber))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This account number is already taken please choose another one!");
                return;
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }


        // Sets placeholder text with gray color and smaller font if empty
        private void SetPlaceholderTextForAccountNumber()
        {
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                txtAccountNumber.StateCommon.Content.Color1 = Color.Gray;
                txtAccountNumber.StateCommon.Content.Font = new Font("Microsoft Sans Serif", 11.6f, FontStyle.Italic);
                txtAccountNumber.Text = PredicatedAccountExample;
            }
        }

        // Handles focus event for entering the textbox
        private void txtAccountNumber_Enter(object sender, EventArgs e)
        {
            if (txtAccountNumber.Text == PredicatedAccountExample)
            {
                txtAccountNumber.StateCommon.Content.Color1 = Color.Black;
                txtAccountNumber.StateCommon.Content.Font = new Font("Poppins", (float)12.8, FontStyle.Regular);
                txtAccountNumber.Text = ""; // Clear placeholder
            }
        }
        // Handles leaving the textbox, resetting placeholder if empty
        private void txtAccountNumber_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                SetPlaceholderTextForAccountNumber(); // Reset to placeholder if left empty
            }
        }

        private void cbSubscriptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbSubscriptionType.SelectedIndex == 3)//Limited subscription
            {
                MessageBox.Show("You have selected limited subscription , reader wont be able to borrow books!","Limited subscription",MessageBoxButtons.OK,MessageBoxIcon.Exclamation); 
            }
            if (cbSubscriptionType.SelectedIndex == 4)//Golden subscription
            {
                MessageBox.Show("You have selected golden subscription , reader will have unlimited subscription!", "Limited subscription", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (cbSubscriptionType.SelectedIndex >= 0 && _Mode == enMode.AddNew) 
            lblFees.Text = GetFees().ToString("N0") + " S.P";
        }

        private void numericDiscount_ValueChanged(object sender, EventArgs e)
        {
            if (cbSubscriptionType.SelectedIndex >= 0 && _Mode == enMode.AddNew)
                lblFees.Text = GetFees().ToString("N0") + " S.P";
        }

        private void dtpDateOfBirth_Validating(object sender, CancelEventArgs e)
        {
            DateTimePicker Temp = (DateTimePicker)sender;

            // Get the selected date and current date without the time component
            DateTime selectedDate = Temp.Value.Date;
            DateTime todayDate = DateTime.Now.Date;

            if (selectedDate == todayDate)
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "The date of birth cannot be today. Please select a valid past date!");
            }
            else if (selectedDate > todayDate)
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "The date of birth cannot be a future date. Please select a valid past date!");
            }
            else
            {
                // Clear error if the date is valid
                errorProvider1.SetError(Temp, null);
            }
        }

    }

}
