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
        public static int AddNewDetain(int licenseId, DateTime detainDate, decimal detainFees, int userId)
        {
            int DetainId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert into DetainedLicenses (LicenseID, DetainDate, DetainFees, CreatedByUserID, IsReleased) Values (@licenseId, @detainDate, @detainFees, @userId, 0); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseId", licenseId);
            command.Parameters.AddWithValue("@detainDate", detainDate);
            command.Parameters.AddWithValue("@detainFees", detainFees);
            command.Parameters.AddWithValue("@userId", userId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    DetainId = insertedId;
                }
            }
            catch (Exception ex)
            {
                DetainId = -1;
            }
            finally
            {
                connection.Close();
            }
            return DetainId;
        }

        public static bool UpdateDetainLicense(int DetainId, int licenseId, DateTime detainDate, decimal detainFees, int userId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "update DetainedLicenses set LicenseID = @licenseId, DetainDate = @detainDate, DetainFees = @detainFees, CreatedByUserID = @userId Where DetainID = @detainId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseId", licenseId);
            command.Parameters.AddWithValue("@detainDate", detainDate);
            command.Parameters.AddWithValue("@detainFees", detainFees);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@detainId", DetainId);
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

        public static bool FindLicense(int DetainId, ref int licenseId, ref DateTime detainDate, ref decimal detainFees, ref int userId,
            ref bool isReleased, ref DateTime releaseDate, ref int releasedByUserId, ref int releaseAppId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * from DetainedLicenses where DetainID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", DetainId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    licenseId = (int)reader["LicenseID"];
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

        public static bool FindLicense(ref int DetainId, int licenseId, ref DateTime detainDate, ref decimal detainFees, ref int userId,
           ref bool isReleased, ref DateTime releaseDate, ref int releasedByUserId, ref int releaseAppId)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT top 1 * from DetainedLicenses where LicenseID = @id order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", licenseId);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public static DataTable GetDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select DetainID AS 'D.ID', dl.LicenseID AS 'L.ID', DetainDate AS 'D.Date', IsReleased AS 'Is Released', DetainFees AS 'Fine Fees', ReleaseDate AS 'Release Date', p.NationalNo AS 'N.No.', (p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName) AS 'Full Name', ReleaseApplicationID AS 'Release App.ID' from DetainedLicenses dl\r\ninner join Licenses l ON l.LicenseID = dl.LicenseID\r\ninner join Driver d ON l.DriverID = d.DriverID\r\ninner join People p ON p.PersonID = d.PersonID";
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

        public static bool ReleaseDetainedLicense(int DetainedId, int releaseByUserId, int releaseAppId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "update DetainedLicenses set IsReleased = 1, ReleaseDate = @releaseDate, ReleasedByUserID = @releaseUserId, ReleaseApplicationID = @releaseId Where DetainID = @detainId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@releaseDate", DateTime.Now);
            command.Parameters.AddWithValue("@releaseUserId", releaseByUserId);
            command.Parameters.AddWithValue("@releaseId", releaseAppId);
            command.Parameters.AddWithValue("@detainId", DetainedId);
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

        public static bool IsDetainedLicense(int licenseId)
        {
            bool isDetained = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select IsDetained = 1 From DetainedLicenses where IsReleased = 0 AND LicenseID = @licenseId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseId", licenseId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    isDetained = Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return isDetained;
        }
    }
}
