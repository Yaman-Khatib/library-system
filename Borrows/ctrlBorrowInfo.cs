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

namespace The_Story_Corner_Project.Borrows
{
    public partial class ctrlBorrowInfo : UserControl
    {
        clsBorrow _Borrow;
        public ctrlBorrowInfo()
        {
            InitializeComponent();
            ctrlBookInfoWithFilter1.ResetDefaultValues();
            ctrlReaderInfoWithFilter1.ResetDefaultValues();
            ctrlBookInfoWithFilter1.FilterEnabled(false);
            ctrlReaderInfoWithFilter1.FilterEnabled(false);

        }
        
        public void SetBorrowingInfo(int BorrowID)
        {
            _Borrow = clsBorrow.Find(BorrowID);
            if(_Borrow == null )
            {
                MessageBox.Show("Borrow wasn't found","Not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            lblBorrowDate.Text = _Borrow.BorrowDate.Value.ToShortDateString();
            lblBorrowTime.Text = _Borrow.BorrowDate.Value.ToShortTimeString();
            lblNotes.Text = _Borrow.Notes ?? "N/A";
            ctrlBookInfoWithFilter1.SelectBook(_Borrow.BookID.Value);
            ctrlReaderInfoWithFilter1.SelectReader(_Borrow.ReaderID.Value);

        }
    }
}
