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
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Insert into Tests Values (@appointment, @result, @notes, @userId); SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appointment", appointment);
            command.Parameters.AddWithValue("@result", testResult);
            command.Parameters.AddWithValue("@notes", notes);
            command.Parameters.AddWithValue("@userId", userId);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedIn))
                    testId = insertedIn;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return testId;
        }

        public static bool UpdateTest(int testId, int appointment, bool testResult, string notes, int userId)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Update Tests set TestAppointmentID = @appointment, TestResult = @result, Notes = @notes, CreatedByUserID = @userId Where TestID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appointment", appointment);
            command.Parameters.AddWithValue("@result", testResult);
            command.Parameters.AddWithValue("@notes", notes);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@id", testId);
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

        public static bool FindTest(int appointment, ref int testId, ref bool testResult, ref string notes, ref int userId)
        {
            bool isFoud = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * From Tests Where TestAppointmentID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", appointment);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        
    }
}
