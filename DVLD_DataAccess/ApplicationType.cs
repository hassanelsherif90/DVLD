using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
            ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                        ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                    }
                    else
                    {
                        // The record was not found
                        isFound = false;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            return dt;

        } 

        public static int AddNewApplicationType( string Title, float Fees)
        {
            int ApplicationTypeID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    /*string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";*/
                    string query = @"DECLARE ApplicationTypeID INT
                                    SP_AddNewApplicationType
	                                @ApplicationTypeTitle = @ApplicationTypeTitle,
	                                @ApplicationFees = @ApplicationFees,
	                                @ApplicationTypeID =  @ApplicationTypeID OUTPUT;
                                    SELECT @ApplicationTypeID AS ApplicationTypeID;
                                       ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationTypeID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }
            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string Title, float Fees)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);
           
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

    }
}
