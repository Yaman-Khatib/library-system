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
namespace The_Story_Corner_Project.Readers
{
    public partial class frmShowReaderInfo : KryptonForm
    {

        public frmShowReaderInfo(int ReaderID)
        {
            InitializeComponent();
            if (clsReader.IsDeleted(ReaderID))
            {
                MessageBox.Show("This reader has been deleted!", "Cannot show deleted readers", MessageBoxButtons.OK, MessageBoxIcon.Stop);                
                this.Close();
                return;
            }
            ctrlReaderInfo1.SetInfo(ReaderID);
        }

        private void frmShowReaderInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
