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
namespace The_Story_Corner_Project.Borrows
{
    public partial class frmShowBorrowInfo : KryptonForm
    {
        public frmShowBorrowInfo(int BorrowID)
        {
            InitializeComponent();
            ctrlBorrowInfo1.SetBorrowingInfo(BorrowID);
        }



        private void frmShowBorrowInfo_Load(object sender, EventArgs e)
        {

        }

        private void ctrlBorrowInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
