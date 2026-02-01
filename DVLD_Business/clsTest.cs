using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTest
    {
        enum enMode { Add, Update };
        enMode mode = enMode.Add;

        public int TestId { get; private set; }
        public int appointmentID {  get; set; }
        public clsTestAppointment appointmentInfo { get; set; }
        public bool result {  get; set; }
        public string notes { get; set; }
        public int userId { get; set; }

        public clsTest()
        {
            TestId = -1;
            appointmentID = -1;
            result = false;
            notes = string.Empty;
            userId = -1;
            mode = enMode.Add;
        }

        public clsTest(int testId, int appointmentId, bool result, string notes, int userId)
        {
            this.TestId = testId;
            this.appointmentID = appointmentId;
            this.result = result;
            this.notes = notes;
            this.userId = userId;
            appointmentInfo = clsTestAppointment.FindAppointment(appointmentId);
            mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            this.TestId = clsTestData.AddNewTest(appointmentID, result, notes, userId);
            return this.TestId != -1;
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(TestId, appointmentID, result, notes, userId);
        }

        public static clsTest FindTest(int appointmentId)
        {
            int testId = -1, userId = -1;
            string notes = string.Empty;
            bool result = false;
            if(clsTestData.FindTest(appointmentId, ref testId, ref result, ref notes, ref userId))
                return new clsTest(testId, appointmentId, result, notes, userId);
            else
                return null;
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if(_AddNewTest())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }
    }
}
