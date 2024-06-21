using System;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;


namespace DashboardService
{
    public class UploadDebtData
    {
        private readonly HttpClient _httpClient;

        public UploadDebtData()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"]);

        }
        public async Task<string> GetValues()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string conString = ConfigurationManager.AppSettings["ConStr2"];
                //string conString = Configuration.GetSection("Server:Excel").Value;
                DataTable dt = new DataTable();
                //conString = string.Format(conString, filePath);
                // #region oracle
                //using (OracleConnection conn = new OracleConnection(conString))
                //{
                OracleConnection conn = new OracleConnection(conString);
                conn.Open(); // Open the connection

                // Your Oracle SQL command goes here
                //using (OracleCommand cmd = conn.CreateCommand())
                //{
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                // Set the name of the stored procedure
                cmd.CommandText = "fsresp_client_sma_report";
                //cmd.CommandTimeout = 8000;
                cmd.Parameters.Add("i_company_id", OracleDbType.Varchar2).Value = "GTF";
                cmd.Parameters.Add("i_business_type", OracleDbType.Varchar2).Value = "ALL";
                cmd.Parameters.Add("i_client_id", OracleDbType.Decimal).Value = 0;
                cmd.Parameters.Add("i_client_account_from", OracleDbType.Varchar2).Value = "1";
                cmd.Parameters.Add("i_client_account_to", OracleDbType.Varchar2).Value = "1";
                cmd.Parameters.Add("i_date", OracleDbType.Date).Value = Convert.ToDateTime("05/27/2024");
                cmd.Parameters.Add("i_bgfcl_branch", OracleDbType.Decimal).Value = 0;
                cmd.Parameters.Add("i_reporttype", OracleDbType.Varchar2).Value = "DET";
                cmd.Parameters.Add("i_login_id", OracleDbType.Varchar2).Value = "AD2";
                cmd.Parameters.Add("errnumber", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("errmessage", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("i_sub_bus_type", OracleDbType.Varchar2).Value = "ALL";
                cmd.Parameters.Add("i_entity_wise_rep", OracleDbType.Varchar2).Value = "Y";
                cmd.Parameters.Add("io_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                // Execute the stored procedure
                cmd.ExecuteNonQuery();
                conn.Close();
                //cmd.CommandText = "SELECT CLIENT_ID,CLIENT_NAME,CLIENT_AREA,CLIENT_STATUS,CLIENT_NPA,CLIENT_RATING,CLIENT_CODE_DESC,BUS_TYPE,SUBPRODUCTTYPE,CURRENCY,FACTOR_TYPE,COMPANY_ID_PK,FACTORS_RETENTION,UNALLOCATE_AMOUNT,SL_LIMIT,FIU_LIMIT,PROVISION_PERCENTAGE,FIU_OUT,AMOUNT_YTM_CUR,AMOUNT_YTD,ALLOCATEAMOUNT_YTD,BALANCE_AMOUNT,APPROVED_AMOUNT,DISAPP,DISPUTED_AMOUNT,FTG_CHG_YTD,BRANCH_CODE_PK,AMOUNT_MTD_CUR,AMOUNT_MTD,ALLOCATEAMOUNT_MTD,FTG_CHG_MTD,COMPANY_NAME,DEDUCT,BASE_CURRENCY_CD,RATE_TO_BASE_CURRENCY,FIU_IN_INR,BDM_NAME,DISCOUNT_TYPE,ROI,ADD_1,ADD_2,ADD_3,ADD_4,ADD_5,ADD_6,ADD_7,ADD_8,ADD_9,ADD_10 FROM tbl_Portfolio_analisys";

                // Your Oracle DataAdapter goes here
                //using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                //{
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                // conn.Open();
                //adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                //}
                // }
                //}
                //#endregion
                //Read Data from First Sheet.

                //dt.Columns.Remove("FileDate");
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "FILE_DATE",
                    DataType = typeof(string),
                    DefaultValue = date
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "PRODUCT",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "PRINC_OUTSTANDING_CR",
                    DataType = typeof(string),
                    DefaultValue = null
                });

                foreach (DataRow row in dt.Rows)
                {
                    decimal maxdays = row["MAX_OVERDUE_DAYS"] != DBNull.Value ? Convert.ToDecimal(row["MAX_OVERDUE_DAYS"]) : 0;
                    if (maxdays <= 30)
                    {
                        row["PRODUCT"] = "SMA0";
                    }
                    else if (maxdays <= 60)
                    {
                        row["PRODUCT"] = "SMA1";
                    }
                    else
                    {
                        row["PRODUCT"] = "SMA2";
                    }

                    decimal cr = row["PRINC_OUTSTANDING_INR"] != DBNull.Value ? Convert.ToDecimal(row["PRINC_OUTSTANDING_INR"]) : 0;
                    row["PRINC_OUTSTANDING_CR"] = cr / 10000000;
                }


                string conString2 = ConfigurationManager.AppSettings["ConStr1"];
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_sma";

                        sqlBulkCopy.ColumnMappings.Add("CLIENT_NAME", "CLIENT_NAME");
                        sqlBulkCopy.ColumnMappings.Add("BUSINESS_TYPE", "BUSINESS_TYPE");
                        sqlBulkCopy.ColumnMappings.Add("SUBPRODUCT_CODE_PK", "SUBPRODUCT_CODE_PK");
                        sqlBulkCopy.ColumnMappings.Add("OVERDUE_BUCKET", "OVERDUE_BUCKET");
                        sqlBulkCopy.ColumnMappings.Add("REGION_NAME", "REGION_NAME");
                        sqlBulkCopy.ColumnMappings.Add("ACCOUNT_LIMIT", "ACCOUNT_LIMIT");
                        sqlBulkCopy.ColumnMappings.Add("FIU_OUTSTANDING", "FIU_OUTSTANDING");
                        sqlBulkCopy.ColumnMappings.Add("SL_OUTSTANDING", "SL_OUTSTANDING");
                        sqlBulkCopy.ColumnMappings.Add("SL_OUTSTANDING_INR", "SL_OUTSTANDING_INR");
                        sqlBulkCopy.ColumnMappings.Add("FIU_OVERDUE", "FIU_OVERDUE");
                        sqlBulkCopy.ColumnMappings.Add("FIU_OVERDUE_INR", "FIU_OVERDUE_INR");
                        sqlBulkCopy.ColumnMappings.Add("PENDING_INTEREST_AMT", "PENDING_INTEREST_AMT");
                        sqlBulkCopy.ColumnMappings.Add("PENDING_INTEREST_AMT_INR", "PENDING_INTEREST_AMT_INR");
                        sqlBulkCopy.ColumnMappings.Add("PENDING_CHARGES_AMT", "PENDING_CHARGES_AMT");
                        sqlBulkCopy.ColumnMappings.Add("PENDING_CHARGES_AMT_INR", "PENDING_CHARGES_AMT_INR");
                        sqlBulkCopy.ColumnMappings.Add("INTR_RECOVER_TILL_DT", "INTR_RECOVER_TILL_DT");
                        sqlBulkCopy.ColumnMappings.Add("MAX_OVERDUE_DAYS", "MAX_OVERDUE_DAYS");
                        sqlBulkCopy.ColumnMappings.Add("DEBTOR_NAMES_WHEREIN_SL_OS", "DEBTOR_NAMES_WHEREIN_SL_OS");
                        sqlBulkCopy.ColumnMappings.Add("RECOURSE_DAYS", "RECOURSE_DAYS");
                        sqlBulkCopy.ColumnMappings.Add("PAN_NO", "PAN_NO");
                        sqlBulkCopy.ColumnMappings.Add("PRINC_OUTSTANDING", "PRINC_OUTSTANDING");
                        sqlBulkCopy.ColumnMappings.Add("PRINC_OUTSTANDING_INR", "PRINC_OUTSTANDING_INR");
                        sqlBulkCopy.ColumnMappings.Add("FILE_DATE", "s_filedate");
                        sqlBulkCopy.ColumnMappings.Add("PRODUCT", "PRODUCT");
                        sqlBulkCopy.ColumnMappings.Add("PRINC_OUTSTANDING_CR", "PRINC_OUTSTANDING_CR");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                HttpResponseMessage response = await _httpClient.GetAsync($"/Debt/Insert?date={date}&d_filename=sma&d_count={dt.Rows.Count}&d_filetype=SMA");
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;

                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog el = new ErrorLog();
                el.WriteError(ex + " Error Time " + DateTime.Now);
                return null;
            }
        }

    }
}