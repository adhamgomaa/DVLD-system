using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Users
{
    public partial class frmChangePassword : Form
    {
        private int _userId = -1;
        private clsUser _user;
        public frmChangePassword(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrlUserInfo1.LoadUserInfo(_userId);
            _user = clsUser.FindUser(_userId);
        }

        private void Box_Validating(TextBox box, CancelEventArgs e)
        {
            if(box == txtCurrent)
            {
                if(clsEncryption.Hashing(txtCurrent.Text) != _user.password)
                {
                    e.Cancel = true;
                    box.Focus();
                    errorProvider1.SetError(box, "Should you match with User password!!");
                } else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(box, "");
                }
            }
            if (box == txtCPass)
            {
                if (txtCPass.Text != txtPass.Text)
                {
                    e.Cancel = true;
                    box.Focus();
                    errorProvider1.SetError(box, "Should you match with new password!!");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(box, "");
                }
            }
        }

        private void txtBox_Validating(object sender, CancelEventArgs e)
        {
            Box_Validating((TextBox)sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _user.password = clsEncryption.Hashing(txtCPass.Text);
            if(_user.Save())
            {
                MessageBox.Show("Password Updated Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
