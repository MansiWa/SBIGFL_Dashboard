using ClosedXML.Excel;
using Common;
using PrintSoftWeb.Models;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using System.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using Amazon.S3;

namespace PrintSoftWeb.Controllers
{
    public class DeptController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DeptController> _logger;
        private readonly IStringLocalizer<DeptController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public DeptController(ILogger<DeptController> logger, IStringLocalizer<DeptController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
        public IActionResult Index(string? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now.ToString();
                }
                var FileUploadDataList = new List<Debt>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                List<Debt> FileUploadlist = new List<Debt>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAll?user_id=" + UserId + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Debt>() };
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
                        var FileUploadDataList2 = new List<Debt>();
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

        public JsonResult Search(string? date)
        {
            try
            {
                if (date == null)
                {
                    date = DateTime.Now.ToString();
                }
                var FileUploadDataList = new List<Debt>();
                string? userid = Request.Cookies["com_id"];

                Guid? UserId = new Guid(userid);
                List<Debt> FileUploadlist = new List<Debt>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAll?user_id=" + UserId + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Debt>() };
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
                        var FileUploadDataList2 = new List<Debt>();
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
        public IActionResult Upload(IFormFile postedFile, string? date)
        {
            try
            {
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

                    string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    DataTable sma = new DataTable();
                    DataTable npa = new DataTable();
                    DataTable woa = new DataTable();
                    conString = string.Format(conString, filePath);
                    string? userid = Request.Cookies["com_id"];
                    if (userid == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }
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
                                string sheetName1 = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string sheetName2 = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                string sheetName3 = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName1 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(npa);
                                npa.Columns.Add(new DataColumn
                                {
                                    ColumnName = "FILE_DATE",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName2 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(sma);
                                sma.Columns.Add(new DataColumn
                                {
                                    ColumnName = "FILE_DATE",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                sma.Columns.Add(new DataColumn
                                {
                                    ColumnName = "PRODUCT",
                                    DataType = typeof(string),
                                    DefaultValue = null
                                });
                                sma.Columns.Add(new DataColumn
                                {
                                    ColumnName = "PRINC_OUTSTANDING_CR",
                                    DataType = typeof(string),
                                    DefaultValue = null
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName3 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(woa);
                                woa.Columns.Add(new DataColumn
                                {
                                    ColumnName = "FILE_DATE",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });


                                foreach (DataRow row in sma.Rows)
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
                            sqlBulkCopy.DestinationTableName = "dbo.tbl_sma";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.

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
                            sqlBulkCopy.WriteToServer(sma);
                            con.Close();

                            SqlBulkCopy sbwoa = new SqlBulkCopy(con);
                            sbwoa.DestinationTableName = "dbo.tbl_woa";
                            sbwoa.ColumnMappings.Add("DEC", "DEC");
                            sbwoa.ColumnMappings.Add("REGION", "REGION");
                            sbwoa.ColumnMappings.Add("PRINCIPAL_WOA", "PRINCIPAL_WOA");
                            sbwoa.ColumnMappings.Add("DATE_NPA", "DATE_NPA");
                            sbwoa.ColumnMappings.Add("DATE_WOA", "DATE_WOA");
                            sbwoa.ColumnMappings.Add("FILE_DATE", "w_filedate");
                            con.Open();
                            sbwoa.WriteToServer(woa);
                            con.Close();

                            SqlBulkCopy sbnpa = new SqlBulkCopy(con);
                            sbnpa.DestinationTableName = "dbo.tbl_npa";
                            sbnpa.ColumnMappings.Add("CLIENT_NAME", "CLIENT_NAME");
                            sbnpa.ColumnMappings.Add("REGION", "REGION");
                            sbnpa.ColumnMappings.Add("PRINCIPAL_NPA", "PRINCIPAL_NPA");
                            sbnpa.ColumnMappings.Add("DATE_NPA", "DATE_NPA");
                            sbnpa.ColumnMappings.Add("FILE_DATE", "n_filedate");
                            con.Open();
                            sbnpa.WriteToServer(npa);
                            con.Close();

                        }
                    }
                    var FileUploadDataList = new List<Debt>();
                   
                    Guid? UserId = new Guid(userid);
                    List<Debt> FileUploadlist = new List<Debt>();
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/Insert?date=" + date + "&d_filename=" + fileName + "&d_count=" + sma.Rows.Count + "&d_filetype=SMA").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic data = response.Content.ReadAsStringAsync().Result;
                        var dataObject = new { data = new List<Debt>() };
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
                            var FileUploadDataList2 = new List<Debt>();
                            return View(FileUploadDataList2);
                        }
                    }

                    HttpResponseMessage response12 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/Insert?date=" + date + "&d_filename=" + fileName + "&d_count=" + npa.Rows.Count + "&d_filetype=NPA").Result;
                    if (response12.IsSuccessStatusCode)
                    {
                        dynamic data = response12.Content.ReadAsStringAsync().Result;
                        var dataObject = new { data = new List<Debt>() };
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
                            var FileUploadDataList2 = new List<Debt>();
                            return View(FileUploadDataList2);
                        }
                    }
                    HttpResponseMessage response13 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/Insert?date=" + date + "&d_filename=" + fileName + "&d_count=" + woa.Rows.Count + "&d_filetype=WOA").Result;
                    if (response13.IsSuccessStatusCode)
                    {
                        dynamic data = response13.Content.ReadAsStringAsync().Result;
                        var dataObject = new { data = new List<Debt>() };
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
                            var FileUploadDataList2 = new List<Debt>();
                            return View(FileUploadDataList2);
                        }
                    }
                }

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
                        var worksheet = workbook.Worksheets.Add("SMA");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "CLIENT_NAME";
                        worksheet.Cell(currentRow, 2).Value = "BUSINESS_TYPE";
                        worksheet.Cell(currentRow, 3).Value = "SUBPRODUCT_CODE_PK";
                        worksheet.Cell(currentRow, 4).Value = "OVERDUE_BUCKET";
                        worksheet.Cell(currentRow, 5).Value = "REGION_NAME";
                        worksheet.Cell(currentRow, 6).Value = "ACCOUNT_LIMIT";
                        worksheet.Cell(currentRow, 7).Value = "FIU_OUTSTANDING";
                        worksheet.Cell(currentRow, 8).Value = "SL_OUTSTANDING";
                        worksheet.Cell(currentRow, 9).Value = "SL_OUTSTANDING_INR";
                        worksheet.Cell(currentRow, 10).Value = "FIU_OVERDUE";
                        worksheet.Cell(currentRow, 11).Value = "FIU_OVERDUE_INR";
                        worksheet.Cell(currentRow, 12).Value = "PENDING_INTEREST_AMT";
                        worksheet.Cell(currentRow, 13).Value = "PENDING_INTEREST_AMT_INR";
                        worksheet.Cell(currentRow, 14).Value = "PENDING_CHARGES_AMT";
                        worksheet.Cell(currentRow, 15).Value = "PENDING_CHARGES_AMT_INR";
                        worksheet.Cell(currentRow, 16).Value = "INTR_RECOVER_TILL_DT";
                        worksheet.Cell(currentRow, 17).Value = "MAX_OVERDUE_DAYS";
                        worksheet.Cell(currentRow, 18).Value = "DEBTOR_NAMES_WHEREIN_SL_OS";
                        worksheet.Cell(currentRow, 19).Value = "RECOURSE_DAYS";
                        worksheet.Cell(currentRow, 20).Value = "PAN_NO";
                        worksheet.Cell(currentRow, 21).Value = "PRINC_OUTSTANDING";
                        worksheet.Cell(currentRow, 22).Value = "PRINC_OUTSTANDING_INR";

                        var worksheet1 = workbook.Worksheets.Add("NPA");
                        var currentRow1 = 1;
                        worksheet1.Cell(currentRow1, 1).Value = "CLIENT_NAME";
                        worksheet1.Cell(currentRow1, 2).Value = "REGION";
                        worksheet1.Cell(currentRow1, 3).Value = "PRINCIPAL_NPA";
                        worksheet1.Cell(currentRow1, 4).Value = "DATE_NPA";

                        var worksheet2 = workbook.Worksheets.Add("WOA");
                        var currentRow2 = 1;
                        worksheet2.Cell(currentRow2, 1).Value = "DEC";
                        worksheet2.Cell(currentRow2, 2).Value = "REGION";
                        worksheet2.Cell(currentRow2, 3).Value = "PRINCIPAL_WOA";
                        worksheet2.Cell(currentRow2, 4).Value = "DATE_NPA";
                        worksheet2.Cell(currentRow2, 5).Value = "DATE_WOA";

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DEBT.xlsx");
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
    }
}
