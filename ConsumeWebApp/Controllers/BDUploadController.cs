using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using PrintSoftWeb.Models;
using System.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO.Packaging;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;

namespace PrintSoftWeb.Controllers
{
    public class BDUploadController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BDUploadController> _logger;
        private readonly IStringLocalizer<BDUploadController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public BDUploadController(ILogger<BDUploadController> logger, IStringLocalizer<BDUploadController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
            Environment = _environment;
            Configuration = configuration;
        }

        public IActionResult Index(string? bd_date)
        {
            try
            {
                if (bd_date == null)
                {
                    bd_date = DateTime.Now.ToString();
                }
                var FileUploadDataList = new List<FileUpload>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDUpload/GetAll?user_id=" + new Guid(userid) + "&bd_date=" + bd_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<FileUpload>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    if (FileUploadDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return View(FileUploadDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var FileUploadDataList2 = new List<FileUpload>();
                        return View(FileUploadDataList2);
                    }
                }
                return View(FileUploadDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        public JsonResult Search(string? bd_date)
        {
            try
            {

                var FileUploadDataList = new List<FileUpload>(); ;
                string? userid = Request.Cookies["com_id"];

                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDUpload/GetAll?user_id=" + new Guid(userid) + "&bd_date=" + bd_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<FileUpload>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    if (FileUploadDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return Json(FileUploadDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var FileUploadDataList2 = new List<FileUpload>();
                        return Json(FileUploadDataList2);
                    }
                }
                return Json(FileUploadDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Json("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        //   public IActionResult Upload(IFormFile postedFile, string? date, string? month)
        //   {
        //       try
        //       {
        //           string? userid = Request.Cookies["com_id"];
        //           if (userid == null)
        //           {
        //               return RedirectToAction("Index", "Login");
        //           }
        //           if (postedFile != null)
        //           {
        //               //Create a Folder.
        //               string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
        //               if (!Directory.Exists(path))
        //               {
        //                   Directory.CreateDirectory(path);
        //               }

        //               //Save the uploaded Excel file.
        //               string fileName = Path.GetFileName(postedFile.FileName);
        //               string filePath = Path.Combine(path, fileName);
        //               using (FileStream stream = new FileStream(filePath, FileMode.Create))
        //               {
        //                   postedFile.CopyTo(stream);
        //               }

        //               //Read the connection string for the Excel file.

        //               string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        //               DataTable summery = new DataTable();
        //               DataTable nbo = new DataTable();
        //               DataTable fsa = new DataTable();
        //               DataTable WithCredit = new DataTable();
        //               DataTable Sanctioned = new DataTable();
        //               DataTable DisbursalNew = new DataTable();
        //               conString = string.Format(conString, filePath);

        //               using (OleDbConnection connExcel = new OleDbConnection(conString))
        //               {
        //                   using (OleDbCommand cmdExcel = new OleDbCommand())
        //                   {
        //                       using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                       {
        //                           cmdExcel.Connection = connExcel;

        //                           //Get the name of First Sheet.
        //                           connExcel.Open();
        //                           DataTable dtExcelSchema;
        //                           dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                           string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                           string sheetName1 = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
        //                           string sheetName2 = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
        //                           string sheetName3 = dtExcelSchema.Rows[3]["TABLE_NAME"].ToString();
        //                           string sheetName4 = dtExcelSchema.Rows[4]["TABLE_NAME"].ToString();
        //                           string sheetName5 = dtExcelSchema.Rows[5]["TABLE_NAME"].ToString();
        //                           connExcel.Close();

        //                           //Read Data from First Sheet.
        //                           connExcel.Open();
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName4 + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(summery);
        //			if (summery.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName4;
        //				return RedirectToAction("Index");
        //			}
        //			summery.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName2 + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(nbo);
        //			if (nbo.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName2;
        //				return RedirectToAction("Index");
        //			}
        //			nbo.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName1 + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(fsa);
        //			if (fsa.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName1;
        //				return RedirectToAction("Index");
        //			}
        //			fsa.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName5 + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(WithCredit);
        //			if (WithCredit.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName5;
        //				return RedirectToAction("Index");
        //			}
        //			WithCredit.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName3 + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(Sanctioned);
        //			if (Sanctioned.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName3;
        //				return RedirectToAction("Index");
        //			}
        //			Sanctioned.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });
        //                           cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //                           odaExcel.SelectCommand = cmdExcel;
        //                           odaExcel.Fill(DisbursalNew);
        //			if (DisbursalNew.Rows.Count == 0)
        //			{
        //				TempData["errorMessage"] = "No data present in sheet: " + sheetName;
        //				return RedirectToAction("Index");
        //			}
        //			DisbursalNew.Columns.Add(new DataColumn
        //                           {
        //                               ColumnName = "Date",
        //                               DataType = typeof(string),
        //                               DefaultValue = date
        //                           });

        //                           connExcel.Close();
        //                       }
        //                   }
        //}

        ////Insert the Data read from the Excel file to Database Table.
        //conString = Configuration.GetSection("Server:Constring").Value;
        //               using (SqlConnection con = new SqlConnection(conString))
        //               {
        //                   using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //                   {
        //                       sqlBulkCopy.BulkCopyTimeout = 0;
        //                       //Set the database table name.



        //                       sqlBulkCopy.DestinationTableName = "tbl_summary";

        //		//[OPTIONAL]: Map the Excel columns with that of the database table.
        //		sqlBulkCopy.ColumnMappings.Add("Branch", "Branch");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals Sourced (NBO Stage) (No)", "NBOStageNo");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals Sourced (NBO Stage) (Limit)", "NBOStageLimit");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals sourced (FSA Stage) (No)", "FSAStageNo");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals sourced (FSA Stage) (Limit)", "FSAStageLimit");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals With Branches (For Query resolution / Deferred) (No)", "PWithBranchesNo");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals With Branches (For Query resolution / Deferred) Limit", "PWithBranchesLimit");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals with credit (No)", "withcreditNo");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals with credit (Limit)", "withcreditLimit");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals sanctioned In (Likely to be activated) (No)", "sanctionedInNo");
        //		sqlBulkCopy.ColumnMappings.Add("Proposals sanctioned In (Likely to be activated) (Limit)", "sanctionedInLimit");
        //		sqlBulkCopy.ColumnMappings.Add("Expected Disbursals in new sanctions (Number)", "DisbursalsNumber");
        //		sqlBulkCopy.ColumnMappings.Add("Expected Disbursals in new sanctions (Amount)", "DisbursalsAmount");
        //		sqlBulkCopy.ColumnMappings.Add("Expected Disbursals in new sanctions (By Date)", "DisbursalsByDate");
        //		sqlBulkCopy.ColumnMappings.Add("Disbursals in Accounts", "DisbursalsinAccounts");
        //		sqlBulkCopy.ColumnMappings.Add("Date", "bd_date");
        //                       con.Open();
        //                       sqlBulkCopy.WriteToServer(summery);
        //                       con.Close();

        //                       SqlBulkCopy sqlbc = new SqlBulkCopy(con);
        //                       sqlbc.DestinationTableName = "tbl_NBO";
        //                       sqlbc.ColumnMappings.Add("Location", "Location");
        //                       sqlbc.ColumnMappings.Add("BDM", "BDM");
        //                       sqlbc.ColumnMappings.Add("Client", "Client");
        //                       sqlbc.ColumnMappings.Add("Proposed Limit", "Proposed_Limit");
        //                       sqlbc.ColumnMappings.Add("Facility", "Facility");
        //                       sqlbc.ColumnMappings.Add("Stage", "Stage");
        //                       sqlbc.ColumnMappings.Add("NBO Issued date", "NBO_Issued_date");
        //                       sqlbc.ColumnMappings.Add("NBO acceptance date", "NBO_acceptance_date");
        //                       sqlbc.ColumnMappings.Add("Updates", "Updates");
        //                       sqlbc.ColumnMappings.Add("Date", "bd_date");
        //                       con.Open();
        //                       sqlbc.WriteToServer(nbo);
        //                       con.Close();

        //                       SqlBulkCopy sqlBulkCopy1 = new SqlBulkCopy(con);
        //                       sqlBulkCopy1.DestinationTableName = "tbl_FSA";
        //                       sqlBulkCopy1.ColumnMappings.Add("Location", "Location");
        //                       sqlBulkCopy1.ColumnMappings.Add("BDM", "BDM");
        //                       sqlBulkCopy1.ColumnMappings.Add("Client", "Client");
        //                       sqlBulkCopy1.ColumnMappings.Add("Proposed Limit", "Proposed_Limit");
        //                       sqlBulkCopy1.ColumnMappings.Add("Facility", "Facility");
        //                       sqlBulkCopy1.ColumnMappings.Add("Stage", "Stage");
        //                       sqlBulkCopy1.ColumnMappings.Add("FSA initiated date", "FSA_in_date");
        //                       sqlBulkCopy1.ColumnMappings.Add("FSA completion date", "FSA_comp_date");
        //                       sqlBulkCopy1.ColumnMappings.Add("Timelime For Pushing to credit", "Timelime_Pushing_credit");
        //                       sqlBulkCopy1.ColumnMappings.Add("Updates", "Updates");
        //                       sqlBulkCopy1.ColumnMappings.Add("Date", "bd_date");
        //                       con.Open();
        //                       sqlBulkCopy1.WriteToServer(fsa);
        //                       con.Close();

        //                       SqlBulkCopy sbc = new SqlBulkCopy(con);
        //                       sbc.DestinationTableName = "tbl_WithCredit";
        //                       sbc.ColumnMappings.Add("Location", "Location");
        //                       sbc.ColumnMappings.Add("BDM", "BDM");
        //                       sbc.ColumnMappings.Add("Client", "Client");
        //                       sbc.ColumnMappings.Add("Credit officer", "Credit_officer");
        //                       sbc.ColumnMappings.Add("Proposed Limit", "Proposed_Limit");
        //                       sbc.ColumnMappings.Add("Debtors", "Debtors");
        //                       sbc.ColumnMappings.Add("Date of submission to credit(mm/dd/yyyy)", "Date_of_submission");
        //                       sbc.ColumnMappings.Add("Days with Credit", "Days_Credit");
        //                       sbc.ColumnMappings.Add("Level", "Level");
        //                       sbc.ColumnMappings.Add("Updates", "Updates");
        //                       sbc.ColumnMappings.Add("Date", "bd_date");
        //                       con.Open();
        //                       sbc.WriteToServer(WithCredit);
        //                       con.Close();


        //                       SqlBulkCopy sqlBulkC = new SqlBulkCopy(con);
        //                       sqlBulkC.DestinationTableName = "tbl_Sanctioned";
        //                       sqlBulkC.ColumnMappings.Add("Location", "Location");
        //                       sqlBulkC.ColumnMappings.Add("Client", "Client");
        //                       sqlBulkC.ColumnMappings.Add("Sanctioned Limit", "Sanctioned_Limit");
        //                       sqlBulkC.ColumnMappings.Add("Facility", "Facility");
        //                       sqlBulkC.ColumnMappings.Add("Month", "Month");
        //                       sqlBulkC.ColumnMappings.Add("Activation Date (Operationalised) expected By", "Activation_Date");
        //                       sqlBulkC.ColumnMappings.Add("Sanctioned on", "Sanctioned_on");
        //                       sqlBulkC.ColumnMappings.Add("Updates", "Updates");
        //                       sqlBulkC.ColumnMappings.Add("Date", "bd_date");

        //                       con.Open();
        //                       sqlBulkC.WriteToServer(Sanctioned);
        //                       con.Close();


        //                       SqlBulkCopy sqlBulkC1 = new SqlBulkCopy(con);
        //                       sqlBulkC1.DestinationTableName = "tbl_DisbursalNew";
        //                       sqlBulkC1.ColumnMappings.Add("Location", "Location");
        //                       sqlBulkC1.ColumnMappings.Add("Client", "Client");
        //                       sqlBulkC1.ColumnMappings.Add("Sanctioned Limit", "Sanctioned_Limit");
        //                       sqlBulkC1.ColumnMappings.Add("Sanctioned date", "Sanctioned_date");
        //                       sqlBulkC1.ColumnMappings.Add("Facility", "Facility");
        //                       sqlBulkC1.ColumnMappings.Add("Disbursal Amount", "Disbursal_Amount");
        //                       sqlBulkC1.ColumnMappings.Add("Expected By", "Expected_By");
        //                       sqlBulkC1.ColumnMappings.Add("Remark", "Remark");
        //                       sqlBulkC1.ColumnMappings.Add("Updates", "Updates");
        //                       sqlBulkC1.ColumnMappings.Add("Date", "bd_date");
        //                       con.Open();
        //                       sqlBulkC1.WriteToServer(DisbursalNew);
        //                       con.Close();

        //                   }
        //               }
        //               var FileUploadDataList = new List<FileUpload>(); ;
        //               Guid? UserId = new Guid(userid);
        //               HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDUpload/Insert?f_date=" + date + "&f_filename=" + fileName + "&f_count=" + summery.Rows.Count + "&f_docid=B651E039-14DB-417B-931D-6096F467B796").Result;
        //               if (response.IsSuccessStatusCode)
        //               {
        //                  TempData["successMessage"] = "File uploaded successfully!";
        //                   return RedirectToAction("Index");

        //               }
        //               TempData["errorMessage"] = response.Headers.ToString();
        //               return RedirectToAction("Index");

        //           }
        //           TempData["errorMessage"] ="Data Not Found!";
        //           return RedirectToAction("Index");
        //       }
        //       catch (Exception ex)
        //       {
        //           TempData["errorMessage"] = ex.Message;
        //           return RedirectToAction("Index");
        //       }
        //   }
        public IActionResult Upload(IFormFile postedFile, string? date, string? month)
        {
            try
            {
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    //Read the connection string for the Excel file.

                    string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    DataTable summery = new DataTable();
                    DataTable stc = new DataTable();
                    DataTable wb = new DataTable();
                    DataTable Sanctioned = new DataTable();

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string sheetName1 = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                string sheetName2 = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                string sheetName3 = dtExcelSchema.Rows[3]["TABLE_NAME"].ToString();


                                connExcel.Close();

                                //Read Data from First Sheet.//summary
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName2 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(summery);
                                if (summery.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName2;
                                    return RedirectToAction("Index");
                                }
                                summery.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });

                                cmdExcel.CommandText = "SELECT * From [" + sheetName1 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(stc);
                                if (stc.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName1;
                                    return RedirectToAction("Index");
                                }
                                stc.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName3 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(wb);
                                if (wb.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName3;
                                    return RedirectToAction("Index");
                                }
                                wb.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });

                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(Sanctioned);
                                if (Sanctioned.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName;
                                    return RedirectToAction("Index");
                                }
                                Sanctioned.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });


                                connExcel.Close();
                            }
                        }
                    }

                    //Insert the Data read from the Excel file to Database Table.
                    conString = Configuration.GetSection("Server:Constring").Value;
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.BulkCopyTimeout = 0;
                            //Set the database table name.



                            sqlBulkCopy.DestinationTableName = "tbl_BDsummary";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("Branch", "Branch");
                            sqlBulkCopy.ColumnMappings.Add("New/ Enhancement Proposals Sourced To Credit (No)", "NewEPSCNo");
                            sqlBulkCopy.ColumnMappings.Add("New/ Enhancement Proposals Sourced To Credit (Limit)", "NewEPSCLimit");
                            sqlBulkCopy.ColumnMappings.Add("New Sanctions(No)", "NewSancNo");
                            sqlBulkCopy.ColumnMappings.Add("New Sanctions(Limit)", "NewSancLimit");
                            sqlBulkCopy.ColumnMappings.Add("Activation (No)", "ActNo");
                            sqlBulkCopy.ColumnMappings.Add("Activation (Limit)", "ActLimit");
                            sqlBulkCopy.ColumnMappings.Add("Sanctions Yet To Be Activated (No)", "SancActNo");
                            sqlBulkCopy.ColumnMappings.Add("Sanctions Yet To Be Activated (Limit) ", "SancActLimit");
                            sqlBulkCopy.ColumnMappings.Add("Proposals With Credit (No)", "ProCrNo");
                            sqlBulkCopy.ColumnMappings.Add("Proposals With Credit (Limit)", "ProCrLimit");
                            sqlBulkCopy.ColumnMappings.Add("Proposals with Branches (No)", "ProBrNo");
                            sqlBulkCopy.ColumnMappings.Add("Proposals with Branches (Limit)", "ProBrlimit");
                            sqlBulkCopy.ColumnMappings.Add("Estimated FIU Growth", "EstFIU");
                            sqlBulkCopy.ColumnMappings.Add("Date", "bd_date");
                            con.Open();
                            sqlBulkCopy.WriteToServer(summery);
                            con.Close();

                            SqlBulkCopy sqlbc = new SqlBulkCopy(con);
                            sqlbc.DestinationTableName = "tbl_BDSubToCr";
                            sqlbc.ColumnMappings.Add("Name Of The Client", "NameOfClient");
                            sqlbc.ColumnMappings.Add("New / Existing", "NewExist");
                            sqlbc.ColumnMappings.Add("Status", "Status");
                            sqlbc.ColumnMappings.Add("Facility", "Facility");
                            sqlbc.ColumnMappings.Add("FIU Limit Sanctioned / Recommended", "FLS");
                            sqlbc.ColumnMappings.Add("Date Submitted To Credit", "Sub_date");
                            sqlbc.ColumnMappings.Add("Credit Analyst", "CreditAnalyst");
                            sqlbc.ColumnMappings.Add("Credit Priority For May", "CreditPriority");
                            sqlbc.ColumnMappings.Add("Expected Disbursement In May", "ExpDis");
                            sqlbc.ColumnMappings.Add("Remarks", "Remarks");
                            sqlbc.ColumnMappings.Add("Date", "bd_date");
                            con.Open();
                            sqlbc.WriteToServer(stc);
                            con.Close();



                            SqlBulkCopy sqlBulkCopy1 = new SqlBulkCopy(con);
                            sqlBulkCopy1.DestinationTableName = "tbl_BDBrQuRl";
                            sqlBulkCopy1.ColumnMappings.Add("Branch", "Branch");
                            sqlBulkCopy1.ColumnMappings.Add("Name Of The Client", "NameOfClient");
                            sqlBulkCopy1.ColumnMappings.Add("New / Existing", "NewExist");
                            sqlBulkCopy1.ColumnMappings.Add("Status", "Status");
                            sqlBulkCopy1.ColumnMappings.Add("Facility", "Facility");
                            sqlBulkCopy1.ColumnMappings.Add("FIU Limit Sanctioned / Recommended", "FLS");
                            sqlBulkCopy1.ColumnMappings.Add("Date Submitted To Credit", "Sub_date");
                            sqlBulkCopy1.ColumnMappings.Add("Credit Analyst", "CreditAnalyst");
                            sqlBulkCopy1.ColumnMappings.Add("Credit Priority For May", "CreditPriority");
                            sqlBulkCopy1.ColumnMappings.Add("Expected Disbursement In May", "ExpDis");
                            sqlBulkCopy1.ColumnMappings.Add("Remarks", "Remarks");
                            sqlBulkCopy1.ColumnMappings.Add("Date", "bd_date");
                            con.Open();
                            sqlBulkCopy1.WriteToServer(wb);
                            con.Close();








                            SqlBulkCopy sqlBulkC = new SqlBulkCopy(con);
                            sqlBulkC.DestinationTableName = "tbl_BDSanctioned";
                            sqlBulkC.ColumnMappings.Add("Branch", "Branch");
                            sqlBulkC.ColumnMappings.Add("Name Of The Client", "NameOfClient");
                            sqlBulkC.ColumnMappings.Add("New / Existing", "NewExist");
                            sqlBulkC.ColumnMappings.Add("Status", "Status");
                            sqlBulkC.ColumnMappings.Add("Facility", "Facility");
                            sqlBulkC.ColumnMappings.Add("FIU Limit Sanctioned / Recommended", "FLS");
                            sqlBulkC.ColumnMappings.Add("Date Submitted To Credit", "Sub_date");
                            sqlBulkC.ColumnMappings.Add("Credit Analyst", "CreditAnalyst");
                            sqlBulkC.ColumnMappings.Add("Credit Priority For May", "CreditPriority");
                            sqlBulkC.ColumnMappings.Add("Sanction Date", "SanDate");
                            sqlBulkC.ColumnMappings.Add("Expected Disbursement In May", "ExpDis");
                            sqlBulkC.ColumnMappings.Add("Remarks", "Remarks");
                            sqlBulkC.ColumnMappings.Add("Date", "bd_date");

                            con.Open();
                            sqlBulkC.WriteToServer(Sanctioned);
                            con.Close();



                        }
                    }
                    var FileUploadDataList = new List<FileUpload>(); ;
                    Guid? UserId = new Guid(userid);
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDUpload/Insert?f_date=" + date + "&f_filename=" + fileName + "&f_count=" + summery.Rows.Count + "&f_docid=B651E039-14DB-417B-931D-6096F467B796").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "File uploaded successfully!";
                        return RedirectToAction("Index");

                    }
                    TempData["errorMessage"] = response.Headers.ToString();
                    return RedirectToAction("Index");

                }
                TempData["errorMessage"] = "Data Not Found!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Excel()
        {

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Summary");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr. No.";
                        worksheet.Cell(currentRow, 2).Value = "Branch";
                        worksheet.Cell(currentRow, 3).Value = "New/ Enhancement Proposals Sourced To Credit (No)";
                        worksheet.Cell(currentRow, 4).Value = "New/ Enhancement Proposals Sourced To Credit (Limit)";
                        worksheet.Cell(currentRow, 5).Value = "New Sanctions(No)";
                        worksheet.Cell(currentRow, 6).Value = "New Sanctions(Limit)";
                        worksheet.Cell(currentRow, 7).Value = "Activation (No)";
                        worksheet.Cell(currentRow, 8).Value = "Activation (Limit)";
                        worksheet.Cell(currentRow, 9).Value = "Sanctions Yet To Be Activated (No)";
                        worksheet.Cell(currentRow, 10).Value = "Sanctions Yet To Be Activated (Limit) ";
                        worksheet.Cell(currentRow, 11).Value = "Proposals With Credit (No)";
                        worksheet.Cell(currentRow, 12).Value = "Proposals With Credit (Limit)";
                        worksheet.Cell(currentRow, 13).Value = "Proposals with Branches (No)";
                        worksheet.Cell(currentRow, 14).Value = "Proposals with Branches (Limit)";
                        worksheet.Cell(currentRow, 15).Value = "Estimated FIU Growth";



                        var worksheet1 = workbook.Worksheets.Add("SubmittedToCredit");
                        var currentRow1 = 1;
                        worksheet1.Cell(currentRow1, 1).Value = "Name Of The Client";
                        worksheet1.Cell(currentRow1, 2).Value = "New / Existing";
                        worksheet1.Cell(currentRow1, 3).Value = "Status";
                        worksheet1.Cell(currentRow1, 4).Value = "Facility";
                        worksheet1.Cell(currentRow1, 5).Value = "FIU Limit Sanctioned / Recommended";
                        worksheet1.Cell(currentRow1, 6).Value = "Date Submitted To Credit";
                        worksheet1.Cell(currentRow1, 7).Value = "Credit Analyst";
                        worksheet1.Cell(currentRow1, 8).Value = "Credit Priority For May";
                        worksheet1.Cell(currentRow1, 9).Value = "Expected Disbursement In May";
                        worksheet1.Cell(currentRow1, 10).Value = "Remarks";

                        var worksheet2 = workbook.Worksheets.Add("WithBranchForQueryResolution");
                        var currentRow2 = 1;
                        worksheet2.Cell(currentRow2, 1).Value = "Sr.No.";
                        worksheet2.Cell(currentRow2, 2).Value = "Branch";
                        worksheet2.Cell(currentRow2, 3).Value = "Name Of The Client";
                        worksheet2.Cell(currentRow2, 4).Value = "New / Existing";
                        worksheet2.Cell(currentRow2, 5).Value = "Status";
                        worksheet2.Cell(currentRow2, 6).Value = "Facility";
                        worksheet2.Cell(currentRow2, 7).Value = "FIU Limit Sanctioned / Recommended";
                        worksheet2.Cell(currentRow2, 8).Value = "Date Submitted To Credit";
                        worksheet2.Cell(currentRow2, 9).Value = "Credit Analyst";
                        worksheet2.Cell(currentRow2, 10).Value = "Credit Priority For May";
                        worksheet2.Cell(currentRow2, 11).Value = "Expected Disbursement In May";
                        worksheet2.Cell(currentRow2, 12).Value = "Remarks";

                        var worksheet3 = workbook.Worksheets.Add("SanctionedPendingActivation");
                        var currentRow3 = 1;
                        worksheet3.Cell(currentRow3, 1).Value = "Sr.No.";
                        worksheet3.Cell(currentRow3, 2).Value = "Branch";
                        worksheet3.Cell(currentRow3, 3).Value = "Name Of The Client";
                        worksheet3.Cell(currentRow3, 4).Value = "New / Existing";
                        worksheet3.Cell(currentRow3, 5).Value = "Status";
                        worksheet3.Cell(currentRow3, 6).Value = "Facility";
                        worksheet3.Cell(currentRow3, 7).Value = "FIU Limit Sanctioned / Recommended";
                        worksheet3.Cell(currentRow3, 8).Value = "Date Submitted To Credit";
                        worksheet3.Cell(currentRow3, 9).Value = "Credit Analyst";
                        worksheet3.Cell(currentRow3, 10).Value = "Credit Priority For May";
                        worksheet3.Cell(currentRow3, 11).Value = "Sanction Date";
                        worksheet3.Cell(currentRow3, 12).Value = "Expected Disbursement In May";
                        worksheet3.Cell(currentRow3, 13).Value = "Remarks";


                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "BusinessDevlopment.xlsx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Delete(Guid? f_id, string bd_date)
        {
            try
            {
                FileUpload model = new FileUpload();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                model.UserId = UserId;
                model.f_id = f_id;
                model.f_date = bd_date;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/BDUpload/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
