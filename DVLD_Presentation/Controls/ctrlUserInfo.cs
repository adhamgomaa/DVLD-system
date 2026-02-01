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

namespace DVLD_Presentation.Controls
{
    public partial class ctrlUserInfo : UserControl
    {
        private clsUser _user;
        private int _userId = -1;
        public int userId
        {
            get { return _userId; }
        }
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        private void _ResetUserInfo()
        {
            _userId = -1;
            lblUserId.Text = "[???]";
            lblUserName.Text = "[???]";
            lblActive.Text = "[???]";
        }

        private void _LoadUserInfo()
        {
            ctrlPersonInfo1.LoadPersonInfo(_user.personId);
            _userId = _user.id;
            lblUserId.Text = userId.ToString();
            lblUserName.Text = _user.userName;
            if (_user.isActive)
                lblActive.Text = "Yes";
            else
                lblActive.Text = "No";
        }

        public void LoadUserInfo(int userId)
        {
            _user = clsUser.FindUser(userId);
            if (_user == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No user with User ID = " + userId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _LoadUserInfo();
        }

    }
}
