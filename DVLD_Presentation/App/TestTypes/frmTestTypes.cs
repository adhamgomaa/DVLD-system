using DVLD_Business;
using DVLD_Presentation.App.AppTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.App.TestTypes
{
    public partial class frmTestTypes : Form
    {
        DataView dv;
        public frmTestTypes()
        {
            InitializeComponent();
        }

        private void _LoadDataOnDgv()
        {
            dv = clsTestType.GetAllTypes();
            dgvTypes.DataSource = dv;
            lblRecords.Text = dgvTypes.RowCount.ToString();
        }

        private void frmTestTypes_Load(object sender, EventArgs e)
        {
            _LoadDataOnDgv();
        }

        private void editAplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTest updateType = new frmUpdateTest((clsTestType.enTestType)dgvTypes.CurrentRow.Cells[0].Value);
            updateType.ShowDialog();
            _LoadDataOnDgv();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
