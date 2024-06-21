using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PrintSoftWeb.Controllers
{
    public class CompanyDetailsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyDetailsController> _logger;
        private readonly IStringLocalizer<CompanyDetailsController> _localizer;
        public CompanyDetailsController(ILogger<CompanyDetailsController> logger, IStringLocalizer<CompanyDetailsController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:User").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            try
            {
                string? comid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (comid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                Guid? UserId = new Guid(comid);
                var comModelList = new CompanyDetailsModel();

                string url = $"{_httpClient.BaseAddress}/CompanyDetails/Get?UserId={UserId}&Server_Value={serverValue}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new CompanyDetailsModel() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    comModelList = response2.data;
                    if (comModelList != null)
                    {
                        return View(comModelList);
                    }
                    else
                    {
                        var comModelDataList = new List<CompanyDetailsModel>();
                        return View(comModelDataList);
                    }
                }
                return View(comModelList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult EditIndex()
        {
            try
            {
                string? comid = HttpContext.Session.GetString("com_id");
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                Guid? UserId = new Guid(comid);
                var comModelList = new CompanyDetailsModel();
                string url = $"{_httpClient.BaseAddress}/CompanyDetails/Get?UserId={UserId}&Server_Value={serverValue}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new CompanyDetailsModel() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    comModelList = response2.data;
                    if (comModelList != null)
                    {
                        return View(comModelList);
                    }
                    else
                    {
                        var comModelDataList = new List<CompanyDetailsModel>();
                        return View(comModelDataList);
                    }
                }
                return View(comModelList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(CompanyDetailsModel model)
        {
            try
            {
                string? comid = HttpContext.Session.GetString("com_id");
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                Guid? UserId = new Guid(comid);
                model.UserId = UserId;
                model.com_id = new Guid(comid);
                model.com_updateddate = DateTime.Now;
                model.com_createddate = DateTime.Now;
                model.Server_Value = serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CompanyDetails/UpdateCompany", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic Succdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(Succdata);
                    dynamic modeldata = rootObject.outcome;
                    string outcomeDetail = modeldata.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return RedirectToAction("EditIndex");
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("EditIndex");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("EditIndex");
            }
        }

        [HttpPost]
        public IActionResult Update([FromForm] CompanyDetailsModel model)
        {
            try
            {
                var content = new MultipartFormDataContent();

                StreamContent? comCompanyLogoContent = null;

                if (model.com_company_logo != null)
                {
                    comCompanyLogoContent = new StreamContent(model.com_company_logo.OpenReadStream());
                    comCompanyLogoContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"com_company_logo\"",
                        FileName = $"\"{model.com_company_logo.FileName}\""
                    };
                    comCompanyLogoContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(comCompanyLogoContent);
                }

                StreamContent? comCompanyLogoContent2 = null;

                if (model.com_company_logo2 != null)
                {
                    comCompanyLogoContent2 = new StreamContent(model.com_company_logo2.OpenReadStream());
                    comCompanyLogoContent2.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"com_company_logo\"",
                        FileName = $"\"{model.com_company_logo2.FileName}\""
                    };
                    comCompanyLogoContent2.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(comCompanyLogoContent2);
                }

                string? comid = HttpContext.Session.GetString("com_id");
                string? serverValue = HttpContext.Session.GetString("Server_Value");
                Guid? UserId = new Guid(comid);
                model.UserId = UserId;
                model.com_id = new Guid(comid);
                model.com_updateddate = DateTime.Now;
                model.com_createddate = DateTime.Now;

                content.Add(new StringContent(model.com_id.ToString() ?? ""), "com_id");
                content.Add(new StringContent(model.UserId?.ToString() ?? ""), "UserId");
                content.Add(new StringContent(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")), "com_updateddate");
                content.Add(new StringContent(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")), "com_createddate");
                content.Add(new StringContent(serverValue ?? ""), "Server_Value");
                content.Add(new StringContent(model.com_contact ?? ""), "com_contact");
                content.Add(new StringContent(model.type ?? ""), "type");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CompanyDetails/UpdateLogo", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    dynamic comid2 = rootObject.data;
                    string id = comid2.OutcomeId;
                    string name = comid2.OutcomeDetail;
                    var result = new { id, name, outcomeDetail };
                    TempData["successMessage"] = outcomeDetail;

                    return Ok(result);
                }
                TempData["errorMessage"] = response.Headers.ToString();
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
