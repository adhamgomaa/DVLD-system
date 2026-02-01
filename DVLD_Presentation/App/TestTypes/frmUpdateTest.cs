using DVLD_Business;
using DVLD_Presentation.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.App.TestTypes
{
    public partial class frmUpdateTest : Form
    {
        clsTestType type;
        private clsTestType.enTestType _TypeID = clsTestType.enTestType.VisionTest;
        public frmUpdateTest(clsTestType.enTestType typeID)
        {
            InitializeComponent();
            _TypeID = typeID;
        }

        private void _loadData()
        {
            type = clsTestType.FindType(_TypeID);
            lblTypeID.Text = _TypeID.ToString();
            txtTitle.Text = type.title;
            txtDesc.Text = type.description;
            txtFees.Text = type.fees.ToString();
        }

        private void frmUpdateTest_Load(object sender, EventArgs e)
        {
            _loadData();
        }

        private void _SaveData()
        {
            type.title = txtTitle.Text;
            type.description = txtDesc.Text;
            type.fees = Convert.ToDecimal(txtFees.Text);
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title can not be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtDesc_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDesc.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDesc, "Title can not be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtDesc, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees can not be empty");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, null);
            }

            if (!clsValidation.IsNumber(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _SaveData();
            if (type.Save())
            {
                MessageBox.Show("Type Updated Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
