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
    public partial class frmNewInternational : Form
    {
        private int _InternationalLicenseId = -1;
        private clsInternationalLicense _internationalLicense;
        public frmNewInternational()
        {
            InitializeComponent();
        }

        private void _loadData()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = lblAppDate.Text;
            lblExpiration.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");
            lblFees.Text = clsAppTypes.FindType((int)clsApp.enApplicationType.NewInternationalLicense).fees.ToString();
            lblCreate.Text = clsGlobal.CurrentUser.userName;
        }

        private void frmNewInternational_Load(object sender, EventArgs e)
        {
            _loadData();
        }

        private void _SaveLicense()
        {
            _internationalLicense = new clsInternationalLicense();
            _internationalLicense.personId = ctrlFilterDriverLicense1.SelectedLicenseInfo.driverInfo.personID;
            _internationalLicense.date = DateTime.Now;
            _internationalLicense.status = 3;
            _internationalLicense.statusDate = _internationalLicense.date;
            _internationalLicense.fees = clsAppTypes.FindType((int)clsApp.enApplicationType.NewInternationalLicense).fees;
            _internationalLicense.userId = clsGlobal.CurrentUser.id;

            _internationalLicense.DriverID = ctrlFilterDriverLicense1.SelectedLicenseInfo.driverID;
            _internationalLicense.LocalLicenseID = ctrlFilterDriverLicense1.LicenseID;
            _internationalLicense.IssueDate = DateTime.Now;
            _internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            _internationalLicense.IsActive = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _SaveLicense();
                if(_internationalLicense.Save())
                {
                    MessageBox.Show($"International License Issued Successfuly With ID = {_internationalLicense.InternationalID} ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _InternationalLicenseId = _internationalLicense.InternationalID;
                    lblDrivingAppID.Text = _internationalLicense.appID.ToString();
                    lblInterLicense.Text = _internationalLicense.InternationalID.ToString();
                    btnSave.Enabled = false;
                    ctrlFilterDriverLicense1.FilterEnabled = false;
                    linkLabel2.Enabled = true;
                }
                else
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowIntLicense showIntLicense = new frmShowIntLicense(_InternationalLicenseId);
            showIntLicense.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(ctrlFilterDriverLicense1.SelectedLicenseInfo.driverID);
            licenseHistory.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlFilterDriverLicense1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            if (SelectedLicenseID == -1)
            {
                MessageBox.Show($"Selected License isn't found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            lblLocalLicenseID.Text = SelectedLicenseID.ToString();
            linkLabel1.Enabled = (SelectedLicenseID != -1);
            linkLabel2.Enabled = false;
            if (ctrlFilterDriverLicense1.SelectedLicenseInfo.classID != 3)
            {
                MessageBox.Show($"Selected License should be class 3, choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            int activeInternationalLicenseId = clsInternationalLicense.GetActiveInternationalLicenseId(ctrlFilterDriverLicense1.SelectedLicenseInfo.driverID);

            if (activeInternationalLicenseId != -1)
            {
                MessageBox.Show($"Person already have an active international license with id = " + activeInternationalLicenseId, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linkLabel2.Enabled = true;
                _InternationalLicenseId = activeInternationalLicenseId;
                btnSave.Enabled = false;
                return;
            }
            btnSave.Enabled = true;
        }

        private void frmNewInternational_Activated(object sender, EventArgs e)
        {
            ctrlFilterDriverLicense1.txtLicenseIDFoucs();
        }
    }
}
