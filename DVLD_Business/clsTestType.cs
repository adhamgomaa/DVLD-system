using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 4 }
        public clsTestType.enTestType Id { get; private set; }
        public string title { get; set; }
        public string description { get; set; }
        public decimal fees { get; set; }

        public clsTestType(clsTestType.enTestType Id, string title, string description, decimal fees)
        {
            this.Id = Id;
            this.title = title;
            this.description = description;
            this.fees = fees;
        }

        private bool _updateAppTypes()
        {
            return clsTestTypeData.updateTypes((int)Id, title, description, fees);
        }

        public static clsTestType FindType(clsTestType.enTestType typeID)
        {
            string title = "", desc = "";
            decimal fees = 0;
            if (clsTestTypeData.FindTypes((int)typeID, ref title, ref desc, ref fees))
            {
                return new clsTestType(typeID, title, desc, fees);
            }
            else
                return null;
        }

        public static DataView GetAllTypes()
        {
            return clsTestTypeData.GetAllTypes().DefaultView;
        }

        public bool Save()
        {
            if (_updateAppTypes())
                return true;
            return false;
        }
    }
}
