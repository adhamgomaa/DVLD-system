using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        public static int AddNewInternationalLicense(int AppId, int driverId, DateTime issueDate, DateTime expiredDate, int userId,
            bool isActive, int LocalLicensID)
        {
            int licenseId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Update InternationalLicenses set IsActive = 0 where DriverID = @driverId" +
                "  Insert into InternationalLicenses Values (@appId, @driverId, @issue, @expire, @userId, @active, @local); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appId", AppId);
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@issue", issueDate);
            command.Parameters.AddWithValue("@expire", expiredDate);
            command.Parameters.AddWithValue("@active", isActive);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@local", LocalLicensID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    licenseId = insertedId;
                }
            }
            catch (Exception ex)
            {
                licenseId = -1;
            }
            finally
            {
                connection.Close();
            }
            return licenseId;
        }

        public static bool FindLicense(int licenseId, ref int AppId, ref int driverId, ref DateTime issueDate,
            ref DateTime expiredDate, ref int userId, ref bool isActive, ref int LocalLicensID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * from InternationalLicenses where InternationalLicenseID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", licenseId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public static bool FindLicense(ref int licenseId, ref int AppId, ref int driverId, ref DateTime issueDate,
            ref DateTime expiredDate, ref int userId, ref bool isActive, int LocalLicensID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * from InternationalLicenses where IssuedUsingLocalLicenseID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", LocalLicensID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select internationalLicenseID AS 'Int.License ID', ApplicationID AS 'Application ID', IssuedUsingLocalLicenseID AS 'L.License ID' , DriverID AS 'Driver ID', IssueDate AS 'Issue Date', ExpirationDate AS 'Expiration Date', IsActive AS 'Is Active' from InternationalLicenses";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return dt;
        }

        public static DataTable GetInternaionalLicenseHistory(int driverId)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select InternationalLicenseID AS 'Int.License ID', ApplicationID AS 'Application ID', IssuedUsingLocalLicenseID AS 'L.License ID', IssueDate AS 'Issue Date', ExpirationDate AS 'Expiration Date', IsActive AS 'Is Active' from InternationalLicenses where DriverID = @driverId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverId", driverId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }
            return dt;
        }

        public static int GetActiveInternationalLicenseIDByDriverId(int driverId)
        {
            int internationlLicenseId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select top 1 InternationalLicenseID from InternationalLicenses where DriverID = @driverId and IsActive = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverId", driverId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    internationlLicenseId = id;
                }
            }catch (Exception ex) { }
            finally { connection.Close(); }
            return internationlLicenseId;
        }
    }
}
