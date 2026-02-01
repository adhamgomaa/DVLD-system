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
    public partial class frmReleaseLicense : Form
    {
        private int _licenseId = -1;
        public frmReleaseLicense()
        {
            InitializeComponent();
        }

        public frmReleaseLicense(int licenseId)
        {
            InitializeComponent();
            _licenseId = licenseId;
            ctrlFilterDriverLicense1.LoadLicenseInfo(_licenseId);
            ctrlFilterDriverLicense1.FilterEnabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Release this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int applicationId = -1;
                bool isRealesed = ctrlFilterDriverLicense1.SelectedLicenseInfo.ReleaseLicense(clsGlobal.CurrentUser.id, ref applicationId);
                lblReleaseId.Text = applicationId.ToString();
                if(!isRealesed)
                {
                    MessageBox.Show("Faild to realese the detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("License Relesed Successfuly", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                ctrlFilterDriverLicense1.FilterEnabled = false;
                linkLabel2.Enabled = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo licenseInfo = new frmDriverLicenseInfo(ctrlFilterDriverLicense1.LicenseID);
            licenseInfo.ShowDialog();
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
            _licenseId = obj;
            if (_licenseId == -1)
            {
                MessageBox.Show($"Selected License isn't found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            if (!ctrlFilterDriverLicense1.SelectedLicenseInfo.IsDetainedLicense)
            {
                MessageBox.Show($"Selected License isn't Detained, choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            lblLicense.Text = _licenseId.ToString();
            lblDetainID.Text = ctrlFilterDriverLicense1.SelectedLicenseInfo.detainedLicenseInfo.detainID.ToString();
            lblCreate.Text = ctrlFilterDriverLicense1.SelectedLicenseInfo.detainedLicenseInfo.userID.ToString();
            lblFees.Text = ctrlFilterDriverLicense1.SelectedLicenseInfo.detainedLicenseInfo.fees.ToString();
            lblAppFees.Text = clsAppTypes.FindType((int)clsApp.enApplicationType.ReleaseDetainedDrivingLicsense).fees.ToString();
            lblTotalFees.Text = Convert.ToString(Convert.ToDecimal(lblFees.Text) + Convert.ToDecimal(lblAppFees.Text));
            lblDetainDate.Text = ctrlFilterDriverLicense1.SelectedLicenseInfo.detainedLicenseInfo.detainDate.ToString("dd/MMM/yyyy");
            linkLabel1.Enabled = (_licenseId != -1);
            btnSave.Enabled = true;
        }
    }
}
