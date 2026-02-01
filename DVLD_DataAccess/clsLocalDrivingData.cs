 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingData
    {
        public static int AddNewLocal(int appId, int classId)
        {
            int localId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert into LocalDrivingLicense Values (@appId, @classId); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appId", appId);
            command.Parameters.AddWithValue("@classId", classId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    localId = insertedId;
                }
            }
            catch (Exception ex)
            {
                localId = -1;
            }
            finally
            {
                connection.Close();
            }
            return localId;
        }

        public static bool UpdateLocal(int localId, int appId, int classId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Update LocalDrivingLicense set ApplicationID = @appId, LicenseClassID = @classId Where LocalDrivingLicenseApplicationID = @localId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appId", appId);
            command.Parameters.AddWithValue("@classId", classId);
            command.Parameters.AddWithValue("@localId", localId);
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

        public static bool DeleteLocal(int localId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Delete from LocalDrivingLicense where LocalDrivingLicenseApplicationID = @id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", localId);
            try
            {
                connection.Open();
                rowAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { connection.Close(); }
            return rowAffected > 0;
        }

        public static bool CheckPersonHasSameClass(int personId, int classId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select app.ApplicantPersonID, app.ApplicationID, ldl.LicenseClassID from Application app inner join LocalDrivingLicense ldl\r\nON app.ApplicationID = ldl.ApplicationID\r\nWHERE (app.ApplicationStatus = 1 OR app.ApplicationStatus = 3) AND  app.ApplicantPersonID = @pId AND ldl.LicenseClassID = @lcId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@pId", personId);
            command.Parameters.AddWithValue("@lcId", classId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                }
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

        public static DataTable GetAllLocalLicenses()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * from LocalDrivingLicenseApplications order by [L.D.L.AppID] desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    table.Load(reader);
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
            return table;
        }

        public static bool CancelLicense(int localLicenseAppID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "update app set app.ApplicationStatus = 2, app.LastStatusDate = GETDATE() from Application app inner join LocalDrivingLicense ldl ON ldl.ApplicationID = app.ApplicationID where ldl.LocalDrivingLicenseApplicationID = @id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", localLicenseAppID);
            try
            {
                connection.Open();
                rowAffected = cmd.ExecuteNonQuery();
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

        public static bool FindLocalLicense(int localLicenseAppID, ref int appId, ref int classId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from LocalDrivingLicense where LocalDrivingLicenseApplicationID = @localId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    appId = (int)reader["ApplicationID"];
                    classId = (int)reader["LicenseClassID"];
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

        public static bool FindLocalLicense(int localLicenseAppID, ref string LicenseClass, ref int PassedTests)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from LocalDrivingLicenseApplications where [L.D.L.AppID] = @localId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    LicenseClass = (string)reader[1];
                    PassedTests = (int)reader[5];
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

        public static bool DoseAttendTestType(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select top 1 found = 1 from LocalDrivingLicense ldl inner join TestAppointment ta" +
                " ON ldl.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseID inner join Tests t" +
                " ON ta.TestAppointmentID = t.TestAppointmentID where ldl.LocalDrivingLicenseApplicationID = @localId AND ta.TestTypeID = @testType" +
                " order by ta.TestAppointmentID desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            cmd.Parameters.AddWithValue("@testType", testTypeID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    isFound = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static byte TotalTrialsPerTest(int localLicenseAppID, int testTypeID)
        {
            byte Totaltrials = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select trials = COUNT(TestID) from LocalDrivingLicense ldl inner join TestAppointment ta" +
                " ON ldl.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseID inner join Tests t" +
                " ON ta.TestAppointmentID = t.TestAppointmentID where ldl.LocalDrivingLicenseApplicationID = @localId AND ta.TestTypeID = @testType";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            cmd.Parameters.AddWithValue("@testType", testTypeID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                {
                    Totaltrials = Trials;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return Totaltrials;
        }

        public static bool IsThereAnActiveScheduleTest(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select top 1 found = 1 from LocalDrivingLicense ldl inner join TestAppointment ta" +
                " ON ldl.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseID" +
                " where ldl.LocalDrivingLicenseApplicationID = @localId AND ta.TestTypeID = @testType AND IsLocked = 0" +
                " order by ta.TestAppointmentID desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            cmd.Parameters.AddWithValue("@testType", testTypeID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    isFound = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool DosePassTestType(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select top 1 t.TestResult from LocalDrivingLicense ldl inner join TestAppointment ta" +
                " ON ldl.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseID inner join Tests t" +
                " ON ta.TestAppointmentID = t.TestAppointmentID where ldl.LocalDrivingLicenseApplicationID = @localId AND ta.TestTypeID = @testType" +
                " order by ta.TestAppointmentID desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
            cmd.Parameters.AddWithValue("@testType", testTypeID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && bool.TryParse(result.ToString(), out bool testResult))
                {
                    isFound = testResult;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetLastTest(int localId, int typeId, ref int testId, ref int appointmentId, ref bool testResult, ref string notes, ref int userId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select top 1 T.* from Tests T inner join TestAppointment TA ON TA.TestAppointmentID = T.TestAppointmentID where TA.LocalDrivingLicenseID = @localId and TA.TestTypeID = @typeId order by T.TestID desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localId);
            cmd.Parameters.AddWithValue("@typeId", typeId);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    testId = (int)reader[0];
                    appointmentId = (int)reader[1];
                    testResult = (bool)reader[2];
                    if (reader[3] != DBNull.Value)
                        notes = (string)reader[3];
                    else
                        notes = "";
                    userId = (int)reader[4];
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

        public static byte GetPassedTestCount(int localLicenseID)
        {
            byte PassedCount = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select PassedCount = count(TestID) from Tests t inner join TestAppointment ta ON t.TestAppointmentID = ta.TestAppointmentID" +
                " where ta.LocalDrivingLicenseID = @localId AND TestResult = 1";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localLicenseID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && byte.TryParse(result.ToString(), out byte count))
                {
                    PassedCount = count;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return PassedCount;
        }
    }
}
