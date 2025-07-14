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

namespace The_Story_Corner_Project.Books
{
    public partial class 
        frmMiniAddNewLanguage : Form
    {
        public Action<int> OnLanguageAdded;
        public frmMiniAddNewLanguage()
        {
            InitializeComponent();
            // Set the form's border style to None
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White; // Background color of the form

            // Create a border panel
            Panel borderPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(86, 103, 120), // Border color (dark blue)
                Padding = new Padding(2) // Border thickness
            };
            this.Controls.Add(borderPanel);

            // Create a content panel inside the border panel
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White // Form's main content background color
            };
            borderPanel.Controls.Add(contentPanel);

            // Add form controls to the content panel instead of directly to the form
            // For example:
            //Label titleLabel = new Label
            //{
            //    Text = "Add New Book",
            //    Font = new Font("Arial", 16, FontStyle.Bold),
            //    Dock = DockStyle.Top,
            //    TextAlign = ContentAlignment.MiddleCenter
            //};
            //contentPanel.Controls.Add(titleLabel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLanguage.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the language name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsLanguage.DoesLanguageExist(txtLanguage.Text.Trim()))
            {
                MessageBox.Show("This language already exists , no need to add it!");
            }


            int? LanguageID = clsLanguage.AddNewLanguage(txtLanguage.Text);
            if (LanguageID != null)
            {
                MessageBox.Show("language was added successfully!");
                OnLanguageAdded?.Invoke(LanguageID.Value);
                this.Close();
            }
            else
            {
                MessageBox.Show("An error occurred , cannot add new language!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMiniAddNewLanguage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();

                //To prevent ding sound
                e.SuppressKeyPress = true;
            }
        }
    }
}
