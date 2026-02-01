using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonByID(int id, ref string nationalNo, ref string firstName, ref string secName, ref string thName,
            ref string lastName, ref DateTime date, ref short gendor, ref string address, ref string phone, ref string email,
            ref int nationalId, ref string imagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM People WHERE PersonID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    nationalNo = (string)reader["NationalNo"];
                    firstName = (string)reader["FirstName"];
                    secName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        thName = (string)reader["ThirdName"];
                    else
                        thName = "";

                    lastName = (string)reader["LastName"];
                    date = (DateTime)reader["BirthOfDate"];
                    gendor = (byte)reader["Gendor"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        email = (string)reader["Email"];
                    else
                        email = "";

                    nationalId = (int)reader["NationalityCountryByID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        imagePath = (string)reader["ImagePath"];
                    else
                        imagePath = "";
                }
                reader.Close();
            }
            catch (Exception ex) {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool GetPersonByNationalNo(string nationalNo, ref int id, ref string firstName, ref string secName, ref string thName,
            ref string lastName, ref DateTime date, ref short gendor, ref string address, ref string phone, ref string email,
            ref int nationalId, ref string imagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM People WHERE NationalNo = @national";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@national", nationalNo);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    id = (int)reader["PersonID"];
                    firstName = (string)reader["FirstName"];
                    secName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        thName = (string)reader["ThirdName"];
                    else
                        thName = "";

                    lastName = (string)reader["LastName"];
                    date = (DateTime)reader["BirthOfDate"];
                    gendor = (byte)reader["Gendor"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        email = (string)reader["Email"];
                    else
                        email = "";

                    nationalId = (int)reader["NationalityCountryByID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        imagePath = (string)reader["ImagePath"];
                    else
                        imagePath = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static int AddNewPerson(string nationalNo, string firstName, string secName, string thName, string lastName, DateTime date,
            short gendor, string address, string phone, string email, int nationalId, string imagePath)
        {
            int personId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "INSERT INTO People Values (@nationalNo, @fname, @secname, @thname, @lastname, @date, @gendor, @address, @phone," +
                " @email, @nationalid, @path); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            command.Parameters.AddWithValue("@fname", firstName);
            command.Parameters.AddWithValue("@secname", secName);
            if(thName != null && thName.Trim() != "")
                command.Parameters.AddWithValue("@thname", thName);
            else
                command.Parameters.AddWithValue("@thname", DBNull.Value);
            command.Parameters.AddWithValue("@lastname", lastName);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@gendor", gendor);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);
            if(email != null && email.Trim() != "")
                command.Parameters.AddWithValue("@email", email);
            else
                command.Parameters.AddWithValue("@email", DBNull.Value);

            command.Parameters.AddWithValue("@nationalid", nationalId);
            if(imagePath != null && imagePath.Trim() != "")
                command.Parameters.AddWithValue("@path", imagePath);
            else
                command.Parameters.AddWithValue("@path", DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId)) {
                    personId = insertedId;
                }
            }
            catch (Exception ex)
            {
                personId = -1;
            }
            finally
            {
                connection.Close();
            }
            return personId;
        }
        public static bool UpdatePerson(int id, string nationalNo, string firstName, string secName, string thName, string lastName,
            DateTime date, short gendor, string address, string phone, string email, int nationalId, string imagePath)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Update People set NationalNo = @nationalNo, FirstName = @fname, SecondName = @sec, ThirdName = @thName, " +
                "LastName = @lName, BirthOfDate = @date, Gendor = @gendor, Address = @address, Phone = @phone, Email = @email, " +
                "NationalityCountryByID = @nationalId, ImagePath = @path Where PersonID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            command.Parameters.AddWithValue("@fname", firstName);
            command.Parameters.AddWithValue("@sec", secName);
            command.Parameters.AddWithValue("@thName", thName);
            command.Parameters.AddWithValue("@lName", lastName);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@gendor", gendor);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@nationalId", nationalId);
            command.Parameters.AddWithValue("@path", imagePath);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }
            return rowAffected > 0;
        }
        public static bool DeletePerson(int id)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Delete from People Where PersonID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }
            return rowAffected > 0;
        }
        public static DataTable GetPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select PersonID as \"Person ID\", NationalNo as \"National No.\", FirstName as \"First Name\", SecondName as \"Second Name\", ThirdName as \"Third Name\", LastName as \"Last Name\",\r\nCASE \r\nWHEN Gendor = 0 Then 'Male'\r\nELSE 'Female'\r\nEND AS Gendor\r\n, BirthOfDate as \"Date Of Birth\", c.CountryName as \"Nationality\", Phone, Email from People INNER JOIN Countries c ON People.NationalityCountryByID = c.CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static bool IsPersonExist(int id)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * from People Where PersonID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                    isFound = true;
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool IsPersonExist(string nationalNo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * from People Where NationalNo = @nationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    isFound = true;
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
    }
}
