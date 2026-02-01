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
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select [Appointment ID] = TA.TestAppointmentID, [Appointment Date] = TA.AppointmentDate, [Paid Fees] = TA.PaidFees, [Is Locked] = TA.IsLocked from TestAppointment TA where TA.LocalDrivingLicenseID = @localId and TA.TestTypeID = @typeId order by TA.TestAppointmentID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localId", localId);
            command.Parameters.AddWithValue("@typeId", typeId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static int AddNewAppointment(int testType, int localId, DateTime date, decimal fees, int userId, bool locked, int retake)
        {
            int AppId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert into TestAppointment Values (@testId, @localId, @date, @fees, @userId, @locked, @retake); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testId", testType);
            command.Parameters.AddWithValue("@localId", localId);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@locked", locked);
            if(retake == -1)
                command.Parameters.AddWithValue("@retake", DBNull.Value);
            else
                command.Parameters.AddWithValue("@retake", retake);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedIn))
                    AppId = insertedIn;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return AppId;
        }
        
        public static bool UpdateAppointment(int appId, int testType, int localId, DateTime date, decimal fees, int userId, bool locked)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Update TestAppointment set TestTypeID = @testId, LocalDrivingLicenseID = @localId, AppointmentDate = @date, PaidFees = @fees, CreatedByUserID = @userId, IsLocked = @locked Where TestAppointmentID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testId", testType);
            command.Parameters.AddWithValue("@localId", localId);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@locked", locked);
            command.Parameters.AddWithValue("@id", appId);
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return rowAffected > 0;
        }

        public static bool FindAppointment(int appId, ref int testType, ref int localId, ref DateTime date, ref decimal fees, ref int userId,
            ref bool locked, ref int retake)
        {
            bool isFoud = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * From TestAppointment Where TestAppointmentID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", appId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch (Exception ex)
            {
                isFoud = false;
            }
            finally
            {
                connection.Close();
            }
            return isFoud;
        }

        public static bool GetLastTestAppointment(int localId, int testType, ref int appId, ref DateTime date, ref decimal fees, ref int userId,
           ref bool locked, ref int retake)
        {
            bool isFoud = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select top 1 * from TestAppointment TA where TA.LocalDrivingLicenseID = @localId and TA.TestTypeID = @typeId order by TA.TestAppointmentID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localId", localId);
            command.Parameters.AddWithValue("@typeId", testType);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch (Exception ex)
            {
                isFoud = false;
            }
            finally
            {
                connection.Close();
            }
            return isFoud;
        }

        public static int GetTrials(int localId, int typeId)
        {
            int trials = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select Count(TestAppointmentID) as 'Trial' from TestAppointment inner join LocalDrivingLicenseApplications ldla On TestAppointment.LocalDrivingLicenseID = ldla.[L.D.L.AppID] where TestAppointment.LocalDrivingLicenseID = @localId and TestAppointment.TestTypeID = @typeId Group by ldla.[Full Name]";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localId);
            cmd.Parameters.AddWithValue("@typeId", typeId);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int count))
                    trials = count;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return trials;
        }

        public static bool AppointmentIsLock(int localId, int typeId)
        {
            bool isLock = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select top 1 TA.IsLocked from TestAppointment TA where TA.LocalDrivingLicenseID = @localId and TA.TestTypeID = @typeId order by TA.TestAppointmentID desc";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@localId", localId);
            cmd.Parameters.AddWithValue("@typeId", typeId);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && bool.TryParse(result.ToString(), out bool Lock))
                    if(Lock)
                        isLock = true;
            }
            catch (Exception ex)
            {
                isLock = false;
            }
            finally
            {
                connection.Close();
            }
            return isLock;
        }

        public static int GetTestID(int testAppointmentID)
        {
            int testID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select TestID from Tests Where TestAppointmentID = @appointment";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@appointment", testAppointmentID);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    testID = id;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return testID;
        }
    }
}
