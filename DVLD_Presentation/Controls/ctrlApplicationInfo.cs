using DVLD_Business;
using DVLD_Presentation.App.Licenses;
using DVLD_Presentation.People;
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
    public partial class ctrlApplicationInfo : UserControl
    {
        enum enStatus { New = 1, Cancelled, Completed};
        enStatus status = enStatus.New;
        private clsLocalDriving _localDriving;
        private int _ApplicationId = -1;
        public int ApplicationID
        {
            get
            {
                return _ApplicationId;
            }
        }
        public int LocalId
        {
            get
            {
                return _localDriving.LocalId;
            }
        }
        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetApplicationInfo()
        {
            lblDrivingAppID.Text = "[???]";
            lblClass.Text = "[???]";
            lblTest.Text = "[???]";
            lblAppId.Text = "[???]";
            lblStatus.Text = "[???]";
            lblType.Text = "[???]";
            lblFees.Text = "[???]";
            lblApplicant.Text = "[???]";
            lblDate.Text = "[???]";
            lblSDate.Text = "[???]";
            lblCreate.Text = "[???]";
        }

        private void _LoadApplicationInfo()
        {
            _ApplicationId = _localDriving.appID;
            lblDrivingAppID.Text = LocalId.ToString();
            lblClass.Text = _localDriving.LicenseClassInfo.className;
            lblTest.Text = clsLocalDriving.GetPassedTestCount(LocalId) + "/3";
            lblAppId.Text = _ApplicationId.ToString();
            status = (enStatus)_localDriving.status;
            switch(status)
            {
                case enStatus.New:
                    lblStatus.Text = "New";
                    break;
                case enStatus.Cancelled:
                    lblStatus.Text = "Cancelled";
                    break;
                default:
                    lblStatus.Text = "Completed";
                    LinkLicense.Enabled = true;
                    break;
            }
            lblFees.Text = _localDriving.fees.ToString();
            lblType.Text = clsAppTypes.FindType(_localDriving.types).title;
            lblApplicant.Text = clsPerson.findPerson(_localDriving.personId).fullName();
            lblDate.Text = _localDriving.date.ToShortDateString();
            lblSDate.Text = _localDriving.statusDate.ToShortDateString();
            lblCreate.Text = clsUser.FindUser(_localDriving.userId).userName;
        }

        public void LoadApplicationInfo(int localId)
        {
            _localDriving = clsLocalDriving.FindLocalLicense(localId);
            if(_localDriving != null)
                _LoadApplicationInfo();
            else
            {
                _ResetApplicationInfo();
                MessageBox.Show("There is no Local License Application with ID = " + localId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo(_localDriving.personId);
            personInfo.ShowDialog();
        }

        private void LinkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo licenseInfo = new frmDriverLicenseInfo(clsLicense.GetLicenseID(LocalId));
            licenseInfo.ShowDialog();
        }
    }
}
