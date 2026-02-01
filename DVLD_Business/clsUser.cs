using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsUser
    {
        enum enMode { Add, Update};
        enMode mode = enMode.Add;

        public int id { get; private set; }
        public int personId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }

        public clsUser() {
            this.id = -1;
            this.personId = -1;
            this.userName = string.Empty;
            this.password = string.Empty;
            this.isActive = false;
            mode = enMode.Add;
        }

        public clsUser(int id, int personId, string username, string password, bool isActive)
        {
            this.id = id;
            this.personId = personId;
            this.userName = username;
            this.password = password;
            this.isActive = isActive;
            mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            this.id = clsUserData.AddNewUser(personId, userName, clsEncryption.Hashing(password), isActive);
            return id != -1;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(id, personId, userName, password, isActive);
        }

        public static clsUser FindUser(int id)
        {
            int personId = -1;
            string userName = "", password = "";
            bool isActive = false;
            if (clsUserData.GetUser(id, ref personId, ref userName, ref password, ref isActive))
            {
                return new clsUser(id, personId, userName, password, isActive);
            }
            else
                return null;
        }

        public static clsUser FindUser(string username)
        {
            int personId = -1, id = -1;
            string password = "";
            bool isActive = false;
            if (clsUserData.GetUser(username, ref id, ref personId, ref password, ref isActive))
            {
                return new clsUser(id, personId, username, password, isActive);
            }
            else
                return null;
        }

        public static clsUser FindUser(string username, string password)
        {
            int personId = -1, id = -1;
            bool isActive = false;
            password = clsEncryption.Hashing(password);
            if (clsUserData.GetUser(username, password, ref id, ref personId, ref isActive))
            {
                return new clsUser(id, personId, username, password, isActive);
            }
            else
                return null;
        }

        public static DataView GetUsers()
        {
            return clsUserData.GetAllUsers().DefaultView;
        }

        public static bool IsUserExist(int id)
        {
            return clsUserData.isUserExist(id);
        }

        public static bool IsUserExist(string username)
        {
            return clsUserData.isUserExist(username);
        }

        public static bool IsUserExistByPersonId(int id)
        {
            return clsUserData.isUserExistByPersonId(id);
        }

        public static bool DeleteUser(int id)
        {
            return IsUserExist(id) ? clsUserData.DeleteUser(id) : false;
        }

        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if (_AddNewUser())
                    {
                        mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }
    }
}
