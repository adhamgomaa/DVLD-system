using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DVLD_DataAccess
{
    public class clsDetainLicenseData
    {
        public static int AddNewDetain(int licenseId, decimal detainFees, int userId)
        {
            int DetainId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewDetain", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        command.Parameters.AddWithValue("@detainFees", detainFees);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlParameter outputId = new SqlParameter("@detainId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        DetainId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                DetainId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return DetainId;
        }

        public static bool UpdateDetainLicense(int DetainId, int licenseId, DateTime detainDate, decimal detainFees, int userId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateDetainLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        command.Parameters.AddWithValue("@detainDate", detainDate);
                        command.Parameters.AddWithValue("@detainFees", detainFees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@detainId", DetainId);
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

        public static bool FindLicense(ref int DetainId, int licenseId, ref DateTime detainDate, ref decimal detainFees, ref int userId,
           ref bool isReleased, ref DateTime releaseDate, ref int releasedByUserId, ref int releaseAppId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindDetainedLicenseByLicenseId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                DetainId = (int)reader["DetainID"];
                                detainDate = (DateTime)reader["DetainDate"];
                                detainFees = (decimal)reader["DetainFees"];
                                userId = (int)reader["CreatedByUserID"];
                                isReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] != DBNull.Value)
                                    releaseDate = (DateTime)reader["ReleaseDate"];
                                else
                                    releaseDate = DateTime.MaxValue;

                                if (reader["ReleasedByUserID"] != DBNull.Value)
                                    releasedByUserId = (int)reader["ReleasedByUserID"];
                                else
                                    releasedByUserId = -1;

                                if (reader["ReleaseApplicationID"] != DBNull.Value)
                                    releaseAppId = (int)reader["ReleaseApplicationID"];
                                else
                                    releaseAppId = -1;
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

        public static DataTable GetDetainedLicenses()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDetainedLicense", connection))
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

        public static int ReleaseDetainedLicense(int personId, int appType, int status, DateTime statusDate, decimal fees, int DetainedId, int releaseByUserId)
        {
            int releaseAppId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_ReleaseDetainedLicense", connection))
                    {
                        command.CommandType= CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", appType);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@releaseUserId", releaseByUserId);
                        command.Parameters.AddWithValue("@detainId", DetainedId);
                        SqlParameter outputId = new SqlParameter("@releaseAppId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        releaseAppId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return releaseAppId;
        }

        public static bool IsDetainedLicense(int licenseId)
        {
            bool isDetained = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsDetainedLicense", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@licenseId", licenseId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                            isDetained = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isDetained;
        }
    }
}
