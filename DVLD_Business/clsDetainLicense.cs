using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDetainLicense
    {
        enum enMode { Add, Update };
        enMode mode = enMode.Add;
        public int detainID { get; private set; }
        public int licenseID { get; set; }
        public DateTime detainDate { get; set; }
        public decimal fees { get; set; }
        public int userID { get; set; }
        public bool isRelease { get; set; }
        public DateTime releaseDate { get; set; }
        public int releaseByUserId { get; set; }
        public int releaseAppId { get; set; }
        public clsDetainLicense()
        {
            detainID = -1;
            licenseID = -1;
            detainDate = DateTime.Now;
            fees = 0;
            userID = -1;
            isRelease = false;
            releaseDate = DateTime.MaxValue;
            releaseByUserId = -1;
            releaseAppId = -1;
            mode = enMode.Add;
        }

        public clsDetainLicense(int detainID, int licenseID, DateTime detainDate, decimal fees, int userID, bool isRelease, DateTime releaseDate, int releaseByUserId, int releaseAppId)
        {
            this.detainID = detainID;
            this.licenseID = licenseID;
            this.detainDate = detainDate;
            this.fees = fees;
            this.userID = userID;
            this.isRelease = isRelease;
            this.releaseDate = releaseDate;
            this.releaseByUserId = releaseByUserId;
            this.releaseAppId = releaseAppId;
            mode = enMode.Update;
        }

        private bool _AddNewDetainLicense()
        {
            this.detainID = clsDetainLicenseData.AddNewDetain(licenseID, detainDate, fees, userID);
            return this.detainID != -1;
        }

        private bool _UpdateDetainLicense()
        {
            return clsDetainLicenseData.UpdateDetainLicense(detainID, licenseID, detainDate, fees, userID);
        }

        public static clsDetainLicense FindDetainLicense(int id)
        {
            int licenseId = -1, userId = -1;
            int releaseByUserId = -1, releaseAppId = -1;
            DateTime detainDate = DateTime.Now;
            DateTime releaseDate = DateTime.MaxValue;
            decimal fees = 0;
            bool isRelease = false;
            if (clsDetainLicenseData.FindLicense(id, ref licenseId, ref detainDate, ref fees, ref userId, ref isRelease, ref releaseDate, ref releaseByUserId,
                ref releaseAppId))
                return new clsDetainLicense(id, licenseId, detainDate, fees, userId, isRelease, releaseDate, releaseByUserId, releaseAppId);
            else
                return null;
        }

        public static clsDetainLicense FindDetainLicenseByLicenseID(int licenseId)
        {
            int detainId = -1, userId = -1;
            int releaseByUserId = -1, releaseAppId = -1;
            DateTime detainDate = DateTime.Now;
            DateTime releaseDate = DateTime.MaxValue;
            decimal fees = 0;
            bool isRelease = false;
            if (clsDetainLicenseData.FindLicense(ref detainId, licenseId, ref detainDate, ref fees, ref userId, ref isRelease, ref releaseDate, ref releaseByUserId,
                ref releaseAppId))
                return new clsDetainLicense(detainId, licenseId, detainDate, fees, userId, isRelease, releaseDate, releaseByUserId, releaseAppId);
            else
                return null;
        }

        public static DataView GetDetainedLicenses()
        {
            return clsDetainLicenseData.GetDetainedLicenses().DefaultView;
        }

        public bool ReleaseDetainedLicense(int releaseByUserId, int releaseAppId)
        {
            return clsDetainLicenseData.ReleaseDetainedLicense(this.detainID, releaseByUserId, releaseAppId);
        }

        public static bool IsDetainedLicense(int licenseId)
        {
            return clsDetainLicenseData.IsDetainedLicense(licenseId);
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if (_AddNewDetainLicense())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateDetainLicense();
            }
            return false;
        }
    }
}
