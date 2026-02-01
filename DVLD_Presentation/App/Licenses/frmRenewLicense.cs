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
    public partial class frmRenewLicense : Form
    {
        public frmRenewLicense()
        {
            InitializeComponent();
        }

        private void _LoadData()
        {
            ctrlFilterDriverLicense1.txtLicenseIDFoucs();
            lblAppDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = lblAppDate.Text;
            lblAppFees.Text = clsAppTypes.FindType((int)clsApp.enApplicationType.RenewDrivingLicense).fees.ToString();
            lblCreate.Text = clsGlobal.CurrentUser.userName;
        }

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsLicense newLicense = ctrlFilterDriverLicense1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.id);
                if(newLicense == null)
                {
                    MessageBox.Show("Faild to renew this License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lblRenewAppID.Text = newLicense.appID.ToString();
                lblRenewLicense.Text = newLicense.licenseID.ToString();
                MessageBox.Show($"License Renewd Successfuly With ID = {lblRenewLicense.Text} ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            int SelectedLicenseID = obj;
            if (SelectedLicenseID == -1)
            {
                MessageBox.Show($"Selected License isn't found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            linkLabel1.Enabled = (SelectedLicenseID != -1);
            int validityLength = ctrlFilterDriverLicense1.SelectedLicenseInfo.licenseClassInfo.length;
            lblExpiration.Text = DateTime.Now.AddYears(validityLength).ToString("dd/MMM/yyyy");
            lblLicenseFees.Text = ctrlFilterDriverLicense1.SelectedLicenseInfo.licenseClassInfo.fees.ToString();
            lblFees.Text = Convert.ToString(Convert.ToDecimal(lblLicenseFees.Text) + Convert.ToDecimal(lblAppFees.Text));

            if(!ctrlFilterDriverLicense1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show($"Selected License isn't yet expired, it will expire on {ctrlFilterDriverLicense1.SelectedLicenseInfo.expiredDate.ToString("dd/MMM/yyyy")}", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if(!ctrlFilterDriverLicense1.SelectedLicenseInfo.isActive)
            {
                MessageBox.Show($"Selected License isn't active", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            btnSave.Enabled = true;
        }
    }
}
