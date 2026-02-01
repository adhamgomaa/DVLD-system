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

namespace DVLD_Presentation.Drivers
{
    public partial class frmListDrivers : Form
    {
        DataView dv;
        public frmListDrivers()
        {
            InitializeComponent();
        }
        private void _LoadDataOnDgv()
        {
            dv = clsDriver.ListDrivers();
            dgvDrivers.DataSource = dv;
            lblRecords.Text = dgvDrivers.RowCount.ToString();
        }

        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("none");
            foreach (DataGridViewColumn Col in dgvDrivers.Columns)
            {
                if(Col.HeaderText == "Date")
                    continue;
                cbFilter.Items.Add(Col.HeaderText);
            }
            cbFilter.SelectedIndex = 0;
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
            }
            else
            {
                tbFilter.Visible = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo((int)dgvDrivers.CurrentRow.Cells[1].Value);
            personInfo.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory((int)dgvDrivers.CurrentRow.Cells[0].Value);
            licenseHistory.ShowDialog();
        }
    }
}
