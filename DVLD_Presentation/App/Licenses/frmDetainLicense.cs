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
    public partial class frmDetainLicense : Form
    {
        private int _licenseID = -1;
        private int _detainLicenseID = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
        }
        private void _LoadData()
        {
            ctrlFilterDriverLicense1.txtLicenseIDFoucs();
            lblDetainDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreate.Text = clsGlobal.CurrentUser.userName;
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
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
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _detainLicenseID = ctrlFilterDriverLicense1.SelectedLicenseInfo.Detain(Convert.ToDecimal(txtFees.Text), clsGlobal.CurrentUser.id);
                if(_detainLicenseID == -1)
                {
                    MessageBox.Show("Faild to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lblDetainID.Text = _detainLicenseID.ToString();
                MessageBox.Show($"License Detain Successfuly With ID = {_detainLicenseID} ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                ctrlFilterDriverLicense1.FilterEnabled = false;
                txtFees.Enabled = false;
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
            _licenseID = obj;
            if(_licenseID == -1)
            {
                MessageBox.Show($"Selected License isn't found", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            lblLicense.Text = _licenseID.ToString();
            linkLabel1.Enabled = (_licenseID != -1);
            if(ctrlFilterDriverLicense1.SelectedLicenseInfo.IsDetainedLicense)
            {
                MessageBox.Show($"Selected License is already Detained, choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            btnSave.Enabled = true;
        }
    }
}
