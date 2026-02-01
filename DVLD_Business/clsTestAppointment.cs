using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestAppointment
    {
        enum enMode { Add, Update};
        enMode mode = enMode.Add;
        public int appointmentId {  get; private set; }
        public clsTestType.enTestType testTypeId { get; set; }
        public int localId { get; set; }
        public DateTime date { get; set; }
        public decimal fees { get; set; }
        public int userId { get; set; }
        public bool isLocked { get; set; }
        public int retake { get; set; }
        public clsApp retakeApplication { get; set; }

        public int TestID
        {
            get
            {
                return _GetTestID();
            }
        }

        private int _GetTestID() 
        {
            return clsTestAppointmentData.GetTestID(appointmentId);
        }

        public clsTestAppointment()
        {
            appointmentId = -1;
            testTypeId = clsTestType.enTestType.VisionTest;
            date = DateTime.Now;
            fees = 0;
            userId = -1;
            isLocked = false;
            retake = -1;
            mode = enMode.Add;
        }

        public clsTestAppointment(int appointmentId, clsTestType.enTestType testTypeId, int localId, DateTime date, decimal fees, int userId, bool isLocked, int retake)
        {
            this.appointmentId = appointmentId;
            this.testTypeId = testTypeId;
            this.localId = localId;
            this.date = date;
            this.fees = fees;
            this.userId = userId;
            this.isLocked = isLocked;
            this.retake = retake;
            retakeApplication = clsApp.FindApp(retake);
            mode = enMode.Update;
        }

        private bool _AddNewAppointment()
        {
            this.appointmentId = clsTestAppointmentData.AddNewAppointment((int)testTypeId, localId, date, fees, userId, isLocked, retake);
            return this.appointmentId != -1;
        }

        private bool _UpdateAppointment()
        {
            return clsTestAppointmentData.UpdateAppointment(appointmentId, (int)testTypeId, localId, date, fees, userId, isLocked);
        }

        public static DataView GetAllAppointment(int localId, int typeId)
        {
            return clsTestAppointmentData.GetAllAppointment(localId, typeId).DefaultView;
        }

        public static int GetTrials(int localId, int typeId)
        {
            return clsTestAppointmentData.GetTrials(localId, typeId);
        }

        public static clsTestAppointment FindAppointment(int appointmentId)
        {
            int typeId = -1, localId = -1, userId = -1;
            DateTime date = DateTime.Now;
            decimal fees = 0;
            bool isLocked = false;
            int retake = -1;
            if (clsTestAppointmentData.FindAppointment(appointmentId, ref typeId, ref localId, ref date, ref fees, ref userId, ref isLocked, ref retake))
                return new clsTestAppointment(appointmentId, (clsTestType.enTestType) typeId, localId, date, fees, userId, isLocked, retake);
            return null;
        }

        public static clsTestAppointment GetLastTestAppointment(int localId, clsTestType.enTestType testType)
        {
            int appointmentId = -1, userId = -1;
            DateTime date = DateTime.Now;
            decimal fees = 0;
            bool isLocked = false;
            int retake = -1;
            if (clsTestAppointmentData.GetLastTestAppointment(localId, (int)testType, ref appointmentId, ref date, ref fees, ref userId, ref isLocked, ref retake))
                return new clsTestAppointment(appointmentId, testType, localId, date, fees, userId, isLocked, retake);
            return null;
        }

        public static bool AppointmentIsLock(int localId, int typeId)
        {
            return clsTestAppointmentData.AppointmentIsLock(localId, typeId);
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if (_AddNewAppointment())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateAppointment();
            }
            return false;
        }

    }
}
