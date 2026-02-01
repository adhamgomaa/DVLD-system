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

namespace DVLD_Presentation.App.AppTypes
{
    public partial class frmAppsTypes : Form
    {
        DataView dv;
        public frmAppsTypes()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsAppTypes.GetAllTypes();
            dgvTypes.DataSource = dv;
            lblRecords.Text = dgvTypes.RowCount.ToString();
        }

        private void frmAppsTypes_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
        }

        private void editAplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateType updateType = new frmUpdateType((int)dgvTypes.CurrentRow.Cells[0].Value);
            updateType.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
