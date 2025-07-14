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

    public partial class frmMiniAddNewBookGenre : Form
    {
        public Action<int> OnGenreAdded;
        public frmMiniAddNewBookGenre()
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtGenre.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the genre name!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);  
                return;
            }

            if(clsBookGenres.DoesGenreExists(txtGenre.Text.Trim()))
            {
                MessageBox.Show("This book genre already exists , no need to add it!");
                return;
            }

            int? GenreID = clsBookGenres.AddNewGenre(txtGenre.Text.Trim());
            if (GenreID != null)
            {
                MessageBox.Show("Genre was added successfully!");
                OnGenreAdded?.Invoke(GenreID.Value);
                this.Close();
            }
            else
            {
                MessageBox.Show("An error occurred , cannot add new genre!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMiniAddNewBookGenre_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
                //e.SuppressKeyPress = true;
            }
        }
    }
}
