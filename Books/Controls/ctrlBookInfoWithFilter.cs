using Library_Business;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Story_Corner_Project.Books.Controls
{
    public partial class ctrlBookInfoWithFilter : UserControl
    {
        public Action<int?> OnBookSelected;

        public int? BookId { get; set; }
        public clsBook BookInfo { get; set; }
        public ctrlBookInfoWithFilter()
        {
            InitializeComponent();
            BookId = null;

            // Subscribe to the event with the wrapper method
            ctrlBookInfo1.OnBookInfoEdited += OnBookInfoEditedHandler;
            
        }

        // Synchronous handler that calls the asynchronous method
        private void OnBookInfoEditedHandler(string serialNumber)
        {
            // Call the async method but don't await it here
            _ = SearchAndDisplayBookAsync(serialNumber);
        }

        // Enables or disables the filter group box
        public void FilterEnabled(bool enabled)
        {
            gbFilter.Enabled = enabled;
        }

        // Sets focus to the filter textbox
        public void FilterFocus()
        {
            txtFilterValue.Focus();
         
        }

        public void ResetDefaultValues()
        {
            ctrlBookInfo1.ResetDefaultValues();
        }

        // Allows external setting of the serial number, performs search, and updates BookId
        public async void SelectBook(string serialNumber)
        {
            
            txtFilterValue.Text = serialNumber;
            await SearchAndDisplayBookAsync(serialNumber);
            BookId = ctrlBookInfo1.BookID;
            BookInfo = ctrlBookInfo1.BookInfo;

        }

        public void SelectBook(int BookID)
        {
            txtFilterValue.Text = clsBook.FindByID(BookID).SerialNumber;
            ctrlBookInfo1.SetInfo(BookID);
        }

        // Event handler for the Find button click event
        private async void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                MessageBox.Show("Please enter the book serial number!");
                return;
            }

            btnFind.Enabled = false; // Disable button to prevent multiple clicks
            ctrlBookInfo1.ResetDefaultValues(); // Clear previous book details while searching

            await SearchAndDisplayBookAsync(txtFilterValue.Text);

            btnFind.Enabled = true; // Re-enable button after search
        }

        // Asynchronous method to search and display book information
        private async Task SearchAndDisplayBookAsync(string serialNumber)
        {
            await ctrlBookInfo1.SetInfoAsync(serialNumber);

            BookId = ctrlBookInfo1.BookID;

            if (BookId == null)
            {
                
                OnBookSelected?.Invoke(null); // Trigger event with null to indicate book not found
            }
            else
            {
                OnBookSelected?.Invoke(BookId); // Trigger event with the found BookID
            }
        }

        public void PerformClickButtonFind()
        {
            btnFind.PerformClick();
        }
        void SetNewBookInfo(int bookID)
        {
            string serialNum = clsBook.FindByID(bookID).SerialNumber;
            txtFilterValue.Text = serialNum;
            _ = SearchAndDisplayBookAsync(serialNum);
        }
        // Event handler for adding a new book        
        private void btnAddNewBook_Click(object sender, EventArgs e)
        {
            frmAddUpdateBook.OnBookAdded += SetNewBookInfo;
            frmAddUpdateBook frm = new frmAddUpdateBook();
            frm.ShowDialog();

            // Optionally, you could refresh the book list or search results here if necessary
        }

        private void ctrlBookInfo1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
