using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OfficeOpenXml;
using PrintSoftWeb.Models;
using System.Data.OleDb;
using System.Data;
using System.Text;

namespace PrintSoftWeb.Controllers
{
    public class FinanceUploadController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FinanceUploadController> _logger;
        private readonly IStringLocalizer<FinanceUploadController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public FinanceUploadController(ILogger<FinanceUploadController> logger, IStringLocalizer<FinanceUploadController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
                    f_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var FileUploadDataList = new List<FileUpload>(); ;
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/HighlightsNew/GetAll?user_id=" + new Guid(userid) + "&f_date=" + f_date).Result;
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
        public JsonResult Search(string? f_date)
        {
            try
            {

                var FileUploadDataList = new List<FileUpload>(); ;
                string? userid = Request.Cookies["com_id"];

                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/HighlightsNew/GetAll?user_id=" + new Guid(userid) + "&f_date=" + f_date).Result;
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
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    DataTable highlightsdt = new DataTable();
                    DataTable financialhighlightsdt = new DataTable();
                    DataTable moudt = new DataTable();
                    DataTable moureviseddt = new DataTable();
                    DataTable yeilddt = new DataTable();
                    DataTable effratiodt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string sheetName1 = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                string sheetName2 = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                string sheetName4 = dtExcelSchema.Rows[3]["TABLE_NAME"].ToString();
                                string sheetName5 = dtExcelSchema.Rows[4]["TABLE_NAME"].ToString();
                                if (sheetName != "'Efficiency Ratio$'")
                                {
                                    TempData["errorMessage"] = "Missing Efficiency Ratio Tab!";
                                    return RedirectToAction("Index");
                                }
                                if (sheetName1 != "FinancialHighlights$")
                                {
                                    TempData["errorMessage"] = "Missing Financial Highlights Tab!";
                                    return RedirectToAction("Index");
                                }
                                if (sheetName2 != "Highlights$")
                                {
                                    TempData["errorMessage"] = "Missing Highlights Tab!";
                                    return RedirectToAction("Index");
                                }

                                if (sheetName4 != "'MOU-Revised$'")
                                {
                                    TempData["errorMessage"] = "Missing MOU-Revised Tab!";
                                    return RedirectToAction("Index");
                                }
                                if (sheetName5 != "Yeild$")
                                {
                                    TempData["errorMessage"] = "Missing Yeild Tab!";
                                    return RedirectToAction("Index");
                                }
                                connExcel.Close();
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(effratiodt);
                                if (effratiodt.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName;
                                    return RedirectToAction("Index");
                                }
                                effratiodt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName1 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(financialhighlightsdt);
                                if (financialhighlightsdt.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName1;
                                    return RedirectToAction("Index");
                                }
                                financialhighlightsdt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName2 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(highlightsdt);
                                if (highlightsdt.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName2;
                                    return RedirectToAction("Index");
                                }
                                highlightsdt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });

                                cmdExcel.CommandText = "SELECT * From [" + sheetName4 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(moureviseddt);
                                if (moureviseddt.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName4;
                                    return RedirectToAction("Index");
                                }
                                moureviseddt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "Date",
                                    DataType = typeof(string),
                                    DefaultValue = date
                                });
                                cmdExcel.CommandText = "SELECT * From [" + sheetName5 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(yeilddt);
                                if (yeilddt.Rows.Count == 0)
                                {
                                    TempData["errorMessage"] = "No data present in sheet: " + sheetName5;
                                    return RedirectToAction("Index");
                                }
                                yeilddt.Columns.Add(new DataColumn
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
                            sqlBulkCopy.DestinationTableName = "tbl_acc_highlights";
                            sqlBulkCopy.ColumnMappings.Add("PARTICULARS", "h_particulars");
                            sqlBulkCopy.ColumnMappings.Add("COL1", "h_col1");
                            sqlBulkCopy.ColumnMappings.Add("COL2", "h_col2");
                            sqlBulkCopy.ColumnMappings.Add("COL3", "h_col3");
                            sqlBulkCopy.ColumnMappings.Add("COL4", "h_col4");
                            sqlBulkCopy.ColumnMappings.Add("COL5", "h_col5");
                            sqlBulkCopy.ColumnMappings.Add("COL6", "h_col6");
                            sqlBulkCopy.ColumnMappings.Add("COL7", "h_col7");
                            sqlBulkCopy.ColumnMappings.Add("COL8", "h_col8");
                            sqlBulkCopy.ColumnMappings.Add("COL9", "h_col9");
                            sqlBulkCopy.ColumnMappings.Add("COL10", "h_col10");
                            sqlBulkCopy.ColumnMappings.Add("COL11", "h_col11");
                            sqlBulkCopy.ColumnMappings.Add("COL12", "h_col12");
                            sqlBulkCopy.ColumnMappings.Add("COL13", "h_col13");
                            sqlBulkCopy.ColumnMappings.Add("COL14", "h_col14");

                            sqlBulkCopy.ColumnMappings.Add("Date", "h_date");
                            con.Open();
                            sqlBulkCopy.WriteToServer(highlightsdt);
                            con.Close();


                            SqlBulkCopy sbc = new SqlBulkCopy(con);
                            sbc.DestinationTableName = "tbl_finhighlights";
                            sbc.ColumnMappings.Add("PARTICULARS", "f_particulars");
                            sbc.ColumnMappings.Add("COL1", "f_col1");
                            sbc.ColumnMappings.Add("COL2", "f_col2");
                            sbc.ColumnMappings.Add("COL3", "f_col3");
                            sbc.ColumnMappings.Add("COL4", "f_col4");
                            sbc.ColumnMappings.Add("COL5", "f_col5");
                            sbc.ColumnMappings.Add("COL6", "f_col6");
                            sbc.ColumnMappings.Add("COL7", "f_col7");
                            sbc.ColumnMappings.Add("COL8", "f_col8");
                            sbc.ColumnMappings.Add("COL9", "f_col9");
                            sbc.ColumnMappings.Add("COL10", "f_col10");
                            sbc.ColumnMappings.Add("COL11", "f_col11");
                            sbc.ColumnMappings.Add("COL12", "f_col12");
                            sbc.ColumnMappings.Add("COL13", "f_col13");
                            sbc.ColumnMappings.Add("COL14", "f_col14");

                            sbc.ColumnMappings.Add("Date", "f_date");
                            con.Open();
                            sbc.WriteToServer(financialhighlightsdt);
                            con.Close();



                            SqlBulkCopy sqlbc = new SqlBulkCopy(con);
                            sqlbc.DestinationTableName = "tbl_acc_yeild";
                            sqlbc.ColumnMappings.Add("PARTICULARS", "y_particulars");
                            sqlbc.ColumnMappings.Add("COL1", "y_col1");
                            sqlbc.ColumnMappings.Add("COL2", "y_col2");
                            sqlbc.ColumnMappings.Add("COL3", "y_col3");
                            sqlbc.ColumnMappings.Add("COL4", "y_col4");
                            sqlbc.ColumnMappings.Add("COL5", "y_col5");
                            sqlbc.ColumnMappings.Add("COL6", "y_col6");
                            sqlbc.ColumnMappings.Add("COL7", "y_col7");
                            sqlbc.ColumnMappings.Add("COL8", "y_col8");
                            sqlbc.ColumnMappings.Add("COL9", "y_col9");
                            sqlbc.ColumnMappings.Add("COL10", "y_col10");
                            sqlbc.ColumnMappings.Add("COL11", "y_col11");
                            sqlbc.ColumnMappings.Add("COL12", "y_col12");
                            sqlbc.ColumnMappings.Add("COL13", "y_col13");
                            sqlbc.ColumnMappings.Add("COL14", "y_col14");
                            sqlbc.ColumnMappings.Add("Date", "y_date");
                            con.Open();
                            sqlbc.WriteToServer(yeilddt);
                            con.Close();



                            SqlBulkCopy sqlBulkCopy1 = new SqlBulkCopy(con);
                            sqlBulkCopy1.DestinationTableName = "tbl_effratio";
                            sqlBulkCopy1.ColumnMappings.Add("PARTICULARS", "e_particulars");
                            sqlBulkCopy1.ColumnMappings.Add("COL1", "e_col1");
                            sqlBulkCopy1.ColumnMappings.Add("COL2", "e_col2");
                            sqlBulkCopy1.ColumnMappings.Add("COL3", "e_col3");
                            sqlBulkCopy1.ColumnMappings.Add("COL4", "e_col4");
                            sqlBulkCopy1.ColumnMappings.Add("COL5", "e_col5");
                            sqlBulkCopy1.ColumnMappings.Add("COL6", "e_col6");
                            sqlBulkCopy1.ColumnMappings.Add("COL7", "e_col7");
                            sqlBulkCopy1.ColumnMappings.Add("COL8", "e_col8");
                            sqlBulkCopy1.ColumnMappings.Add("COL9", "e_col9");
                            sqlBulkCopy1.ColumnMappings.Add("COL10", "e_col10");
                            sqlBulkCopy1.ColumnMappings.Add("COL11", "e_col11");
                            sqlBulkCopy1.ColumnMappings.Add("COL12", "e_col12");
                            sqlBulkCopy1.ColumnMappings.Add("COL13", "e_col13");
                            sqlBulkCopy1.ColumnMappings.Add("COL14", "e_col14");
                            sqlBulkCopy1.ColumnMappings.Add("Date", "e_date");
                            con.Open();
                            sqlBulkCopy1.WriteToServer(effratiodt);
                            con.Close();


                            SqlBulkCopy sqlBulkC1 = new SqlBulkCopy(con);
                            sqlBulkC1.DestinationTableName = "tbl_mourevised";
                            sqlBulkC1.ColumnMappings.Add("PARTICULARS", "m_particulars");
                            sqlBulkC1.ColumnMappings.Add("COL1", "m_col1");
                            sqlBulkC1.ColumnMappings.Add("COL2", "m_col2");
                            sqlBulkC1.ColumnMappings.Add("COL3", "m_col3");
                            sqlBulkC1.ColumnMappings.Add("COL4", "m_col4");
                            sqlBulkC1.ColumnMappings.Add("COL5", "m_col5");
                            sqlBulkC1.ColumnMappings.Add("COL6", "m_col6");
                            sqlBulkC1.ColumnMappings.Add("COL7", "m_col7");
                            sqlBulkC1.ColumnMappings.Add("COL8", "m_col8");
                            sqlBulkC1.ColumnMappings.Add("COL9", "m_col9");
                            sqlBulkC1.ColumnMappings.Add("COL10", "m_col10");
                            sqlBulkC1.ColumnMappings.Add("COL11", "m_col11");
                            sqlBulkC1.ColumnMappings.Add("COL12", "m_col12");
                            sqlBulkC1.ColumnMappings.Add("COL13", "m_col13");
                            sqlBulkC1.ColumnMappings.Add("COL14", "m_col14");
                            sqlBulkC1.ColumnMappings.Add("Date", "m_date");
                            con.Open();
                            sqlBulkC1.WriteToServer(moureviseddt);
                            con.Close();

                        }
                    }
                    var FileUploadDataList = new List<FileUpload>(); ;
                    Guid? UserId = new Guid(userid);
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/HighlightsNew/Insert?f_date=" + date + "&f_filename=" + fileName + "&f_count=" + highlightsdt.Rows.Count + "&f_docid=22826842-FB61-46AD-9199-2B14B9319DA5").Result;
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
                        var worksheet = workbook.Worksheets.Add("Highlights");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "PARTICULARS";
                        worksheet.Cell(currentRow, 2).Value = "COL1";
                        worksheet.Cell(currentRow, 3).Value = "COL2";
                        worksheet.Cell(currentRow, 4).Value = "COL3";
                         worksheet.Cell(currentRow, 5).Value = "COL4";
                         worksheet.Cell(currentRow, 6).Value = "COL5";
                         worksheet.Cell(currentRow, 7).Value = "COL6";
                         worksheet.Cell(currentRow, 8).Value = "COL7";
                         worksheet.Cell(currentRow, 9).Value = "COL8";
                         worksheet.Cell(currentRow, 10).Value = "COL9";
                         worksheet.Cell(currentRow, 11).Value = "COL10";
                         worksheet.Cell(currentRow, 12).Value = "COL11";
                         worksheet.Cell(currentRow, 13).Value = "COL12";
                         worksheet.Cell(currentRow, 14).Value = "COL13";
                         worksheet.Cell(currentRow, 15).Value = "COL14";

                        var worksheet1 = workbook.Worksheets.Add("FinancialHighlights");
                        var currentRow1 = 1;
                        worksheet1.Cell(currentRow1, 1).Value = "PARTICULARS";
                        worksheet1.Cell(currentRow1, 2).Value = "COL1";
                        worksheet1.Cell(currentRow1, 3).Value = "COL2";
                        worksheet1.Cell(currentRow1, 4).Value = "COL3";
                        worksheet1.Cell(currentRow1, 5).Value = "COL4";
                        worksheet1.Cell(currentRow1, 6).Value = "COL5";
                        worksheet1.Cell(currentRow1, 7).Value = "COL6";
                        worksheet1.Cell(currentRow1, 8).Value = "COL7";
                        worksheet1.Cell(currentRow1, 9).Value = "COL8";
                        worksheet1.Cell(currentRow1, 10).Value = "COL9";
                        worksheet1.Cell(currentRow1, 11).Value = "COL10";
                        worksheet1.Cell(currentRow1, 12).Value = "COL11";
                        worksheet1.Cell(currentRow1, 13).Value = "COL12";
                        worksheet1.Cell(currentRow1, 14).Value = "COL13";
                        worksheet1.Cell(currentRow1, 15).Value = "COL14";


                        var worksheet3 = workbook.Worksheets.Add("MOU-Revised");
                        var currentRow3 = 1;
                        worksheet3.Cell(currentRow3, 1).Value = "PARTICULARS";
                        worksheet3.Cell(currentRow3, 2).Value = "COL1";
                        worksheet3.Cell(currentRow3, 3).Value = "COL2";
                        worksheet3.Cell(currentRow3, 4).Value = "COL3";
                        worksheet3.Cell(currentRow3, 5).Value = "COL4";
                        worksheet3.Cell(currentRow3, 6).Value = "COL5";
                        worksheet3.Cell(currentRow3, 7).Value = "COL6";
                        worksheet3.Cell(currentRow3, 8).Value = "COL7";
                        worksheet3.Cell(currentRow3, 9).Value = "COL8";
                        worksheet3.Cell(currentRow3, 10).Value = "COL9";
                        worksheet3.Cell(currentRow3, 11).Value = "COL10";
                        worksheet3.Cell(currentRow3, 12).Value = "COL11";
                        worksheet3.Cell(currentRow3, 13).Value = "COL12";
                        worksheet3.Cell(currentRow3, 14).Value = "COL13";
                        worksheet3.Cell(currentRow3, 15).Value = "COL14";


                        var worksheet4 = workbook.Worksheets.Add("Yeild");
                        var currentRow4 = 1;
                        worksheet4.Cell(currentRow4, 1).Value = "PARTICULARS";
                        worksheet4.Cell(currentRow4, 2).Value = "COL1";
                        worksheet4.Cell(currentRow4, 3).Value = "COL2";
                        worksheet4.Cell(currentRow4, 4).Value = "COL3";
                        worksheet4.Cell(currentRow4, 5).Value = "COL4";
                        worksheet4.Cell(currentRow4, 6).Value = "COL5";
                        worksheet4.Cell(currentRow4, 7).Value = "COL6";
                        worksheet4.Cell(currentRow4, 8).Value = "COL7";
                        worksheet4.Cell(currentRow4, 9).Value = "COL8";
                        worksheet4.Cell(currentRow4, 10).Value = "COL9";
                        worksheet4.Cell(currentRow4, 11).Value = "COL10";
                        worksheet4.Cell(currentRow4, 12).Value = "COL11";
                        worksheet4.Cell(currentRow4, 13).Value = "COL12";
                        worksheet4.Cell(currentRow4, 14).Value = "COL13";
                        worksheet4.Cell(currentRow4, 15).Value = "COL14";

                        var worksheet5 = workbook.Worksheets.Add("Efficiency Ratio");
                        var currentRow5 = 1;
                        worksheet5.Cell(currentRow5, 1).Value = "PARTICULARS";
                        worksheet5.Cell(currentRow5, 2).Value = "COL1";
                        worksheet5.Cell(currentRow5, 3).Value = "COL2";
                        worksheet5.Cell(currentRow5, 4).Value = "COL3";
                        worksheet5.Cell(currentRow5, 5).Value = "COL4";
                        worksheet5.Cell(currentRow5, 6).Value = "COL5";
                        worksheet5.Cell(currentRow5, 7).Value = "COL6";
                        worksheet5.Cell(currentRow5, 8).Value = "COL7";
                        worksheet5.Cell(currentRow5, 9).Value = "COL8";
                        worksheet5.Cell(currentRow5, 10).Value = "COL9";
                        worksheet5.Cell(currentRow5, 11).Value = "COL10";
                        worksheet5.Cell(currentRow5, 12).Value = "COL11";
                        worksheet5.Cell(currentRow5, 13).Value = "COL12";
                        worksheet5.Cell(currentRow5, 14).Value = "COL13";
                        worksheet5.Cell(currentRow5, 15).Value = "COL14";

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Accounts.xlsx");
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
        public IActionResult Delete(Guid? f_id, string f_date)
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
                model.f_date = f_date;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/HighlightsNew/Delete", content).Result;
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
