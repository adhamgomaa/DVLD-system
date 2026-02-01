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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Presentation.App.Tests
{
    public partial class frmScheduleVisionTest : Form
    {
        clsTestType.enTestType _testTypeID = clsTestType.enTestType.VisionTest;
        private int _appointmentId = -1;
        private int _LocalId = -1;

        public frmScheduleVisionTest(clsTestType.enTestType testTypeID, int localId, int appointmentId = -1)
        {
            InitializeComponent();
            this._testTypeID = testTypeID;
            this._appointmentId = appointmentId;
            this._LocalId = localId;
        }

        private void frmScheduleVisionTest_Load(object sender, EventArgs e)
        {
            ctrScheduleTest1.TestTypeID = _testTypeID;
            ctrScheduleTest1.LoadTest(_LocalId, _appointmentId);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
