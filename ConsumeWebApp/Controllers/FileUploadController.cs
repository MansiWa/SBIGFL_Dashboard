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
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Humanizer;
using System.Runtime.Intrinsics.Arm;
using System.Reflection;
using System.IO;
using System.Reflection;

namespace PrintSoftWeb.Controllers
{

    public class FileUploadController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FileUploadController> _logger;
        private readonly IStringLocalizer<FileUploadController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        private string m_exePath = string.Empty;
        public FileUploadController(ILogger<FileUploadController> logger, IStringLocalizer<FileUploadController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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

        public IActionResult Index(string? f_date)
        {
            try
            {
                if (f_date == null)
                {
                    f_date=DateTime.Now.ToString();
                }
                var FileUploadDataList = new List<FileUpload>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/GetAll?user_id=" + UserId + "&f_date=" + f_date).Result;
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
        public IActionResult Edit(string? f_date)
        {
            var FileUploadDataList = new List<FileUpload>();
            string? userid = Request.Cookies["com_id"];
            if (userid == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Guid? UserId = new Guid(userid);
            List<FileUpload> FileUploadlist = new List<FileUpload>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/Update?user_id=" + UserId + "&f_date=" + f_date).Result;
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
        public IActionResult Delete(string? f_date)
        {
            var FileUploadDataList = new List<FileUpload>();
            string? userid = Request.Cookies["com_id"];
            if (userid == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Guid? UserId = new Guid(userid);
            List<FileUpload> FileUploadlist = new List<FileUpload>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/Delete?user_id=" + UserId + "&f_date=" + f_date).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic resdata = response.Content.ReadAsStringAsync().Result;
                dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                dynamic resmodel = rootObject.outcome;
                string outcomeDetail = resmodel.outcomeDetail;
                TempData["successMessage"] = outcomeDetail;
                return RedirectToAction("Index");
            }
			return View("Index", FileUploadDataList);
		}
		public JsonResult Search(string? f_date)
        {
            try
            {
                var FileUploadDataList = new List<FileUpload>(); 
                Guid? UserId = new Guid(Request.Cookies["com_id"]);
                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/GetAll?user_id=" + UserId + "&f_date=" + f_date).Result;
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
        public IActionResult Upload(IFormFile postedFile, string? date, string? docid)
        {
            LogWrite("Started");
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
                    string? userid = Request.Cookies["com_id"];
                    if (userid == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    //string conString = Configuration.GetSection("Server:Excel").Value;
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);
                    LogWrite("Open connection");
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
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
								if (dt.Rows.Count == 0)
								{
									TempData["errorMessage"] = "No data present in sheet: " + sheetName;
									return RedirectToAction("Index");
								}
								LogWrite("dt count" + dt.Rows.Count.ToString());
                                //dt.Columns.Remove("FileDate");
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "FileDate",
                                    DataType = typeof(string),
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
                                LogWrite("before loop" + dt.Rows.Count.ToString());
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
                                LogWrite("after loop" + dt.Rows.Count.ToString());
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
                            con.Open();
                            LogWrite("before upload" + dt.Rows.Count.ToString());
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }

                    var FileUploadDataList = new List<FileUpload>(); ;
                    Guid? UserId = new Guid(userid);
                    List<FileUpload> FileUploadlist = new List<FileUpload>();
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileUpload/Insert?f_date=" + date + "&f_filename=" + fileName + "&f_count=" + dt.Rows.Count + "&f_docid=4DDB3983-A80C-464F-93D6-263F8AABF0ED").Result;
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
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogWrite("error:" + ex.Message.ToString());
                return View();
            }   
        }

        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = System.IO.File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
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
                        var worksheet = workbook.Worksheets.Add("Portfolio Analysis");
                        var currentRow = 1;


                        worksheet.Cell(currentRow, 1).Value = "CLIENT_ID";
                        worksheet.Cell(currentRow, 2).Value = "SL_LIMIT";
                        worksheet.Cell(currentRow, 3).Value = "FIU_LIMIT";
                        worksheet.Cell(currentRow, 4).Value = "PROVISION_PERCENTAGE";
                        worksheet.Cell(currentRow, 5).Value = "CLIENT_NAME";
                        worksheet.Cell(currentRow, 6).Value = "CLIENT_AREA";
                        worksheet.Cell(currentRow, 7).Value = "CLIENT_STATUS";
                        worksheet.Cell(currentRow, 8).Value = "CLIENT_NPA";
                        worksheet.Cell(currentRow, 9).Value = "CLIENT_RATING";
                        worksheet.Cell(currentRow, 10).Value = "CLIENT_CODE_DESC";
                        worksheet.Cell(currentRow, 11).Value = "BUS_TYPE";
                        worksheet.Cell(currentRow, 12).Value = "SUBPRODUCTTYPE";
                        worksheet.Cell(currentRow, 13).Value = "CURRENCY";
                        worksheet.Cell(currentRow, 14).Value = "FACTOR_TYPE";
                        worksheet.Cell(currentRow, 15).Value = "COMPANY_ID_PK";
                        worksheet.Cell(currentRow, 16).Value = "FACTORS_RETENTION";
                        worksheet.Cell(currentRow, 17).Value = "UNALLOCATE_AMOUNT";
                        worksheet.Cell(currentRow, 18).Value = "FIU_OUT";
                        worksheet.Cell(currentRow, 19).Value = "AMOUNT_YTM_CUR";
                        worksheet.Cell(currentRow, 20).Value = "AMOUNT_YTD";
                        worksheet.Cell(currentRow, 21).Value = "ALLOCATEAMOUNT_YTD";
                        worksheet.Cell(currentRow, 22).Value = "BALANCE_AMOUNT";
                        worksheet.Cell(currentRow, 23).Value = "APPROVED_AMOUNT";
                        worksheet.Cell(currentRow, 24).Value = "DISAPP";
                        worksheet.Cell(currentRow, 25).Value = "DISPUTED_AMOUNT";
                        worksheet.Cell(currentRow, 26).Value = "FTG_CHG_YTD";
                        worksheet.Cell(currentRow, 27).Value = "BRANCH_CODE_PK";
                        worksheet.Cell(currentRow, 28).Value = "AMOUNT_MTD_CUR";
                        worksheet.Cell(currentRow, 29).Value = "AMOUNT_MTD";
                        worksheet.Cell(currentRow, 30).Value = "ALLOCATEAMOUNT_MTD";
                        worksheet.Cell(currentRow, 31).Value = "FTG_CHG_MTD";
                        worksheet.Cell(currentRow, 32).Value = "COMPANY_NAME";
                        worksheet.Cell(currentRow, 33).Value = "DEDUCT";
                        worksheet.Cell(currentRow, 34).Value = "BASE_CURRENCY_CD";
                        worksheet.Cell(currentRow, 35).Value = "RATE_TO_BASE_CURRENCY";
                        worksheet.Cell(currentRow, 36).Value = "FIU_IN_INR";
                        worksheet.Cell(currentRow, 37).Value = "BDM_NAME";
                        worksheet.Cell(currentRow, 38).Value = "DISCOUNT_TYPE";
                        worksheet.Cell(currentRow, 39).Value = "ADD_1";
                        worksheet.Cell(currentRow, 40).Value = "ADD_2";
                        worksheet.Cell(currentRow, 41).Value = "ADD_3";
                        worksheet.Cell(currentRow, 42).Value = "ADD_4";
                        worksheet.Cell(currentRow, 43).Value = "ADD_5";
                        worksheet.Cell(currentRow, 44).Value = "ADD_6";
                        worksheet.Cell(currentRow, 45).Value = "ADD_7";
                        worksheet.Cell(currentRow, 46).Value = "ADD_8";
                        worksheet.Cell(currentRow, 47).Value = "ADD_9";
                        worksheet.Cell(currentRow, 48).Value = "ADD_10";
                        worksheet.Cell(currentRow, 49).Value = "ROI";

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Portfolio Analysis.xlsx");
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
