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

namespace The_Story_Corner_Project.Users
{
    public partial class frmShowUserInfo : KryptonForm
    {
        public frmShowUserInfo(int UserID)
        {
            InitializeComponent();
            ctrUserInfo1.SetInfo(UserID);
        }

        private void ShowUserInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
