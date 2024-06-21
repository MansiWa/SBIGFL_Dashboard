using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;

namespace PrintSoftWeb.Controllers
{
    public class DebtDashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DebtDashboardController> _logger;
        private readonly IStringLocalizer<DebtDashboardController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public DebtDashboardController(ILogger<DebtDashboardController> logger, IStringLocalizer<DebtDashboardController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
        public IActionResult Index(string? date)
        {

            try
            {
                if (date == null)
                {
                    date = null;
                }
                var FileUploadDataList = new List<Debt>();
                var smaList = new List<sma>();
                var smaList1 = new List<sma>();
                var smaList2 = new List<sma>();
                var npaList = new List<npa>();
                var woaList = new List<woa>();
                var weekList = new List<Branch_DUEWeeklySMA>();
                var dueList = new List<Branch_DUEWeeklySMA>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllVal?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage woaresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllWOAList?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage nparesponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllNPAList?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage smaresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME0?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage sma1response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME1?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage sma2response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME2?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage weekresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllWeeklySMA?UserId=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage dueresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllBranchwise?UserId=" + new Guid(userid) + "&date=" + date).Result;

                if (response.IsSuccessStatusCode)
                {
                    //debt
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Debt>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    if (FileUploadDataList.Count > 0)
                    {
                        TempData["Date"] = FileUploadDataList[0].date;
                    }
                    // woa
                    dynamic woadata = woaresponse.Content.ReadAsStringAsync().Result;
                    var woadataObject = new { data = new List<woa>() };
                    var woaresponses = JsonConvert.DeserializeAnonymousType(woadata, woadataObject);
                    woaList = woaresponses.data;
                    //npa
                    dynamic npadata = nparesponse.Content.ReadAsStringAsync().Result;
                    var npadataObject = new { data = new List<npa>() };
                    var nparesponses = JsonConvert.DeserializeAnonymousType(npadata, npadataObject);
                    npaList = nparesponses.data;
                    //sma
                    dynamic smadata = smaresponse.Content.ReadAsStringAsync().Result;
                    var smadataObject = new { data = new List<sma>() };
                    var smaresponses = JsonConvert.DeserializeAnonymousType(smadata, smadataObject);
                    smaList = smaresponses.data;
                    //sma1
                    dynamic sma1data = sma1response.Content.ReadAsStringAsync().Result;
                    var sma1dataObject = new { data = new List<sma>() };
                    var sma1responses = JsonConvert.DeserializeAnonymousType(sma1data, sma1dataObject);
                    smaList1 = sma1responses.data;
                    //sma2
                    dynamic sma2data = sma2response.Content.ReadAsStringAsync().Result;
                    var sma2dataObject = new { data = new List<sma>() };
                    var sma2responses = JsonConvert.DeserializeAnonymousType(sma2data, sma2dataObject);
                    smaList2 = sma2responses.data;
                    //week
                    dynamic weekdata = weekresponse.Content.ReadAsStringAsync().Result;
                    var weekdataObject = new { data = new List<Branch_DUEWeeklySMA>() };
                    var weekresponses = JsonConvert.DeserializeAnonymousType(weekdata, weekdataObject);
                    weekList = weekresponses.data;
                    //due
                    dynamic duedata = dueresponse.Content.ReadAsStringAsync().Result;
                    var duedataObject = new { data = new List<Branch_DUEWeeklySMA>() };
                    var dueresponses = JsonConvert.DeserializeAnonymousType(duedata, duedataObject);
                    dueList = dueresponses.data;
                    if (FileUploadDataList != null)
                    {
                        var viewModel = new DebtModel
                        {
                            debt = FileUploadDataList,
                            npa = npaList,
                            woa = woaList,
                            sma = smaList,
                            sma1 = smaList1,
                            sma2 = smaList2,
                            WeeklySMA = weekList,
                            Branch_DUE = dueList
                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var viewModel = new DebtModel
                        {
                            debt = FileUploadDataList,
                            npa = npaList,
                            woa = woaList,
                            sma = smaList,
                            sma1 = smaList1,
                            sma2 = smaList2,
                            WeeklySMA = weekList,
                            Branch_DUE = dueList
                        };
                        return View(viewModel);
                    }
                }
                var viewModel2 = new DebtModel
                {
                    debt = FileUploadDataList,
                    npa = npaList,
                    woa = woaList,
                    sma = smaList,
                    sma1 = smaList1,
                    sma2 = smaList2,
                    WeeklySMA = weekList,
                    Branch_DUE = dueList
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
                var FileUploadDataList = new List<Debt>();
                var smaList = new List<sma>();
                var smaList1 = new List<sma>();
                var smaList2 = new List<sma>();
                var npaList = new List<npa>();
                var woaList = new List<woa>();
                var weekList = new List<Branch_DUEWeeklySMA>();
                var dueList = new List<Branch_DUEWeeklySMA>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllVal?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage woaresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllWOAList?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage nparesponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllNPAList?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage smaresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME0?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage sma1response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME1?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage sma2response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllSME2?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage weekresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllWeeklySMA?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;
                HttpResponseMessage dueresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Debt/GetAllBranchwise?UserId=" + new Guid(userid) + "&date=" + DebtDate).Result;

                if (response.IsSuccessStatusCode)
                {
                    //debt
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<Debt>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    // woa
                    dynamic woadata = woaresponse.Content.ReadAsStringAsync().Result;
                    var woadataObject = new { data = new List<woa>() };
                    var woaresponses = JsonConvert.DeserializeAnonymousType(woadata, woadataObject);
                    woaList = woaresponses.data;
                    //npa
                    dynamic npadata = nparesponse.Content.ReadAsStringAsync().Result;
                    var npadataObject = new { data = new List<npa>() };
                    var nparesponses = JsonConvert.DeserializeAnonymousType(npadata, npadataObject);
                    npaList = nparesponses.data;
                    //sma
                    dynamic smadata = smaresponse.Content.ReadAsStringAsync().Result;
                    var smadataObject = new { data = new List<sma>() };
                    var smaresponses = JsonConvert.DeserializeAnonymousType(smadata, smadataObject);
                    smaList = smaresponses.data;
                    //sma1
                    dynamic sma1data = sma1response.Content.ReadAsStringAsync().Result;
                    var sma1dataObject = new { data = new List<sma>() };
                    var sma1responses = JsonConvert.DeserializeAnonymousType(sma1data, sma1dataObject);
                    smaList1 = sma1responses.data;
                    //sma2
                    dynamic sma2data = sma2response.Content.ReadAsStringAsync().Result;
                    var sma2dataObject = new { data = new List<sma>() };
                    var sma2responses = JsonConvert.DeserializeAnonymousType(sma2data, sma2dataObject);
                    smaList2 = sma2responses.data;
                    //week
                    dynamic weekdata = weekresponse.Content.ReadAsStringAsync().Result;
                    var weekdataObject = new { data = new List<Branch_DUEWeeklySMA>() };
                    var weekresponses = JsonConvert.DeserializeAnonymousType(weekdata, weekdataObject);
                    weekList = weekresponses.data;
                    //due
                    dynamic duedata = dueresponse.Content.ReadAsStringAsync().Result;
                    var duedataObject = new { data = new List<Branch_DUEWeeklySMA>() };
                    var dueresponses = JsonConvert.DeserializeAnonymousType(duedata, duedataObject);
                    dueList = dueresponses.data;
                    if (woaList != null)
                    {
                        var viewModel = new DebtModel
                        {
                            debt = FileUploadDataList,
                            npa = npaList,
                            woa = woaList,
                            sma = smaList,
                            sma1 = smaList1,
                            sma2 = smaList2,
                            WeeklySMA = weekList,
                            Branch_DUE = dueList
                        };
                        if (woaList.Count == 0)
                        {
							TempData["errorMessage"] = "No Data Found!";
							return RedirectToAction("Index");
						}
                        return View("Index", viewModel);
                    }
                    else
                    {
                        var viewModel = new DebtModel
                        {
                            debt = FileUploadDataList,
                            npa = npaList,
                            woa = woaList,
                            sma = smaList,
                            sma1 = smaList1,
                            sma2 = smaList2,
                            WeeklySMA = weekList,
                            Branch_DUE = dueList
                        };
                        TempData["errorMessage"] = "No Data Found!";
                        return RedirectToAction("Index");
                    }
                }
                var viewModel2 = new DebtModel
                {
                    debt = FileUploadDataList,
                    npa = npaList,
                    woa = woaList,
                    sma = smaList,
                    sma1 = smaList1,
                    sma2 = smaList2,
                    WeeklySMA = weekList,
                    Branch_DUE = dueList
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
