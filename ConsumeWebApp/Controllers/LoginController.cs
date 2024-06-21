using Common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PrintSoftWeb.Models;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UAParser;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Org.BouncyCastle.Asn1.Ocsp;

namespace PrintSoftWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginController> _logger;
        private readonly IStringLocalizer<LoginController> _localizer;
        private readonly string baseaddresuser;
        public LoginController(ILogger<LoginController> logger, IStringLocalizer<LoginController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            baseaddresuser = new Uri(configuration.GetSection("Server:User").Value).ToString();
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            try
            {
                //string tblurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CountryMaster&sValue=co_country_name&id=co_id&IsActiveColumn=co_isactive&sCoulmnName";
                //HttpResponseMessage responseView = _httpClient.GetAsync(tblurl).Result;
                //dynamic tbldata = responseView.Content.ReadAsStringAsync().Result;
                //var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(tbldata);
                //ViewBag.CountryId = rootObject;
                //string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                //var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                //string ipAddress = feature?.RemoteIpAddress?.ToString();

                //// Check if the IP address is in IPv6 format and convert it to IPv4
                //using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                //{
                //    socket.Connect("8.8.8.8", 65530);
                //    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                //    ipAddress = endPoint.Address.ToString();
                //}
                //string userAgentString = Request.Headers["User-Agent"].ToString();
                //var uaParser = Parser.GetDefault();
                //ClientInfo clientInfo = uaParser.Parse(userAgentString);
                //string browserName = clientInfo.UserAgent.Family;
                //string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                //string url = $"{_httpClient.BaseAddress}/LoginDetails/Get?ip_address={ipAddress}&browserName={browserName}&browserVersion={browserVersion}";
                //var Logindetails = new LoginDetailsModel();
                //HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    dynamic data = response.Content.ReadAsStringAsync().Result;
                //    var dataObject = new { data = new LoginDetailsModel() };
                //    var responselogin = JsonConvert.DeserializeObject<LoginDetailsModel>(data);
                //    if (responselogin != null)
                //    {
                //        if (responselogin.com_password == null)
                //        {
                //            return View(responselogin);
                //        }
                //        else
                //        {
                //            string comid = responselogin.com_id;
                //            string? comname = responselogin.com_company_name;
                //            string? rolename = responselogin.rolename;
                //            string? staffid = responselogin.staffid;
                //            string? RoleId = responselogin.RoleId;
                //            string? co_country_code = responselogin.co_country_code;
                //            string? co_timezone = responselogin.co_timezone;
                //            string? cm_currency_format = responselogin.cm_currency_format;
                //            string? cm_currencysymbol = responselogin.cm_currencysymbol;
                //            //ViewBag.comname=comname;
                //            TempData["comname"] = comname;
                //            string serverValue = responselogin.server_Value;
                //            string loginId = responselogin.LoginId;
                //            string Baseaddress = _httpClient.BaseAddress.ToString();
                //            HttpContext.Session.SetString("com_id", comid);
                //            HttpContext.Session.SetString("Server_Value", serverValue);
                //            HttpContext.Session.SetString("loginId", loginId);
                //            HttpContext.Session.SetString("RoleId", RoleId);
                //            HttpContext.Session.SetString("RoleName", rolename);
                //            HttpContext.Session.SetString("StaffId", staffid);
                //            HttpContext.Session.SetString("BaseAddress", Baseaddress);
                //            HttpContext.Session.SetString("co_country_code", co_country_code);
                //            HttpContext.Session.SetString("co_timezone", co_timezone);
                //            HttpContext.Session.SetString("cm_currency_format", cm_currency_format);
                //            HttpContext.Session.SetString("cm_currencysymbol", cm_currencysymbol);
                //            HttpContext.Session.SetString("BaseAddressUser", baseaddresuser);
                //            TempData["successMessage"] = "Login successfully";
                //            return RedirectToAction("Index", "Dashboard");
                //        }
                //    }
                //    else
                //    {
                //        return View();
                //    }
                //}
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorIndex","Home");
            } 
        }
        [HttpPost]
        public IActionResult Create(LoginDetailsModel model)
        {
            try
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);
                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string outcomeDetail = "Please enter valid credentials!";

                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.result.outcome;
                     outcomeDetail = responsemodel.outcomeDetail;
                    if (rootObject.result.data == null)
                    {
                        return Ok(outcomeDetail);

                    }
                    dynamic comid = rootObject.result.data;
                    string com_id = comid.com_id;
                    string loginId = comid.LoginId;
                    //string? comname = comid.com_company_name;
                    //string? RoleId = comid.RoleId;
                    //string? staffid = comid.staffid;
                    //string? rolename = comid.rolename;
                    //string? co_country_code = comid.co_country_code;
                    //string? co_timezone = comid.co_timezone;
                    //string? cm_currency_format = comid.cm_currency_format;
                    //string? cm_currencysymbol = comid.cm_currencysymbol;

                    // ViewBag.comname = comname;

                    if (outcomeDetail == "Please enter valid credentials" || outcomeDetail == "Your subscription has been expired!" || com_id == null)
                    {
                        if (com_id == null)
                        {
                            outcomeDetail = "Please enter valid credentials!";
                            return Ok(outcomeDetail);
                        }
                        return Ok(outcomeDetail);
                    }
                    else
                    {
                        var result = new { com_id };
                        TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                        //TempData["comname"] = comname;
                        //HttpContext.Session.SetString("com_id", com_id);
                        Response.Cookies.Append("com_id", com_id);
                        Response.Cookies.Append("loginId", loginId);


                        return Ok(result);
                    }
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return Ok(outcomeDetail);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Delete()
        {
            try
            {
                LoginDetailsModel model = new LoginDetailsModel();
                string? comid = Request.Cookies["com_id"];
                string? loginId = Request.Cookies["loginId"];
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);
                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                model.com_id = comid;
                model.LoginId = loginId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails/LogOut", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responcedata = response.Content.ReadAsStringAsync().Result;
                    if (responcedata != null)
                    {
                        TempData["successMessage"] = responcedata;
                        //HttpContext.Session.Remove("Server_Value");
                        Response.Cookies.Delete("com_id");
                        Response.Cookies.Delete("loginId");
                        //HttpContext.Session.Remove("RoleId");
                        //HttpContext.Session.Remove("RoleName");
                        //HttpContext.Session.Remove("StaffId");
                        //HttpContext.Session.Remove("BaseAddress");
                        //HttpContext.Session.Remove("BaseAddressUser");
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword(LoginDetailsModel model)
        {
            try
            {
              
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails/ForgotPass", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responcedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responcedata);
                    dynamic responsemodel = rootObject.outcomeDetail;
                    string outcomeDetail = responsemodel;
                    if (responcedata != null)
                    {
                       TempData["successMessage"] = outcomeDetail;
                    }
                    return Ok(outcomeDetail);
                }
				return Ok("Invalid data!");
			}
			catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult Language(string culture)
        {
            culture = culture.Replace("?ui-culture=", "");
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) });
            return RedirectToAction("Index", "Login");
        }
    }
}
