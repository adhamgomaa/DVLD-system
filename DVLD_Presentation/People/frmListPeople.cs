using DVLD_Business;
using DVLD_Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.People
{
    public partial class frmListPeople : Form
    {
        DataView dv;
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsPerson.GetPeople();
            dgvPeople.DataSource = dv;
            lblRecords.Text = dgvPeople.RowCount.ToString();
        }

        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("none");
            foreach (DataGridViewColumn Col in dgvPeople.Columns)
            {
                cbFilter.Items.Add(Col.HeaderText);
            }   
            cbFilter.SelectedIndex = 0;
        }

        private void _ApplyFilters()
        {
            string selectedColumn = cbFilter.SelectedItem.ToString();
            string keyword = tbFilter.Text.Trim();
            if (selectedColumn == "Person ID")
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
            else if (selectedColumn == "Date Of Birth")
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
            else {
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
            if (selectedColumn == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
            _LoadFiltersInBox();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            personInfo.ShowDialog();

        }

        private void dgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            showDetailsToolStripMenuItem_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addEdit = new frmAddEditPerson();
            addEdit.personData += _LoadDataOnDgv;
            addEdit.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addEdit = new frmAddEditPerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            addEdit.personData += _LoadDataOnDgv;
            addEdit.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show($"Are you sure remove Person ID = {(int)dgvPeople.CurrentRow.Cells[0].Value}", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if(clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadDataOnDgv();
                } else
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilter.Text != "none") ;
        } 
    }
}
