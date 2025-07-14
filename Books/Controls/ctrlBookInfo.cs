using Library_Business;
using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Story_Corner_Project.Books.Controls
{
    public partial class ctrlBookInfo : UserControl
    {
        public clsBook BookInfo;
        public int? BookID { get; private set; }

        public event Action<int> OnBookSelected;
        public event Action<string> OnBookInfoEdited;

        public ctrlBookInfo()
        {
            InitializeComponent();
            BookID = null;
            ResetDefaultValues();
        }

        public void ResetDefaultValues()
        {
            lblAuthor.Text = "N/A";
            lblCoppies.Text = "N/A";
            lblDescription.Text = "N/A";
            lblGenre.Text = "N/A";
            lblISBN.Text = "N/A";
            lblLanguage.Text = "N/A";
            lblSerialNumber.Text = "N/A";
            lblTitle.Text = "N/A";
            llEditBookInfo.Enabled = false;
        }

        private void UpdateUIWithBookInfo()
        {
            BookID = BookInfo.BookID.Value;
            lblAuthor.Text = BookInfo.Author ?? "N/A";
            lblTitle.Text = BookInfo.Title ?? "N/A";
            if (BookInfo.Description == null)
            {
                lblDescription.Font = new Font(lblDescription.Font.FontFamily, 10);
                lblDescription.Text = "N/A";
            }
            else
            {
                lblDescription.Font = new Font(lblDescription.Font.FontFamily, 8);
                lblDescription.Text = BookInfo.Description;
            }


            lblCoppies.Text = BookInfo.CopiesCount.ToString();
            lblISBN.Text = BookInfo.ISBN ?? "N/A";
            lblSerialNumber.Text = BookInfo.SerialNumber ?? "N/A";
            lblLanguage.Text = BookInfo.LanguageInfo?.LanguageName ?? "N/A";
            lblGenre.Text = BookInfo.BookGenreInfo?.GenreName ?? "N/A";
            
        }

        public void SetInfo(int bookID)
        {
            
            if (clsBook.IsDeleted(bookID))
            {

                DialogResult result =  MessageBox.Show("The selected book has been deleted.\nDo you want to restore it?", "Book Not Available", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if(clsBook.RestoreDeletedBook(bookID))
                    {
                        MessageBox.Show("Book was restored successfully!", "Book was restored", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("An error occurred cannot restore book", "Book was not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            BookInfo = clsBook.FindByID(bookID);
            if (BookInfo == null)
            {
                MessageBox.Show("Book was not found!");
                this.Enabled = false;
                BookID = null;
                return;
            }
            llEditBookInfo.Enabled = true;
            UpdateUIWithBookInfo();
            OnBookSelected?.Invoke(bookID);
        }

        public async Task SetInfoAsync(string BookNumber)
        {
            if (clsBook.IsDeleted(BookNumber))
            {

                DialogResult result = MessageBox.Show("The selected book has been deleted.\nDo you want to restore it?", "Book Not Available", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (clsBook.RestoreDeletedBook(BookNumber))
                    {
                        MessageBox.Show("Book was restored successfully!", "Book was restored", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("An error occurred cannot restore book", "Book was not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            BookInfo = await Task.Run(() => clsBook.FindBySerialNumber(BookNumber));
            if (BookInfo == null)
            {
                MessageBox.Show("Book was not found!");
                BookID = null;
                ResetDefaultValues();
                return;
            }
            llEditBookInfo.Enabled = true;
            Invoke((Action)UpdateUIWithBookInfo);
            OnBookSelected?.Invoke(BookInfo.BookID.Value);
        }

        

        private void llEditBookInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmAddUpdateBook frm = new frmAddUpdateBook(BookInfo.BookID.Value))
            {
                frm.ShowDialog();
            }

            SetInfo(BookInfo.BookID.Value);
            OnBookInfoEdited?.Invoke(BookInfo.SerialNumber);
        }

        private void lblCoppies_Click(object sender, EventArgs e)
        {

        }

        private void lblLanguage_Click(object sender, EventArgs e)
        {

        }

        private void lblSerialNumber_Click(object sender, EventArgs e)
        {

        }
    }
}
