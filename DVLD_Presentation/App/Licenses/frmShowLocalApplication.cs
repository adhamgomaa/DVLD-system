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
    public partial class frmShowLocalApplication : Form
    {
        private int _LocalId = -1;
        public frmShowLocalApplication(int localId)
        {
            InitializeComponent();
            _LocalId = localId;
        }

        private void frmShowLocalApplication_Load(object sender, EventArgs e)
        {
            ctrlApplicationInfo1.LoadApplicationInfo(_LocalId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
