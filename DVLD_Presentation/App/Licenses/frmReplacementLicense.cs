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
    public partial class frmReplacementLicense : Form
    { 
        public frmReplacementLicense()
        {
            InitializeComponent();
        }
        private void _LoadData()
        {
            ctrlFilterDriverLicense1.txtLicenseIDFoucs();
            lblAppDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            if(rbDamaged.Checked)
                lblAppFees.Text = clsAppTypes.FindType(_GetApplicationTypeID()).fees.ToString();
            else
                lblAppFees.Text = clsAppTypes.FindType(_GetApplicationTypeID()).fees.ToString();
            lblCreate.Text = clsGlobal.CurrentUser.userName;
        }

        private int _GetApplicationTypeID()
        {
            if (rbDamaged.Checked)
                return (int)clsApp.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApp.enApplicationType.ReplaceLostDrivingLicense;
        }

        private void frmReplacementLicense_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue a Replacement the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsLicense ReplaceLicens = ctrlFilterDriverLicense1.SelectedLicenseInfo.Replace((clsLicense.enIssueReason)_GetApplicationTypeID(), clsGlobal.CurrentUser.id);
                if (ReplaceLicens == null)
                {
                    MessageBox.Show("Faild to Replace this License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lblReplacedAppID.Text = ReplaceLicens.appID.ToString();
                lblReplacedLicense.Text = ReplaceLicens.licenseID.ToString();
                MessageBox.Show($"License Replaced Successfuly With ID = {lblReplacedLicense.Text} ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                gbReplacment.Enabled = false;
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

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamaged.Checked)
            {
                lblAppFees.Text = clsAppTypes.FindType(_GetApplicationTypeID()).fees.ToString();
                this.Text = "Replacement For Damaged License";
            } else
            {
                lblAppFees.Text = clsAppTypes.FindType(_GetApplicationTypeID()).fees.ToString();
                this.Text = "Replacement For Lost License";
            }
            label1.Text = this.Text;
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

            if (ctrlFilterDriverLicense1.SelectedLicenseInfo.IsDetainedLicense)
            {
                MessageBox.Show($"Selected License is Derained, you should Release this License First", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if (!ctrlFilterDriverLicense1.SelectedLicenseInfo.isActive)
            {
                MessageBox.Show($"Selected License isn't active", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            btnSave.Enabled = true;
        }
    }
}
