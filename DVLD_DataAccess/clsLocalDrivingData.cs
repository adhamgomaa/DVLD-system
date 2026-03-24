 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingData
    {
        public static int AddNewLocal(int personId, int appType, int status, DateTime statusDate, decimal fees, int userId, int classId)
        {
            int localId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CreateNewLocalDrivingLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", appType);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@classId", classId);
                        SqlParameter outputId = new SqlParameter("@localId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        localId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                localId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return localId;
        }

        public static bool UpdateLocal(int personId, int appType, int status, DateTime statusDate, decimal fees, int userId, int localId, int appId, int classId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateLocalDrivingLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", appType);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@appId", appId);
                        command.Parameters.AddWithValue("@classId", classId);
                        command.Parameters.AddWithValue("@localId", localId);
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

        public static bool DeleteLocal(int localId, int appId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteLocalDrivingLicense", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@appId", appId);
                        cmd.Parameters.AddWithValue("@localId", localId);
                        connection.Open();
                        rowAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }

        public static bool CheckPersonHasSameClass(int personId, int classId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CheckPersonHasSameClass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@classId", classId);
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

        public static DataTable GetAllLocalLicenses()
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetAllLocalLicenses", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return table;
        }

        public static bool CancelLicense(int localLicenseAppID)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("SP_CancelLicense", connection))
                    {
                        cmd.CommandType=CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        connection.Open();
                        rowAffected = cmd.ExecuteNonQuery();
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

        public static bool FindLocalLicense(int localLicenseAppID, ref int appId, ref int classId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_FindLocalDrivingLicenseById", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                appId = (int)reader["ApplicationID"];
                                classId = (int)reader["LicenseClassID"];
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

        public static bool DoseAttendTestType(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DoesAttendTestType", connection))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        cmd.Parameters.AddWithValue("@testTypeId", testTypeID);
                        connection.Open();
                        isFound = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }

        public static byte TotalTrialsPerTest(int localLicenseAppID, int testTypeID)
        {
            byte Totaltrials = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_TotalTrialsPerTest", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        cmd.Parameters.AddWithValue("@testTypeId", testTypeID);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                            Totaltrials = Trials;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return Totaltrials;
        }

        public static bool IsThereAnActiveScheduleTest(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsThereAnActiveScheduleTest", connection))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        cmd.Parameters.AddWithValue("@testTypeId", testTypeID);
                        connection.Open();
                        isFound = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }

        public static bool DosePassTestType(int localLicenseAppID, int testTypeID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DoesAttendTestType", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseAppID);
                        cmd.Parameters.AddWithValue("@testTypeId", testTypeID);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool testResult))
                            isFound = testResult;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }

        public static bool GetLastTest(int localId, int typeId, ref int testId, ref int appointmentId, ref bool testResult, ref string notes, ref int userId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetLastTest", connection))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localId);
                        cmd.Parameters.AddWithValue("@testTypeId", typeId);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
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

        public static byte GetPassedTestCount(int localLicenseID)
        {
            byte PassedCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPassedTestCount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localLicenseID);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && byte.TryParse(result.ToString(), out byte count))
                        {
                            PassedCount = count;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return PassedCount;
        }
    }
}
