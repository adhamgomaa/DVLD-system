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
        public static int AddNewApp(int personId, DateTime date, int type, int status, DateTime statusDate, decimal fees, int userId)
        {
            int appId = -1;
            try
            {
               using( SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Insert into Application Values (@pId, @date, @type, @status, @sDate, @fees, @userId); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pId", personId);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@sDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedId))
                        {
                            appId = insertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                appId = -1;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return appId;
        }

        public static bool UpdateApp(int appId, int personId, DateTime date, int type, int status, DateTime statusDate, decimal fees, int userId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "update Application set ApplicantPersonID = @pId, ApplicationDate = @date, ApplicationTypesID = @type, ApplicationStatus = @status, LastStatusDate = @sDate, PaidFees = @fees, CreatedByUserID = @userId where ApplicationID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pId", personId);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@sDate", statusDate);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@id", appId);
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
                    connection.Open();
                    string query = "Delete from Application where ApplicationID = @id";
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", appId);
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
                    connection.Open();
                    string query = "select * from Application where ApplicationID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
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

        public static bool setComplete(int applicationId, byte status)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "update Application set ApplicationStatus = @status, LastStatusDate = @sDate where ApplicationID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@sDate", DateTime.Now);
                        command.Parameters.AddWithValue("@id", applicationId);
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
    }
}
