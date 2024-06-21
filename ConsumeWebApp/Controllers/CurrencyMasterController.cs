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
    public class CurrencyMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyMasterController> _logger;
        private readonly IStringLocalizer<CurrencyMasterController> _localizer;
        public CurrencyMasterController(ILogger<CurrencyMasterController> logger, IStringLocalizer<CurrencyMasterController> localizer, IConfiguration configuration)
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
                var CurrencyDataList = new List<CurrencyMasterModel>(); 
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CurrencyMaster/GetAll?UserId=" + UserId + "&status=" + status).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CurrencyMasterModel>() };
                    var curesponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    CurrencyDataList = curesponse.data;
                    if (CurrencyDataList != null)
                    {
                        return View(CurrencyDataList);
                    }
                    else
                    {
                        var CurrencyDataListS = new List<CurrencyRateMasterModel>();
                        return View(CurrencyDataListS);
                    }
                }
                return View(CurrencyDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }
        [HttpPost]
        public IActionResult Create(CurrencyMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.cm_createddate = DateTime.Now;
                model.cm_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CurrencyMaster", content).Result;
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
        public IActionResult Delete(Guid? cm_id)
        {
            try
            {
                CurrencyMasterModel model = new CurrencyMasterModel();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.cm_id = cm_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CurrencyMaster/Delete", content).Result;
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
                string url = $"{_httpClient.BaseAddress}/CurrencyMaster/GetExcel?UserId={UserId}&status={status}";
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
                            var worksheet = workbook.Worksheets.Add("Currency Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Sr.No";
                            worksheet.Cell(currentRow, 2).Value = "Currency Code";
                            worksheet.Cell(currentRow, 3).Value = "Currency Name";
                            worksheet.Cell(currentRow, 4).Value = "Currency Symbol";
                            worksheet.Cell(currentRow, 5).Value = "Currency Format";
                            worksheet.Cell(currentRow, 6).Value = "Status";
                            if (dt == null)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/CurrencyMaster?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = index.ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["cm_currencycode"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["cm_currencyname"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["cm_currencysymbol"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["cm_currency_format"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["cm_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Currency Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/CurrencyMaster?status={status}");
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
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/CurrencyMaster/GetPdf?UserId={UserId}&status={status}";

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
                                return File(pdfBytes, "application/pdf", "CurrencyMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/CurrencyMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/CurrencyMaster?status={status}");
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
