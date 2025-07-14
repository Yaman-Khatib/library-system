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
using The_Story_Corner_Project.Global_Classes;
using The_Story_Corner_Project.Readers;

namespace The_Story_Corner_Project.Borrows
{
    public partial class frmAddUpdateBorrow : KryptonForm
    {
        enum enMode { AddNew = 1,Update = 2};
        enMode _Mode;
        clsBorrow _Borrow;
        int? _BorrowID;
        int? BookID = null;
        clsBook _Book;
        int? ReaderID = null;
        clsReader _Reader;
        bool CanReaderBorrow = false;
        int BorrowedBooksCount = 0;
        int BorrowedMagazinesCount = 0;
        bool CanReaderBorrowClassicBook = true;
        
        public frmAddUpdateBorrow()
        {
            InitializeComponent();
            _BorrowID = null;
            _Mode = enMode.AddNew;
            
            
        }

        //If the mode is update we automatically choose select the 
        void CheckReaderSelectedUpdateMode(int? readerID)
        {


            ReaderID = readerID;
            if (ReaderID == null)
            {
                ctrlReaderInfoWithFilter1.ResetDefaultValues();
                
                btnSave.Enabled = false;
                return;
            }

            if(!clsSubscription.DoesReaderHaveAnActiveSubscription(ReaderID.Value))
            {
                if(MessageBox.Show("Reader does not have an active subscription!\nDo you want to open renew subscription page?","No active subscription",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    frmRenewSubscription frm = new frmRenewSubscription(ReaderID.Value);
                    frm.ShowDialog();
                    ctrlReaderInfoWithFilter1.SelectReader(ReaderID.Value);
                }
                else
                {
                    btnSave.Enabled = false;
                    return;
                }
                
            }

            if(this.ctrlBookInfoWithFilter1.BookId.HasValue)
                btnSave.Enabled = true;
            else
            btnSave.Enabled = false;

            

        }
        void CheckBookSelectedUpdateMode(int? bookID)
        {
            BookID = bookID;
            if (BookID == null)
            {

                ctrlBookInfoWithFilter1.ResetDefaultValues();
                btnSave.Enabled = false;
                return;
            }

            if(ctrlReaderInfoWithFilter1.ReaderID.HasValue)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        
        public frmAddUpdateBorrow(int BorrowID)
        {
            InitializeComponent( );
            
            _BorrowID = BorrowID;
            _Mode = enMode.Update;
            dtpSubscriptionDate.Enabled = false;
        }

        private void frmAddUpdateBorrow_Load(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                _Borrow = clsBorrow.Find(_BorrowID.Value);
            ctrlBookInfoWithFilter1.OnBookSelected += CheckBookSelectedUpdateMode;
            ctrlReaderInfoWithFilter1.OnReaderSelected += CheckReaderSelectedUpdateMode;
                
                ctrlBookInfoWithFilter1.SelectBook(_Borrow.BookInfo.SerialNumber);
                this.BookID = ctrlBookInfoWithFilter1.BookId;
                ctrlReaderInfoWithFilter1.SelectReader(_Borrow.ReaderInfo.AccountNumber);
                this.ReaderID = ctrlReaderInfoWithFilter1.ReaderID;
                tpBookInfo.Enabled = true;
                _Book = clsBook.FindByID(_Borrow.BookID.Value);
                _Reader = clsReader.FindByReaderID(_Borrow.ReaderID.Value);
                dtpSubscriptionDate.Enabled = false;
            }
            else
            {
                _Borrow = new clsBorrow();
                _Book = new clsBook();
                _Reader = new clsReader();
                ctrlReaderInfoWithFilter1.OnReaderSelected += CheckBorrows_OnReaderSelected;
                ctrlBookInfoWithFilter1.OnBookSelected += CheckBorrows_OnBookSelected;
                dtpSubscriptionDate.Value = DateTime.Now;                
                dtpSubscriptionDate.Enabled = true;               
                tpBookInfo.Enabled = false;
                ctrlReaderInfoWithFilter1.FilterFocus();
                btnSave.Enabled = false;
            }

        }

        void CheckBorrows_OnReaderSelected(int? ReaderID)
        {
            
            this.ReaderID = ReaderID;
            tpBookInfo.Enabled = false;
            if (!ReaderID.HasValue) {
                
                tpBookInfo.Enabled = false;
                btnSave.Enabled = false;
                return;
            }
            _Reader = clsReader.FindByReaderID(ReaderID.Value);

            

            if (!clsSubscription.DoesReaderHaveAnActiveSubscription(ReaderID.Value))
            {
                if (MessageBox.Show("Reader does not have an active subscription!\nDo you want to open renew subscription page?", "No active subscription", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    frmRenewSubscription frm = new frmRenewSubscription(ReaderID.Value);
                    frm.ShowDialog();
                    ctrlReaderInfoWithFilter1.SelectReader(ReaderID.Value);
                }
                else
                {
                    btnSave.Enabled = false;
                    return;
                }

            }


            //In case the ctrlBook and ctrlReader had info and the user selected a reader account number that doesn't exist the save btn will be disabled so we need to check this case
            if (this.BookID.HasValue)
            {
                btnSave.Enabled = true;

            }
            BorrowedBooksCount = clsBorrow.GetNumberOfCurrentlyBorrowedClassicBooksToReader(ReaderID.Value);
            BorrowedMagazinesCount = clsBorrow.GetNumberOfCurrentlyBorrowedMagazinesForReader(ReaderID.Value);

            if (BorrowedBooksCount == 2 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed two books !","Not allowed to borrow");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                return;
            }
            else if (BorrowedMagazinesCount == 4 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed four magazines !", "Not allowed to borrow");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                return;
            }
            else if (BorrowedBooksCount == 1 && BorrowedMagazinesCount == 2 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed one book and two magazines!", "Not allowed to borrow");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                CanReaderBorrowClassicBook = false;
                return;
            }
            else if (BorrowedBooksCount == 1 && BorrowedMagazinesCount == 1 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed one book and one magazine ,he can borrow only one magazine!");
                //Disable search for book
                CanReaderBorrowClassicBook = false;
                CanReaderBorrow = true;
                
            }
            else if (BorrowedMagazinesCount == 3 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed 3 magazines ,he can borrow only one magazine!");
                //Disable search for book
                CanReaderBorrowClassicBook = false;
                CanReaderBorrow = true;
            }

            tpBookInfo.Enabled = true;
          
            CanReaderBorrow = true;
        }

        void CheckBorrows_OnBookSelected(int? BookID)
        {
            this.BookID = BookID;
            if (!BookID.HasValue)
            {
                btnSave.Enabled = false;
                
                return;
            }

            _Book = clsBook.FindByID(BookID.Value);

            if(_Book.Status == clsBook.enStatus.Borrowed)
            {
                MessageBox.Show("All copies of this book is borrowed , choose another book");
                btnSave.Enabled=false;
                return;
            }

            btnSave.Enabled = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ctrlBookInfoWithFilter1.BookId == null)
            {
                MessageBox.Show("Please select a book!","No book selected",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ctrlBookInfoWithFilter1.BookId == null)
            {
                MessageBox.Show("Please select a reader!", "No reader selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (BorrowedBooksCount == 2 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed two books, he must return borrowed books first!");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                return;
            }
            else if (BorrowedMagazinesCount == 4 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed four magazines, he must return borrowed magazines first!");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                return;
            }

            else if (BorrowedBooksCount == 1 && BorrowedMagazinesCount == 2 && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed one book and two magazines");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                return;
            }

            else if (BorrowedBooksCount == 1 && BorrowedMagazinesCount == 1 && clsBook.FindByID(ctrlBookInfoWithFilter1.BookId.Value).BookGenreInfo.GenreName != "Magazine" && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have borrowed one book and one magazine ,he can only borrow a magazine");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;
                
                return;
            }
            else if (BorrowedMagazinesCount == 3 && clsBook.FindByID(ctrlBookInfoWithFilter1.BookId.Value).BookGenreInfo.GenreName != "Magazine" && _Mode == enMode.AddNew)
            {
                MessageBox.Show("The reader have already borrowed three books , he can only borrow a magazine!");
                //Disable search for book
                btnSave.Enabled = false;
                CanReaderBorrow = false;

                return;
            }

                if (_Mode == enMode.AddNew)
            {
                _Borrow.BorrowDate = dtpSubscriptionDate.Value;
                _Borrow.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                _Borrow.DueDate = dtpSubscriptionDate.Value.AddDays(clsLibrarySettings.GetDefaultBorrowingDays());
            }

            _Borrow.BookID = this.BookID;
            _Borrow.ReaderID = this.ReaderID;            
            _Borrow.BookInfo = _Book;
            _Borrow.ReaderInfo = _Reader;
            if(!String.IsNullOrEmpty(txtNotes.Text))
            {
                _Borrow.Notes = txtNotes.Text; 
            }
            else
            {
                _Borrow.Notes = null;
            }

            if(_Borrow.Save())
            {
                MessageBox.Show("Borrow saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                return;                
            }
            else
            {
                MessageBox.Show("An error occurred , cannot save borrow!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if(!ReaderID.HasValue)
            {
                MessageBox.Show("Please choose a reader first!");
                return;
            }
            if (!CanReaderBorrow)
            {
                MessageBox.Show("Reader cannot borrow new books , please return borrowed books first");
                return;
            }

            tpBookInfo.Enabled = true;
            tabControl1.SelectedTab = tabControl1.TabPages["tpBookInfo"];
            ctrlBookInfoWithFilter1.FilterFocus();
        }

        private void pnlSubscriptionDate_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
