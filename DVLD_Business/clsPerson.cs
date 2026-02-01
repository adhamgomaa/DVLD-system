using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsPerson
    {
        enum enState { Add, Update};
        enState state = enState.Add;

        public int id { get; private set; }
        public string NationalNo { get; set; }
        public string fName { get; set; }
        public string secName { get; set; }
        public string thName { get; set; }
        public string lName { get; set; }
        public string fullName()
        {
            return $"{fName} {secName} {thName} {lName}";
        }
        public DateTime date {  get; set; }
        public short gendor {  get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int NationaltyId { get; set; }
        public string imagePath { get; set; }
        public clsCountry countryInfo;
        public clsPerson()
        {
            id = -1;
            NationalNo = string.Empty;
            fName = string.Empty;
            secName = string.Empty;
            thName = string.Empty;
            lName = string.Empty;
            date = DateTime.Now;
            gendor = 0;
            address = string.Empty;
            phone = string.Empty;
            email = string.Empty;
            NationaltyId = -1;
            imagePath = string.Empty;
            state = enState.Add;
        }

        public clsPerson(int id, string nationalNo, string fname, string secname, string thName, string lName, DateTime date, short gendor, 
            string address, string phone, string email, int nationaltyId, string path)
        {
            this.id = id;
            this.NationalNo = nationalNo;
            this.fName = fname;
            this.secName = secname;
            this.thName = thName;
            this.lName = lName;
            this.date = date;
            this.gendor = gendor;
            this.address = address;
            this.phone = phone;
            this.email = email;
            this.NationaltyId = nationaltyId;
            this.imagePath = path;
            this.countryInfo = clsCountry.FindCountry(nationaltyId);
            state = enState.Update;
        }
        private bool _AddNewPerson()
        {
            this.id = clsPersonData.AddNewPerson(NationalNo, fName, secName, thName, lName, date, gendor, address, phone, email, NationaltyId, imagePath);
            return this.id != -1;
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(id, NationalNo, fName, secName, thName, lName, date, gendor, address, phone, email, NationaltyId, imagePath);
        }

        public static clsPerson findPerson(int id)
        {
            string nationalNo = "", fName = "", secName = "", thName = "", lName = "", address = "", phone = "", email = "", imgPath = "";
            DateTime date = DateTime.Now;
            short gendor = 0;
            int nationaltyId = -1;
            if(clsPersonData.GetPersonByID(id, ref nationalNo, ref fName, ref secName, ref thName, ref lName, ref date, ref gendor, ref address,
                ref phone, ref email, ref nationaltyId, ref imgPath))
            {
                return new clsPerson(id, nationalNo, fName, secName, thName, lName, date, gendor, address, phone, email, nationaltyId, imgPath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson findPerson(string nationalNo)
        {
            string fName = "", secName = "", thName = "", lName = "", address = "", phone = "", email = "", imgPath = "";
            DateTime date = DateTime.Now;
            short gendor = 0;
            int id = -1, nationaltyId = -1;
            if (clsPersonData.GetPersonByNationalNo(nationalNo, ref id, ref fName, ref secName, ref thName, ref lName, ref date, ref gendor, ref address,
                ref phone, ref email, ref nationaltyId, ref imgPath))
            {
                return new clsPerson(id, nationalNo, fName, secName, thName, lName, date, gendor, address, phone, email, nationaltyId, imgPath);
            }
            else
            {
                return null;
            }
        }

        public static DataView GetPeople()
        {
            return clsPersonData.GetPeople().DefaultView;
        }

        public static bool IsPersonExist(int id)
        {
            return clsPersonData.IsPersonExist(id);
        }

        public static bool IsPersonExist(string nationalNo)
        {
            return clsPersonData.IsPersonExist(nationalNo);
        }

        public static bool DeletePerson(int id)
        {
            return IsPersonExist(id) ? clsPersonData.DeletePerson(id) : false;
        }

        public bool Save()
        {
            switch(state)
            {
                case enState.Add:
                    if(_AddNewPerson())
                    {
                        state = enState.Update;
                        return true;
                    }
                    return false;
                case enState.Update:
                    return _UpdatePerson();
            }
            return false;
        }

    }
}
