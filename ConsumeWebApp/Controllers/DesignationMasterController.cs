using ClosedXML.Excel;
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

namespace PrintSoftWeb.Controllers
{
    public class DesignationMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DesignationMasterController> _logger;
        private readonly IStringLocalizer<DesignationMasterController> _localizer;
        public DesignationMasterController(ILogger<DesignationMasterController> logger, IStringLocalizer<DesignationMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string DepartmentId, string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var DesignationMasterList = new List<DesignationMasterModel>();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/DesignationMaster/GetAll?UserId={UserId}&DepartmentId={DepartmentId}&status={status}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<DesignationMasterModel>() };
                    var seresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    DesignationMasterList = seresponse.data;
                    if (DesignationMasterList != null)
                    {
                        return View(DesignationMasterList);
                    }
                    else
                    {
                        var DesignationMasterDataList = new List<DesignationMasterModel>();
                        return View(DesignationMasterDataList);
                    }
                }
                return View(DesignationMasterList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(DesignationMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.de_createddate = DateTime.Now;
                model.de_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/DesignationMaster", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    dynamic sedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(sedata);
                    dynamic semodel = rootObject.outcome;
                    string outcomeDetail = semodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                return Redirect($"/DesignationMaster?DepartmentId={model.de_department_id}&DepartmentName={model.de_department_name}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/DesignationMaster?DepartmentId={model.de_department_id}&DepartmentName={model.de_department_name}");
            }
        }

        public IActionResult Delete(Guid? de_id, string DepartmentId, string DepartmentName)
        {
            try
            {
                DesignationMasterModel model = new DesignationMasterModel();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.de_id = de_id;
                model.de_department_id = DepartmentId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/DesignationMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic sedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(sedata);
                    dynamic semodel = rootObject.outcome;
                    string outcomeDetail = semodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                    return Redirect($"/DesignationMaster?DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
                }
                return Redirect($"/DesignationMaster?DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/DesignationMaster?DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
            }
        }

        public async Task<IActionResult> Excel(Guid? UserId, string status, string DepartmentId, string DepartmentName)
        {
            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/DesignationMaster/GetExcel?UserId={UserId}&status={status}&DepartmentId={DepartmentId}";
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
                            var worksheet = workbook.Worksheets.Add("Designation Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Department Name";
                            worksheet.Cell(currentRow, 2).Value = "Designation Code";
                            worksheet.Cell(currentRow, 3).Value = "Designation Name";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/DesignationMaster?status={status}&DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["de_department_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["de_designation_code"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["de_designation_name"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["de_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Designation Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/DesignationMaster?status={status}&DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Pdf(Guid? UserId, string status, string DepartmentId, string DepartmentName)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/DesignationMaster/GetPdf?UserId={UserId}&status={status}&DepartmentId={DepartmentId}";
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
                                return File(pdfBytes, "application/pdf", "DesignationMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/DesignationMaster?status={status}&DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/DesignationMaster?status={status}&DepartmentId={DepartmentId}&DepartmentName={DepartmentName}");
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

