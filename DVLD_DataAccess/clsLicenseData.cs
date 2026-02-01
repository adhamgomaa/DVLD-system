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
        public static int AddNewLicense(int driverId, int AppId, int classId, DateTime issueDate, DateTime expiredDate, decimal fees,
            string notes, bool isActive, byte issueReason, int userId)
        {
            int licenseId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert into Licenses Values (@driverId, @appId, @classId, @issue, @expire, @fees, @note, @active, @reason, @userId); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@appId", AppId);
            command.Parameters.AddWithValue("@classId", classId);
            command.Parameters.AddWithValue("@issue", issueDate);
            command.Parameters.AddWithValue("@expire", expiredDate);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@note", notes);
            command.Parameters.AddWithValue("@active", isActive);
            command.Parameters.AddWithValue("@reason", issueReason);
            command.Parameters.AddWithValue("@userId", userId);
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

        public static bool UpdateLicense(int licenseId, int driverId, int AppId, int classId, DateTime issueDate, DateTime expiredDate, decimal fees,
           string notes, bool isActive, byte issueReason, int userId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "update Licenses set DriverID = @driverId, ApplicationID = @appId, LicenseClassID = @classId, IssuDate = @issue, ExpirationDate = @expired, PaidFees = @fees, Notes = @notes, IsActive = @active, IssueReason = @reason, CreatedByUserID = @userId Where LicenseID = @licenseId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverId", driverId);
            command.Parameters.AddWithValue("@appId", AppId);
            command.Parameters.AddWithValue("@classId", classId);
            command.Parameters.AddWithValue("@issue", issueDate);
            command.Parameters.AddWithValue("@expired", expiredDate);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@notes", notes);
            command.Parameters.AddWithValue("@active", isActive);
            command.Parameters.AddWithValue("@reason", issueReason);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@licenseId", licenseId);
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

        public static bool FindLicense(int licenseId, ref int driverId, ref int AppId, ref int classId, ref DateTime issueDate,
            ref DateTime expiredDate, ref decimal fees, ref string notes, ref bool isActive, ref byte issueReason, ref int userId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * from Licenses where LicenseID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", licenseId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public static int GetLicenseID(int localId)
        {
            int licenseID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select l.LicenseID from Licenses l inner join LocalDrivingLicense ldl ON l.ApplicationID = ldl.ApplicationID Where ldl.LocalDrivingLicenseApplicationID = @localId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localId", localId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    licenseID = id;
            }
            catch (Exception ex)
            {
                licenseID = -1;
            }
            finally
            {
                connection.Close();
            }
            return licenseID;
        }

        public static int GetLicenseID(string nationalNo)
        {
            int licenseID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select LicenseID from Licenses l inner join Driver D ON D.DriverID = l.DriverID inner join People p on p.PersonID = D.PersonID where NationalNo = @nationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    licenseID = id;
            }
            catch (Exception ex)
            {
                licenseID = -1;
            }
            finally
            {
                connection.Close();
            }
            return licenseID;
        }

        public static DataTable GetLocalLicenseHistory(int driverId)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select LicenseID AS 'Lic.ID', ApplicationID AS 'App.ID', lc.ClassName AS 'Class Name', l.IssuDate AS 'Issue Date', l.ExpirationDate AS 'Expiration Date', l.IsActive AS 'Is Active'  from Licenses l inner join LicenseClass lc ON l.LicenseClassID = lc.LicenseClassID where l.DriverID = @driverId";
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

        public static bool DeactivateLicense(int licenseId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "update Licenses set IsActive = 0 Where LicenseID = @licenseId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseId", licenseId);
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

        public static int GetActiveLicenseWithLicenseClass(int personId, int licenseClassId)
        {
            int LicenseId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select l.LicenseID from Licenses l inner join Driver d ON l.DriverID = d.DriverID Where d.PersonID = @personId AND l.LicenseClassID = @classId  AND l.IsActive = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personId", personId);
            command.Parameters.AddWithValue("@classId", licenseClassId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    LicenseId = id;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
            return LicenseId;
        }
    }
}
