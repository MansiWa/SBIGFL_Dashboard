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
using Common;

namespace PrintSoftWeb.Controllers
{
    public class CurrencyRateMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyRateMasterController> _logger;
        private readonly IStringLocalizer<CurrencyRateMasterController> _localizer;
        public CurrencyRateMasterController(ILogger<CurrencyRateMasterController> logger, IStringLocalizer<CurrencyRateMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string status,string CurrencyId,string CurrencyName)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CurrencyMaster&sValue=cm_currencyname&id=cm_id&IsActiveColumn=cm_isactive&sCoulmnName";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
                ViewBag.cr_currency_name = rootObject;
                var currencyrateDataList = new List<CurrencyRateMasterModel>(); ;
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CurrencyRateMaster/GetAll?UserId=" + UserId + "&status=" + status+ "&CurrencyId="+ CurrencyId).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CurrencyRateMasterModel>() };
                    var responsese = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    currencyrateDataList = responsese.data;
                    if (currencyrateDataList != null)
                    {
                        return View(currencyrateDataList);
                    }
                    else
                    {
                        var currencyrateDataList2 = new List<CurrencyRateMasterModel>();
                        return View(currencyrateDataList2);
                    }
                }

                return View(currencyrateDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(CurrencyRateMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.cr_createddate = DateTime.Now;
                model.cr_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CurrencyRateMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; 
                    return Ok(outcomeDetail);
                }
                return Redirect($"/CurrencyRateMaster?CurrencyId={model.cr_currencyid}&CurrencyName={model.cr_currency_name}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/CurrencyRateMaster?CurrencyId={model.cr_currencyid}&CurrencyName={model.cr_currency_name}");
            }
        }

        public IActionResult Delete(Guid? cr_id,string CurrencyId,string CurrencyName)
        {
            try
            {
                CurrencyRateMasterModel model = new CurrencyRateMasterModel();
                Guid? UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                model.UserId = UserId;
                model.cr_id = cr_id;
                model.cr_currencyid = CurrencyId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CurrencyRateMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
                }
                return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
            }
        }
        public async Task<IActionResult> Excel(Guid? UserId, string status,string CurrencyId,string CurrencyName)
        {
            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/CurrencyRateMaster/GetExcel?UserId={UserId}&status={status}&CurrencyId={CurrencyId}";
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
                            var worksheet = workbook.Worksheets.Add("Currency Rate Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Sr.No";
                            worksheet.Cell(currentRow, 2).Value = "Currency Name";
                            worksheet.Cell(currentRow, 3).Value = "Currency Rate";
                            worksheet.Cell(currentRow, 4).Value = "From Date";
                            worksheet.Cell(currentRow, 5).Value = "To Date";
                            worksheet.Cell(currentRow, 6).Value = "Status";
                            if (dt == null)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = index.ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["cr_currency_name"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["cr_currencyrate"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["cr_fromdate"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["cr_todate"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["cr_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Currency Rate Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status, string CurrencyId,string CurrencyName)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                UserId = new Guid("C587998C-9C7F-4547-B9EA-FEE649274EBC");
                string url = $"{_httpClient.BaseAddress}/CurrencyRateMaster/GetPdf?UserId={UserId}&status={status}&CurrencyId={CurrencyId}";
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
                                return File(pdfBytes, "application/pdf", "CurrencyRateMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/CurrencyRateMaster?CurrencyId={CurrencyId}&CurrencyName={CurrencyName}");
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
