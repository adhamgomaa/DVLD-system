using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApp
    {
        enum enMode { Add, Update}
        enMode mode = enMode.Add;
        public enum enApplicationType
        {
            NewDrivingLicense = 1,
            RenewDrivingLicense,
            ReplaceLostDrivingLicense,
            ReplaceDamagedDrivingLicense,
            ReleaseDetainedDrivingLicsense,
            NewInternationalLicense,
            RetakeTest
        }
        public int appID { get; private set; }
        public int personId { get; set; }
        public clsPerson personInfo { get; set; }
        public DateTime date { get; set; }
        public int types { get; set; }
        public clsAppTypes appTypes;
        public byte status { get; set; }
        public DateTime statusDate { get; set; }
        public decimal fees { get; set; }
        public int userId { get; set; }
        public clsApp()
        {
            appID = -1;
            personId = -1;
            date = DateTime.Now;
            types = 0;
            status = 0;
            statusDate = DateTime.Now;
            fees = 0;
            userId = -1;
            mode = enMode.Add;
        }

        protected clsApp(int appID, int personId, DateTime date, int types, byte status, DateTime statusDate, decimal fees, int userId)
        {
            this.appID = appID;
            this.personId = personId;
            personInfo = clsPerson.findPerson(personId);
            this.date = date;
            this.types = types;
            appTypes = clsAppTypes.FindType(types);
            this.status = status;
            this.statusDate = statusDate;
            this.fees = fees;
            this.userId = userId;
            mode = enMode.Update;
        }

        private bool _AddNewApp()
        {
            this.appID = clsAppData.AddNewApp(personId, date, types, status, statusDate, fees, userId);
            return this.appID != -1;
        }

        private bool _UpdateApp()
        {
            return clsAppData.UpdateApp(appID, personId, date, types, status, statusDate, fees, userId);
        }

        public static clsApp FindApp(int id)
        {
            int personId = -1, types = -1, userId = -1;
            byte status = 0;
            DateTime date = DateTime.Now, sDate = DateTime.Now;
            decimal fees = 0;
            if(clsAppData.FindApp(id, ref personId, ref date, ref types, ref status, ref sDate, ref fees, ref userId))
                return new clsApp(id, personId, date, types, status, sDate, fees, userId);
            else
                return null;
        }

        public bool Delete()
        {
            return clsAppData.DeleteApp(this.appID);
        }

        public bool SetComplete()
        {
            return clsAppData.setComplete(this.appID, 3);
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if (_AddNewApp())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateApp();
            }
            return false;
        }
    }
}
