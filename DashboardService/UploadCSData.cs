using System;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;


namespace DashboardService
{
    public class UploadCSData
    {
        private readonly HttpClient _httpClient;

        public UploadCSData()
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
                cmd.CommandText = "fsrep_portfolio_analysis_sau";

                // Add parameters to the stored procedure
                cmd.Parameters.Add("i_company_id", OracleDbType.Varchar2).Value = "GTF";
                cmd.Parameters.Add("i_bus_type", OracleDbType.Varchar2).Value = "ALL";
                cmd.Parameters.Add("i_sub_bus_type", OracleDbType.Varchar2).Value = "ALL";
                cmd.Parameters.Add("i_branch_id", OracleDbType.Int32).Value = 0;
                cmd.Parameters.Add("i_date_from", OracleDbType.Varchar2).Value = "1 APR 2024";
                cmd.Parameters.Add("i_date_to", OracleDbType.Varchar2).Value = DateTime.Now.ToString("dd MMM yyyy");
                cmd.Parameters.Add("i_login_id", OracleDbType.Varchar2).Value = "AD2";
                cmd.Parameters.Add("errnumber", OracleDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("errmessage", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("io_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("i_npa_status", OracleDbType.Varchar2).Value = 2;
                cmd.Parameters.Add("i_period_fun", OracleDbType.Varchar2).Value = 0;

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
                    ColumnName = "FileDate",
                    DataType = typeof(DateTime),
                    DefaultValue = date
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "BUS_TYPE_NEW",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "CLIENT_AREA_NEW",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "FIU_IN_CR",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "AMOUNT_YTD_CR",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "AMOUNT_MTD_CR",
                    DataType = typeof(string),
                    DefaultValue = null
                });

                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "YEILD",
                    DataType = typeof(string),
                    DefaultValue = null
                });
                foreach (DataRow row in dt.Rows)
                {
                    decimal fin = row["FIU_IN_INR"] != DBNull.Value ? Convert.ToDecimal(row["FIU_IN_INR"]) : 0;
                    row["FIU_IN_CR"] = fin / 10000000;
                    decimal ytd = row["AMOUNT_YTD"] != DBNull.Value ? Convert.ToDecimal(row["AMOUNT_YTD"]) : 0;
                    row["AMOUNT_YTD_CR"] = ytd / 10000000;
                    decimal mtd = row["AMOUNT_MTD"] != DBNull.Value ? Convert.ToDecimal(row["AMOUNT_MTD"]) : 0;
                    row["AMOUNT_MTD_CR"] = (mtd / 10000000) + (ytd / 10000000);
                    decimal roi = row["ROI"] != DBNull.Value ? Convert.ToDecimal(row["ROI"]) : 0;
                    row["YEILD"] = roi * fin;
                    // Check conditions
                    if (row["BUS_TYPE"].ToString() == "RF")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "RF";
                    }
                    if (row["BUS_TYPE"].ToString() == "DPDM" && row["SUBPRODUCTTYPE"].ToString() == "NA")
                    {
                        // Set value for the new column
                        row["CLIENT_AREA_NEW"] = "HO";
                    }
                    if (row["BUS_TYPE"].ToString() == "RF" && row["SUBPRODUCTTYPE"].ToString() != "NA")
                    {
                        // Set value for the new column
                        row["CLIENT_AREA_NEW"] = "HO";
                        row["BUS_TYPE_NEW"] = "TREDS";
                    }
                    else
                    {
                        row["CLIENT_AREA_NEW"] = row["CLIENT_AREA"];
                    }
                    if (row["BUS_TYPE"].ToString() == "DADM" || row["BUS_TYPE"].ToString() == "DF" || row["BUS_TYPE"].ToString() == "PO" || row["BUS_TYPE"].ToString() == "VFF")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "DF";
                    }
                    if (row["BUS_TYPE"].ToString() == "EF" || row["BUS_TYPE"].ToString() == "DAEX" || row["BUS_TYPE"].ToString() == "DPEX")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "EF";
                    }
                    if (row["BUS_TYPE"].ToString() == "DPDM")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "GOLD POOL";
                    }
                    if (row["BUS_TYPE"].ToString() == "LCDM")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "LCDM";
                    }
                    if (row["BUS_TYPE"].ToString() == "LCEX")
                    {
                        // Set value for the new column
                        row["BUS_TYPE_NEW"] = "LCEX";
                    }
                }


                string conString2 = ConfigurationManager.AppSettings["ConStr1"];
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();

                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM dbo.tbl_Portfolio_analisys WHERE CAST(FileDate AS DATE) = '" + date + "'", con))
                    {
                        int recordCount = (int)checkCmd.ExecuteScalar();
                        if (recordCount > 0)
                        {
                            // Data for the current day already exists, skip the insertion
                            return "Data for the current day already exists.";
                        }
                    }
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_Portfolio_analisys";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_ID", "CLIENT_ID");
                        sqlBulkCopy.ColumnMappings.Add("SL_LIMIT", "SL_LIMIT");
                        sqlBulkCopy.ColumnMappings.Add("FIU_LIMIT", "FIU_LIMIT");
                        sqlBulkCopy.ColumnMappings.Add("PROVISION_PERCENTAGE", "PROVISION_PERCENTAGE");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_NAME", "CLIENT_NAME");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_AREA", "CLIENT_AREA");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_STATUS", "CLIENT_STATUS");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_NPA", "CLIENT_NPA");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_RATING", "CLIENT_RATING");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_CODE_DESC", "CLIENT_CODE_DESC");
                        sqlBulkCopy.ColumnMappings.Add("BUS_TYPE", "BUS_TYPE");
                        sqlBulkCopy.ColumnMappings.Add("SUBPRODUCTTYPE", "SUBPRODUCTTYPE");
                        sqlBulkCopy.ColumnMappings.Add("CURRENCY", "CURRENCY");
                        sqlBulkCopy.ColumnMappings.Add("FACTOR_TYPE", "FACTOR_TYPE");
                        sqlBulkCopy.ColumnMappings.Add("COMPANY_ID_PK", "COMPANY_ID_PK");
                        sqlBulkCopy.ColumnMappings.Add("FACTORS_RETENTION", "FACTORS_RETENTION");
                        sqlBulkCopy.ColumnMappings.Add("UNALLOCATE_AMOUNT", "UNALLOCATE_AMOUNT");
                        sqlBulkCopy.ColumnMappings.Add("FIU_OUT", "FIU_OUT");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_YTM_CUR", "AMOUNT_YTM_CUR");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_YTD", "AMOUNT_YTD");
                        sqlBulkCopy.ColumnMappings.Add("ALLOCATEAMOUNT_YTD", "ALLOCATEAMOUNT_YTD");
                        sqlBulkCopy.ColumnMappings.Add("BALANCE_AMOUNT", "BALANCE_AMOUNT");
                        sqlBulkCopy.ColumnMappings.Add("APPROVED_AMOUNT", "APPROVED_AMOUNT");
                        sqlBulkCopy.ColumnMappings.Add("DISAPP", "DISAPP");
                        sqlBulkCopy.ColumnMappings.Add("DISPUTED_AMOUNT", "DISPUTED_AMOUNT");
                        sqlBulkCopy.ColumnMappings.Add("FTG_CHG_YTD", "FTG_CHG_YTD");
                        sqlBulkCopy.ColumnMappings.Add("BRANCH_CODE_PK", "BRANCH_CODE_PK");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_MTD_CUR", "AMOUNT_MTD_CUR");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_MTD", "AMOUNT_MTD");
                        sqlBulkCopy.ColumnMappings.Add("ALLOCATEAMOUNT_MTD", "ALLOCATEAMOUNT_MTD");
                        sqlBulkCopy.ColumnMappings.Add("FTG_CHG_MTD", "FTG_CHG_MTD");
                        sqlBulkCopy.ColumnMappings.Add("COMPANY_NAME", "COMPANY_NAME");
                        sqlBulkCopy.ColumnMappings.Add("DEDUCT", "DEDUCT");
                        sqlBulkCopy.ColumnMappings.Add("BASE_CURRENCY_CD", "BASE_CURRENCY_CD");
                        sqlBulkCopy.ColumnMappings.Add("RATE_TO_BASE_CURRENCY", "RATE_TO_BASE_CURRENCY");
                        sqlBulkCopy.ColumnMappings.Add("FIU_IN_INR", "FIU_IN_INR");
                        sqlBulkCopy.ColumnMappings.Add("BDM_NAME", "BDM_NAME");
                        sqlBulkCopy.ColumnMappings.Add("DISCOUNT_TYPE", "DISCOUNT_TYPE");
                        sqlBulkCopy.ColumnMappings.Add("ADD_1", "ADD_1");
                        sqlBulkCopy.ColumnMappings.Add("ADD_2", "ADD_2");
                        sqlBulkCopy.ColumnMappings.Add("ADD_3", "ADD_3");
                        sqlBulkCopy.ColumnMappings.Add("ADD_4", "ADD_4");
                        sqlBulkCopy.ColumnMappings.Add("ADD_5", "ADD_5");
                        sqlBulkCopy.ColumnMappings.Add("ADD_6", "ADD_6");
                        sqlBulkCopy.ColumnMappings.Add("ADD_7", "ADD_7");
                        sqlBulkCopy.ColumnMappings.Add("ADD_8", "ADD_8");
                        sqlBulkCopy.ColumnMappings.Add("ADD_9", "ADD_9");
                        sqlBulkCopy.ColumnMappings.Add("ADD_10", "ADD_10");
                        sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");
                        sqlBulkCopy.ColumnMappings.Add("BUS_TYPE_NEW", "BUS_TYPE_NEW");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_AREA_NEW", "CLIENT_AREA_NEW");
                        sqlBulkCopy.ColumnMappings.Add("FIU_IN_CR", "FIU_IN_CR");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_YTD_CR", "AMOUNT_YTD_CR");
                        sqlBulkCopy.ColumnMappings.Add("AMOUNT_MTD_CR", "AMOUNT_MTD_CR");
                        sqlBulkCopy.ColumnMappings.Add("ROI", "ROI");
                        sqlBulkCopy.ColumnMappings.Add("YEILD", "YEILD");
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/Insert?f_date=" + date + "&f_filename=ClientService&f_count=" + dt.Rows.Count + "&f_docid=4DDB3983-A80C-464F-93D6-263F8AABF0ED").Result;
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