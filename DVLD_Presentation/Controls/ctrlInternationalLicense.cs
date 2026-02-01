using DVLD_Business;
using DVLD_Presentation.Properties;
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

namespace DVLD_Presentation.Controls
{
    public partial class ctrlInternationalLicense : UserControl
    {
        private clsInternationalLicense _internationalLicense;
        private int _LicenseID = -1;
        public int InternationalLicenseID
        {
            get
            {
                return _LicenseID;
            }
        }
        public ctrlInternationalLicense()
        {
            InitializeComponent();
        }

        private void _LoadLicenseData()
        {
            _LicenseID = _internationalLicense.InternationalID;
            lblIntID.Text = _LicenseID.ToString();
            lblId.Text = _internationalLicense.LocalLicenseID.ToString();
            lblName.Text = _internationalLicense.personInfo.fullName();
            lblNationalNo.Text = _internationalLicense.personInfo.NationalNo;
            if (_internationalLicense.personInfo.gendor == 0)
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
            lblIssueDate.Text = _internationalLicense.IssueDate.ToString("dd/MMM/yyyy");
            lblExipration.Text = _internationalLicense.ExpirationDate.ToString("dd/MMM/yyyy");
            lblBirth.Text = _internationalLicense.personInfo.date.ToString("dd/MMM/yyyy");
            lblDriverId.Text = _internationalLicense.DriverID.ToString();
            lblActive.Text = _internationalLicense.IsActive ? "Yes" : "No";
            lblApplication.Text = _internationalLicense.appID.ToString();
            if (_internationalLicense.personInfo.imagePath != "")
            {
                if (File.Exists(_internationalLicense.personInfo.imagePath))
                    pbPerson.Load(_internationalLicense.personInfo.imagePath);
            }
        }

        public bool LoadLicenseData(int LicenseId)
        {
            _internationalLicense = clsInternationalLicense.FindLicense(LicenseId);
            if (_internationalLicense != null)
            {
                _LoadLicenseData();
                return true;
            }
            return false;
        }
    }
}
