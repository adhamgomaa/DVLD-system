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
    public partial class frmShowInternationalApplication : Form
    {
        DataView dv;
        public frmShowInternationalApplication()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsInternationalLicense.GetAllLicenses();
            dgvApp.DataSource = dv;
            lblRecords.Text = dgvApp.RowCount.ToString();
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
            else if (isDateTimeColumns(selectedColumn))
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

        private bool isNumericColumns(string selectedColumn)
        {
            return dv.ToTable().Columns[selectedColumn].DataType == typeof(int);
        }
        private bool isDateTimeColumns(string selectedColumn)
        {
            return dv.ToTable().Columns[selectedColumn].DataType == typeof(DateTime);
        }
        private void _LoadFiltersInIsActive()
        {
            comboBox1.Items.Add("All");
            comboBox1.Items.Add("Yes");
            comboBox1.Items.Add("No");
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                dv.RowFilter = "[Is Active] = true";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                dv.RowFilter = "[Is Active] = false";
            }
            else
            {
                dv.RowFilter = "";
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
            {
                tbFilter.Visible = false;
                comboBox1.Visible = false;
            }
            else if (cbFilter.SelectedItem.ToString() == "Is Active")
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

        private void frmShowInternationalApplication_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo(clsApp.FindApp((int)dgvApp.CurrentRow.Cells[1].Value).personId);
            personInfo.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowIntLicense showIntLicense = new frmShowIntLicense((int)dgvApp.CurrentRow.Cells[0].Value);
            showIntLicense.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewInternational newInternational = new frmNewInternational();
            newInternational.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(clsLicense.FindLicense((int)dgvApp.CurrentRow.Cells[2].Value).driverID);
            licenseHistory.ShowDialog();
        }
    }
}
