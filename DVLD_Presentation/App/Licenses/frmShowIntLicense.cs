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
    public partial class frmShowIntLicense : Form
    {
        private int _LicenseID = -1;
        public frmShowIntLicense(int licenseId)
        {
            InitializeComponent();
            _LicenseID = licenseId;
        }

        private void frmShowIntLicense_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicense1.LoadLicenseData(_LicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
