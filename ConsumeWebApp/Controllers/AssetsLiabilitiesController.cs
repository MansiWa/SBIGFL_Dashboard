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
    public class AssetsLiabilitiesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AssetsLiabilitiesController> _logger;
        private readonly IStringLocalizer<AssetsLiabilitiesController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public AssetsLiabilitiesController(ILogger<AssetsLiabilitiesController> logger, IStringLocalizer<AssetsLiabilitiesController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
        public IActionResult Index(string? al_date)
        {
            try
            {
                if (al_date == null)
                {
                    al_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var AssetsLiabilitiesDataList = new List<AssetsLiabilities>(); ;
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                List<AssetsLiabilities> AssetsLiabilitieslist = new List<AssetsLiabilities>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/GetAll?user_id=" + new Guid(userid) + "&al_date=" + al_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<AssetsLiabilities>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    AssetsLiabilitiesDataList = responses.data;
                    if (AssetsLiabilitiesDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return View(AssetsLiabilitiesDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var AssetsLiabilitiesDataList2 = new List<AssetsLiabilities>();
                        return View(AssetsLiabilitiesDataList2);
                    }
                }
                return View(AssetsLiabilitiesDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public JsonResult Search(string? al_date)
        {
            try
            {

                var AssetsLiabilitiesDataList = new List<AssetsLiabilities>(); ;
                string? userid = Request.Cookies["com_id"];

                List<AssetsLiabilities> AssetsLiabilitieslist = new List<AssetsLiabilities>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/GetAll?user_id=" + new Guid(userid) + "&al_date=" + al_date).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<AssetsLiabilities>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    AssetsLiabilitiesDataList = responses.data;
                    if (AssetsLiabilitiesDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return Json(AssetsLiabilitiesDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var AssetsLiabilitiesDataList2 = new List<AssetsLiabilities>();
                        return Json(AssetsLiabilitiesDataList2);
                    }
                }
                return Json(AssetsLiabilitiesDataList);
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
                string batchid = Guid.NewGuid().ToString();
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

                   // string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

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
                                    ColumnName = "al_id",
                                    DataType = typeof(Guid),
                                    DefaultValue = null
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "al_createdby",
                                    DataType = typeof(string),
                                    DefaultValue = userid
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "al_createddate",
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
                                }); dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "al_isactive",
                                    DataType = typeof(char),
                                    DefaultValue = null
                                });
                                dt.Columns.Add(new DataColumn
                                {
                                    ColumnName = "seq",
                                    DataType = typeof(int),
                                    DefaultValue = null
                                });
                                int i = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    i++;
                                    row["al_id"] = Guid.NewGuid();
                                    row["al_isactive"] = '1';
                                    row["seq"] = i;
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



                            sqlBulkCopy.DestinationTableName = "tbl_AssetsLiabilities";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("al_id", "al_id");
                            sqlBulkCopy.ColumnMappings.Add("PARTICULARS", "al_particulars");
                            sqlBulkCopy.ColumnMappings.Add("COL1", "al_col1");
                            sqlBulkCopy.ColumnMappings.Add("COL2", "al_col2");
                            sqlBulkCopy.ColumnMappings.Add("COL3", "al_col3");
                            sqlBulkCopy.ColumnMappings.Add("COL4", "al_col4");
                            sqlBulkCopy.ColumnMappings.Add("COL5", "al_col5");
                            sqlBulkCopy.ColumnMappings.Add("FileDate", "al_date");
                            sqlBulkCopy.ColumnMappings.Add("batchid", "batchid");
                            sqlBulkCopy.ColumnMappings.Add("al_createdby", "al_createdby");
                            sqlBulkCopy.ColumnMappings.Add("al_createddate", "al_createddate");
                            sqlBulkCopy.ColumnMappings.Add("al_createddate", "al_updateddate");
                            sqlBulkCopy.ColumnMappings.Add("al_isactive", "al_isactive");
                            sqlBulkCopy.ColumnMappings.Add("seq", "seq");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    var AssetsLiabilitiesDataList = new List<AssetsLiabilities>();
                    Guid? UserId = new Guid(userid);
                    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/Insert?al_date=" + date + "&al_filename=" + fileName + "&al_count=" + dt.Rows.Count + "&al_docid=F606ADF7-3362-43F9-BFE9-10E1C3E485F9&UserId=" + userid + "&batchid=" + batchid).Result;
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
                        var worksheet = workbook.Worksheets.Add("AssetsLiabilities");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "PARTICULARS";
                        worksheet.Cell(currentRow, 2).Value = "COL1";
                        worksheet.Cell(currentRow, 3).Value = "COL2";
                        worksheet.Cell(currentRow, 4).Value = "COL3";
                        worksheet.Cell(currentRow, 5).Value = "COL4";
                        worksheet.Cell(currentRow, 6).Value = "COL5";

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "AssetsLiabilities.xlsx");
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

        public IActionResult Delete( AssetsLiabilities model)
        {
            try
            {
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Guid? UserId = new Guid(userid);
                model.UserId = UserId;
               
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/AssetsLiabilities/Delete", content).Result;
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
