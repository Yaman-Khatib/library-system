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
namespace The_Story_Corner_Project.Courses
{
    public partial class frmCourseDetatils : KryptonForm
    {
        public frmCourseDetatils(int courseID)
        {
            InitializeComponent();
            if (!ctrlCourseInfo1.SetInfo(courseID))
            {
                MessageBox.Show("Course wasn't found","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void frmCourseDetatils_Load(object sender, EventArgs e)
        {

        }
    }
}
