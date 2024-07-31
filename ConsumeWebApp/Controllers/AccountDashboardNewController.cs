using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Web.WebPages;
namespace PrintSoftWeb.Controllers
{
    public class AccountDashboardNewController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountDashboardNewController> _logger;
        private readonly IStringLocalizer<AccountDashboardNewController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public AccountDashboardNewController(ILogger<AccountDashboardNewController> logger, IStringLocalizer<AccountDashboardNewController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
                //date = "2024-02-27";
                //if(date==null)
                //{
                //    date = DateTime.Now.ToString();
                //}

                var FileHighlightList = new List<AccountDashboard>();
                var FileDataList1 = new List<AccountDashboard>();
                var FileDataList11 = new List<AccountDashboard>();
                var FileDataList112 = new List<AccountDashboard>();
                var FileDataList1122 = new List<AccountDashboard>();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage finhighlights = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllFinhighlights?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage highlight = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetHighlights?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage mourevised = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllMourevised?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage effratio = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllEffratio?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage yeild = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllYeild?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (highlight.IsSuccessStatusCode)
                {

                    dynamic hidata = highlight.Content.ReadAsStringAsync().Result;
                    var hidataObject = new { data = new List<AccountDashboard>() };
                    var hiresponses = JsonConvert.DeserializeAnonymousType(hidata, hidataObject);
                    FileHighlightList = hiresponses.data;

                    dynamic data12 = finhighlights.Content.ReadAsStringAsync().Result;
                    var dataObject12 = new { data = new List<AccountDashboard>() };
                    var responses12 = JsonConvert.DeserializeAnonymousType(data12, dataObject12);
                    FileDataList1 = responses12.data;

                    dynamic data121 = mourevised.Content.ReadAsStringAsync().Result;
                    var dataObject121 = new { data = new List<AccountDashboard>() };
                    var responses121 = JsonConvert.DeserializeAnonymousType(data121, dataObject121);
                    FileDataList11 = responses121.data;

                    dynamic data1211 = effratio.Content.ReadAsStringAsync().Result;
                    var dataObject1211 = new { data = new List<AccountDashboard>() };
                    var responses1211 = JsonConvert.DeserializeAnonymousType(data1211, dataObject1211);
                    FileDataList112 = responses1211.data;

                    dynamic data1212 = yeild.Content.ReadAsStringAsync().Result;
                    var dataObject1212 = new { data = new List<AccountDashboard>() };
                    var responses1212 = JsonConvert.DeserializeAnonymousType(data1212, dataObject1212);
                    FileDataList1122 = responses1212.data;
                    var singleValue = "";

                    if (FileHighlightList.Count > 0)
                    {
                        singleValue = FileHighlightList[0].h_date;
                    }
                    TempData["Date"] = singleValue.AsDateTime().ToString("dd-MMM-yy");
                    if (highlight != null)
                    {
                        var localizedTitle = _localizer[""];
                        //var viewModel = new Tuple<IEnumerable<CreditRatingModel>, IEnumerable<CreditRatingModel>, IEnumerable<CostOfBorrowingModel>, IEnumerable<TBillModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>>(creditList, prrlist, cobList, FileUploadDataList, borrowList, rfrList, borrow1List, borrow2List, borrow3List, borrow4List, borrow5List);

                        var viewModel = new MyViewModel
                        {
                            acclist1 = FileHighlightList,
                            acclist2 = FileDataList1,
                            acclist3 = FileDataList11,
                            acclist4 = FileDataList112,
                            acclist5 = FileDataList1122

                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var viewModel = new MyViewModel
                        {

                            acclist1 = FileHighlightList,
                            acclist2 = FileDataList1,
                            acclist3 = FileDataList11,
                            acclist4 = FileDataList112,
                            acclist5 = FileDataList1122
                        };
                        return View(viewModel);
                    }
                }
                var viewModel2 = new MyViewModel
                {

                    acclist1 = FileHighlightList,
                    acclist2 = FileDataList1,
                    acclist3 = FileDataList11,
                    acclist4 = FileDataList112,
                    acclist5 = FileDataList1122
                };

                return View(viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }


        public IActionResult Find(string? treDate)
        {
            try
            {
                TempData["Date"] = treDate.AsDateTime().ToString("dd-MMM-yy");

                var FileHighlightList = new List<AccountDashboard>();
                var FileDataList1 = new List<AccountDashboard>();
                var FileDataList11 = new List<AccountDashboard>();
                var FileDataList112 = new List<AccountDashboard>();
                var FileDataList1122 = new List<AccountDashboard>();
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";

                List<AccountDashboard> FileUploadlist = new List<AccountDashboard>();
                HttpResponseMessage finhighlights = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllFinhighlights?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage highlight = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetHighlights?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage mourevised = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllMourevised?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage effratio = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllEffratio?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage yeild = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllYeild?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                if (highlight.IsSuccessStatusCode)
                {

                    dynamic hidata = highlight.Content.ReadAsStringAsync().Result;
                    var hidataObject = new { data = new List<AccountDashboard>() };
                    var hiresponses = JsonConvert.DeserializeAnonymousType(hidata, hidataObject);
                    FileHighlightList = hiresponses.data;

                    dynamic data12 = finhighlights.Content.ReadAsStringAsync().Result;
                    var dataObject12 = new { data = new List<AccountDashboard>() };
                    var responses12 = JsonConvert.DeserializeAnonymousType(data12, dataObject12);
                    FileDataList1 = responses12.data;

                    dynamic data121 = mourevised.Content.ReadAsStringAsync().Result;
                    var dataObject121 = new { data = new List<AccountDashboard>() };
                    var responses121 = JsonConvert.DeserializeAnonymousType(data121, dataObject121);
                    FileDataList11 = responses121.data;

                    dynamic data1211 = effratio.Content.ReadAsStringAsync().Result;
                    var dataObject1211 = new { data = new List<AccountDashboard>() };
                    var responses1211 = JsonConvert.DeserializeAnonymousType(data1211, dataObject1211);
                    FileDataList112 = responses1211.data;

                    dynamic data1212 = yeild.Content.ReadAsStringAsync().Result;
                    var dataObject1212 = new { data = new List<AccountDashboard>() };
                    var responses1212 = JsonConvert.DeserializeAnonymousType(data1212, dataObject1212);
                    FileDataList1122 = responses1212.data;
                    var singleValue = "";

                    if (FileHighlightList.Count > 0)
                    {
                        singleValue = FileHighlightList[0].h_date;
                    }
                    TempData["Date"] = singleValue.AsDateTime().ToString("dd-MMM-yy");
                    if (highlight != null)
                    {
                        var localizedTitle = _localizer[""];
                        //var viewModel = new Tuple<IEnumerable<CreditRatingModel>, IEnumerable<CreditRatingModel>, IEnumerable<CostOfBorrowingModel>, IEnumerable<TBillModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>>(creditList, prrlist, cobList, FileUploadDataList, borrowList, rfrList, borrow1List, borrow2List, borrow3List, borrow4List, borrow5List);

                        var viewModel = new MyViewModel
                        {
                            acclist1 = FileHighlightList,
                            acclist2 = FileDataList1,
                            acclist3 = FileDataList11,
                            acclist4 = FileDataList112,
                            acclist5 = FileDataList1122

                        };
                        return View("Index",viewModel);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var viewModel = new MyViewModel
                        {

                            acclist1 = FileHighlightList,
                            acclist2 = FileDataList1,
                            acclist3 = FileDataList11,
                            acclist4 = FileDataList112,
                            acclist5 = FileDataList1122
                        };
                        return View("Index",viewModel);
                    }
                }
                var viewModel2 = new MyViewModel
                {

                    acclist1 = FileHighlightList,
                    acclist2 = FileDataList1,
                    acclist3 = FileDataList11,
                    acclist4 = FileDataList112,
                    acclist5 = FileDataList1122
                };

                return View("Index", viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Json("ErrorIndex", "Home");
            }
        }
    }
}
