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
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 0;
        private int _totalRecords = 0;
        private string _currentLanguage = "";

        private System.Windows.Forms.Timer _searchTimer;

        public frmManageBooks()
        {
            InitializeComponent();
            _searchTimer = new System.Windows.Forms.Timer();
            _searchTimer.Interval = 500; // 500ms delay
            _searchTimer.Tick += _searchTimer_Tick;
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

        private async Task<DataTable> GetDataFromDatabaseAsync(string Language, int pageNumber, int pageSize)
        {
            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            
            // Get filter values
            string filterColumn = GetFilterColumn();
            string filterValue = txtFilterValue.Text.Trim();
            
            return clsBook.GetAllBooksPaged(Language, filterColumn, filterValue, pageNumber, pageSize, out _totalRecords);
        }

        private string GetFilterColumn()
        {
            switch (cbFilterBy.Text)
            {
                case "Book ID":
                    return "BookID";
                case "Title":
                    return "Title";
                case "Author":
                    return "Author";
                case "ISBN":
                    return "ISBN";
                case "Book Number":
                    return "SerialNumber";
                case "Genre":
                    return "Genre";
                case "Language":
                    return "Language";
                default:
                    return "";
            }
        }

        private async void LoadDataGridViewAsync()
        {
            cbFilterBy.SelectedIndex = 0;
            _currentLanguage = cbLanguages.Text;
            _currentPage = 1; // Reset to first page
            
            await LoadCurrentPageAsync();
        }

        private async Task LoadCurrentPageAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Disable the DataGridView while loading data
                dgvBooks.Enabled = false;
                dgvBooks.DataSource = null;

                // Show a loading message or spinner
                pctrLoading.Visible = true;

                // Get paged data from the database asynchronously
                _dtBooks = await GetDataFromDatabaseAsync(_currentLanguage, _currentPage, _pageSize);
                
                // Calculate total pages
                _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                // Bind the data to the DataGridView
                dgvBooks.DataSource = _dtBooks;

                if (dgvBooks.RowCount > 0)
                {
                    pctrLoading.Visible = false;
                }
                else
                { 
                    pctrLoading.Visible = true; 
                }
            }
            catch (Exception ex)
            {
                // Check for SQL connection errors
                if (ex is System.Data.SqlClient.SqlException || (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException))
                {
                    if (MessageBox.Show("Connection to the database was lost. Would you like to retry?", "Connection Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        await LoadCurrentPageAsync();
                        return;
                    }
                }

                // Handle any other errors that occur during the data fetch
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable the DataGridView
                dgvBooks.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            
            UpdatePaginationInfo();
            StyleColumns();
        }


        private void UpdatePaginationInfo()
        {
            lblRecordsCount.Text = $"Page {_currentPage} of {_totalPages} ({_totalRecords} total records)";
            
            // Enable/disable navigation buttons (if they exist)
            // Note: Pagination buttons need to be added to the form designer
            // For now, we'll just update the label with pagination info
        }

        private async void GoToNextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                await LoadCurrentPageAsync();
            }
        }

        private async void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                await LoadCurrentPageAsync();
            }
        }

        private void StyleColumns()
        {
            if (dgvBooks.Rows.Count == 0)
                return;

            // Hide ID and IsDeleted columns
            if (dgvBooks.Columns.Contains("BookID"))
                dgvBooks.Columns["BookID"].Visible = false;
            if (dgvBooks.Columns.Contains("IsDeleted"))
                dgvBooks.Columns["IsDeleted"].Visible = false;

            dgvBooks.Columns["Title"].Width = 190;            
            dgvBooks.Columns["Author"].Width = 180;            
            dgvBooks.Columns["ISBN"].Width = 120;
            dgvBooks.Columns["SerialNumber"].HeaderText = "Book Number";
            dgvBooks.Columns["SerialNumber"].Width = 150;            
            dgvBooks.Columns["Genre"].Width = 140;
            dgvBooks.Columns["Language"].Width = 130;
            dgvBooks.Columns["Description"].Width = 160;
            dgvBooks.Columns["CopiesCount"].HeaderText = "Copies count";
            
            // Make the last column (CopiesCount) responsive to fill remaining width
            if (dgvBooks.Columns.Contains("CopiesCount"))
            {
                dgvBooks.Columns["CopiesCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
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
            // Stop the timer if it's running (user is still typing)
            _searchTimer.Stop();

            // Restart the timer
            _searchTimer.Start();
        }

        private async void _searchTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer so it doesn't fire again
            _searchTimer.Stop();

            // Reset to first page
            _currentPage = 1;
            await LoadCurrentPageAsync();
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

        #region Pagination Event Handlers

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        #endregion

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }
    }
}
