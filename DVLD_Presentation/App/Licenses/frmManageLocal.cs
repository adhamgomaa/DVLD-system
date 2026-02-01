using DVLD_Business;
using DVLD_Presentation.App.Tests;
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
    public partial class frmManageLocal : Form
    {
        DataView dv;
        public frmManageLocal()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsLocalDriving.GetAllLocalLicenses();
            dgvApp.DataSource = dv;
            lblRecords.Text = dgvApp.RowCount.ToString();
        }

        private void frmManageLocal_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }

        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("none");
            foreach (DataGridViewColumn Col in dgvApp.Columns)
            {
                cbFilter.Items.Add(Col.HeaderText);
            }
            cbFilter.SelectedIndex = 0;
        }
        private void _ApplyFilters()
        {
            string selectedColumn = cbFilter.SelectedItem.ToString();
            string keyword = tbFilter.Text.Trim();
            if (selectedColumn == "L.D.L.AppID" || selectedColumn == "Passed Tests")
            {
                if (int.TryParse(keyword, out int value))
                {
                    dv.RowFilter = $"[{selectedColumn}] = {value}";
                }
                else
                {
                    dv.RowFilter = "";
                }
            }
            else if (selectedColumn == "ApplicationDate")
            {
                if (DateTime.TryParse(keyword, out DateTime date))
                {
                    string formattedDate = date.ToString("MM/dd/yyyy");
                    dv.RowFilter = $"CONVERT([{selectedColumn}], 'System.DateTime') = #{formattedDate}#";
                }
                else
                {
                    dv.RowFilter = "";
                }
            }
            else
            {
                dv.RowFilter = string.Format("[{0}] LIKE '{1}%'", selectedColumn, keyword.Replace("''", "'"));
            }
        }
        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilters();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewLocal newLocal = new frmNewLocal();
            newLocal.ShowDialog();
            _LoadDataOnDgv();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            string selectedColumn = cbFilter.SelectedItem.ToString();
            if (selectedColumn == "L.D.L.AppID" || selectedColumn == "Passed Tests")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
            {
                tbFilter.Visible = false;
            }
            else
            {
                tbFilter.Visible = true;
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsLocalDriving.CancelLicense((int)dgvApp.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadDataOnDgv();
                } else
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVisionTest visionTest = new frmVisionTest(clsTestType.enTestType.VisionTest, (int)dgvApp.CurrentRow.Cells[0].Value);
            visionTest.ShowDialog();
            _LoadDataOnDgv();
        }

        private void dgvApp_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            int LocalDrivingLicenseAppID = (int)dgvApp.CurrentRow.Cells[0].Value;
            clsLocalDriving localDrivingLicenseApp = clsLocalDriving.FindLocalLicense(LocalDrivingLicenseAppID);

            int totalTestsPass = (int)dgvApp.CurrentRow.Cells[5].Value;

            bool LicenseExist = localDrivingLicenseApp.IsLicenseIssued();

            editApplicationToolStripMenuItem.Enabled = !LicenseExist && localDrivingLicenseApp.status == 1;
            deleteApplicationToolStripMenuItem.Enabled = localDrivingLicenseApp.status == 1;
            cancelApplicationToolStripMenuItem.Enabled = localDrivingLicenseApp.status == 1;

            bool PassedVisionTest = localDrivingLicenseApp.DosePassTestType(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = localDrivingLicenseApp.DosePassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = localDrivingLicenseApp.DosePassTestType(clsTestType.enTestType.StreetTest);

            sechduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && localDrivingLicenseApp.status == 1;
            if(sechduleTestsToolStripMenuItem.Enabled)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.
                visionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must passed the vision test and not passed the written test before.
                sechduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule street test, Person must passed the vision and written test and not passed the street test before.
                sechduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (totalTestsPass == 3) && !LicenseExist;
            showLicenseToolStripMenuItem.Enabled = LicenseExist && (totalTestsPass == 3);
        }

        private void sechduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVisionTest visionTest = new frmVisionTest(clsTestType.enTestType.WrittenTest, (int)dgvApp.CurrentRow.Cells[0].Value);
            visionTest.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVisionTest visionTest = new frmVisionTest(clsTestType.enTestType.StreetTest, (int)dgvApp.CurrentRow.Cells[0].Value);
            visionTest.ShowDialog();
            _LoadDataOnDgv();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueLicense issueLicense = new frmIssueLicense((int)dgvApp.CurrentRow.Cells[0].Value);
            issueLicense.ShowDialog();
            _LoadDataOnDgv();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLocalApplication localApplication = new frmShowLocalApplication((int)dgvApp.CurrentRow.Cells[0].Value);
            localApplication.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseInfo licenseInfo = new frmDriverLicenseInfo(clsLicense.GetLicenseID((int)dgvApp.CurrentRow.Cells[0].Value));
            licenseInfo.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicense license = clsLicense.FindLicense(clsLicense.GetLicenseID((string)dgvApp.CurrentRow.Cells[2].Value));
            if (license != null)
            {
                frmLicenseHistory licenseHistory = new frmLicenseHistory(license.driverID);
                licenseHistory.ShowDialog();
            } else
            {
                MessageBox.Show("There is no License for this person", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewLocal newLocal = new frmNewLocal((int)dgvApp.CurrentRow.Cells[0].Value);
            newLocal.ShowDialog();
            _LoadDataOnDgv();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                 clsLocalDriving localDrivingLicense = clsLocalDriving.FindLocalLicense((int)dgvApp.CurrentRow.Cells[0].Value);
                if (localDrivingLicense.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadDataOnDgv();
                }
                else
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
