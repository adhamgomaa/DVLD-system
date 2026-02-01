using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicenseClass
    {
        public int id {  get; private set; }
        public string className { get; set; }
        public string description { get; set; }
        public int age { get; set; }
        public int length { get; set; }
        public decimal fees { get; set; }

        private clsLicenseClass(int id, string className, string desc, int age, int length, decimal fees)
        {
            this.id = id;
            this.className = className;
            this.description = desc;
            this.age = age;
            this.length = length;
            this.fees = fees;
        }

        public static clsLicenseClass FindClasses(string className)
        {
            int id = -1, age = 0, length = 0;
            decimal fees = 0;
            string desc = "";
            if(clsLicenseClassData.FindClasses(className, ref id, ref desc, ref age, ref length, ref fees))
            {
                return new clsLicenseClass(id, className, desc, age, length, fees);
            }
            else
                return null;
        }
        public static clsLicenseClass FindClasses(int id)
        {
            int age = 0, length = 0;
            decimal fees = 0;
            string desc = "", className = "";
            if (clsLicenseClassData.FindClasses(id, ref className, ref desc, ref age, ref length, ref fees))
            {
                return new clsLicenseClass(id, className, desc, age, length, fees);
            }
            else
                return null;
        }
        public static DataView GetAllClasses()
        {
            return clsLicenseClassData.GetAllClasses().DefaultView;
        }
    }
}
