using DVLD_Business;
using DVLD_Presentation.Global_Classes;
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

namespace DVLD_Presentation.Controls
{
    public partial class ctrScheduleTest : UserControl
    {

        enum enMode { Add, Edit }
        private enMode _mode = enMode.Add;

        enum enCreationMode { FirstTime, RetakeTest}
        private enCreationMode _creationMode = enCreationMode.FirstTime;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private int _LocalDrivingLicenseAppID = -1;
        private clsLocalDriving _LocalDrivingLicenseApp;
        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;

        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            { 
                _TestTypeID = value;
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbAppointmentTest.Text = "Vision Test";
                            pictureBox1.Image = Resources.eye;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbAppointmentTest.Text = "Written Test";
                            pictureBox1.Image = Resources.exam;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbAppointmentTest.Text = "Street Test";
                            pictureBox1.Image = Resources.cars;
                            break;
                        }
                }
            }
        }

        public void LoadTest(int LocalDrivingLicenseID, int appointmentID = -1)
        {
            if (appointmentID == -1)
                _mode = enMode.Add;
            else
                _mode = enMode.Edit;

            _LocalDrivingLicenseAppID = LocalDrivingLicenseID;
            _TestAppointmentID = appointmentID;
            _LocalDrivingLicenseApp = clsLocalDriving.FindLocalLicense(LocalDrivingLicenseID);
            if (_LocalDrivingLicenseApp == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseAppID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if (_LocalDrivingLicenseApp.DoseAttendTestType(_TestTypeID))
                _creationMode = enCreationMode.RetakeTest;
            else
                _creationMode = enCreationMode.FirstTime;

            if(_creationMode == enCreationMode.RetakeTest)
            {
                lblAppFees.Text = clsAppTypes.FindType((int)clsApp.enApplicationType.RetakeTest).fees.ToString();
                gbRetake.Enabled = true;
                lblTest.Text = "Retake Schedule Test";
                lblRAppId.Text = "0";
                _TestAppointment = clsTestAppointment.GetLastTestAppointment(_LocalDrivingLicenseAppID, _TestTypeID);
                dateTimePicker1.MinDate = _TestAppointment.date.AddDays(1);
            }
            else
            {
                gbRetake.Enabled = false;
                lblRAppId.Text = "N/A";
                lblTest.Text = "Schedule Test";
                lblAppFees.Text = "0";
                dateTimePicker1.MinDate = DateTime.Now;
            }

            lblDrivingAppID.Text = _LocalDrivingLicenseAppID.ToString();
            lblClass.Text = _LocalDrivingLicenseApp.LicenseClassInfo.className;
            lblName.Text = _LocalDrivingLicenseApp.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApp.TotalTrialsPerTest(_TestTypeID).ToString();
            lblFees.Text = clsTestType.FindType(_TestTypeID).fees.ToString();
            lblRAppId.Text = "N/A";

            if(_mode == enMode.Add)
            {
                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotal.Text = (Convert.ToDecimal(lblFees.Text) + Convert.ToDecimal(lblAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointment())
                return;
            if(!_HandleAppointmentLocked())
                return;
            if (!_HandlePrviousTest())
                return;
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.FindAppointment(_TestAppointmentID);
            if(_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.fees.ToString();
            
            dateTimePicker1.MinDate = _TestAppointment.date;
            dateTimePicker1.Value = _TestAppointment.date;
            if(_TestAppointment.retake == -1)
            {
                lblRAppId.Text = "N/A";
                lblAppFees.Text = "0";
            } else
            {
                gbRetake.Enabled = true;
                lblTest.Text = "Retake Schedule Test";
                lblAppFees.Text = _TestAppointment.retakeApplication.fees.ToString();
                lblRAppId.Text = _TestAppointment.retake.ToString();
            }
            return true;
        }

        private bool _HandleActiveTestAppointment()
        {
            if(_mode == enMode.Add && clsLocalDriving.IsThereAnActiveScheduleTest(_LocalDrivingLicenseAppID, _TestTypeID))
            {
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                lblLocked.Visible = true;
                lblLocked.Text = "Person Already have an active appointment for this test";
                return false;
            }
            lblLocked.Visible = false;
            return true;
        }

        private bool _HandleAppointmentLocked()
        {
            if(_TestAppointment.isLocked)
            {
                lblLocked.Visible = true;
                lblLocked.Text = "Person already sat for the test, appointment loacked.";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                return false;
            }
            lblLocked.Visible = false;
            return true;
        }

        private bool _HandlePrviousTest()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblLocked.Visible = false;
                    return true;
                case clsTestType.enTestType.WrittenTest:
                    {
                        if(!_LocalDrivingLicenseApp.DosePassTestType(clsTestType.enTestType.VisionTest))
                        {
                            lblLocked.Visible = true;
                            lblLocked.Text = "Cannot Sechule, Vision Test should be passed first";
                            btnSave.Enabled = false;
                            dateTimePicker1.Enabled = false;
                            return false;
                        } else
                        {
                            lblLocked.Visible = false;
                            btnSave.Enabled = true;
                            dateTimePicker1.Enabled = true;
                        }
                        return true;
                    }
                case clsTestType.enTestType.StreetTest:
                    if (!_LocalDrivingLicenseApp.DosePassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblLocked.Visible = true;
                        lblLocked.Text = "Cannot Sechule, Written Test should be passed first";
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblLocked.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;
                    }
                    return true;
            }
            return true;
        }
        public ctrScheduleTest()
        {
            InitializeComponent();
        }

        private bool _HandleRetakeApp()
        {
            if (_mode == enMode.Add && _creationMode == enCreationMode.RetakeTest)
            {
                clsApp RetakeApp = new clsApp();
                RetakeApp.personId = _LocalDrivingLicenseApp.personId;
                RetakeApp.date = DateTime.Now;
                RetakeApp.types = (int)clsApp.enApplicationType.RetakeTest;
                RetakeApp.status = 3;
                RetakeApp.statusDate = DateTime.Now;
                RetakeApp.fees = clsAppTypes.FindType((int)clsApp.enApplicationType.RetakeTest).fees;
                RetakeApp.userId = clsGlobal.CurrentUser.id;
                if(RetakeApp.Save())
                {
                    _TestAppointment.retake = RetakeApp.appID;
                } else
                {
                    _TestAppointment.retake = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void DataSave()
        {
            _TestAppointment.date = dateTimePicker1.Value;
            _TestAppointment.fees = Convert.ToDecimal(lblFees.Text);
            _TestAppointment.localId = _LocalDrivingLicenseApp.LocalId;
            _TestAppointment.userId = clsGlobal.CurrentUser.id;
            _TestAppointment.testTypeId = _TestTypeID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_HandleRetakeApp())
                return;
            DataSave();
            if(_TestAppointment.Save())
            {
                _mode = enMode.Edit;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
