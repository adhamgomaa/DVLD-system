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
    public class clsInternationalLicenseData
    {
        public static int AddNewInternationalLicense(int personId, int appType, int status, DateTime statusDate, decimal fees,
            int driverId, DateTime expiredDate, int userId, bool isActive, int LocalLicensID)
        {
            int licenseId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CreateInternationalLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", appType);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@expireDate", expiredDate);
                        command.Parameters.AddWithValue("@isActive", isActive);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@localLicenseId", LocalLicensID);
                        SqlParameter outputId = new SqlParameter("@internationalLicenseId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        licenseId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                licenseId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return licenseId;
        }

        public static bool FindLicense(int licenseId, ref int AppId, ref int driverId, ref DateTime issueDate,
            ref DateTime expiredDate, ref int userId, ref bool isActive, ref int LocalLicensID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindInternationalLicenseById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                AppId = (int)reader["ApplicationID"];
                                driverId = (int)reader["DriverID"];
                                issueDate = (DateTime)reader["IssueDate"];
                                expiredDate = (DateTime)reader["ExpirationDate"];
                                userId = (int)reader["CreatedByUserID"];
                                isActive = (bool)reader["IsActive"];
                                LocalLicensID = (int)reader["IssuedUsingLocalLicenseID"];
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

        public static bool FindLicense(ref int licenseId, ref int AppId, ref int driverId, ref DateTime issueDate,
            ref DateTime expiredDate, ref int userId, ref bool isActive, int LocalLicensID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindInternationalLicenseByLocalId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@localId", LocalLicensID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                licenseId = (int)reader["InternationalLicenseID"];
                                AppId = (int)reader["ApplicationID"];
                                driverId = (int)reader["DriverID"];
                                issueDate = (DateTime)reader["IssueDate"];
                                expiredDate = (DateTime)reader["ExpirationDate"];
                                userId = (int)reader["CreatedByUserID"];
                                isActive = (bool)reader["IsActive"];
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

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllInternationalLicenses", connection))
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

        public static DataTable GetInternaionalLicenseHistory(int driverId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllInternationalLicensesHistory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@driverId", driverId);
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

        public static int GetActiveInternationalLicenseIDByDriverId(int driverId)
        {
            int internationlLicenseId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetActiveInternationalLicenseByDriverId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@driverId", driverId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            internationlLicenseId = id;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return internationlLicenseId;
        }
    }
}
