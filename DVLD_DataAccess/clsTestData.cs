using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static int AddNewTest(int appointment, bool testResult, string notes, int userId)
        {
            int testId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewTest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@appointmentId", appointment);
                        command.Parameters.AddWithValue("@result", testResult);
                        command.Parameters.AddWithValue("@notes", notes);
                        command.Parameters.AddWithValue("@userId", userId);
                        SqlParameter outputId = new SqlParameter("@testId", SqlDbType.Int);
                        outputId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputId);
                        connection.Open();
                        command.ExecuteNonQuery();
                        testId = (int)outputId.Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return testId;
        }

        public static bool UpdateTest(int testId, int appointment, bool testResult, string notes, int userId)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateTest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@appointmentId", appointment);
                        command.Parameters.AddWithValue("@result", testResult);
                        command.Parameters.AddWithValue("@notes", notes);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@testId", testId);
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

        public static bool FindTest(int appointment, ref int testId, ref bool testResult, ref string notes, ref int userId)
        {
            bool isFoud = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_FindTestById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@testId", appointment);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFoud = true;
                                testId = (int)reader[0];
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
                isFoud = false;
                clsLogger.LoggingAllExepctions(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return isFoud;
        }
    }
}
