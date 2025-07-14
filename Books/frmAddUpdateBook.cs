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
namespace The_Story_Corner_Project.Books
{
    public partial class frmAddUpdateBook : KryptonForm
    {
        enum enMode { AddNew = 1,Update = 2}
        clsBook _Book;
        int _BookID;
        enMode _Mode;
        public static event Action<int> OnBookAdded;
        public frmAddUpdateBook()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            this.Text = "Add new book";
        }
        public frmAddUpdateBook(int bookID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _BookID = bookID;
            this.Text = "Update book";
        }

        private void _FillGenresComboBox()
        {
            DataTable dt = clsBookGenres.GetAllGenres();
            cbGenre.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbGenre.Items.Add(dr["GenreName"].ToString());
            }

        }

        private void _FillAuthorsComboBox()
        {
            DataTable dt = clsBook.GetAllAuthors();
            cbAuthors.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbAuthors.Items.Add(dr["Author"].ToString());
            }

        }
        private void _FillLanguagesComboBox()
        {
            DataTable dt = clsLanguage.GetAllLanguages();
             cbLanguages.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbLanguages.Items.Add(dr["LanguageName"].ToString());
            }
        }
        private void _LoadData()
        {
            pctrAddNewBook.Visible = false;
            _Book = clsBook.FindByID(_BookID);
            if(_Book == null)
            {
                MessageBox.Show("Book was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            cbAuthors.Text = _Book.Author;
            txtTitle.Text = _Book.Title;
            txtDescription.Text = _Book.Description;
            txtISBN.Text = _Book.ISBN;
            txtSerialNumber.Text = _Book.SerialNumber;
            txtDescription.Text = _Book.Description;
            cbGenre.SelectedIndex = _Book.GenreID.Value - 1;
            cbLanguages.SelectedIndex = _Book.LanguageID.Value - 1;
            numericCopies.Value = _Book.CopiesCount.Value;

        }

  

        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                this.Text = "Add new book";
                lblModeTitle.Text = this.Text;
                this.ActiveControl = txtTitle;
                _Book = new clsBook();
                cbAuthors.Text = "";
                txtDescription.Text = "";
                txtISBN.Text = "";
                txtSerialNumber.Text = "";
                txtTitle.Text = "";
                cbGenre.Text = "";
                cbLanguages.Text = "";
            }
            else
            {
                this.Text = "Edit book info";
                lblModeTitle.Text = this.Text;
                this.ActiveControl = lblModeTitle;
            }
            numericCopies.Minimum = 0;
            numericCopies.Value = 1;
            _FillGenresComboBox();
            _FillLanguagesComboBox();
            _FillAuthorsComboBox();


        }
        private void frmAddUpdateBook_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show( "Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Book.Title = txtTitle.Text;
            _Book.Author = cbAuthors.Text;
            _Book.SerialNumber = txtSerialNumber.Text;
            if(txtSerialNumber.Text != "" )
            _Book.ISBN = txtISBN.Text;

            if(txtDescription.Text != "")
            _Book.Description = txtDescription.Text;

            _Book.CopiesCount = Convert.ToInt16(numericCopies.Value);
            _Book.GenreID = clsBookGenres.IDOfGenreName(cbGenre.Text);
            _Book.LanguageID = clsLanguage.IDOfLanguageName(cbLanguages.Text);
            bool saveResult = _Book.Save();
            if (saveResult && _Mode == enMode.AddNew)
            {
                MessageBox.Show("Book was added successfully","Successfully",MessageBoxButtons.OK);
                OnBookAdded?.Invoke(_Book.BookID.Value);
                this._ResetDefaultValues();
                return;
            }
            else if (saveResult && _Mode == enMode.Update)
            {
                MessageBox.Show("Book was updated successfully", "Successfully", MessageBoxButtons.OK);
                
            }
            else
            {
                MessageBox.Show("An error occurred , book wasn't added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnSave.Enabled = false;
            
        }
        private void OnGenreAdded(int GenreID)
        {
            _FillGenresComboBox();
            cbGenre.SelectedIndex = cbGenre.Items.IndexOf(clsBookGenres.Find(GenreID).GenreName);
        }
        private void OnLanguageAdded(int LanguageID)
        {
            _FillLanguagesComboBox();
            cbLanguages.SelectedIndex = cbLanguages.Items.IndexOf(clsLanguage.Find(LanguageID).LanguageName);
        }
        private void btnAddGenre_Click(object sender, EventArgs e)
        {
            frmMiniAddNewBookGenre frm = new frmMiniAddNewBookGenre();
            frm.OnGenreAdded += OnGenreAdded;
            frm.ShowDialog();
        }

        private void btnAddLanguage_Click(object sender, EventArgs e)
        {
            frmMiniAddNewLanguage frm = new frmMiniAddNewLanguage();
            frm.OnLanguageAdded += OnLanguageAdded;
            frm.ShowDialog();
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

        private void ValidateComboBox(object sender, CancelEventArgs e)
        {
            ComboBox temp = ((ComboBox)sender);
            if(temp.SelectedIndex == -1)
            {
                e.Cancel=true;
                errorProvider1.SetError(temp, "Pease select a language");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void cbGenre_Validating(object sender, CancelEventArgs e)
        {
            ComboBox temp = ((ComboBox)sender);
            if (temp.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "Pease select a book genre");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void cbLanguage_Validating(object sender, CancelEventArgs e)
        {
            ComboBox temp = ((ComboBox)sender);
            if (temp.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "Pease select a language");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void cbGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            if (temp.SelectedIndex != -1)            
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            if (temp.SelectedIndex != -1)                        
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if (temp.Text  != "")            
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtAuthor_TextChanged(object sender, EventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if (temp.Text != "")
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtISBN_TextChanged(object sender, EventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if (temp.Text != "")
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtSerialNumber_TextChanged(object sender, EventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if (temp.Text != "")
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void frmAddUpdateBook_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    btnSave.PerformClick();
            //    //e.SuppressKeyPress = true;
            //}
        }

        private void cmsAddLanguage_Click(object sender, EventArgs e)
        {
            btnAddLanguage.PerformClick();
        }

        private void cmsAddBookGenre_Click(object sender, EventArgs e)
        {
            btnAddGenre.PerformClick();
        }

        

        private void cmsDeleteGenre_Click(object sender, EventArgs e)
        {
            String Genre = cbGenre.Text.Trim();
            if (!String.IsNullOrEmpty(Genre))
            {
                int GenreID = clsBookGenres.IDOfGenreName(Genre).Value;
                if(clsBookGenres.DeleteGenre(GenreID))
                {
                    MessageBox.Show("Genre was deleted successfully!");
                    _FillGenresComboBox();
                }
                else
                {
                    MessageBox.Show("An error occurred cannot delete genre!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void cmsDeleteLanguage_Click(object sender, EventArgs e)
        {
            String Language = cbLanguages.Text.Trim();
            if (!String.IsNullOrEmpty(Language))
            {
                int LanguageID = clsLanguage.IDOfLanguageName(Language).Value;
                if (clsBookGenres.DeleteGenre(LanguageID))
                {
                    MessageBox.Show("Language was deleted successfully!");
                    _FillLanguagesComboBox();
                }
                else
                {
                    MessageBox.Show("An error occurred cannot delete genre!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
