using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsAppTypes
    {
        public int typeID {  get; set; }
        public string title { get; set; }
        public decimal fees { get; set; }

        private clsAppTypes(int typeID, string title, decimal fees)
        {
            this.typeID = typeID;
            this.title = title;
            this.fees = fees;
        }

        private bool _updateAppTypes()
        {
            return clsAppTypesData.updateTypes(typeID, title, fees);
        }

        public static clsAppTypes FindType(int typeID)
        {
            string title = "";
            decimal fees = 0;
            if(clsAppTypesData.FindTypes(typeID, ref title, ref fees))
            {
                return new clsAppTypes(typeID, title, fees);
            }
            else
                return null;
        }

        public static clsAppTypes FindType(string title)
        {
            int id = -1;
            decimal fees = 0;
            if (clsAppTypesData.FindTypes(title, ref id, ref fees))
            {
                return new clsAppTypes(id, title, fees);
            }
            else
                return null;
        }

        public static DataView GetAllTypes()
        {
            return clsAppTypesData.GetAllTypes().DefaultView;
        }

        public bool Save()
        {
            if(_updateAppTypes())
                return true;
            return false;
        }
    }
}
