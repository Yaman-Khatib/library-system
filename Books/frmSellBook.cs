using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Library_Business;
using The_Story_Corner_Project.Global_Classes;
namespace The_Story_Corner_Project.Books
{
    public partial class frmSaleBook : KryptonForm
    {
        clsBookSale BookSale = new clsBookSale();
        clsBook BookInfo;
        clsReader Reader;
        public frmSaleBook()
        {
            InitializeComponent();
            ctrlBookInfoWithFilter1.OnBookSelected += OnBook_Selected;
            ctrlReaderInfoWithFilter1.OnReaderSelected += OnReader_Selected;
            
        }

        private void OnReader_Selected(int? ReaderID)
        {
            Reader = clsReader.FindByReaderID(ReaderID.Value);
            BookSale.ReaderID = ReaderID;
            
        }
        private void OnBook_Selected(int? BookId)
        {
            BookInfo = clsBook.FindByID(BookId.Value);
            if(BookInfo.CopiesCount <= 0)
            {
                DialogResult result =  MessageBox.Show("There is no available copies!\nDo you want to edit book copies?","No available copies",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation); ;
                if(result == DialogResult.Yes)
                {
                    frmAddUpdateBook frmAddUpdateBook = new frmAddUpdateBook(BookId.Value);
                    frmAddUpdateBook.ShowDialog();
                    ctrlBookInfoWithFilter1.SelectBook(BookId.Value);
                    
                }
                
            }
            
            BookSale.BookID = BookId; 
        }
        private void frmSellBook_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(BookInfo == null)
            {
                MessageBox.Show("Please choose a book first!", "No book has been selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (BookInfo.CopiesCount <= 0)
            {
                DialogResult result = MessageBox.Show("There is no available copies!\nDo you want to edit book copies?", "No available copies", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation); ;
                if (result == DialogResult.Yes)
                {
                    frmAddUpdateBook frmAddUpdateBook = new frmAddUpdateBook(BookInfo.BookID.Value);
                    frmAddUpdateBook.ShowDialog();
                    ctrlBookInfoWithFilter1.SelectBook(BookInfo.BookID.Value);
                    BookInfo= clsBook.FindByID(BookInfo.BookID.Value);
                    if (BookInfo.CopiesCount <= 0)
                        return;
                }
                else
                    return;
            }

            if (Reader == null)
            {
                MessageBox.Show("Please choose a reader first!", "No reader has been selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (numericFeesAmount.Value == 0)
            {
                MessageBox.Show("Please choose the price amount of the book!", "Set the price", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            BookSale.BookID = BookInfo.BookID;
            BookSale.ReaderID = Reader.ReaderID;
            if(txtNotes.Text != "")
            {
                BookSale.Notes = txtNotes.Text;
            }
            BookSale.PaidAmount = (double)numericFeesAmount.Value;
            BookSale.SoldByUserID = clsGlobal.CurrentUser.UserID;
            if(BookSale.Save())
            {
                MessageBox.Show("The sale info was saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                return;
            }
            else
            {
                MessageBox.Show("An error occurred cannot save", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
