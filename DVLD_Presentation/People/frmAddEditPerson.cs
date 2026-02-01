using DVLD_Business;
using DVLD_Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD_Presentation.Global_Classes;
namespace DVLD_Presentation.People
{
    public partial class frmAddEditPerson : Form
    {
        enum enMode { add, edit}
        enMode mode = enMode.add;
        private clsPerson _person;
        private int _PersonId = -1;
        
        public delegate void SendPersonData();
        public event SendPersonData personData;

        public delegate void SendPersonDataToUserData(int personId);
        public event SendPersonDataToUserData Person;

        public frmAddEditPerson()
        {
            InitializeComponent();
            mode = enMode.add;
        }

        public frmAddEditPerson(int personId)
        {
            InitializeComponent();
            _PersonId = personId;
            mode = enMode.edit;
        }

        private void _LoadCountryNameInBox()
        {
            DataView dv = clsCountry.GetCountries();
            for (int i = 0; i < dv.Count; i++)
            {
                cbCountry.Items.Add(dv[i][1]);
            }
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (mode == enMode.edit)
                AddEditPerson();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbPerson.ImageLocation == null)
                pbPerson.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPerson.ImageLocation == null)
                pbPerson.Image = Resources.Female_512;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Choose a picture";
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "Image files|*.jpg;*.png;*.jpeg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPerson.Load(openFileDialog1.FileName);
                linkLabel2.Visible = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPerson.ImageLocation = null;
            if (rbMale.Checked)
            {
                pbPerson.Image = Resources.Male_512;
            } else
            {
                pbPerson.Image = Resources.Female_512;
            }
            linkLabel2.Visible = false;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void Box_Validating(TextBox box, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(box.Text.Trim()) && box != txtEmail)
            {
                errorProvider1.SetError(box, "Required");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(box, "");
                e.Cancel = false;
            }
            if (box == txtEmail && !string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                if (!IsValidEmail(txtEmail.Text.Trim()))
                {
                    errorProvider1.SetError(box, "Invalid email, include an '@' in the email address");
                    e.Cancel = true;
                }
            }
            if(box == txtNationalNo)
            {
                if(_person.NationalNo != txtNationalNo.Text.Trim() && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
                {
                    errorProvider1.SetError(box, "National No. is already exists, please enter another National No.");
                    e.Cancel = true;
                }
            }
        }

        private void txtBox_Validating(object sender, CancelEventArgs e)
        {
            Box_Validating((TextBox)sender, e);
        }

        private bool _HandlePersonImage()
        {
            if (pbPerson.ImageLocation != _person.imagePath)
            {
                if (_person.imagePath != "")
                {
                    try
                    {
                        if(_person.imagePath != null)
                            File.Delete(_person.imagePath);
                    }
                    catch (IOException)
                    {

                    }
                }
                if (pbPerson.ImageLocation != null)
                {
                    string SourceFile = pbPerson.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceFile))
                    {
                        pbPerson.ImageLocation = SourceFile;
                        return true;
                    } else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private void _ResetDefualtValues()
        {
            _LoadCountryNameInBox();
            if(mode == enMode.add)
            {
                lblAddEdit.Text = "Add Person";
                _person = new clsPerson();
            } else
                lblAddEdit.Text = "Edit Person";

            if (rbMale.Checked)
            {
                pbPerson.Image = Resources.Male_512;
            }
            else
            {
                pbPerson.Image = Resources.Female_512;
            }
            dateTimePicker1.MaxDate = DateTime.Today.AddYears(-18);
            dateTimePicker1.MinDate = DateTime.Today.AddYears(-100);
            cbCountry.SelectedIndex = 9;
        }

        private void AddEditPerson()
        {          
            _person = clsPerson.findPerson(_PersonId);
            if (_person == null)
            {
                MessageBox.Show("This person is no longer present.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblPersonId.Text = _PersonId.ToString();
            txtFirst.Text = _person.fName;
            txtSecond.Text = _person.secName;
            txtThird.Text = _person.thName;
            txtLast.Text = _person.lName;
            txtNationalNo.Text = _person.NationalNo;
            if (_person.gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            txtEmail.Text = _person.email;
            txtPhone.Text = _person.phone;
            txtAddres.Text = _person.address;
            dateTimePicker1.Value = _person.date;
            cbCountry.SelectedIndex  = cbCountry.FindString(_person.countryInfo.CountryName);
            if(_person.imagePath != null)
            {
                pbPerson.ImageLocation = _person.imagePath;
                linkLabel2.Visible = true;
            }
            else
            {
                if (rbMale.Checked)
                {
                    pbPerson.Image = Resources.Male_512;
                }
                else
                {
                    pbPerson.Image = Resources.Female_512;
                }
                linkLabel2.Visible = false;
            }
        }

        private void _SaveData()
        {
            _person.fName = txtFirst.Text.Trim();
            _person.secName = txtSecond.Text.Trim();
            if (string.IsNullOrEmpty(txtThird.Text.Trim()))
                _person.thName = "";
            else
                _person.thName = txtThird.Text.Trim();
            _person.lName = txtLast.Text.Trim();
            _person.NationalNo = txtNationalNo.Text.Trim();
            if (rbMale.Checked)
                _person.gendor = 0;
            else
                _person.gendor = 1;
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                _person.email = "";
            else
                _person.email = txtEmail.Text.Trim();
            _person.address = txtAddres.Text.Trim();
            _person.date = dateTimePicker1.Value;
            _person.phone = txtPhone.Text.Trim();
            _person.NationaltyId = clsCountry.FindCountry(cbCountry.Text).CountryId;
            if (linkLabel2.Visible == true)
            {
                _person.imagePath = pbPerson.ImageLocation;
            }
            else
                _person.imagePath = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            _SaveData();
            if (_person.Save())
            {
                if(mode == enMode.add)
                    MessageBox.Show("Person Added Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Person Updated Successfuly!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                personData?.Invoke();
                Person?.Invoke(_person.id);
                this.Close();
            } else
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
