using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
namespace PrintSoftWeb.Controllers
{
    public class BusinessDevController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BusinessDevController> _logger;
        private readonly IStringLocalizer<BusinessDevController> _localizer;
        private readonly IConfiguration _configuration;

        public BusinessDevController(ILogger<BusinessDevController> logger, IStringLocalizer<BusinessDevController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _configuration = configuration;
            _logger = logger;
            _localizer = localizer;

        }
        public IActionResult Index(string? date)
        {
            try
            {
                // date = "2024-02-27";
                //var summery = new List<summery>();
                //var nbo = new List<NBO>();
                //var fsa = new List<FSA>();
                //var withcredit = new List<WithCredit>();
                //var sanctioned = new List<Sanctioned>();
                //var disbursalnew = new List<DisbursalNew>();

                var bdsummery = new List<BDSummary>();
                var bdcredit = new List<BDCredit>();
                var bdquery = new List<BDQuryResolve>();
                var bdSanctions = new List<BDSanction>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDSummary?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage bdcreditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDCredit?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage queryresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDQueryResolve?user_id=" + new Guid(userid) + "&date=" + date).Result;
                // HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetWithCredit?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage sanctresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDSanctioned?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //  HttpResponseMessage disresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetDisbursalNew?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    //summery
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<BDSummary>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    bdsummery = responses.data;
                    if (bdsummery.Count > 0)
                    {
                        TempData["Date"] = bdsummery[0].bd_date;
                    }
                    //bdcredit
                    dynamic prrdata = bdcreditresponse.Content.ReadAsStringAsync().Result;
                    var prrdataObject = new { data = new List<BDCredit>() };
                    var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                    bdcredit = prrresponses.data;
                    //query response
                    dynamic fsadata = queryresponse.Content.ReadAsStringAsync().Result;
                    var fsadataObject = new { data = new List<BDQuryResolve>() };
                    var fsaresponses = JsonConvert.DeserializeAnonymousType(fsadata, fsadataObject);
                    bdquery = fsaresponses.data;

                    //sanctioned
                    dynamic sandata = sanctresponse.Content.ReadAsStringAsync().Result;
                    var sandataObject = new { data = new List<BDSanction>() };
                    var sanresponses = JsonConvert.DeserializeAnonymousType(sandata, sandataObject);
                    bdSanctions = sanresponses.data;



                    if (bdsummery != null)
                    {
                        var viewModel = new BusinessModel
                        {
                            BDSummary = bdsummery,
                            BDCredit = bdcredit,
                            BDQuryResolve = bdquery,

                            BDSanction = bdSanctions

                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var viewModel = new BusinessModel
                        {
                            BDSummary = bdsummery,

                            BDCredit = bdcredit,
                            BDQuryResolve = bdquery,

                            BDSanction = bdSanctions
                        };
                        return View(viewModel);
                    }
                }
                var viewModel2 = new BusinessModel
                {
                    BDSummary = bdsummery,

                    BDCredit = bdcredit,
                    BDQuryResolve = bdquery,

                    BDSanction = bdSanctions
                };

                return View(viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult Find(DateTime? CustDate)
        {
            try
            {
                if (CustDate == null)
                {
                    return RedirectToAction("Index");
                }
                string date = CustDate.Value.ToString("yyyy-MM-dd"); // Change the format as per your requirement
                TempData["Date"] = date;
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                var bdsummery = new List<BDSummary>();
                var bdcredit = new List<BDCredit>();
                var bdquery = new List<BDQuryResolve>();
                var bdSanctions = new List<BDSanction>();
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDSummary?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage bdcreditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDCredit?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage queryresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDQueryResolve?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage sanctresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetBDSanctioned?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetSummary?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage nboresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetNBO?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage fsaresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetFSA?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetWithCredit?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage sanctresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetSanctioned?user_id=" + new Guid(userid) + "&date=" + date).Result;
                //HttpResponseMessage disresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/BDDashboard/GetDisbursalNew?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    //summery
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<BDSummary>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    bdsummery = responses.data;

                    //nbo
                    //bdcredit
                    dynamic prrdata = bdcreditresponse.Content.ReadAsStringAsync().Result;
                    var prrdataObject = new { data = new List<BDCredit>() };
                    var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                    bdcredit = prrresponses.data;
                    //query response
                    dynamic fsadata = queryresponse.Content.ReadAsStringAsync().Result;
                    var fsadataObject = new { data = new List<BDQuryResolve>() };
                    var fsaresponses = JsonConvert.DeserializeAnonymousType(fsadata, fsadataObject);
                    bdquery = fsaresponses.data;

                    //sanctioned
                    dynamic sandata = sanctresponse.Content.ReadAsStringAsync().Result;
                    var sandataObject = new { data = new List<BDSanction>() };
                    var sanresponses = JsonConvert.DeserializeAnonymousType(sandata, sandataObject);
                    bdSanctions = sanresponses.data;

                    if (bdsummery != null)
                    {
                        var viewModel = new BusinessModel
                        {
                            BDSummary = bdsummery,
                            BDCredit = bdcredit,
                            BDQuryResolve = bdquery,

                            BDSanction = bdSanctions

                        };
                        return View("Index", viewModel);
                    }
                    else
                    {
                        var viewModel = new BusinessModel
                        {
                            BDSummary = bdsummery,

                            BDCredit = bdcredit,
                            BDQuryResolve = bdquery,

                            BDSanction = bdSanctions
                        };
                        return View("Index", viewModel);
                    }
                }
                var viewModel2 = new BusinessModel
                {
                    BDSummary = bdsummery,

                    BDCredit = bdcredit,
                    BDQuryResolve = bdquery,

                    BDSanction = bdSanctions
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
