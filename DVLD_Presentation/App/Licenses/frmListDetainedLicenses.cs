using DVLD_Business;
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

namespace DVLD_Presentation.App.Licenses
{
    public partial class frmListDetainedLicenses : Form
    {
        DataView dv;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }
        private void _LoadDataOnDgv()
        {
            dv = clsDetainLicense.GetDetainedLicenses();
            dgvDetain.DataSource = dv;
            lblRecords.Text = dgvDetain.RowCount.ToString();
        }
        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("none");
            foreach (DataGridViewColumn Col in dgvDetain.Columns)
            {
                if (Col.HeaderText == "D.Date" || Col.HeaderText == "Release Date")
                    continue;
                cbFilter.Items.Add(Col.HeaderText);
            }
            cbFilter.SelectedIndex = 0;
        }
        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }
        private void _LoadFiltersInIsActive()
        {
            comboBox1.Items.Add("All");
            comboBox1.Items.Add("Yes");
            comboBox1.Items.Add("No");
            comboBox1.SelectedIndex = 0;
        }

        private bool isNumericColumns(string selectedColumn)
        {
            return dv.ToTable().Columns[selectedColumn].DataType == typeof(int);
        }

        private void _ApplyFilters()
        {
            string selectedColumn = cbFilter.SelectedItem.ToString();
            string keyword = tbFilter.Text.Trim();
            if (isNumericColumns(selectedColumn))
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
            else
            {
                dv.RowFilter = string.Format("[{0}] LIKE '{1}%'", selectedColumn, keyword.Replace("''", "'"));
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilters();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            string selectedColumn = cbFilter.SelectedItem.ToString();
            if (isNumericColumns(selectedColumn))
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
                comboBox1.Visible = false;
            }
            else if (cbFilter.SelectedItem.ToString() == "Is Released")
            {
                tbFilter.Visible = false;
                comboBox1.Visible = true;
                _LoadFiltersInIsActive();
            }
            else
            {
                tbFilter.Visible = true;
                comboBox1.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                dv.RowFilter = "[Is Released] = true";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                dv.RowFilter = "[Is Released] = false";
            }
            else
            {
                dv.RowFilter = "";
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense detainLicense = new frmDetainLicense();
            detainLicense.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseLicense releaseLicense = new frmReleaseLicense();
            releaseLicense.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo(clsPerson.findPerson((string)dgvDetain.CurrentRow.Cells[6].Value).id);
            personInfo.ShowDialog();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriverLicenseInfo licenseInfo = new frmDriverLicenseInfo((int)dgvDetain.CurrentRow.Cells[1].Value);
            licenseInfo.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(clsLicense.FindLicense((int)dgvDetain.CurrentRow.Cells[1].Value).driverID);
            licenseHistory.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmReleaseLicense releaseLicense = new frmReleaseLicense((int)dgvDetain.CurrentRow.Cells[1].Value);
            releaseLicense.ShowDialog();
            _LoadDataOnDgv();
        }

        private void dgvDetain_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            toolStripMenuItem1.Enabled = !(bool)dgvDetain.CurrentRow.Cells[3].Value;
        }
    }
}
