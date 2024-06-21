using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Data.OleDb;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Text;
using CsvHelper;

namespace PrintSoftWeb.Controllers
{
    public class BorroAndRFRController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BorroAndRFRController> _logger;
        private readonly IStringLocalizer<BorroAndRFRController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public BorroAndRFRController(ILogger<BorroAndRFRController> logger, IStringLocalizer<BorroAndRFRController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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

        public IActionResult Index(string? rb_date)
        {
            try
            {
                if (rb_date == null)
                {
                    rb_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var FileUploadDataList = new List<BorroAndRFRModel>();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<BorroAndRFRModel> FileUploadlist = new List<BorroAndRFRModel>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetAll?UserId=" + new Guid(userid) + "&rb_date=" + rb_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<BorroAndRFRModel>() };
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
                        var FileUploadDataList2 = new List<BorroAndRFRModel>();
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
        public IActionResult Search(string? rb_date)
        {
            try
            {
                if (rb_date == null)
                {
                    rb_date = null;
                }
                var FileUploadDataList = new List<BorroAndRFRModel>();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                
                List<BorroAndRFRModel> FileUploadlist = new List<BorroAndRFRModel>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetAll?UserId=" + new Guid(userid) + "&rb_date=" + rb_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<BorroAndRFRModel>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    
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
        public IActionResult Upload(IFormFile postedFile, string? rb_doctype,string? date)
        {
            try
            {
                string batchid = Guid.NewGuid().ToString();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
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
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);
                    DateTime date1 = DateTime.Now;

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
								dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "b_id",
                                    DataType = typeof(Guid),
                                    DefaultValue = null
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "b_createdby",
                                    DataType = typeof(string),
                                    DefaultValue = userid
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "b_createddate",
                                    DataType = typeof(DateTime),
                                    DefaultValue = date1
                                }); dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "batchid",
                                    DataType = typeof(string),
                                    DefaultValue = batchid
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "FileDate",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["b_id"] = Guid.NewGuid();
                                }

                                connExcel.Close();
                            }
                        }
                    }

                    //Insert the Data read from the Excel file to Database Table.
                    conString = Configuration.GetSection("Server:Constring").Value;
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        if (rb_doctype == "Borrowing")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_Borrowing";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("Product", "Product");
                                sqlBulkCopy.ColumnMappings.Add("Issued_date", "Issued_date");
                                sqlBulkCopy.ColumnMappings.Add("Rate", "Rate");
                                sqlBulkCopy.ColumnMappings.Add("Amount", "Amount");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "TIER_II_BOND")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_TIER_II_BOND";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                                sqlBulkCopy.ColumnMappings.Add("Issued Date", "Issued_date");
                                sqlBulkCopy.ColumnMappings.Add("Rate(%)", "Rate");
                                sqlBulkCopy.ColumnMappings.Add("Outstanding Amount (Rs)", "Amount");
                                sqlBulkCopy.ColumnMappings.Add("Last Intrest Paid upto", "Paid_UpTo");
                                sqlBulkCopy.ColumnMappings.Add("Maturity / Due Date", "Due_Date");
                                sqlBulkCopy.ColumnMappings.Add("Next Intrest Payment Due on", "PymntDue_Date");

                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");
                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "Commercial_Paper")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_Commercial_Paper";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("ISIN NO", "ISIN_No");
                                sqlBulkCopy.ColumnMappings.Add("Investor", "Investor");
                                sqlBulkCopy.ColumnMappings.Add("CP NOS", "CP_NOS");
                                sqlBulkCopy.ColumnMappings.Add("Issued Date", "Issued_date");
                                sqlBulkCopy.ColumnMappings.Add("CP Deal Rate (%)", "CP_Deal_Rate");
                                sqlBulkCopy.ColumnMappings.Add("CP Total Cost (CP Deal Rate+Stamp duty+IPA Charges) (%)", "CP_Total_Cost");
                                sqlBulkCopy.ColumnMappings.Add("No of Days", "No_Of_Days");
                                sqlBulkCopy.ColumnMappings.Add("Amount(Rs)", "Amount");
                                sqlBulkCopy.ColumnMappings.Add("Due Date", "Due_Date");
                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "Bank_Line_CC")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_Bank_Line_CC";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("Bank Name", "Bank_Name");
                                sqlBulkCopy.ColumnMappings.Add("Issued Date", "Issued_date");
                                sqlBulkCopy.ColumnMappings.Add("Type of Loan", "Type_of_Loan");
                                sqlBulkCopy.ColumnMappings.Add("Rate(%)", "Rate");
                                sqlBulkCopy.ColumnMappings.Add("Tenor (No of Days)", "No_Of_Days");
                                sqlBulkCopy.ColumnMappings.Add("Amount(Rs)", "Amount");
                                sqlBulkCopy.ColumnMappings.Add("Due Date", "Due_Date");
                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "Bank_Line_WCL")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_Bank_Line_WCL";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("Bank Name", "Bank_Name");
                                sqlBulkCopy.ColumnMappings.Add("Issued Date", "Issued_date");
                                sqlBulkCopy.ColumnMappings.Add("Type of Loan", "Type_of_Loan");
                                sqlBulkCopy.ColumnMappings.Add("Rate(%)", "Rate");
                                sqlBulkCopy.ColumnMappings.Add("Tenor (No of Days)", "No_Of_Days");
                                sqlBulkCopy.ColumnMappings.Add("Amount(Rs)", "Amount");
                                sqlBulkCopy.ColumnMappings.Add("Due Date", "Due_Date");
                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "FOREX")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_FOREX";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "b_id");
                                sqlBulkCopy.ColumnMappings.Add("SBI LONDON", "SBI_LONDON");
                                sqlBulkCopy.ColumnMappings.Add("OUTSTANDING", "OUTSTANDING");
                                sqlBulkCopy.ColumnMappings.Add("CONVERSION RATE", "CONVERSION_RATE");
                                sqlBulkCopy.ColumnMappings.Add("INR O/S", "INR_OS");
                                sqlBulkCopy.ColumnMappings.Add("USD Equiv O/S", "USD_Equiv_OS");
                                sqlBulkCopy.ColumnMappings.Add("Limit", "Limit");
                                sqlBulkCopy.ColumnMappings.Add("Reference Rate", "Reference_Rate");
                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "b_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "b_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                        else if (rb_doctype == "RFR")
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                sqlBulkCopy.BulkCopyTimeout = 0;
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.tbl_RFRDoc";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.
                                sqlBulkCopy.ColumnMappings.Add("b_id", "rfr_id");
                                sqlBulkCopy.ColumnMappings.Add("Currency", "Currency");
                                sqlBulkCopy.ColumnMappings.Add("RFR", "RFR");
                                sqlBulkCopy.ColumnMappings.Add("Period", "Effectivedate");
                                sqlBulkCopy.ColumnMappings.Add("Rate", "Rate");
                                sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                                sqlBulkCopy.ColumnMappings.Add("FileDate", "FileDate");

                                sqlBulkCopy.ColumnMappings.Add("b_createdby", "rfr_createdby");
                                sqlBulkCopy.ColumnMappings.Add("b_createddate", "rfr_createddate");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                            }

                        }
                    }
                    string? rb_docid = "";
                    if (rb_doctype == "RFR")
                    {
                        rb_docid = "539593FF-0807-479E-914B-D68497D5BC3A";
                    }
                    else
                    {
                        rb_docid = "A0341EBE-2FB2-40D4-95D5-795DE789006B";
                    }

                    var FileUploadDataList = new List<BorroAndRFRModel>(); ;
                    List<BorroAndRFRModel> FileUploadlist = new List<BorroAndRFRModel>();
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/Insert?rb_date=" + date + "&rb_filename=" + fileName + "&rb_count=" + dt.Rows.Count + "&rb_docid=" + rb_docid + "&UserId=" + userid + "&rb_doctype=" + rb_doctype + "&batchid="+ batchid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resdata = response.Content.ReadAsStringAsync().Result;
                        dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                        dynamic resmodel = rootObject.outcome;
                        string outcomeDetail = resmodel.outcomeDetail;
                        TempData["successMessage"] = outcomeDetail;
                        return RedirectToAction("Index");

                    }
                    TempData["errorMessage"] = response.Headers.ToString();

                }

                return RedirectToAction("Index");
            }
           
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> RFRExcel()
        {
            try
            {
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (string.IsNullOrEmpty(userid))
                {
                    // Session variable is not set or is empty
                    TempData["errorMessage"] = "User ID not found in session.";
                    return RedirectToAction("Error");
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("RFR Document");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "Currency";
                        worksheet.Cell(currentRow, 2).Value = "RFR";
                        worksheet.Cell(currentRow, 3).Value = "Period";
                        worksheet.Cell(currentRow, 4).Value = "Rate";
                        worksheet.Row(1).Style.Font.Bold = true;

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "RFRDocument.xlsx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> Excel()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Borrowing");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "Product";
                        worksheet.Cell(1, 2).Value = "Issued_date";
                        worksheet.Cell(1, 3).Value = "Rate";
                        worksheet.Cell(1, 4).Value = "Amount";
                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Borrowing.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> ExcelBank_Line_WCL()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Bank_Line_WCL");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "Bank Name";
                        worksheet.Cell(1, 2).Value = "Issued Date";
                        worksheet.Cell(1, 3).Value = "Type of Loan";
                        worksheet.Cell(1, 4).Value = "Rate(%)";
                        worksheet.Cell(1, 5).Value = "Tenor (No of Days)";
                        worksheet.Cell(1, 6).Value = "Due Date";
                        worksheet.Cell(1, 7).Value = "Amount(Rs)";
                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Bank_Line_WCL.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> ExcelBank_Line_CC()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Bank_Line_CC");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "Bank Name";
                        worksheet.Cell(1, 2).Value = "Issued Date";
                        worksheet.Cell(1, 3).Value = "Type of Loan";
                        worksheet.Cell(1, 4).Value = "Rate(%)";
                        worksheet.Cell(1, 5).Value = "Tenor (No of Days)";
                        worksheet.Cell(1, 6).Value = "Due Date";
                        worksheet.Cell(1, 7).Value = "Amount(Rs)";
                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Bank_Line_CC.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> ExcelCommercial_Paper()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Commercial_Paper");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "ISIN NO";
                        worksheet.Cell(1, 2).Value = "Investor";
                        worksheet.Cell(1, 3).Value = "CP NOS";
                        worksheet.Cell(1, 4).Value = "Issued Date";
                        worksheet.Cell(1, 5).Value = "CP Deal Rate (%)";
                        worksheet.Cell(1, 6).Value = "CP Total Cost (CP Deal Rate+Stamp duty+IPA Charges) (%)";
                        worksheet.Cell(1, 7).Value = "No of Days";
                        worksheet.Cell(1, 8).Value = "Due Date";
                        worksheet.Cell(1, 9).Value = "Amount(Rs)";
                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Commercial_Paper.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> ExcelTIER_II_BOND()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("TIER_II_BOND");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "Name";
                        worksheet.Cell(1, 2).Value = "Issued Date";
                        worksheet.Cell(1, 3).Value = "Rate(%)";
                        worksheet.Cell(1, 4).Value = "Last Intrest Paid upto";
                        worksheet.Cell(1, 5).Value = "Next Intrest Payment Due on";
                        worksheet.Cell(1, 6).Value = "Maturity / Due Date";
                        worksheet.Cell(1, 7).Value = "Outstanding Amount (Rs)";

                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TIER_II_BOND.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> ExcelFOREX()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    // Load the data into an Excel package (using the EPPlus library)

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("FOREX");

                        // Set fixed column headers
                        worksheet.Cell(1, 1).Value = "SBI LONDON";
                        worksheet.Cell(1, 2).Value = "OUTSTANDING";
                        worksheet.Cell(1, 3).Value = "CONVERSION RATE";
                        worksheet.Cell(1, 4).Value = "INR O/S";
                        worksheet.Cell(1, 5).Value = "USD Equiv O/S";
                        worksheet.Cell(1, 6).Value = "Limit";
                        worksheet.Cell(1, 7).Value = "Reference Rate";
                        worksheet.Cells().Style.Protection.SetLocked(false);

                        // Optionally, you can add some styling or formatting to the worksheet
                        worksheet.Row(1).Style.Font.Bold = true;
                        workbook.SaveAs(memoryStream);


                    }
                    var content = memoryStream.ToArray();

                    // Return the template Excel file as a download response
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "FOREX.xlsx");
                }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }
        public IActionResult Delete(Guid? rb_id, string rb_date, string rb_doctype,string batchid)
        {
            try
            {
                BorroAndRFRModel model = new BorroAndRFRModel();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                model.UserId = UserId;
                model.rb_id = rb_id;
                model.rb_date = rb_date;
                model.rb_doctype = rb_doctype;
                model.batchid = batchid;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UploadBorroRFR/Delete", content).Result;
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