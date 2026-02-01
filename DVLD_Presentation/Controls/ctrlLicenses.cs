 using DVLD_Business;
using DVLD_Presentation.App.Licenses;
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
    public partial class ctrlLicenses : UserControl
    {
        public ctrlLicenses()
        {
            InitializeComponent();
        }

        private void LoadDataOnDgvInternational(int driverId)
        {
            DataView dv = clsInternationalLicense.GetInternationalLicensesHistory(driverId);
            dgvInt.DataSource = dv;
            lblRecordsInt.Text = dgvInt.RowCount.ToString();
        }

        private void LoadDataOnDgvLocal(int driverId)
        {
            DataView dv = clsLicense.GetLocalLicensesHistory(driverId);
            dgvLocal.DataSource = dv;
            lblRecordsLocal.Text = dgvLocal.RowCount.ToString();
        }

        public void LoadData(int driverId)
        {
            LoadDataOnDgvLocal(driverId);
            LoadDataOnDgvInternational(driverId);
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseInfo licenseInfo = new frmDriverLicenseInfo((int)dgvLocal.CurrentRow.Cells[0].Value);
            licenseInfo.ShowDialog();
        }

        private void showInternationlLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowIntLicense intLicense = new frmShowIntLicense((int)dgvInt.CurrentRow.Cells[2].Value);
            intLicense.ShowDialog();
        }
    }
}
