using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDriver
    {
        public int driverID {  get; private set; }
        public int personID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public int userID { get; set; }
        public DateTime createdDate { get; set; }

        public clsDriver()
        {
            driverID = -1;
            personID = -1;
            userID = -1;
            createdDate = DateTime.Now;
        }

        private clsDriver(int driverID, int personID, int userID, DateTime createdDate)
        {
            this.driverID = driverID;
            this.personID = personID;
            this.userID = userID;
            this.createdDate = createdDate;
            PersonInfo = clsPerson.findPerson(personID);
        }

        private bool _AddNewDriver()
        {
            this.driverID = clsDriverData.AddNewDriver(personID, createdDate, userID);
            return this.driverID != -1;
        }

        public static DataView ListDrivers()
        {
            return clsDriverData.ListDrivers().DefaultView;
        }

        public static bool IsPersonDriver(int personId)
        {
            return clsDriverData.IsPersonDriver(personId);
        }

        public static clsDriver FindDriver(int personId)
        {
            int driverId = -1, userId = -1;
            DateTime createdDate = DateTime.Now;
            if(clsDriverData.FindDriver(personId, ref driverId, ref userId, ref createdDate))
                return new clsDriver(driverId, personId, userId, createdDate);
            else 
                return null;
        }

        public static clsDriver FindDriverWithDriverID(int driverId)
        {
            int personId = -1, userId = -1;
            DateTime createdDate = DateTime.Now;
            if (clsDriverData.FindDriver(ref personId, driverId, ref userId, ref createdDate))
                return new clsDriver(driverId, personId, userId, createdDate);
            else
                return null;
        }

        public bool Save()
        {
            return _AddNewDriver();
        }
    }
}
