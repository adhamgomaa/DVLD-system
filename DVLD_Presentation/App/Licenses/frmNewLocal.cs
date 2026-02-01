using DVLD_Business;
using DVLD_Presentation.Global_Classes;
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
    public partial class frmNewLocal : Form
    {
        enum enMode { New, Edit}
        enMode mode = enMode.New;
        enum enStatus { New = 1, Cancel, Complete}
        DataView dv;
        clsLocalDriving _localDriving;
        private int _LocalID = -1;
        public frmNewLocal()
        {
            InitializeComponent();
            mode = enMode.New;
        }

        public frmNewLocal(int localID)
        {
            InitializeComponent();
            this._LocalID = localID;
            mode = enMode.Edit;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(mode == enMode.Edit)
            {
                btnSave.Enabled = true;
                tabPage2.Enabled = true;
                tabControl1.SelectTab(1);
                return;
            }

            if(ctrlFilterPersonInfo1.PersonId != -1)
            {
                btnSave.Enabled = true;
                tabPage2.Enabled = true;
                tabControl1.SelectTab(1);
            }
        }

        private void _ResetDefaultData()
        {
            lblFees.Text = clsAppTypes.FindType((int)clsApp.enApplicationType.NewDrivingLicense).fees.ToString();
            dv = clsLicenseClass.GetAllClasses();
            for (int i = 0; i < dv.Count; i++)
            {
                cbClass.Items.Add(dv[i][1]);
            }

            if (mode == enMode.New)
            {
                lblAddEdit.Text = "New Local Driving License Application";
                _localDriving = new clsLocalDriving();
                tabPage2.Enabled = false;
                ctrlFilterPersonInfo1.FilterFoucs();
                lblAppDate.Text = DateTime.Now.ToShortDateString();
                lblCreate.Text = clsGlobal.CurrentUser.userName;
                cbClass.SelectedIndex = 2;
            } else
            {
                lblAddEdit.Text = "Update Local Driving License Application";
                btnSave.Enabled = true;
                tabPage2.Enabled = true;
            }
            this.Text = lblAddEdit.Text;
        }

        private void _LoadApplicationInfo()
        {
            ctrlFilterPersonInfo1.FilterEnabled = false;
            _localDriving = clsLocalDriving.FindLocalLicense(_LocalID);
            ctrlFilterPersonInfo1.ctrlPersonInfo1.LoadPersonInfo(_localDriving.personId);  
            lblAppDate.Text = _localDriving.date.ToShortDateString();
            lblCreate.Text = clsUser.FindUser(_localDriving.userId).userName;
            cbClass.SelectedIndex = _localDriving.classId - 1;
            lblAppID.Text = _localDriving.appID.ToString();
        }

        private void frmNewLocal_Load(object sender, EventArgs e)
        {
            _ResetDefaultData();
            if(mode == enMode.Edit)
                _LoadApplicationInfo();
        }

        private void _SaveData()
        {
            _localDriving.personId = ctrlFilterPersonInfo1.PersonId;
            _localDriving.date = DateTime.Now;
            _localDriving.types = 1;
            _localDriving.status = (int)enStatus.New;
            _localDriving.statusDate = DateTime.Now;
            _localDriving.fees = clsAppTypes.FindType(_localDriving.types).fees;
            _localDriving.userId = clsGlobal.CurrentUser.id;
            _localDriving.classId = clsLicenseClass.FindClasses(cbClass.Text).id;
        }

        private bool _IsPersonAgeMatchTheLicenseClass()
        {
            clsLicenseClass licenseClass = clsLicenseClass.FindClasses(cbClass.Text);
            DateTime minAge = DateTime.Now.AddYears(-licenseClass.age);
            if (clsPerson.findPerson(ctrlFilterPersonInfo1.PersonId).date > minAge)
                return true;
            return false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int classId = clsLicenseClass.FindClasses(cbClass.Text).id;
            int personId = ctrlFilterPersonInfo1.PersonId;
            if(personId == -1)
            {
                MessageBox.Show("Please choose a person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_IsPersonAgeMatchTheLicenseClass())
            {
                MessageBox.Show("Choose another License Class, the selected person is under allowed of age", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clsLocalDriving.CheckPersonHasSameClass(personId, classId))
            {
                MessageBox.Show("Choose another License Class, the selected person already have an active application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _SaveData();
            if(_localDriving.Save())
            {
                lblAppID.Text = _localDriving.LocalId.ToString();
                mode = enMode.Edit;
                lblAddEdit.Text = "Update Local Driving License Application";
                this.Text = lblAddEdit.Text;
                ctrlFilterPersonInfo1.FilterEnabled = false;
                MessageBox.Show("Data Saved Successfuly!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
