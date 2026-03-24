using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        public static int AddNewLicense(int driverId, int AppId, int classId, DateTime expiredDate, decimal fees,
            string notes, bool isActive, byte issueReason, int userId)
        {
            int licenseId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CreateLocalLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@appId", AppId);
                        command.Parameters.AddWithValue("@classId", classId);
                        command.Parameters.AddWithValue("@expireDate", expiredDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@notes", notes);
                        command.Parameters.AddWithValue("@isActive", isActive);
                        command.Parameters.AddWithValue("@issueReason", issueReason);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlParameter outputId = new SqlParameter("@licenseId", SqlDbType.Int);
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

        public static bool UpdateLicense(int licenseId, int driverId, int AppId, int classId, DateTime issueDate, DateTime expiredDate,
            decimal fees, string notes, bool isActive, byte issueReason, int userId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateLocalLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@appId", AppId);
                        command.Parameters.AddWithValue("@classId", classId);
                        command.Parameters.AddWithValue("@issueDate", issueDate);
                        command.Parameters.AddWithValue("@expireDate", expiredDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@notes", notes);
                        command.Parameters.AddWithValue("@isActive", isActive);
                        command.Parameters.AddWithValue("@issueReason", issueReason);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }

        public static bool FindLicense(int licenseId, ref int driverId, ref int AppId, ref int classId, ref DateTime issueDate,
            ref DateTime expiredDate, ref decimal fees, ref string notes, ref bool isActive, ref byte issueReason, ref int userId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindLicenseById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                driverId = (int)reader["DriverID"];
                                AppId = (int)reader["ApplicationID"];
                                classId = (int)reader["LicenseClassID"];
                                issueDate = (DateTime)reader["IssuDate"];
                                expiredDate = (DateTime)reader["ExpirationDate"];
                                fees = (decimal)reader["PaidFees"];
                                if (reader["Notes"] != DBNull.Value)
                                    notes = (string)reader["Notes"];
                                else
                                    notes = "";
                                isActive = (bool)reader["IsActive"];
                                issueReason = (byte)reader["IssueReason"];
                                userId = (int)reader["CreatedByUserID"];
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

        public static int GetLicenseID(int localId)
        {
            int licenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetLicenseIdByLocalId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@localId", localId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            licenseID = id;
                    }
                }
            }
            catch (SqlException ex)
            {
                licenseID = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return licenseID;
        }

        public static int GetLicenseID(string nationalNo)
        {
            int licenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetLicenseIdByNationalNo", connection))
                    {
                        command.CommandType= CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@nationalNo", nationalNo);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            licenseID = id;
                    }
                }
            }
            catch (SqlException ex)
            {
                licenseID = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return licenseID;
        }

        public static DataTable GetLocalLicenseHistory(int driverId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetLocalLicenseHistory", connection))
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

        public static bool DeactivateLicense(int licenseId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeactivateLicense", connection))
                    {
                        command.CommandType= CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }

        public static int GetActiveLicenseWithLicenseClass(int personId, int licenseClassId)
        {
            int LicenseId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetActiveLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@classId", licenseClassId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            LicenseId = id;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return LicenseId;
        }
    }
}
