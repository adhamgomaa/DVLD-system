using DVLD_Business;
using DVLD_Presentation.People;
using DVLD_Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Controls
{
    public partial class ctrlPersonInfo : UserControl
    {
        private clsPerson _person;
        private int _PersonId = -1;
        public int PersonId
        {
            get { return _PersonId; }
        }
        public ctrlPersonInfo()
        {
            InitializeComponent();
        }

        private void _ResetPersonInfo()
        {
            _PersonId = -1;
            lblPersonId.Text = "[???]";
            lblName.Text = "[???]";
            lblNationalNo.Text = "[???]";
            pbGendor.BackgroundImage = Resources.patient_boy__1_;
            lblGendor.Text = "Male";
            lblEmail.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDate.Text = "[???]";
            lblPhone.Text = "[???]";
            lblCountry.Text = "[???]";
            pbPerson.Image = Resources.Male_512;
            linkLabel1.Enabled = false;
        }

        private void _LoadPersonInfo()
        {
            linkLabel1.Enabled = true;
            _PersonId = _person.id;
            lblPersonId.Text = _PersonId.ToString();
            lblName.Text = _person.fullName();
            lblNationalNo.Text = _person.NationalNo;
            if(_person.gendor == 0)
            {
                lblGendor.Text = "Male";
                pbGendor.BackgroundImage = Resources.patient_boy__1_;
                pbPerson.Image = Resources.Male_512;
            } else
            {
                lblGendor.Text = "Female";
                pbGendor.BackgroundImage = Resources.user_female;
                pbPerson.Image = Resources.Female_512;
            }
            lblEmail.Text = _person.email;
            lblAddress.Text = _person.address;
            lblDate.Text = _person.date.ToShortDateString();
            lblPhone.Text = _person.phone;
            lblCountry.Text = clsCountry.FindCountry(_person.NationaltyId).CountryName;

            if(_person.imagePath != "")
            {
                if(File.Exists(_person.imagePath))
                    pbPerson.ImageLocation = _person.imagePath;
            }
        }

        public void LoadPersonInfo(int id)
        {
            _person = clsPerson.findPerson(id);
            if (_person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No person with Person ID = " + id, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _LoadPersonInfo();
        }

        public void LoadPersonInfo(string nationalNo)
        {
            _person = clsPerson.findPerson(nationalNo);
            if (_person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No person with National Number = " + nationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _LoadPersonInfo();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson editPerson = new frmAddEditPerson(PersonId);
            editPerson.ShowDialog();
            editPerson.Person += LoadPersonInfo;
        }
    }
}
