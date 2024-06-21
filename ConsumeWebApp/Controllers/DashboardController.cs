using Amazon.Runtime.Internal.Util;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;

namespace PrintSoftWeb.Controllers
{

	public class DashboardController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DashboardController> _logger;
		private readonly IStringLocalizer<DashboardController> _localizer;
		public DashboardController(ILogger<DashboardController> logger, IStringLocalizer<DashboardController> localizer, IConfiguration configuration)
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
            var FileUploadDataList = new Dashboard();

            string? userid = Request.Cookies["com_id"];
            if (userid == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var prrlist = new List<companydata>();
            var fiulist = new List<companydata>();
            var turnlist = new List<companydata>();

            HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Dashboard/GetGrowthFIU?user_id=" + new Guid(userid)).Result;
            string jsonData = @"{
    ""UserId"": """ + userid + @""",
    ""client_npa"": [""0""],
    ""Product"": [""RF"",""DF"",""EF"",""LCDM"",""Treds"",""Gold Pool""]
}";
            List<string> clientNpa = new List<string> { "0" };
            List<string> product = new List<string> { "all","RF", "DF", "EF", "LCDM", "Treds", "Gold Pool" };


            ViewBag.SelectedClientNpa = clientNpa; // Assuming clientNpa is an array of selected values
            ViewBag.SelectedProduct = product;
            ViewBag.SelectedClientNpa1 = clientNpa; 
            ViewBag.SelectedProduct1 = product;
            // Encode JSON data
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send the HTTP POST request
            HttpResponseMessage fiuresponse = _httpClient.PostAsync(_httpClient.BaseAddress + "/Dashboard/GetFIU", content).Result;
            HttpResponseMessage turnresponse = _httpClient.PostAsync(_httpClient.BaseAddress + "/Dashboard/GetTurnover", content).Result;

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Dashboard/GetValue?UserId=" + new Guid(userid) + "&date=" + date).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new Dashboard() };
                var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                FileUploadDataList = responses.data;
                dynamic prrdata = prrresponse.Content.ReadAsStringAsync().Result;
                var prrdataObject = new { data = new List<companydata>() };
                var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                prrlist = prrresponses.data;
                if (prrlist.Count > 0)
                {
                    TempData["Date"] = prrlist[0].g_updateddate;
                }
                //fiu
                dynamic fiudata = fiuresponse.Content.ReadAsStringAsync().Result;
                var fiudataObject = new { data = new List<companydata>() };
                var fiuresponses = JsonConvert.DeserializeAnonymousType(fiudata, fiudataObject);
                fiulist = fiuresponses.data;
                if (fiulist.Count > 0)
                {
                    TempData["fiuDate"] = fiulist[0].filedate;
                }
                //turnover
                dynamic turndata = turnresponse.Content.ReadAsStringAsync().Result;
                var turndataObject = new { data = new List<companydata>() };
                var turnresponses = JsonConvert.DeserializeAnonymousType(turndata, turndataObject);
                turnlist = turnresponses.data;
                if (turnlist.Count > 0)
                {
                    TempData["turnDate"] = turnlist[0].filedate;
                }
                var viewModel = new DashboardC
                {
                    data = FileUploadDataList,
                    growth = prrlist,
                    fiu = fiulist,
                    turnover = turnlist
                };
                return View(viewModel);
            }
            var viewModel2 = new DashboardC
            {
                data = FileUploadDataList,
                growth = prrlist,
                fiu = fiulist,
                turnover = turnlist
            };
            return View(viewModel2);

        }
        [HttpPost]
        public IActionResult Find(string fiuDate, string[] clientNpa, string[] product,string turnDate, string[] clientNpa1, string[] product1)
        {
            string? userid = Request.Cookies["com_id"];

            if (clientNpa.Count() ==0)
            {
                clientNpa = new string[] { "0" };
                product = new string[] { "all", "RF", "DF", "EF", "LCDM", "Treds", "Gold Pool" };
            }
            
            if (clientNpa1.Count() == 0)
            {
                 clientNpa1 = new string[] { "0" };
                 product1 = new string[] { "all", "RF", "DF", "EF", "LCDM", "Treds", "Gold Pool" };
            }
            ViewBag.SelectedClientNpa = clientNpa.ToList(); // Assuming clientNpa is an array of selected values
            ViewBag.SelectedProduct = product.ToList(); ;
            ViewBag.SelectedClientNpa1 = clientNpa1.ToList(); ;
            ViewBag.SelectedProduct1 = product1.ToList(); ;
            var FileUploadDataList = new Dashboard();
             string? date="";
            if (userid == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var prrlist = new List<companydata>();
            var fiulist = new List<companydata>();
            var turnlist = new List<companydata>();

            HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Dashboard/GetGrowthFIU?user_id=" + new Guid(userid)).Result;
            string clientNpaJson = "[" + string.Join(",", clientNpa.Select(x => $"\"{x}\"")) + "]";
            string productJson = "[" + string.Join(",", product.Select(x => $"\"{x}\"")) + "]";
            string clientNpaJson1 = "[" + string.Join(",", clientNpa1.Select(x => $"\"{x}\"")) + "]";
            string productJson1 = "[" + string.Join(",", product1.Select(x => $"\"{x}\"")) + "]";
            // Construct the JSON string using string interpolation
            string jsonData = $@"{{
        ""UserId"": ""{userid}"",
        ""client_npa"": {clientNpaJson},
        ""date"": ""{fiuDate}"",
        ""Product"": {productJson}
    }}";
            string turnData = $@"{{
        ""UserId"": ""{userid}"",
        ""client_npa"": {clientNpaJson1},
        ""date"": ""{turnDate}"",
        ""Product"": {productJson1}
    }}";

            // Encode JSON data
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            StringContent content1 = new StringContent(turnData, Encoding.UTF8, "application/json");

            // Send the HTTP POST request
            HttpResponseMessage fiuresponse = _httpClient.PostAsync(_httpClient.BaseAddress + "/Dashboard/GetFIU", content).Result;
            HttpResponseMessage turnresponse = _httpClient.PostAsync(_httpClient.BaseAddress + "/Dashboard/GetTurnover", content1).Result;

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Dashboard/GetValue?UserId=" + new Guid(userid) + "&date=" + date).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new Dashboard() };
                var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                FileUploadDataList = responses.data;
                dynamic prrdata = prrresponse.Content.ReadAsStringAsync().Result;
                var prrdataObject = new { data = new List<companydata>() };
                var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                prrlist = prrresponses.data;
                if (prrlist.Count > 0)
                {
                    TempData["Date"] = prrlist[0].g_updateddate;
                }
                //fiu
                dynamic fiudata = fiuresponse.Content.ReadAsStringAsync().Result;
                var fiudataObject = new { data = new List<companydata>() };
                var fiuresponses = JsonConvert.DeserializeAnonymousType(fiudata, fiudataObject);
                fiulist = fiuresponses.data;
                if (fiulist.Count > 0)
                {
                    TempData["fiuDate"] = fiulist[0].filedate;
                }
                //turnover
                dynamic turndata = turnresponse.Content.ReadAsStringAsync().Result;
                var turndataObject = new { data = new List<companydata>() };
                var turnresponses = JsonConvert.DeserializeAnonymousType(turndata, turndataObject);
                turnlist = turnresponses.data;
                if (turnlist.Count > 0)
                {
                    TempData["turnDate"] = turnlist[0].filedate;
                }
                var viewModel = new DashboardC
                {
                    data = FileUploadDataList,
                    growth = prrlist,
                    fiu = fiulist,
                    turnover = turnlist
                };
                return View( "Index",viewModel);
            }
            var viewModel2 = new DashboardC
            {
                data = FileUploadDataList,
                growth = prrlist,
                fiu = fiulist,
                turnover = turnlist
            };
            return View("Index", viewModel2);

        }

    }
}
