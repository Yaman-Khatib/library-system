using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
namespace The_Story_Corner_Project.Books
{
    public partial class frmBookSales : KryptonForm
    {
        DataTable _dtBookSales;
        public frmBookSales()
        {
            InitializeComponent();
        }

        private void frmBookSales_Load(object sender, EventArgs e)
        {
            LoadDataGridViewAsync();
        }

        private async Task<DataTable> GetDataFromDatabaseAsync()
        {

            // Simulate a delay to mimic a database call
            await Task.Delay(2);
            return clsBookSale.GetAllBookSales();
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
                _dtBookSales = await GetDataFromDatabaseAsync();

                // Bind the data to the DataGridView
                dgvBooks.DataSource = _dtBookSales;
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

            dgvBooks.Columns["BookSaleID"].HeaderText = "Sale ID";
            dgvBooks.Columns["SerialNumber"].HeaderText = "Book Number";
            dgvBooks.Columns["SerialNumber"].Width = 130;
            dgvBooks.Columns["Title"].Width = 220;
            
            dgvBooks.Columns["AccountNumber"].HeaderText = "Account number";
            dgvBooks.Columns["AccountNumber"].Width = 130;

            dgvBooks.Columns["FullName"].HeaderText = "Full Name";
            dgvBooks.Columns["FullName"].Width = 150;

            dgvBooks.Columns["BookPrice"].HeaderText = "Book Price";
            dgvBooks.Columns["BookPrice"].Width = 130;

            dgvBooks.Columns["LanguageName"].HeaderText = "Language";
            


            dgvBooks.Columns["SellDate"].HeaderText = "Sell Date";
            dgvBooks.Columns["SellDate"].Width = 165;

            dgvBooks.Columns["Sold by User"].Width = 130;


            dgvBooks.Columns["Notes"].Width = 130;

        }


        private void btnAddBook_Click(object sender, EventArgs e)
        {
            frmSaleBook frm = new frmSaleBook();
            frm.ShowDialog();
            //Load data again
            LoadDataGridViewAsync();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
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
            else if (filterColumn == "Reader name")
                filterColumn = "FullName";
            else if (filterColumn == "Account number")
                filterColumn = "AccountNumber";
            string filterValue = txtFilterValue.Text.Trim();

            if (filterColumn == "None" || filterValue == "")
            {
                _dtBookSales.DefaultView.RowFilter = "";

            }
            if (filterValue != "")
                _dtBookSales.DefaultView.RowFilter = String.Format($" {filterColumn} like '{filterValue}%' ");


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

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
