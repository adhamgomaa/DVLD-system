using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLocalDriving : clsApp
    {
        enum enMode { add, update}
        enMode mode = enMode.add;
        public int LocalId { get; private set; }
        public int classId { get; set; }
        public clsLicenseClass LicenseClassInfo;

        public string PersonFullName
        {
            get
            {
                return base.personInfo.fullName();
            }
        }

        public clsLocalDriving()
        {
            LocalId = -1;
            classId = -1;
            mode = enMode.add;
        }

        private clsLocalDriving(int localId, int appId, int personId, DateTime date, int types, byte status, DateTime statusDate, 
            decimal fees, int userId, int classId) : base(appId, personId, date,  types, status, statusDate, fees, userId)
        {
            LocalId = localId;
            this.classId = classId;
            LicenseClassInfo = clsLicenseClass.FindClasses(classId);
            mode = enMode.update;
        }

        private bool _AddNewLocal()
        {
            this.LocalId = clsLocalDrivingData.AddNewLocal(base.appID, classId);
            return this.LocalId != -1;
        }

        private bool _UpdateLocal()
        {
            return clsLocalDrivingData.UpdateLocal(LocalId, base.appID, classId);
        }

        public static bool CheckPersonHasSameClass(int personId, int classId)
        {
            return clsLocalDrivingData.CheckPersonHasSameClass(personId, classId);
        }

        public static DataView GetAllLocalLicenses()
        {
            return clsLocalDrivingData.GetAllLocalLicenses().DefaultView;
        }

        public static bool CancelLicense(int localLicenseAppID)
        {
            return clsLocalDrivingData.CancelLicense(localLicenseAppID);
        }

        public static clsLocalDriving FindLocalLicense(int localLicenseAppId)
        {
            int appId = -1, classId = -1;
            if (clsLocalDrivingData.FindLocalLicense(localLicenseAppId, ref appId, ref classId))
            {
                clsApp app = clsApp.FindApp(appId);
                return new clsLocalDriving(localLicenseAppId, app.appID, app.personId, app.date, app.types, app.status, app.statusDate, 
                    app.fees, app.userId, classId);
            }
            return null;
        }

        public clsTest GetLastTest(clsTestType.enTestType testType)
        {
            int testId = -1, appointmentId = -1, userId = -1;
            bool result = false;
            string notes = "";
            if(clsLocalDrivingData.GetLastTest(this.LocalId, (int)testType, ref testId, ref appointmentId, ref result, ref notes, ref userId))
                return new clsTest(testId, appointmentId, result, notes, userId);
            return null;
        }

        public new bool Delete()
        {
            if(!clsLocalDrivingData.DeleteLocal(this.LocalId))
                return false;
            return base.Delete();
        }

        public new bool Save()
        {
            if(!base.Save()) return false;

            switch(mode)
            {
                case enMode.add:
                    if(_AddNewLocal())
                    {
                        mode = enMode.update;
                        return true;
                    }
                    else
                        return false;
                case enMode.update:
                    return _UpdateLocal();
            }
            return false;
        }

        public bool DoseAttendTestType(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingData.DoseAttendTestType(this.LocalId, (int)testTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingData.TotalTrialsPerTest(this.LocalId, (int)testTypeID);
        }

        public static bool IsThereAnActiveScheduleTest(int localLicesneID, clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingData.IsThereAnActiveScheduleTest(localLicesneID, (int)testTypeID);
        }

        public bool IsThereAnActiveScheduleTest(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingData.IsThereAnActiveScheduleTest(this.LocalId, (int)testTypeID);
        }

        public bool DosePassTestType(clsTestType.enTestType testTypeID)
        {
            return clsLocalDrivingData.DosePassTestType(this.LocalId, (int)testTypeID);
        }

        public static byte GetPassedTestCount(int localLicenseID)
        {
            return clsLocalDrivingData.GetPassedTestCount(localLicenseID);
        }

        public bool IsPassedAllTests()
        {
            return GetPassedTestCount(this.LocalId) == 3;
        }

        public bool IsLicenseIssued()
        {
            return GetActiveLicense() != -1;
        }

        public int GetActiveLicense()
        {
            return clsLicense.GetActiveLicenseWithLicenseClass(this.personId, this.classId);
        }

        public int IssueLicenseForFirstTime(string notes, int createdByUserId)
        {
            int driverId = -1;
            clsDriver driver;
            if (!clsDriver.IsPersonDriver(this.personId))
            {
                driver = new clsDriver();
                driver.personID = this.personId;
                driver.userID = createdByUserId;
                driver.createdDate = DateTime.Now;
                if (driver.Save())
                    driverId = driver.driverID;
                else
                    return -1;
            } else
            {
                driver = clsDriver.FindDriver(this.personId);
                driverId = driver.driverID;
            }

            clsLicense FirstLicense = new clsLicense();
            FirstLicense.driverID = driverId;
            FirstLicense.notes = notes;
            FirstLicense.appID = this.appID;
            FirstLicense.classID = this.classId;
            FirstLicense.issueDate = DateTime.Now;
            FirstLicense.expiredDate = FirstLicense.issueDate.AddYears(this.LicenseClassInfo.length);
            FirstLicense.fees = this.LicenseClassInfo.fees;
            FirstLicense.isActive = true;
            FirstLicense.issueReason = clsLicense.enIssueReason.FirstTime;
            FirstLicense.userID = createdByUserId;

            if(FirstLicense.Save())
            {
                // change status for application to complete
                this.SetComplete();
                return FirstLicense.licenseID;
            }
            else return -1;
        }
    }
}
