using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Security.Policy;

namespace DashboardService
{
    public class UploadCreditData
    {
        private readonly HttpClient _httpClient;

        public UploadCreditData()
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
                DateTime date = DateTime.Now;
                string conString = ConfigurationManager.AppSettings["ConStr2"];
                //string conString = Configuration.GetSection("Server:Excel").Value;
                DataTable dt = new DataTable();
                //using (OracleConnection conn = new OracleConnection(conString))
                //{
                OracleConnection conn = new OracleConnection(conString);
                conn.Open(); // Open the connection

                // Your Oracle SQL command goes here
                //using (OracleCommand cmd = conn.CreateCommand())
                //{
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select fc.client_name,fca.client_account_cd,fca.status,br.branch_name,fca.cur_balance  AS FIU_Outstanding from fcm_client_account fca join fcm_client fc on fca.client_id_pk=fc.client_id_pk join fcs_company_branch br on br.branch_code_pk=fca.branch_code_pk where fca.status='A' and fca.deleted_by is null and fca.authorised_by is not null and fc.deleted_by is null and fc.authorised_by is not null";
                conn.Close();
                //using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                //{
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                //Read Data from First Sheet.
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
                //conn.Close();
                //Read Data from First Sheet.

                //dt.Columns.Remove("FileDate");
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = "FileDate",
                    DataType = typeof(DateTime),
                    DefaultValue = date
                });

                //        }
                //    }
                //}

                string conString2 = ConfigurationManager.AppSettings["ConStr1"];
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM dbo.tbl_CreditData WHERE CAST(FileDate AS DATE) = '"+ date+"'", con))
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
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_CreditData";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_NAME", "CLIENT_NAME");
                        sqlBulkCopy.ColumnMappings.Add("CLIENT_ACCOUNT_CD", "CLIENT_ACCOUNT_CD");
                        sqlBulkCopy.ColumnMappings.Add("STATUS", "STATUS");
                        sqlBulkCopy.ColumnMappings.Add("BRANCH_NAME", "BRANCH_NAME");
                        sqlBulkCopy.ColumnMappings.Add("FIU_OUTSTANDING", "FIU_OUTSTANDING");
                        sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                //Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                //HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/Insert?f_date=" + date + "&f_filename=CreditDetails&f_count=" + dt.Rows.Count + "&f_docid=4DDB3983-A80C-464F-93D6-263F8AABF0ED").Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    dynamic data = response.Content.ReadAsStringAsync().Result;

                //}
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
