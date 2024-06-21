using ClosedXML.Excel;
using PrintSoftWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text;
using System.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;


namespace PrintSoftWeb.Controllers
{
    public class HighlightsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HighlightsController> _logger;
        private readonly IStringLocalizer<HighlightsController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public HighlightsController(ILogger<HighlightsController> logger, IStringLocalizer<HighlightsController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
                if(f_date== null)
                {
                    f_date=DateTime.Now.ToString("yyyy-MM-dd");
                }
                var FileUploadDataList = new List<FileUpload>(); ;
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<FileUpload> FileUploadlist = new List<FileUpload>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Highlights/GetAll?user_id=" + new Guid(userid) + "&f_date=" + f_date).Result;
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
        public IActionResult BKIndex(string? h_date)
        {
            try
            {

                var FileUploadDataList = new List<Highlights>(); ;
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<Highlights> FileUploadlist = new List<Highlights>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Highlights/GetAll?user_id=" + new Guid(userid) + "&f_date=" + h_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Highlights>() };
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
                        var FileUploadDataList2 = new List<Highlights>();
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
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Highlights/GetAll?user_id=" + new Guid(userid) + "&f_date=" + f_date).Result;
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

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string sheetName1 = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                                string sheetName2 = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                                string sheetName3 = dtExcelSchema.Rows[3]["TABLE_NAME"].ToString();
                                string sheetName4 = dtExcelSchema.Rows[4]["TABLE_NAME"].ToString();
                                string sheetName5 = dtExcelSchema.Rows[5]["TABLE_NAME"].ToString();
                                if(sheetName != "'Efficiency Ratio$'")
                                {
									TempData["errorMessage"] = "Missing Efficiency Ratio Tab!";
									return RedirectToAction("Index");
								}
								if(sheetName1 != "FinancialHighlights$")
                                {
									TempData["errorMessage"] = "Missing Financial Highlights Tab!";
									return RedirectToAction("Index");
								}
								if(sheetName2 != "Highlights$")
                                {
									TempData["errorMessage"] = "Missing Highlights Tab!";
									return RedirectToAction("Index");
								}
								if(sheetName3 != "MOU$")
                                {
									TempData["errorMessage"] = "Missing MOU Tab!";
									return RedirectToAction("Index");
								}
								if(sheetName4 != "'MOU-Revised$'")
                                {
									TempData["errorMessage"] = "Missing MOU-Revised Tab!";
									return RedirectToAction("Index");
								}
								if(sheetName5 != "Yeild$")
                                {
									TempData["errorMessage"] = "Missing Yeild Tab!";
									return RedirectToAction("Index");
								}
								connExcel.Close();

                                //Read Data from First Sheet.
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
                                cmdExcel.CommandText = "SELECT * From [" + sheetName3 + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(moudt);
								if (moudt.Rows.Count == 0)
								{
									TempData["errorMessage"] = "No data present in sheet: " + sheetName3;
									return RedirectToAction("Index");
								}
								moudt.Columns.Add(new DataColumn
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

                                //foreach (DataRow row in dt.Rows)
                                //{
                                //    row["Y-O-Y Growth(%)"] = (Convert.ToDecimal(row["Col4"]) - Convert.ToDecimal(row["Col1"])) / Convert.ToDecimal(row["Col1"]) * 100;
                                //    row["Y-T-D Growth(%)"] = ((((Convert.ToDecimal(row["Col4"])) / Convert.ToDecimal(month))*12) -(Convert.ToDecimal(row["COL2"]))) / (Convert.ToDecimal(row["COL2"])) * 100;
                                //    // Check conditions

                                //}
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



                            sqlBulkCopy.DestinationTableName = "tbl_acc_highlights";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("PARTICULARS", "h_particulars");
                            sqlBulkCopy.ColumnMappings.Add("COL1", "h_col1");
                            sqlBulkCopy.ColumnMappings.Add("COL2", "h_col2");
                            sqlBulkCopy.ColumnMappings.Add("COL3", "h_col3");
                            sqlBulkCopy.ColumnMappings.Add("COL4", "h_col4");
                            sqlBulkCopy.ColumnMappings.Add("M-O-M Growth(%)", "h_mom");
                            sqlBulkCopy.ColumnMappings.Add("Y-O-Y Growth(%)", "h_yoy");
                            sqlBulkCopy.ColumnMappings.Add("Y-T-D Growth(%)", "h_ytd");
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
                            sbc.ColumnMappings.Add("Y-O-Y Growth(%)", "f_yoy");
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
                            sqlBulkCopy1.ColumnMappings.Add("Date", "e_date");
                            con.Open();
                            sqlBulkCopy1.WriteToServer(effratiodt);
                            con.Close();


                            SqlBulkCopy sqlBulkC = new SqlBulkCopy(con);
                            sqlBulkC.DestinationTableName = "tbl_mou";
                            sqlBulkC.ColumnMappings.Add("PARTICULARS", "mou_particulars");
                            sqlBulkC.ColumnMappings.Add("COL1", "mou_col1");
                            sqlBulkC.ColumnMappings.Add("COL2", "mou_col2");
                            sqlBulkC.ColumnMappings.Add("%Achieved", "mou_achieved");
                            sqlBulkC.ColumnMappings.Add("To Be Achieved", "mou_tobeachieved");
                            sqlBulkC.ColumnMappings.Add("Date", "mou_date");
                            con.Open();
                            sqlBulkC.WriteToServer(moudt);
                            con.Close();


                            SqlBulkCopy sqlBulkC1 = new SqlBulkCopy(con);
                            sqlBulkC1.DestinationTableName = "tbl_mourevised";
                            sqlBulkC1.ColumnMappings.Add("PARTICULARS", "m_particulars");
                            sqlBulkC1.ColumnMappings.Add("COL1", "m_col1");
                            sqlBulkC1.ColumnMappings.Add("COL2", "m_col2");
                            sqlBulkC1.ColumnMappings.Add("Annualized % Achievement of Budget", "m_achievement");
                            sqlBulkC1.ColumnMappings.Add("Date", "m_date");
                            con.Open();
                            sqlBulkC1.WriteToServer(moureviseddt);
                            con.Close();

                        }
                    }
                    var FileUploadDataList = new List<FileUpload>(); ;
                    Guid? UserId = new Guid(userid);
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Highlights/Insert?f_date=" + date + "&f_filename=" + fileName + "&f_count=" + highlightsdt.Rows.Count + "&f_docid=22826842-FB61-46AD-9199-2B14B9319DA5").Result;
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
                        worksheet.Cell(currentRow, 6).Value = "M-O-M Growth(%)";
                        worksheet.Cell(currentRow, 7).Value = "Y-O-Y Growth(%)";
                        worksheet.Cell(currentRow, 8).Value = "Y-T-D Growth(%)";

                        var worksheet1 = workbook.Worksheets.Add("FinancialHighlights");
                        var currentRow1 = 1;
                        worksheet1.Cell(currentRow1, 1).Value = "PARTICULARS";
                        worksheet1.Cell(currentRow1, 2).Value = "COL1";
                        worksheet1.Cell(currentRow1, 3).Value = "COL2";
                        worksheet1.Cell(currentRow1, 4).Value = "COL3";
                        worksheet1.Cell(currentRow1, 5).Value = "Y-O-Y Growth(%)";

                        var worksheet2 = workbook.Worksheets.Add("MOU");
                        var currentRow2 = 1;
                        worksheet2.Cell(currentRow2, 1).Value = "PARTICULARS";
                        worksheet2.Cell(currentRow2, 2).Value = "COL1";
                        worksheet2.Cell(currentRow2, 3).Value = "COL2";
                        worksheet2.Cell(currentRow2, 4).Value = "%Achieved";
                        worksheet2.Cell(currentRow2, 5).Value = "To Be Achieved";

                        var worksheet3 = workbook.Worksheets.Add("MOU-Revised");
                        var currentRow3 = 1;
                        worksheet3.Cell(currentRow3, 1).Value = "PARTICULARS";
                        worksheet3.Cell(currentRow3, 2).Value = "COL1";
                        worksheet3.Cell(currentRow3, 3).Value = "COL2";
                        worksheet3.Cell(currentRow3, 4).Value = "Annualized % Achievement of Budget";

                        var worksheet4 = workbook.Worksheets.Add("Yeild");
                        var currentRow4 = 1;
                        worksheet4.Cell(currentRow4, 1).Value = "PARTICULARS";
                        worksheet4.Cell(currentRow4, 2).Value = "COL1";
                        worksheet4.Cell(currentRow4, 3).Value = "COL2";
                        worksheet4.Cell(currentRow4, 4).Value = "COL3";
                        worksheet4.Cell(currentRow4, 5).Value = "COL4";

                        var worksheet5 = workbook.Worksheets.Add("Efficiency Ratio");
                        var currentRow5 = 1;
                        worksheet5.Cell(currentRow5, 1).Value = "PARTICULARS";
                        worksheet5.Cell(currentRow5, 2).Value = "COL1";
                        worksheet5.Cell(currentRow5, 3).Value = "COL2";
                        worksheet5.Cell(currentRow5, 4).Value = "COL3";
                        worksheet5.Cell(currentRow5, 5).Value = "COL4";

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
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Highlights/Delete", content).Result;
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
