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

namespace DVLD_Presentation.App.Tests
{
    public partial class frmTakeTest : Form
    {
        private clsTestType.enTestType _testTypeId = clsTestType.enTestType.VisionTest;
        private int _AppointmentId; 
        private clsTestAppointment _Appointment;
        private clsTest _test;
        public frmTakeTest(clsTestType.enTestType testTypeId, int appointmentId)
        {
            InitializeComponent();
            _AppointmentId = appointmentId;
            this._testTypeId = testTypeId;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _testTypeId;
            ctrlScheduledTest1.LoadInfo(_AppointmentId);
            if(ctrlScheduledTest1.TestAppointment == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
            int testId = ctrlScheduledTest1.TestID;
            if(testId != -1)
            {
                _test = clsTest.FindTest(_AppointmentId);
                if(_test.result)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                textBox1.Text = _test.notes;
                lblMessage.Visible = true;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                _test = new clsTest();
                _Appointment = clsTestAppointment.FindAppointment(_AppointmentId);
            }
        }

        private void _SaveData()
        {
            if (rbPass.Checked)
                _test.result = true;
            else
                _test.result = false;
            _test.notes = textBox1.Text.Trim();
            _test.appointmentID = _AppointmentId;
            _test.userId = clsGlobal.CurrentUser.id;
            _Appointment.isLocked = true;
            _Appointment.Save();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _SaveData();
            if (_test.Save())
            {
                MessageBox.Show("Data Saved Successfuly!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                this.Close(); 
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
