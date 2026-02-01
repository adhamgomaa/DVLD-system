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

namespace DVLD_Presentation.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestAppointment _AppointmentTest;
        private clsLocalDriving _LocalDriving;
        private int _LocalDrivingID = -1;
        private int _AppointmentTestID = -1;
        private int _TestID = -1;

        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;
                switch (_TestTypeID)
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

        public int TestAppointment
        {
            get { return _AppointmentTestID; }
        }

        public int TestID
        {
            get { return _TestID; }
        }
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int TestAppointment)
        {
            _AppointmentTestID = TestAppointment;
            _AppointmentTest = clsTestAppointment.FindAppointment(_AppointmentTestID);
            if (_AppointmentTest == null)
            {
                MessageBox.Show("Error: No  Appointment ID = " + _AppointmentTestID.ToString(),
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _AppointmentTestID = -1;
                return;
            }

            _TestID = _AppointmentTest.TestID;
            _LocalDrivingID = _AppointmentTest.localId;
            _LocalDriving = clsLocalDriving.FindLocalLicense(_LocalDrivingID);
            if (_LocalDriving == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingID.ToString(),
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LocalDrivingID = -1;
                return;
            }
            lblDrivingAppID.Text = _LocalDrivingID.ToString();
            lblClass.Text = _LocalDriving.LicenseClassInfo.className;
            lblName.Text = _LocalDriving.PersonFullName;
            lblTrial.Text = _LocalDriving.TotalTrialsPerTest(TestTypeID).ToString();
            lblDate.Text = _AppointmentTest.date.ToString("dd/MMM/yyyy");
            lblFees.Text = _AppointmentTest.fees.ToString();
            lblTestID.Text = (TestID == -1)? "Not Taken yet": TestID.ToString();
        }
    }
}
