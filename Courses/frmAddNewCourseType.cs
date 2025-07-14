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
namespace The_Story_Corner_Project.Courses
{
    public partial class frmAddUpdateCourseType : KryptonForm
    {
        int? CourseTypeID = null;
        clsCourseType courseType;
        public static Action<int> OnCourseTypeAdded;
        enum enMode { AddNew= 1, Update=2};
        enMode _Mode;
        public frmAddUpdateCourseType()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            lblModeTitle.Text = "Add new course type";
            courseType = new clsCourseType();
        }

        public frmAddUpdateCourseType(int TypeID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            lblModeTitle.Text = "Update course type";
            CourseTypeID = TypeID;
            courseType = clsCourseType.Find(TypeID);
            if( courseType == null )
            {
                MessageBox.Show("Course type wasn't found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
                this.Close();
            }
            else
            {
                txtCourseType.Text = courseType.CourseTypeName;
                txtDescription.Text = courseType.Description;
            }
        }

        private void frmAddNewCourseType_Load(object sender, EventArgs e)
        {

        }
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;
            if (Temp.Text.Trim() == String.Empty)
            {
                e.Cancel = true;

                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
    

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            courseType.CourseTypeName = txtCourseType.Text;
            courseType.Description = (txtDescription.Text != "")?txtDescription.Text:null;
            if(courseType.Save())
            {
                MessageBox.Show("Course type was saved successfully!","Saved",MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                OnCourseTypeAdded?.Invoke(courseType.CourseTypeID.Value);
                return;
            }
            else
            {
                MessageBox.Show("An error occurred while saving course type info!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)(sender);
            if (Temp.Text.Trim() == String.Empty)
            {
                e.Cancel = true;

                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            //RichTextBox Temp =  (RichTextBox)(sender);
            //if (Temp.Text.Trim() == String.Empty)
            //{
            //    e.Cancel = true;

            //    errorProvider1.SetError(Temp, "This field is required!");
            //}
            //else
            //{
            //    errorProvider1.SetError(Temp, null);
            //}
        }
    }
}
