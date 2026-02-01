using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsCountry
    {
        public int CountryId { get; private set; }
        public string CountryName { get; set; }

        public clsCountry() { 
            CountryId = -1;
            CountryName = string.Empty;
        }

        public clsCountry(int CountryId, string countryName)
        {
            this.CountryId = CountryId;
            this.CountryName = countryName;
        }

        public static clsCountry FindCountry(int CountryId)
        {
            string countryName = string.Empty;
            if (clsCountryData.FindCountry(CountryId, ref countryName))
            {
                return new clsCountry(CountryId, countryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry FindCountry(string countryName)
        {
            int countryId = -1;
            if (clsCountryData.FindCountry(countryName, ref countryId))
            {
                return new clsCountry(countryId, countryName);
            }
            else
            {
                return null;
            }
        }

        public static DataView GetCountries()
        {
            return clsCountryData.GetCountries().DefaultView;
        }
    }
}
