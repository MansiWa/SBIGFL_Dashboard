using PrintSoftWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Data;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace PrintSoftWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);

            _logger = logger;
            _localizer = localizer;

        }

        public IActionResult Index()
        {
            //         var homeDataList = new List<Home>(); ;
            //         //string RoleId = "D88D23D9-2BD2-4D68-A1F9-FBE4168B8DB1";
            //         string RoleId = HttpContext.Session.GetString("RoleId");
            //         string server = HttpContext.Session.GetString("Server_Value");

            //List<Home> homeList = new List<Home>();
            //         HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/GetWebMenu/GetAll?RoleId={RoleId}&server={server}").Result;
            //         if (response.IsSuccessStatusCode)
            //         {
            //             dynamic data = response.Content.ReadAsStringAsync().Result;
            //             var dataObject = new { data = new List<Home>() };
            //             var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
            //             homeDataList = response2.data;


            //             if (homeDataList != null)
            //             {
            //                  HttpContext.Session.SetString("HomeDataList", JsonConvert.SerializeObject(homeDataList));
            //                 ViewBag.menu = homeDataList;
            //                 var localizedTitle = _localizer[""];
            //                 return View(homeDataList);
            //             }
            //             else
            //             {
            //                 var localizedTitle = _localizer[""];
            //                 var homeDataList2 = new List<Home>();
            //                 return View(homeDataList2);
            //             }
            //         }

            return RedirectToAction("Index", "Login");
        }



		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ErrorIndex()
        {
            return View();
        }

    }
}