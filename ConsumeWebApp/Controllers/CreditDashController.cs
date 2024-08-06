using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;

namespace PrintSoftWeb.Controllers
{
    public class CreditDashController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CreditDashController> _logger;
        private readonly IStringLocalizer<CreditDashController> _localizer;
        public CreditDashController(ILogger<CreditDashController> logger, IStringLocalizer<CreditDashController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);

            _logger = logger;
            _localizer = localizer;

        }
        public IActionResult Index(string? date)
        {

            try
            {
                if (date == null)
                {
                    date = null;
                }
                var FileUploadDataList = new List<CreditDetails>();

                string? userid = Request.Cookies["com_id"];
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexII?UserId=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIII?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response3 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIV?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response4 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexVI?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response5 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexVII?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response6 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIX?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response7 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexX?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response8 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexXI?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                if (response.IsSuccessStatusCode)
                {
                    //debt
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CreditDetails>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;


                    if (FileUploadDataList != null)
                    {
                        var viewModel = new CreditModel
                        {
                            listI = FileUploadDataList

                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var viewModel = new CreditModel
                        {
                            listI = FileUploadDataList
                        };
                        return View(viewModel);
                    }
                }
                var viewModel2 = new CreditModel
                {
                    listI = FileUploadDataList
                };
                return View(viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }

        }
        public IActionResult Find(string? DebtDate)
        {

            try
            {
                if (DebtDate == null)
                {
                    return RedirectToAction("Index");
                }
                var FileUploadDataList = new List<CreditDetails>();

                string? userid = Request.Cookies["com_id"];
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexII?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIII?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response3 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIV?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response4 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexVI?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response5 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexVII?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response6 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexIX?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response7 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexX?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                //HttpResponseMessage response8 = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditDashboard/GetAnnexXI?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;

                if (response.IsSuccessStatusCode)
                {
                    //debt
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CreditDetails>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    if (FileUploadDataList != null)
                    {
                        var viewModel = new CreditModel
                        {
                            listI = FileUploadDataList
                        };
                        return View("Index", viewModel);
                    }
                    else
                    {
                        var viewModel = new CreditModel
                        {
                            listI = FileUploadDataList
                        };
                        return View("Index", viewModel);
                    }
                }
                var viewModel2 = new CreditModel
                {
                    listI = FileUploadDataList
                };
                return View("Index", viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }

        }
    }
}
