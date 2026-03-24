using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static DataTable GetAllAppointment(int localId, int typeId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@localId", localId);
                        command.Parameters.AddWithValue("@typeId", typeId);
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

        public static int AddNewAppointment(int testType, int localId, DateTime date, decimal fees, int userId, int retake)
        {
            int AppId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CreateTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@typeId", testType);
                        command.Parameters.AddWithValue("@localId", localId);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        if (retake == -1)
                            command.Parameters.AddWithValue("@retakeId", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@retakeId", retake);
                        SqlParameter outputId = new SqlParameter("@testAppId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        AppId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return AppId;
        }
        
        public static bool UpdateAppointment(int appId, int testType, int localId, DateTime date, decimal fees, int userId, bool locked)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@typeId", testType);
                        command.Parameters.AddWithValue("@localId", localId);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@fees", fees);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@locked", locked);
                        command.Parameters.AddWithValue("@testAppId", appId);
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

        public static bool FindAppointment(int appId, ref int testType, ref int localId, ref DateTime date, ref decimal fees, ref int userId,
            ref bool locked, ref int retake)
        {
            bool isFoud = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@testAppId", appId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFoud = true;
                                testType = (int)reader[1];
                                localId = (int)reader[2];
                                date = (DateTime)reader[3];
                                fees = (decimal)reader[4];
                                userId = (int)reader[5];
                                locked = (bool)reader[6];
                                if (reader[7] != DBNull.Value)
                                    retake = (int)reader[7];
                                else
                                    retake = -1;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                isFoud = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFoud;
        }

        public static bool GetLastTestAppointment(int localId, int testType, ref int appId, ref DateTime date, ref decimal fees, ref int userId,
           ref bool locked, ref int retake)
        {
            bool isFoud = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetLastAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@localId", localId);
                        command.Parameters.AddWithValue("@typeId", testType);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFoud = true;
                                appId = (int)reader[0];
                                date = (DateTime)reader[3];
                                fees = (decimal)reader[4];
                                userId = (int)reader[5];
                                locked = (bool)reader[6];
                                if (reader[7] != DBNull.Value)
                                    retake = (int)reader[7];
                                else
                                    retake = -1;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                isFoud = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFoud;
        }

        public static int GetTrials(int localId, int typeId)
        {
            int trials = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTrials", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localId);
                        cmd.Parameters.AddWithValue("@typeId", typeId);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int count))
                            trials = count;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return trials;
        }

        public static bool AppointmentIsLock(int localId, int typeId)
        {
            bool isLock = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsAppointmentLock", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@localId", localId);
                        cmd.Parameters.AddWithValue("@typeId", typeId);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool Lock))
                                isLock = Lock;
                    }
                }
            }
            catch (SqlException ex)
            {
                isLock = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isLock;
        }

        public static int GetTestID(int testAppointmentID)
        {
            int testID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetTestId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@testAppId", testAppointmentID);
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            testID = id;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return testID;
        }
    }
}
