using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsAppData
    {
        public static int AddNewApp(int personId, int type, int status, DateTime statusDate, decimal fees, int userId)
        {
            int appId = -1;
            try
            {
               using( SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CreateNewApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", type);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlParameter outputId = new SqlParameter("@appId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        appId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                appId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return appId;
        }

        public static bool UpdateApp(int appId, int personId, int type, int status, DateTime statusDate, decimal fees, int userId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@personId", personId);
                        command.Parameters.AddWithValue("@appType", type);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@statusDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@appId", appId);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                rowAffected = 0;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }

        public static bool DeleteApp(int appId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using(SqlCommand cmd = new SqlCommand("SP_DeleteApp", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@appId", appId);
                        connection.Open();
                        rowAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                rowAffected = 0;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return rowAffected > 0;
        }
        public static bool FindApp(int id, ref int personId, ref DateTime date, ref int type, ref byte status, ref DateTime statusDate,
            ref decimal fees, ref int userId)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindAppById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@appId", id);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                personId = (int)reader["ApplicantPersonID"];
                                date = (DateTime)reader["ApplicationDate"];
                                type = (int)reader["ApplicationTypesID"];
                                status = (byte)reader["ApplicationStatus"];
                                statusDate = (DateTime)reader["LastStatusDate"];
                                fees = (decimal)reader["PaidFees"];
                                userId = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFound;
        }
    }
}
