﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
namespace The_Story_Corner_Project.Books
{
    public partial class frmShowBookInfo : KryptonForm
    {
        public frmShowBookInfo(int bookID)
        {
            InitializeComponent();
            ctrlBookInfo1.SetInfo(bookID);
        }

        private void frmShowBookInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
