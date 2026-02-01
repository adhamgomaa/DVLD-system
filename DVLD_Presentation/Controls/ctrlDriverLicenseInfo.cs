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
using System.IO;
using DVLD_Presentation.Properties;

namespace DVLD_Presentation.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private clsLicense _License;
        private int _LicenseID = -1;
        public int LicenseID
        {
            get
            {
                return _LicenseID;
            }
        }

        public clsLicense LicenseInfo
        {
            get
            {
                return _License;
            }
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadLicenseData()
        {
            _LicenseID = _License.licenseID;
            lblClass.Text = _License.licenseClassInfo.className;
            lblId.Text = _LicenseID.ToString();
            lblName.Text = _License.driverInfo.PersonInfo.fullName();
            lblNationalNo.Text = _License.driverInfo.PersonInfo.NationalNo;
            if (_License.driverInfo.PersonInfo.gendor == 0)
            {
                lblGendor.Text = "Male";
                pbGendor.BackgroundImage = Resources.patient_boy__1_;
                pbPerson.Image = Resources.Male_512;
            }
            else
            {
                lblGendor.Text = "Female";
                pbGendor.BackgroundImage = Resources.user_female;
                pbPerson.Image = Resources.Female_512;
            }
            lblIssueDate.Text = _License.issueDate.ToString("dd/MMM/yyyy");
            lblExipration.Text = _License.expiredDate.ToString("dd/MMM/yyyy");
            lblBirth.Text = _License.driverInfo.PersonInfo.date.ToString("dd/MMM/yyyy");
            lblReason.Text = _License.IssueReasonString;
            lblDriverId.Text = _License.driverID.ToString();
            lblNotes.Text = _License.notes == ""? "No Notes": _License.notes;
            lblActive.Text = _License.isActive? "Yes" : "No";
            lblDetain.Text = _License.IsDetainedLicense ? "Yes" : "No";
            if (_License.driverInfo.PersonInfo.imagePath != "")
            {
                if (File.Exists(_License.driverInfo.PersonInfo.imagePath))
                    pbPerson.Load(_License.driverInfo.PersonInfo.imagePath);
            }
        }

        public bool LoadLicenseData(int LicenseId)
        {
            _License = clsLicense.FindLicense(LicenseId);
            if (_License != null)
            {
                _LoadLicenseData();
                return true;
            }
            return false;
        }
    }
}
