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

namespace ConsumeWebApp.Controllers
{
    public class DepartmentMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DepartmentMasterController> _logger;
        private readonly IStringLocalizer<DepartmentMasterController> _localizer;
        public DepartmentMasterController(ILogger<DepartmentMasterController> logger, IStringLocalizer<DepartmentMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var departmentDataList = new List<DepartmentMasterModel>(); ;
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                List<DepartmentMasterModel> departmentlist = new List<DepartmentMasterModel>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/DepartmentMaster/GetAll?user_id=" + UserId + "&status=" + status).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<DepartmentMasterModel>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    departmentDataList = responses.data;
                    if (departmentDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        return View(departmentDataList);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var departmentDataList2 = new List<DepartmentMasterModel>();
                        return View(departmentDataList2);
                    }
                }
                return View(departmentDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(DepartmentMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.d_createddate = DateTime.Now;
                model.d_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/DepartmentMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(Guid? d_id)
        {
            try
            {
                DepartmentMasterModel model = new DepartmentMasterModel();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.d_id = d_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/DepartmentMaster/Delete", content).Result;
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

        public async Task<IActionResult> Excel(Guid? UserId, string status)
        {

            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/DepartmentMaster/GetExcel?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Department Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Sr.No";
                            worksheet.Cell(currentRow, 2).Value = "Department Code";
                            worksheet.Cell(currentRow, 3).Value = "Department Name";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/DepartmentMaster?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = index.ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["d_department_code"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["d_department_name"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["d_isactive"].ToString();
                                }

                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Department Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/DepartmentMaster?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                DepartmentMasterModel model = new DepartmentMasterModel();
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.d_isactive = status;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/DepartmentMaster/GetPdf?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        string htmlContent = content.ToString();
                        if (htmlContent.Contains("<td"))
                        {
                            MemoryStream ms = new MemoryStream();
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                            pdfDoc.Open();
                            TextReader sr = new StringReader(htmlContent);
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            pdfDoc.Close();
                            byte[] pdfBytes = ms.ToArray();
                            string base64Pdf = Convert.ToBase64String(pdfBytes);
                            ms1 = ms;
                            using (var stream = new MemoryStream())
                            {
                                return File(pdfBytes, "application/pdf", "DepartmentMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/DepartmentMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/DepartmentMaster?status={status}");
                    }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return new FileStreamResult(ms1, "application/pdf");
        }
    }
}
