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

namespace DVLD_Presentation.App.Licenses
{
    public partial class frmLicenseHistory : Form
    {
        private int _driverId = -1;

        public frmLicenseHistory()
        {
            InitializeComponent();
        }
        public frmLicenseHistory(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            if(_driverId != -1)
            {
                clsDriver driver = clsDriver.FindDriverWithDriverID(_driverId);
                if(driver == null)
                {
                    MessageBox.Show("There is no driver with Id = " + _driverId, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ctrlFilterPersonInfo1.LoadPersonInfo(driver.personID);
                ctrlFilterPersonInfo1.FilterEnabled = false;
                ctrlLicenses1.LoadData(_driverId);
            } else
            {
                ctrlFilterPersonInfo1.FilterEnabled = true;
                ctrlFilterPersonInfo1.FilterFoucs();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlFilterPersonInfo1_OnPersonSelected(int obj)
        {
            int _personId = obj;
            if(_personId != -1)
            {
               clsDriver driver = clsDriver.FindDriver(_personId);
                if(driver != null)
                {
                    ctrlLicenses1.LoadData(driver.driverID);

                }
            }
        }
    }
}
