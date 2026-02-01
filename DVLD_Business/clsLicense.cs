using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicense
    {
        enum enMode { Add, Update};
        enMode mode = enMode.Add;
        public enum enIssueReason { FirstTime = 1, Renew, Damaged, Lost}
        public int licenseID {  get; private set; }
        public int driverID { get; set; }
        public clsDriver driverInfo { get; set; }
        public int appID { get; set; }
        public int classID { get; set; }
        public clsLicenseClass licenseClassInfo { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expiredDate { get; set; }
        public decimal fees {  get; set; }
        public string notes { get; set; }
        public bool isActive { get; set; }
        public enIssueReason issueReason { get; set; }
        public int userID { get; set; }
        public clsDetainLicense detainedLicenseInfo { get; set; }

        public string IssueReasonString
        {
            get { return GetIssueReasonText(issueReason); }
        }
        
        public bool IsDetainedLicense
        {
            get { return clsDetainLicense.IsDetainedLicense(this.licenseID); }
        }

        public clsLicense()
        {
            licenseID = -1;
            driverID = -1;
            appID = -1;
            classID = -1;
            issueDate = DateTime.Now;
            expiredDate = issueDate;
            fees = 0;
            notes = string.Empty;
            isActive = false;
            issueReason = enIssueReason.FirstTime;
            userID = -1;
            mode = enMode.Add;
        }

        private clsLicense(int licenseID, int driverID, int appID, int classID, DateTime issueDate, DateTime expiredDate, decimal fees, string notes, bool isActive, enIssueReason issueReason, int userID)
        {
            this.licenseID = licenseID;
            this.driverID = driverID;
            this.appID = appID;
            this.classID = classID;
            this.issueDate = issueDate;
            this.expiredDate = expiredDate;
            this.fees = fees;
            this.notes = notes;
            this.isActive = isActive;
            this.issueReason = issueReason;
            this.userID = userID;
            driverInfo = clsDriver.FindDriverWithDriverID(driverID);
            licenseClassInfo = clsLicenseClass.FindClasses(classID);
            detainedLicenseInfo = clsDetainLicense.FindDetainLicenseByLicenseID(licenseID);
            mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            this.licenseID = clsLicenseData.AddNewLicense(driverID, appID, classID, issueDate, expiredDate, fees, notes, isActive, (byte)issueReason, userID);
            return this.licenseID != -1;
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(licenseID, driverID, appID, classID, issueDate, expiredDate, fees, notes, isActive, (byte)issueReason, userID);
        }

        public static clsLicense FindLicense(int id)
        {
            int driverId = -1, appId = -1, classId = -1, userId = -1;
            DateTime issueDate = DateTime.Now, expiredDate = DateTime.Now;
            decimal fees = 0;
            string notes = string.Empty;
            bool isActive = false;
            byte issueReason = 0;
            if(clsLicenseData.FindLicense(id, ref driverId, ref appId, ref classId, ref issueDate, ref expiredDate, ref fees, ref notes,
                ref isActive, ref issueReason, ref userId))
                    return new clsLicense(id, driverId, appId, classId, issueDate, expiredDate, fees, notes, isActive, (enIssueReason)issueReason, userId);
            else
                return null;
        } 

        public static int GetLicenseID(int LocalId)
        {
            return clsLicenseData.GetLicenseID(LocalId);
        }

        public static int GetLicenseID(string nationalNo)
        {
            return clsLicenseData.GetLicenseID(nationalNo);
        }

        public static DataView GetLocalLicensesHistory(int driverId)
        {
            return clsLicenseData.GetLocalLicenseHistory(driverId).DefaultView;
        }

        public bool IsLicenseExpired()
        {
            return this.expiredDate < DateTime.Now;
        }

        public static string GetIssueReasonText(enIssueReason issueReason)
        {
            switch (issueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.Damaged:
                    return "Replacement For Damaged";
                case enIssueReason.Lost:
                    return "Replacement For Lost";
                default:
                    return "First Time";
            }
        }

        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(this.licenseID);
        }

        public int Detain(decimal FineFees, int CreatedByUserId)
        {
            clsDetainLicense detainLicense = new clsDetainLicense();
            detainLicense.licenseID = this.licenseID;
            detainLicense.detainDate = DateTime.Now;
            detainLicense.fees = FineFees;
            detainLicense.userID = CreatedByUserId;
            if (!detainLicense.Save())
                return -1;
            return detainLicense.detainID;
        }

        public bool ReleaseLicense(int releaseByUserId, ref int releaseAppId)
        {
            clsApp ReleaseApp = new clsApp();
            ReleaseApp.date = DateTime.Now;
            ReleaseApp.status = 3;
            ReleaseApp.personId = this.driverInfo.personID;
            ReleaseApp.types = (int)clsApp.enApplicationType.ReleaseDetainedDrivingLicsense;
            ReleaseApp.fees = clsAppTypes.FindType(ReleaseApp.types).fees;
            ReleaseApp.userId = releaseByUserId;
            ReleaseApp.statusDate = DateTime.Now;
            if(!ReleaseApp.Save())
            {
                releaseAppId = -1;
                return false;
            }
            releaseAppId = ReleaseApp.appID;

            return this.detainedLicenseInfo.ReleaseDetainedLicense(releaseByUserId, releaseAppId);
        }

        public clsLicense RenewLicense(string notes, int userId)
        {
            if(!this.isActive || !this.IsLicenseExpired())
            {
                return null;
            }

            clsApp RenewApp = new clsApp();
            RenewApp.date = DateTime.Now;
            RenewApp.status = 3;
            RenewApp.personId = this.driverInfo.personID;
            RenewApp.types = (int)clsApp.enApplicationType.RenewDrivingLicense;
            RenewApp.fees = clsAppTypes.FindType(RenewApp.types).fees;
            RenewApp.userId = userId;
            RenewApp.statusDate = DateTime.Now;
            if (!RenewApp.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();
            NewLicense.classID = this.classID;
            NewLicense.notes = notes;
            NewLicense.fees = this.licenseClassInfo.fees;
            NewLicense.appID = RenewApp.appID;
            NewLicense.driverID = this.driverID;
            NewLicense.isActive = true;
            NewLicense.issueDate = DateTime.Now;
            NewLicense.expiredDate = NewLicense.issueDate.AddYears(this.licenseClassInfo.length);
            NewLicense.issueReason = enIssueReason.Renew;
            NewLicense.userID = userId;

            if(!NewLicense.Save()) { return null; }

            //Deactivate the old License
            DeactivateLicense();

            return NewLicense;
        }

        public clsLicense Replace(enIssueReason enIssueReason, int userId)
        {
            if (enIssueReason == enIssueReason.FirstTime || !this.isActive || this.IsDetainedLicense)
                return null;
            if(enIssueReason == enIssueReason.Renew)
                return RenewLicense(this.notes, userId);

            clsApp ReplaceApp = new clsApp();
            ReplaceApp.date = DateTime.Now;
            ReplaceApp.status = 3;
            ReplaceApp.personId = this.driverInfo.personID;
            ReplaceApp.types = enIssueReason == enIssueReason.Damaged ? (int)clsApp.enApplicationType.ReplaceDamagedDrivingLicense : (int)clsApp.enApplicationType.ReplaceLostDrivingLicense;
            ReplaceApp.fees = clsAppTypes.FindType(ReplaceApp.types).fees;
            ReplaceApp.userId = userId;
            ReplaceApp.statusDate = DateTime.Now;
            if (!ReplaceApp.Save())
            {
                return null;
            }

            clsLicense ReplaceLicense = new clsLicense();
            ReplaceLicense.classID = this.classID;
            ReplaceLicense.notes = this.notes;
            ReplaceLicense.appID = ReplaceApp.appID;
            ReplaceLicense.driverID = this.driverID;
            ReplaceLicense.isActive = true;
            ReplaceLicense.issueDate = DateTime.Now;
            ReplaceLicense.expiredDate = this.expiredDate;
            ReplaceLicense.issueReason = this.issueReason;
            ReplaceLicense.userID = userId;
            ReplaceLicense.fees = 0;

            if(!ReplaceLicense.Save()) { return null; }

            //Deactivate the old License
            DeactivateLicense();

            return ReplaceLicense;
        }

        public static int GetActiveLicenseWithLicenseClass(int personId, int classID)
        {
            return clsLicenseData.GetActiveLicenseWithLicenseClass(personId, classID);
        }

        public bool Save()
        {
            switch(mode)
            {
                case enMode.Add:
                    if(_AddNewLicense())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }
    }
}
