using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Text;
using System.Web.WebPages;

namespace PrintSoftWeb.Controllers
{
	public class TreasuryDashboardController : Controller
	{
        private readonly HttpClient _httpClient;
        private readonly ILogger<TreasuryDashboardController> _logger;
        private readonly IStringLocalizer<TreasuryDashboardController> _localizer;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        private IConfiguration Configuration;
        public TreasuryDashboardController(ILogger<TreasuryDashboardController> logger, IStringLocalizer<TreasuryDashboardController> localizer, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
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
                var FileUploadDataList = new List<TBillModel>();
                var prrlist = new List<CreditRatingModel>();
                var creditList = new List<CreditRatingModel>();
                var cobList = new List<CostOfBorrowingModel>();
                var rfrList = new List<borrow>();
                var borrowList = new List<BorroAndRFRModel>();
                var borrow1List = new List<BorroAndRFRModel>();
                var borrow2List = new List<BorroAndRFRModel>();
                var borrow3List = new List<BorroAndRFRModel>();
                var borrow4List = new List<BorroAndRFRModel>();
                var borrow5List = new List<BorroAndRFRModel>();
                var borrowvalue = new List<BorroAndRFRModel>();
                var Assets = new List<AssetsLiabilities>();

                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/TBill/GetTopAll?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAllPRR?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAll?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage cobresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CostOfBorrowing/GetAll?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage rfrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBorrow?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrowresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetRFR?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrow1response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetTIER_II_BOND?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrow2response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetCommercial_Paper?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrow3response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_CC?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrow4response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_WCL?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrow5response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetFOREX?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage borrowrvalue = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBorrowValue?user_id=" + new Guid(userid)+ "&date=" + date).Result;
                HttpResponseMessage assetresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/GetAssests?UserId=" + new Guid(userid) + "&date=" + date).Result;

                if (response.IsSuccessStatusCode)
                {
                    //borrow value
                    dynamic bdata = borrowrvalue.Content.ReadAsStringAsync().Result;
                    var bdataObject = new { data = new List<BorroAndRFRModel>() };
                    var bresponses = JsonConvert.DeserializeAnonymousType(bdata, bdataObject);
                    borrowvalue = bresponses.data;
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
                    var singleValue = "";

                    if (borrowList.Count > 0)
                    {
                        singleValue = borrowList[0].FileDate;
                    }
                    TempData["Date"] = singleValue.AsDateTime().ToString("dd-MMM-yy");

                    //rfr
                    dynamic rfrdata = rfrresponse.Content.ReadAsStringAsync().Result;
                    var rfrdataObject = new { data = new List<borrow>() };
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
                    //asset
                    dynamic adata = assetresponse.Content.ReadAsStringAsync().Result;
                    var adataObject = new { data = new List<AssetsLiabilities>() };
                    var aresponses = JsonConvert.DeserializeAnonymousType(adata, adataObject);
                    Assets = aresponses.data;
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
                            borrow = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
                            borrowvalue = borrowvalue,
                            Assets = Assets


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
                            borrow = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
                            borrowvalue = borrowvalue,
                            Assets = Assets

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
                    borrow = rfrList,
                    BorrowAndRFRModels1 = borrowList,
                    BorrowAndRFRModels2 = borrow1List,
                    BorrowAndRFRModels3 = borrow2List,
                    BorrowAndRFRModels4 = borrow3List,
                    BorrowAndRFRModels5 = borrow4List,
                    BorrowAndRFRModels6 = borrow5List,
                    borrowvalue = borrowvalue,
                    Assets = Assets
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

                if (treDate == null)
                {
                    return RedirectToAction("Index");
                }
                var FileUploadDataList = new List<TBillModel>();
                var prrlist = new List<CreditRatingModel>();
                var creditList = new List<CreditRatingModel>();
                var cobList = new List<CostOfBorrowingModel>();
                var rfrList = new List<borrow>();
                var borrowList = new List<BorroAndRFRModel>();
                var borrow1List = new List<BorroAndRFRModel>();
                var borrow2List = new List<BorroAndRFRModel>();
                var borrow3List = new List<BorroAndRFRModel>();
                var borrow4List = new List<BorroAndRFRModel>();
                var borrow5List = new List<BorroAndRFRModel>();
                var borrowvalue = new List<BorroAndRFRModel>();
                var Assets = new List<AssetsLiabilities>();

                string? userid = Request.Cookies["com_id"];
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/TBill/GetTopAll?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAllPRR?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CreditRating/GetAll?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage cobresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/CostOfBorrowing/GetAll?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage rfrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBorrow?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrowresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetRFR?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrow1response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetTIER_II_BOND?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrow2response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetCommercial_Paper?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrow3response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_CC?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrow4response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBank_Line_WCL?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrow5response = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetFOREX?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage borrowrvalue = _httpClient.GetAsync(_httpClient.BaseAddress + "/UploadBorroRFR/GetBorrowValue?user_id=" + new Guid(userid) + "&date=" + treDate).Result;
                HttpResponseMessage assetresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/AssetsLiabilities/GetAssests?UserId=" + new Guid(userid) + "&date=" + treDate).Result;

                if (response.IsSuccessStatusCode)
                {
                    //borrow value
                    dynamic bdata = borrowrvalue.Content.ReadAsStringAsync().Result;
                    var bdataObject = new { data = new List<BorroAndRFRModel>() };
                    var bresponses = JsonConvert.DeserializeAnonymousType(bdata, bdataObject);
                    borrowvalue = bresponses.data;
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
                    var rfrdataObject = new { data = new List<borrow>() };
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
                    //asset
                    dynamic adata = assetresponse.Content.ReadAsStringAsync().Result;
                    var adataObject = new { data = new List<AssetsLiabilities>() };
                    var aresponses = JsonConvert.DeserializeAnonymousType(adata, adataObject);
                    Assets = aresponses.data;
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
                            borrow = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
                            borrowvalue = borrowvalue,
                            Assets = Assets


                        };
                        return View("Index",viewModel);
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
                            borrow = rfrList,
                            BorrowAndRFRModels1 = borrowList,
                            BorrowAndRFRModels2 = borrow1List,
                            BorrowAndRFRModels3 = borrow2List,
                            BorrowAndRFRModels4 = borrow3List,
                            BorrowAndRFRModels5 = borrow4List,
                            BorrowAndRFRModels6 = borrow5List,
                            borrowvalue = borrowvalue,
                            Assets = Assets

                        };
                        return View("Index", viewModel);
                    }
                }
                var viewModel2 = new MyViewModel
                {
                    CreditRatingModels = creditList,
                    OtherCreditRatingModels = prrlist,
                    CostOfBorrowingModels = cobList,
                    TBillModels = FileUploadDataList,
                    borrow = rfrList,
                    BorrowAndRFRModels1 = borrowList,
                    BorrowAndRFRModels2 = borrow1List,
                    BorrowAndRFRModels3 = borrow2List,
                    BorrowAndRFRModels4 = borrow3List,
                    BorrowAndRFRModels5 = borrow4List,
                    BorrowAndRFRModels6 = borrow5List,
                    borrowvalue = borrowvalue,
                    Assets = Assets
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
