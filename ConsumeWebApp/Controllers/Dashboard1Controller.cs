using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Globalization;
using Tuple = System.Tuple;

namespace PrintSoftWeb.Controllers
{

    public class Dashboard1Controller : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Dashboard1Controller> _logger;
        private readonly IStringLocalizer<Dashboard1Controller> _localizer;
        public Dashboard1Controller(ILogger<Dashboard1Controller> logger, IStringLocalizer<Dashboard1Controller> localizer, IConfiguration configuration)
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
                date = "2024-02-27";
                var FileUploadDataList = new List<TBillModel>();
                var prrlist = new List<CreditRatingModel>();
                var creditList = new List<CreditRatingModel>();
                var cobList = new List<CostOfBorrowingModel>();
                var rfrList = new List<BorroAndRFRModel>();
                var borrowList = new List<BorroAndRFRModel>();
                var borrow1List = new List<BorroAndRFRModel>();
                var borrow2List = new List<BorroAndRFRModel>();
                var borrow3List = new List<BorroAndRFRModel>();
                var borrow4List = new List<BorroAndRFRModel>();
                var borrow5List = new List<BorroAndRFRModel>();
                var FileHighlightList = new List<AccountDashboard>();
                var FileDataList1 = new List<AccountDashboard>();
                var FileDataList11 = new List<AccountDashboard>();
                var FileDataList112 = new List<AccountDashboard>();
                var FileDataList1122 = new List<AccountDashboard>();
                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/TBill/GetAll?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAllPRR?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAll?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage cobresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CostOfBorrowing/GetAll?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage rfrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBorrow?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrowresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetRFR?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrow1response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetTIER_II_BOND?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrow2response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetCommercial_Paper?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrow3response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_CC?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrow4response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_WCL?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage borrow5response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetFOREX?user_id=" + new Guid(userid)).Result;
                HttpResponseMessage finhighlights = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllFinhighlights?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage highlight = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetHighlights?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage mourevised = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllMourevised?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage effratio = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllEffratio?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage yeild = _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountDashboard/GetAllYeild?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    //tbill
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<TBillModel>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FileUploadDataList = responses.data;
                    //prr
                    dynamic prrdata = prrresponse.Content.ReadAsStringAsync().Result;
                    var prrdataObject = new { data = new List<CreditRatingModel>() };
                    var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                    prrlist = prrresponses.data;
                    //creditrating
                    dynamic creditdata = creditresponse.Content.ReadAsStringAsync().Result;
                    var creditdataObject = new { data = new List<CreditRatingModel>() };
                    var creditresponses = JsonConvert.DeserializeAnonymousType(creditdata, creditdataObject);
                    creditList = creditresponses.data;
                    //cob
                    dynamic cobdata = cobresponse.Content.ReadAsStringAsync().Result;
                    var cobdataObject = new { data = new List<CostOfBorrowingModel>() };
                    var cobresponses = JsonConvert.DeserializeAnonymousType(cobdata, cobdataObject);
                    cobList = cobresponses.data;
                    //borrow
                    dynamic borrowdata = borrowresponse.Content.ReadAsStringAsync().Result;
                    var borrowdataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrowresponses = JsonConvert.DeserializeAnonymousType(borrowdata, borrowdataObject);
                    borrowList = borrowresponses.data;
                    //rfr
                    dynamic rfrdata = rfrresponse.Content.ReadAsStringAsync().Result;
                    var rfrdataObject = new { data = new List<BorroAndRFRModel>() };
                    var rfrresponses = JsonConvert.DeserializeAnonymousType(rfrdata, rfrdataObject);
                    rfrList = rfrresponses.data;
                    //borrow1
                    dynamic borrow1data = borrow1response.Content.ReadAsStringAsync().Result;
                    var borrow1dataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrow1responses = JsonConvert.DeserializeAnonymousType(borrow1data, borrow1dataObject);
                    borrow1List = borrow1responses.data;
                    //borrow
                    dynamic borrow2data = borrow2response.Content.ReadAsStringAsync().Result;
                    var borrow2dataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrow2responses = JsonConvert.DeserializeAnonymousType(borrow2data, borrow2dataObject);
                    borrow2List = borrow2responses.data;
                    //borrow
                    dynamic borrow3data = borrow3response.Content.ReadAsStringAsync().Result;
                    var borrow3dataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrow3responses = JsonConvert.DeserializeAnonymousType(borrow3data, borrow3dataObject);
                    borrow3List = borrow3responses.data;
                    //borrow
                    dynamic borrow4data = borrow4response.Content.ReadAsStringAsync().Result;
                    var borrow4dataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrow4responses = JsonConvert.DeserializeAnonymousType(borrow4data, borrow4dataObject);
                    borrow4List = borrow4responses.data;
                    //borrow
                    dynamic borrow5data = borrow5response.Content.ReadAsStringAsync().Result;
                    var borrow5dataObject = new { data = new List<BorroAndRFRModel>() };
                    var borrow5responses = JsonConvert.DeserializeAnonymousType(borrow5data, borrow5dataObject);
                    borrow5List = borrow5responses.data;

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
                    if (FileUploadDataList != null)
                    {
                        var localizedTitle = _localizer[""];
                        //var viewModel = new Tuple<IEnumerable<CreditRatingModel>, IEnumerable<CreditRatingModel>, IEnumerable<CostOfBorrowingModel>, IEnumerable<TBillModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>>(creditList, prrlist, cobList, FileUploadDataList, borrowList, rfrList, borrow1List, borrow2List, borrow3List, borrow4List, borrow5List);

                        var viewModel = new MyViewModel
                        {
                            CreditRatingModels = creditList,
                            OtherCreditRatingModels = prrlist,
                            CostOfBorrowingModels = cobList,
                            TBillModels = FileUploadDataList,
                            BorrowAndRFRModels = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
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
                            CreditRatingModels = creditList,
                            OtherCreditRatingModels = prrlist,
                            CostOfBorrowingModels = cobList,
                            TBillModels = FileUploadDataList,
                            BorrowAndRFRModels = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
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
                    CreditRatingModels = creditList,
                    OtherCreditRatingModels = prrlist,
                    CostOfBorrowingModels = cobList,
                    TBillModels = FileUploadDataList,
                    BorrowAndRFRModels = rfrList,
                    BorrowAndRFRModels1 = borrowList,
                    BorrowAndRFRModels2 = borrow1List,
                    BorrowAndRFRModels3 = borrow2List,
                    BorrowAndRFRModels4 = borrow3List,
                    BorrowAndRFRModels5 = borrow4List,
                    BorrowAndRFRModels6 = borrow5List,
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


    }
    public class MyViewModel
    {
        public IEnumerable<CreditRatingModel>? CreditRatingModels { get; set; }
        public IEnumerable<CreditRatingModel>? OtherCreditRatingModels { get; set; }
        public IEnumerable<CostOfBorrowingModel>? CostOfBorrowingModels { get; set; }
        public IEnumerable<TBillModel>? TBillModels { get; set; }
        public IEnumerable<borrow>? borrow { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels1 { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels2 { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels3 { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels4 { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels5 { get; set; }
        public IEnumerable<BorroAndRFRModel>? BorrowAndRFRModels6 { get; set; }
        public IEnumerable<BorroAndRFRModel>? borrowvalue { get; set; }
        public IEnumerable<AccountDashboard>? acclist1 { get; set; }
        public IEnumerable<AccountDashboard>? acclist2 { get; set; }
        public IEnumerable<AccountDashboard>? acclist3 { get; set; }
        public IEnumerable<AccountDashboard>? acclist4 { get; set; }
        public IEnumerable<AccountDashboard>? acclist5 { get; set; }
        public IEnumerable<AssetsLiabilities>? Assets { get; set; }
    }
}
