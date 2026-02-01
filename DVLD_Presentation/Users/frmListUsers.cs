using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Users
{
    public partial class frmListUsers : Form
    {
        DataView dv;
        public frmListUsers()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsUser.GetUsers();
            dgvUsers.DataSource = dv;
            lblRecords.Text = dgvUsers.RowCount.ToString();
        }

        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("none");
            foreach (DataGridViewColumn Col in dgvUsers.Columns)
            {
                cbFilter.Items.Add(Col.HeaderText);
            }
            cbFilter.SelectedIndex = 0;
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
            else if(cbFilter.SelectedItem.ToString() == "IsActive")
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
                dv.RowFilter = "[IsActive] = true";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                dv.RowFilter = "[IsActive] = false";
            }
            else
            {
                dv.RowFilter = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUsers addUsers = new frmAddEditUsers();
            addUsers.ShowDialog();
            _LoadDataOnDgv();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            userInfo.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUsers EditUsers = new frmAddEditUsers((int)dgvUsers.CurrentRow.Cells[0].Value);
            EditUsers.ShowDialog();
            _LoadDataOnDgv();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure remove User ID = {(int)dgvUsers.CurrentRow.Cells[0].Value}", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (clsUser.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User Deleted Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadDataOnDgv();
                }
                else
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            changePassword.ShowDialog();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            userInfo.ShowDialog();
        }
    }
}
