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
using The_Story_Corner_Project.Books;
using The_Story_Corner_Project.Global_Classes;
namespace The_Story_Corner_Project
{
    public partial class frmManageBooks : KryptonForm
    {
        DataTable _dtBooks;

        public frmManageBooks()
        {
            InitializeComponent();
        }
        private void _FillLanguagesComboBox()
        {
            DataTable dt = clsLanguage.GetAllLanguages();
            foreach (DataRow dr in dt.Rows)
            {
                cbLanguages.Items.Add(dr["LanguageName"].ToString());
            }
        }
        
        private void frmManageBooks_Load(object sender, EventArgs e)
        {
            _FillLanguagesComboBox();
            if(cbLanguages.Items.Count > 0)
            {
                int? EnglishIndex = clsLanguage.IDOfLanguageName("English") - 1;
                if(EnglishIndex != null)
                cbLanguages.SelectedIndex = (EnglishIndex.Value);
            }
            LoadDataGridViewAsync();
        }

        private async Task<DataTable> GetDataFromDatabaseAsync(string Language)
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsBook.GetAllBooks(Language);
        }

        private async void LoadDataGridViewAsync()
        {
            cbFilterBy.SelectedIndex = 0;
            try
            {
                // Disable the DataGridView while loading data
                dgvBooks.Enabled = false;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get the data from the database asynchronously
                _dtBooks = await GetDataFromDatabaseAsync(cbLanguages.Text);

                // Bind the data to the DataGridView
                dgvBooks.DataSource = _dtBooks;
                if (dgvBooks.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { pctrLoading.Visible = true; }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // Re-enable the DataGridView
                dgvBooks.Enabled = true;
            }
            lblRecordsCount.Text = dgvBooks.RowCount.ToString();
            StyleColumns();
        }
        private void StyleColumns()
        {
            if (dgvBooks.Rows.Count == 0)
                return;

            dgvBooks.Columns["BookID"].HeaderText = "Book ID";            
            dgvBooks.Columns["Title"].Width = 190;            
            dgvBooks.Columns["Author"].Width = 180;            
            dgvBooks.Columns["ISBN"].Width = 120;
            dgvBooks.Columns["SerialNumber"].HeaderText = "Book Number";
            dgvBooks.Columns["SerialNumber"].Width = 150;            
            dgvBooks.Columns["Genre"].Width = 140;
            dgvBooks.Columns["Language"].Width = 130;
            dgvBooks.Columns["Description"].Width = 160;
            dgvBooks.Columns["CopiesCount"].HeaderText = "Copies count";
            dgvBooks.Columns["IsDeleted"].Visible = false;


        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            frmAddUpdateBook frm = new frmAddUpdateBook();
            frm.ShowDialog();
            LoadDataGridViewAsync();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "None")
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Text = "";
                txtFilterValue.Visible = true;
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = cbFilterBy.Text.Trim();
            if (filterColumn == "Book Number")
                filterColumn = "SerialNumber";
            string filterValue = txtFilterValue.Text.Trim();

            if(filterColumn == "None" || filterValue == "")
            {
                _dtBooks.DefaultView.RowFilter = "";
                
            }
            if (filterValue != "")
                _dtBooks.DefaultView.RowFilter = String.Format($" {filterColumn} like '{filterValue}%' ");


            lblRecordsCount.Text = dgvBooks.Rows.Count.ToString();

            if (dgvBooks.Rows.Count == 0) 
            {
                pctrLoading.Visible = true;
            }
            else
            {
                pctrLoading.Visible = false;
            }
        }

        private void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvBooks.ClearSelection();
            dgvBooks.CurrentRow.Selected = true;
        }

        private void dgvBooks_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void dgvBooks_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check for right-click and valid row
            {
                // Select the entire row on right-click
                dgvBooks.ClearSelection(); // Clear any previous selections
                dgvBooks.Rows[e.RowIndex].Selected = true; // Select the row
            }
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookID = Convert.ToInt16(dgvBooks.SelectedRows[0].Cells["BookID"].Value);
                frmAddUpdateBook frm = new frmAddUpdateBook(bookID);
                frm.ShowDialog();
                LoadDataGridViewAsync();
            }
            else
            {
                MessageBox.Show("Please choose a book first to view it's info", "Choose a book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void toolStripMenuItemshowDetails_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookID = Convert.ToInt16(dgvBooks.SelectedRows[0].Cells["BookID"].Value);
                frmShowBookInfo frm = new frmShowBookInfo(bookID);
                frm.ShowDialog();
                LoadDataGridViewAsync();
            }
            else
            {
                MessageBox.Show("Please choose a book first to view it's info", "Choose a book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.DoesCurrentUserHavePermission(clsUser.enPermissions.FullAccess))
            {
                MessageBox.Show("Only admins have permission to delete books.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookID = Convert.ToInt16(dgvBooks.SelectedRows[0].Cells["BookID"].Value);
                if(MessageBox.Show("Are you sure you want to delete this book?","Delete book",MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return;
                }

                if(clsBook.DeleteBook(bookID))
                {
                    MessageBox.Show("Book was deleted successfully!");
                    LoadDataGridViewAsync();
                    return;
                }
                else
                {
                    MessageBox.Show("An error occurred can't delete book!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                MessageBox.Show("Please choose a book first to delete it", "Choose a book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void toolStripMenuItemAddBook_Click(object sender, EventArgs e)
        {
            frmAddUpdateBook frm = new frmAddUpdateBook();
            frm.ShowDialog();            
            LoadDataGridViewAsync();
        }
    }
}
