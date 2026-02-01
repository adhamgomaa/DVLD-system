using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsInternationalLicense : clsApp
    {
        public int InternationalID { get; private set; }
        public int DriverID { get; set; }
        public clsDriver driverInfo { get; set; } 
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int LocalLicenseID { get; set; }

        public clsInternationalLicense()
        {
            base.types = 6;
            InternationalID = -1;
            DriverID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = false;
            LocalLicenseID = -1;
        }

        public clsInternationalLicense(int internationalId, int appId, int personId, DateTime date, int types, byte status, DateTime statusDate, decimal fees, int driverId, DateTime issue, DateTime expired, int userId, bool isActive
            , int localId) : base(appId, personId, date, types, status, statusDate, fees, userId)
        {
            this.InternationalID = internationalId;
            this.DriverID = driverId;
            this.IssueDate = issue;
            this.ExpirationDate = expired;
            IsActive = isActive;
            LocalLicenseID = localId;
            driverInfo = clsDriver.FindDriverWithDriverID(driverId);
        }

        private bool _AddNewLicense()
        {
            this.InternationalID = clsInternationalLicenseData.AddNewInternationalLicense(base.appID, DriverID, IssueDate, ExpirationDate, base.userId, IsActive, LocalLicenseID);
            return this.InternationalID != -1;
        }

        public static clsInternationalLicense FindLicense(int licenseID)
        {
            int driverId = -1, appId = -1, userId = -1, localId = -1;
            DateTime issueDate = DateTime.Now, expired = DateTime.Now;
            bool isActive = false;
            if (clsInternationalLicenseData.FindLicense(licenseID, ref appId, ref driverId, ref issueDate, ref expired, ref userId,
                ref isActive, ref localId))
            {
                clsApp app = clsApp.FindApp(appId);
                return new clsInternationalLicense(licenseID, appId, app.personId, app.date, app.types, app.status, app.statusDate, app.fees, driverId, issueDate, expired, app.userId, isActive, localId);
            }
            else 
                return null;
        }

        public static clsInternationalLicense FindLicenseByLocal(int localId)
        {
            int driverId = -1, appId = -1, userId = -1, licenseID = -1;
            DateTime issueDate = DateTime.Now, expired = DateTime.Now;
            bool isActive = false;
            if (clsInternationalLicenseData.FindLicense(ref licenseID, ref appId, ref driverId, ref issueDate, ref expired, ref userId,
                ref isActive, localId))
            {
                clsApp app = clsApp.FindApp(appId);
                return new clsInternationalLicense(licenseID, appId, app.personId, app.date, app.types, app.status, app.statusDate, app.fees, driverId, issueDate, expired, app.userId, isActive, localId);
            }
            else
                return null;
        }

        public static DataView GetAllLicenses()
        {
            return clsInternationalLicenseData.GetAllLicenses().DefaultView;
        }

        public static DataView GetInternationalLicensesHistory(int driverId)
        {
            return clsInternationalLicenseData.GetInternaionalLicenseHistory(driverId).DefaultView;
        }

        public static int GetActiveInternationalLicenseId(int driverId)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverId(driverId);
        }

        public new bool Save()
        {
            if (!base.Save()) return false;
            return _AddNewLicense();
        }

    }
}
