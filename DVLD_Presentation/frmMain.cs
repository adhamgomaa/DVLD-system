using DVLD_Business;
using DVLD_Presentation.App;
using DVLD_Presentation.App.AppTypes;
using DVLD_Presentation.App.Licenses;
using DVLD_Presentation.App.TestTypes;
using DVLD_Presentation.Controls;
using DVLD_Presentation.Drivers;
using DVLD_Presentation.Global_Classes;
using DVLD_Presentation.People;
using DVLD_Presentation.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation
{
    public partial class frmMain : Form
    {
        public bool isLogout = false;
        private frmListPeople _listPeople;
        private frmListUsers _listUsers;
        public frmMain()
        {
            InitializeComponent();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_listPeople == null || _listPeople.IsDisposed)
            {
                _listPeople = new frmListPeople();
                _listPeople.MdiParent = this;
                _listPeople.Show();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_listUsers == null || _listUsers.IsDisposed)
            {
                _listUsers = new frmListUsers();
                _listUsers.MdiParent = this;
                _listUsers.Show();
            }
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(clsGlobal.CurrentUser.id);
            userInfo.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(clsGlobal.CurrentUser.id);
            changePassword.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            isLogout = true;
            this.Close();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmAppsTypes appTypes = new frmAppsTypes();
            appTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestTypes testTypes = new frmTestTypes();
            testTypes.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewLocal newLocal = new frmNewLocal();
            newLocal.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocal manageLocal = new frmManageLocal();
            manageLocal.ShowDialog();
        }

        private void driverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers drivers = new frmListDrivers();
            drivers.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternational newInternational = new frmNewInternational();
            newInternational.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalApplication internationalApplication = new frmShowInternationalApplication();
            internationalApplication.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLicense renewLicense = new frmRenewLicense();
            renewLicense.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacementLicense replacement = new frmReplacementLicense();
            replacement.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDetainLicense detainLicense = new frmDetainLicense();
            detainLicense.ShowDialog();
        }

        private void releasDetainedLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmReleaseLicense releaseLicense = new frmReleaseLicense();
            releaseLicense.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses detainedLicenses = new frmListDetainedLicenses();
            detainedLicenses.ShowDialog();
        }

        private void releaseDetainedDrivingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseLicense releaseLicense = new frmReleaseLicense();
            releaseLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocal manageLocal = new frmManageLocal();
            manageLocal.ShowDialog();
        }
    }
}
