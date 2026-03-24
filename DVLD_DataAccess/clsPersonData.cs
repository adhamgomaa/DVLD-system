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
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindPersonByPersonId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; 
                        command.Parameters.AddWithValue("@personId", id);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                        }
                    }
                }
            }
            catch (SqlException ex) {
                isFound = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }
        public static bool GetPersonByNationalNo(string nationalNo, ref int id, ref string firstName, ref string secName, ref string thName,
            ref string lastName, ref DateTime date, ref short gendor, ref string address, ref string phone, ref string email,
            ref int nationalId, ref string imagePath)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindPersonByNationalNo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@national", nationalNo);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                isFound = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }
        public static int AddNewPerson(string nationalNo, string firstName, string secName, string thName, string lastName, DateTime date,
            short gendor, string address, string phone, string email, int nationalId, string imagePath)
        {
            int personId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewPerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@nationalNo", nationalNo);
                        command.Parameters.AddWithValue("@fname", firstName);
                        command.Parameters.AddWithValue("@sec", secName);
                        if(thName != null && thName.Trim() != "")
                            command.Parameters.AddWithValue("@thname", thName);
                        else
                            command.Parameters.AddWithValue("@thname", DBNull.Value);
                        command.Parameters.AddWithValue("@lName", lastName);
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
                        SqlParameter outputId = new SqlParameter("@personId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        personId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                personId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return personId;
        }
        public static bool UpdatePerson(int id, string nationalNo, string firstName, string secName, string thName, string lastName,
            DateTime date, short gendor, string address, string phone, string email, int nationalId, string imagePath)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
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
                        command.Parameters.AddWithValue("@personId", id);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                rowAffected = 0;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }
        public static bool DeletePerson(int id)
        {
            int rowAffected = 0;
            try
            {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@personId", id);
                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
            }
            catch (SqlException ex)
            {
                rowAffected = 0;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }
        public static DataTable GetPeople()
        {
            DataTable dt = new DataTable();
            try
            {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }
                }
            }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return dt;
        }
        public static bool IsPersonExist(int id)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsPersonExists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", id);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                isFound = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                isFound = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }
        public static bool IsPersonExist(string nationalNo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsPersonExists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NationalNo", nationalNo);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                isFound = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                isFound = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }
    }
}
