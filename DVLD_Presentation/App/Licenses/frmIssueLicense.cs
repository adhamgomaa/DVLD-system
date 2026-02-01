using DVLD_Business;
using DVLD_Presentation.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.App.Licenses
{
    public partial class frmIssueLicense : Form
    {
        private int _localId = -1;
        private clsLocalDriving _localDrivingLicense;
        public frmIssueLicense(int localId)
        {
            InitializeComponent();
            _localId = localId;
        }

        private void frmIssueLicense_Load(object sender, EventArgs e)
        {
            _localDrivingLicense = clsLocalDriving.FindLocalLicense(_localId);
            if(_localDrivingLicense == null)
            {
                MessageBox.Show($"No application with ID = {_localId}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(!_localDrivingLicense.IsPassedAllTests())
            {
                MessageBox.Show($"This person should pass all tests first", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(_localDrivingLicense.GetActiveLicense() != -1)
            {
                MessageBox.Show($"This person has an active license already", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlApplicationInfo1.LoadApplicationInfo(_localId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int licenseId = _localDrivingLicense.IssueLicenseForFirstTime(textBox1.Text.Trim(), clsGlobal.CurrentUser.id);

            if(licenseId != -1)
            {

                MessageBox.Show($"License issued successfully with License ID = {licenseId}", "Succeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
