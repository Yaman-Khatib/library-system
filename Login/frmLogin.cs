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
namespace The_Story_Corner_Project.Login
{
    public partial class frmLogin : KryptonForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        void _reset()
        {
            string UserName = "User name";
            string Password = "Password";
                this.ActiveControl = label1;

            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                cbRememberMe.Enabled = true;
                txtUserName.Text = UserName;
                txtUserName.StateCommon.Content.Color1 = Color.Black;
                txtPassword.Text = Password;
                txtPassword.StateCommon.Content.Color1 = Color.Black;
                txtPassword.PasswordChar = '*';
                
            }
            else
            {
                txtUserName.Text = UserName; //User name
                txtUserName.StateCommon.Content.Color1 = Color.Gray;
                txtPassword.Text = Password;//Password
                txtPassword.StateCommon.Content.Color1 = Color.Gray;
                txtPassword.PasswordChar = '\0';
                cbRememberMe.Checked = false;
            }
            label1.Focus();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            _reset();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            KryptonTextBox temp = (KryptonTextBox)sender;
            if(temp.Text == "User name")
            {
                temp.Text = "";
                temp.StateCommon.Content.Color1 = Color.Black;
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            KryptonTextBox temp = (KryptonTextBox)sender;
            if (String.IsNullOrWhiteSpace(temp.Text))
            {
                temp.Text = "User name";
                temp.StateCommon.Content.Color1 = Color.Gray;
            }

        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            KryptonTextBox temp = (KryptonTextBox)sender;
            if (String.IsNullOrWhiteSpace(temp.Text))
            {
                temp.Text = "Password";
                temp.StateCommon.Content.Color1 = Color.Gray;
                temp.PasswordChar = '\0';
            }

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            KryptonTextBox temp = (KryptonTextBox)sender;
            if (temp.Text == "Password")
            {
                temp.Text = "";
                temp.PasswordChar = '*';
                temp.StateCommon.Content.Color1 = Color.Black;
            }

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            bool emptyTextBox = false;
            if(txtUserName.Text == "User name")
            {
                errorProvider1.SetError(txtUserName, "Please enter the user name!");
                emptyTextBox = true;
            }
            if(txtPassword.Text == "Password")
            {
                errorProvider1.SetError(txtPassword, "Please enter the password!");
                    emptyTextBox = true;
            }
            if(emptyTextBox)
            {
                MessageBox.Show("Both user name and password are required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            clsGlobal.CurrentUser = clsUser.GetUserInfoByUserNameAndPassword(txtUserName.Text, txtPassword.Text);
            if (clsGlobal.CurrentUser != null)
            {

            if(clsGlobal.CurrentUser.IsDeleted)
            {
                MessageBox.Show("Your account was deleted , please contact your admin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(cbRememberMe.Checked)
            {
                clsGlobal.RememberUserNameAndPassword(txtUserName.Text, txtPassword.Text);
            }
            else
            {
                clsGlobal.RememberUserNameAndPassword("", "");
            }

                frmMainMenu frm = new frmMainMenu();
                this.Visible = false;
                frm.ShowDialog();
                _reset();
                this.Visible = true;
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.RememberUserNameAndPassword("", "");
            }
        }
    }
}
