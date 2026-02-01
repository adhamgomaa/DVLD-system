using DVLD_Business;
using DVLD_Presentation.Controls;
using DVLD_Presentation.People;
using DVLD_Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace DVLD_Presentation.Users
{
    public partial class frmAddEditUsers : Form
    {
        enum enMode { add, update}
        enMode mode = enMode.add;
        private clsUser _user;
        private int _userId;
        public frmAddEditUsers()
        {
            InitializeComponent();
            mode = enMode.add;
        }

        public frmAddEditUsers(int userId)
        {
            InitializeComponent();
            mode = enMode.update;
            _userId = userId;
        }

        private void _LoadData()
        {
            _user = clsUser.FindUser(_userId);
            ctrlFilterPersonInfo1.FilterEnabled = false;
            ctrlFilterPersonInfo1.LoadPersonInfo(_user.personId);
            lblUserID.Text = _userId.ToString();
            txtUsername.Text = _user.userName;
            txtPass.Text = _user.password;
            txtCPass.Text = _user.password;
            cbActive.Checked = _user.isActive;
        }

        private void _ResetDefaultValues()
        {
            if (mode == enMode.add)
            {
                lblAddEdit.Text = "Add New User";
                this.Text = "Add New User";
                ctrlFilterPersonInfo1.FilterFoucs();
                _user = new clsUser();
                tabPage2.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                lblAddEdit.Text = "Update User";
                this.Text = "Update User";
                txtPass.Enabled = false;
                txtCPass.Enabled = false;
                tabPage2.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void SaveData()
        {
            _user.userName = txtUsername.Text.Trim();
            _user.password = txtPass.Text.Trim();
            _user.isActive = cbActive.Checked;
            _user.personId = ctrlFilterPersonInfo1.PersonId;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(mode == enMode.update)
            {
                btnSave.Enabled = true;
                tabPage2.Enabled = true;
                tabControl1.SelectTab(1);
                return;
            }
            if(ctrlFilterPersonInfo1.PersonId != -1)
            {
                if(clsUser.IsUserExistByPersonId(ctrlFilterPersonInfo1.PersonId))
                {
                    MessageBox.Show("This User is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlFilterPersonInfo1.FilterFoucs();
                }else
                {
                    btnSave.Enabled = true;
                    tabPage2.Enabled = true;
                    tabControl1.SelectTab(1);
                }
            } else
            {
                MessageBox.Show("Please Select a person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlFilterPersonInfo1.FilterFoucs();
            }
        }

        private void frmAddEditUsers_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (mode == enMode.update)
                _LoadData();
        }

        private void Box_Validating(TextBox box, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(box.Text))
            {
                e.Cancel = true;
                box.Focus();
                errorProvider1.SetError(box, "Required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(box, "");
            }
            if(box == txtUsername)
            {
                if(clsUser.FindUser(txtUsername.Text) != null)
                {
                    e.Cancel = true;
                    box.Focus();
                    errorProvider1.SetError(box, "This username is already exist, please change it");
                } else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(box, "");
                }
            }
            if(box == txtCPass)
            {
                if(txtCPass.Text != txtPass.Text)
                {
                    e.Cancel = true;
                    box.Focus();
                    errorProvider1.SetError(box, "Required");
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
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveData();
            if(mode == enMode.add)
            {
                if (clsUser.IsUserExistByPersonId(ctrlFilterPersonInfo1.PersonId))
                {
                    MessageBox.Show("This User is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlFilterPersonInfo1.FilterFoucs();
                }
                return;
            }
            if (_user.Save())
            {
                lblUserID.Text = _user.id.ToString();
                mode = enMode.update;
                lblAddEdit.Text = "Update User";
                this.Text = "Update User";
                ctrlFilterPersonInfo1.FilterEnabled = false;
                MessageBox.Show("Data Saved Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
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
