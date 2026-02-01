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

namespace DVLD_Presentation.App.Tests
{
    public partial class frmVisionTest : Form
    {
        clsTestType.enTestType _testTypeID = clsTestType.enTestType.VisionTest;
        private int _LocalId = -1;
        private clsTestAppointment _appointment;
        DataView dv;
        public frmVisionTest(clsTestType.enTestType testTypeID, int LocalId)
        {
            InitializeComponent();
            _LocalId = LocalId;
            this._testTypeID = testTypeID;
        }

        private void _LoadDataOnDgv()
        {
            dv = clsTestAppointment.GetAllAppointment(_LocalId, (int)_testTypeID);
            dgvAppointment.DataSource = dv;
            lblRecords.Text = dgvAppointment.RowCount.ToString();
        }

        private void _LoadData()
        {
            switch (_testTypeID)
            {
                case clsTestType.enTestType.WrittenTest:
                    pictureBox1.Image = Resources.exam;
                    lblTitle.Text = "Written Test Appointments";
                    break;
                case clsTestType.enTestType.StreetTest:
                    pictureBox1.Image = Resources.cars;
                    lblTitle.Text = "Street Test Appointments";
                    break;
            } 
            this.Text = lblTitle.Text;
        }

        private void frmVisionTest_Load(object sender, EventArgs e)
        {
            ctrlApplicationInfo1.LoadApplicationInfo(_LocalId);
            _LoadDataOnDgv();
            _LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsLocalDriving localDriving = clsLocalDriving.FindLocalLicense(_LocalId);
            if(localDriving.IsThereAnActiveScheduleTest(_testTypeID))
            {
                MessageBox.Show("This person Already have an active appointment for this test, you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest lastTest = localDriving.GetLastTest(_testTypeID);
            if(lastTest == null)
            {
                frmScheduleVisionTest test1 = new frmScheduleVisionTest(_testTypeID, _LocalId);
                test1.ShowDialog();
                frmVisionTest_Load(sender, e);
                return;
            }

            if(lastTest.result)
            {
                MessageBox.Show("This person Already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleVisionTest test2 = new frmScheduleVisionTest(_testTypeID, _LocalId);
            test2.ShowDialog();
            frmVisionTest_Load(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleVisionTest scheduleTest = new frmScheduleVisionTest(_testTypeID, _LocalId, (int)dgvAppointment.CurrentRow.Cells[0].Value);
            scheduleTest.ShowDialog();
            frmVisionTest_Load(sender, e);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest takeTest = new frmTakeTest(_testTypeID, (int)dgvAppointment.CurrentRow.Cells[0].Value);
            takeTest.ShowDialog();
            frmVisionTest_Load(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
