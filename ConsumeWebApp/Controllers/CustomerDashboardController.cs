using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using iTextSharp.text;
using Common;
using System.Globalization;
using PrintSoftWeb.Models;
using System.Web.WebPages;
using MimeKit;
using MailKit.Net.Smtp;
using Common.Token;

namespace PrintSoftWeb.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerDashboardController> _logger;
        private readonly IStringLocalizer<CustomerDashboardController> _localizer;
        private readonly IConfiguration _configuration;

        public CustomerDashboardController(ILogger<CustomerDashboardController> logger, IStringLocalizer<CustomerDashboardController> localizer, IConfiguration configuration)
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
                var FIU_Turnover = new List<FIU_Turnover>();
                var FIU_Outstanding = new List<FIU_Outstanding>();
                var Branchwise_FIU = new List<Branchwise_FIU>();
                var dataView = _configuration.GetSection("EmailIds").GetChildren();
                var rootObject = dataView.Select(option => new ServerOption
                {
                    Key = option.Value,
                    Value = option.Key
                }).ToList();
                ViewBag.EmailId = rootObject;
                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetFIU_Turnover?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetFIU_Outstanding?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetBranchwise_FIU?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    //FIU_Turnover
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<FIU_Turnover>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FIU_Turnover = responses.data;

                    //FIU_Outstanding
                    dynamic prrdata = prrresponse.Content.ReadAsStringAsync().Result;
                    var prrdataObject = new { data = new List<FIU_Outstanding>() };
                    var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                    FIU_Outstanding = prrresponses.data;
                    //Branchwise_FIU
                    dynamic creditdata = creditresponse.Content.ReadAsStringAsync().Result;
                    var creditdataObject = new { data = new List<Branchwise_FIU>() };
                    var creditresponses = JsonConvert.DeserializeAnonymousType(creditdata, creditdataObject);
                    Branchwise_FIU = creditresponses.data;
                    var singleValue = "";
                    var Seconddate = "";
                    if (FIU_Outstanding.Count > 0)
                    {
                        singleValue = FIU_Outstanding[0].FileDate;
                        Seconddate = FIU_Outstanding[0].FileDate2;
                    }
                    TempData["Date"] = singleValue.AsDateTime().ToString("dd-MMM-yy");
                    TempData["Date1"] = Seconddate.AsDateTime().ToString("dd-MMM-yy");
                  
                    if (DateTime.TryParseExact(singleValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
                    {
                        // Get the first day of the current month
                        DateTime firstDayOfThisMonth = new DateTime(date1.Year, date1.Month, 1);
                        DateTime firstDayOfThisMonth1 = new DateTime(date1.Year-1, date1.Month, 1);
                        // Subtract one day to get the last day of the previous month
                        DateTime lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);
                        DateTime lastDayOfPreviousMonth1 = firstDayOfThisMonth.AddDays(-1);

                        string formattedDate = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        string formattedDate1 = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        // Now you can use formattedDate as needed
                        TempData["PreviousMonth"] = formattedDate;
                        TempData["PreviousMonthandyear"] = "MAR-23"; //formattedDate1;
                    }
                  int lastyear= date1.Year - 1;
                    if (DateTime.TryParseExact(singleValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2))
                    {
                        DateTime previousFinancialYearEnd;
                        DateTime previousFinancialYearEnd1;
                        // Check if the month of the input date is April or later
                        if (date2.Month >= 4)
                        {
                            // If yes, set the previous financial year end to March 31st of the same year
                            previousFinancialYearEnd = new DateTime(date2.Year, 3, 31);
                            previousFinancialYearEnd1 = new DateTime(date2.Year-1, 3, 31);
                        }
                        else
                        {
                            // If not, set the previous financial year end to March 31st of the previous year
                            previousFinancialYearEnd = new DateTime(date2.Year - 1, 3, 31);
                            previousFinancialYearEnd1 = new DateTime(date2.Year - 1, 3, 31);
                        }

                        // Format the date as needed
                        string formattedDate = previousFinancialYearEnd.ToString("MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        string formattedDate1 = previousFinancialYearEnd1.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        // Now you can use formattedDate as needed
                        TempData["LastMonth"] = formattedDate;
                        TempData["LastMonthandyear"] = "MAR-23";// formattedDate1;
                    }
                    if (FIU_Turnover != null)
                    {
                        var localizedTitle = _localizer[""];
                        //var viewModel = new Tuple<IEnumerable<CreditRatingModel>, IEnumerable<CreditRatingModel>, IEnumerable<CostOfBorrowingModel>, IEnumerable<TBillModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>>(creditList, prrlist, cobList, FileUploadDataList, borrowList, rfrList, borrow1List, borrow2List, borrow3List, borrow4List, borrow5List);

                        var viewModel = new CustomerModel
                        {
                            FIU_Turnover = FIU_Turnover,
                            FIU_Outstanding = FIU_Outstanding,
                            Branchwise_FIU = Branchwise_FIU
                        };
                        return View(viewModel);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var viewModel = new CustomerModel
                        {
                            FIU_Turnover = FIU_Turnover,
                            FIU_Outstanding = FIU_Outstanding,
                            Branchwise_FIU = Branchwise_FIU
                        };
                        return View(viewModel);
                    }
                }
                var viewModel2 = new CustomerModel
                {
                    FIU_Turnover = FIU_Turnover,
                    FIU_Outstanding = FIU_Outstanding,
                    Branchwise_FIU = Branchwise_FIU
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
                var dataView = _configuration.GetSection("EmailIds").GetChildren();
                var rootObject = dataView.Select(option => new ServerOption
                {
                    Key = option.Value,
                    Value = option.Key
                }).ToList();
                ViewBag.EmailId = rootObject;
                var FIU_Turnover = new List<FIU_Turnover>();
                var FIU_Outstanding = new List<FIU_Outstanding>();
                var Branchwise_FIU = new List<Branchwise_FIU>();
                string date = CustDate.Value.ToString("yyyy-MM-dd"); // Change the format as per your requirement

                string? userid = Request.Cookies["com_id"];// "C587998C-9C7F-4547-B9EA-FEE649274EBC";
                if (userid == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetFIU_Turnover?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage prrresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetFIU_Outstanding?user_id=" + new Guid(userid) + "&date=" + date).Result;
                HttpResponseMessage creditresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer/GetBranchwise_FIU?user_id=" + new Guid(userid) + "&date=" + date).Result;
                if (response.IsSuccessStatusCode)
                {
                    //FIU_Turnover
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<FIU_Turnover>() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    FIU_Turnover = responses.data;

                    //FIU_Outstanding
                    dynamic prrdata = prrresponse.Content.ReadAsStringAsync().Result;
                    var prrdataObject = new { data = new List<FIU_Outstanding>() };
                    var prrresponses = JsonConvert.DeserializeAnonymousType(prrdata, prrdataObject);
                    FIU_Outstanding = prrresponses.data;
                    //Branchwise_FIU
                    dynamic creditdata = creditresponse.Content.ReadAsStringAsync().Result;
                    var creditdataObject = new { data = new List<Branchwise_FIU>() };
                    var creditresponses = JsonConvert.DeserializeAnonymousType(creditdata, creditdataObject);
                    Branchwise_FIU = creditresponses.data;

                    TempData["Date"] = date.AsDateTime().ToString("dd-MMM-yy");
                    var Seconddate = "";
                    if (FIU_Outstanding.Count > 0)
                    {
                        Seconddate = FIU_Outstanding[0].FileDate2;
                    }
                    TempData["Date1"] = Seconddate.AsDateTime().ToString("dd-MMM-yy");
                    if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
                    {
                        // Get the first day of the current month
                        DateTime firstDayOfThisMonth = new DateTime(date1.Year, date1.Month, 1);
                        // Subtract one day to get the last day of the previous month
                        DateTime lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);

                        string formattedDate = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        // Now you can use formattedDate as needed
                        TempData["PreviousMonth"] = formattedDate;

                        TempData["PreviousMonthandyear"] = "MAR-23"; //formattedDate1;
                    }
                    if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2))
                    {
                        DateTime previousFinancialYearEnd;
                        // Check if the month of the input date is April or later
                        if (date2.Month >= 4)
                        {
                            // If yes, set the previous financial year end to March 31st of the same year
                            previousFinancialYearEnd = new DateTime(date2.Year, 3, 31);
                        }
                        else
                        {
                            // If not, set the previous financial year end to March 31st of the previous year
                            previousFinancialYearEnd = new DateTime(date2.Year - 1, 3, 31);
                        }

                        // Format the date as needed
                        string formattedDate = previousFinancialYearEnd.ToString("MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                        // Now you can use formattedDate as needed
                        TempData["LastMonth"] = formattedDate;
                        TempData["LastMonthandyear"] = "MAR-23";
                    }
                    if (FIU_Turnover != null)
                    {
                        var localizedTitle = _localizer[""];
                        //var viewModel = new Tuple<IEnumerable<CreditRatingModel>, IEnumerable<CreditRatingModel>, IEnumerable<CostOfBorrowingModel>, IEnumerable<TBillModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>, IEnumerable<BorroAndRFRModel>>(creditList, prrlist, cobList, FileUploadDataList, borrowList, rfrList, borrow1List, borrow2List, borrow3List, borrow4List, borrow5List);

                        var viewModel = new CustomerModel
                        {
                            FIU_Turnover = FIU_Turnover,
                            FIU_Outstanding = FIU_Outstanding,
                            Branchwise_FIU = Branchwise_FIU
                        };
                        return View("Index", viewModel);
                    }
                    else
                    {
                        var localizedTitle = _localizer[""];
                        var viewModel = new CustomerModel
                        {
                            FIU_Turnover = FIU_Turnover,
                            FIU_Outstanding = FIU_Outstanding,
                            Branchwise_FIU = Branchwise_FIU
                        };
                        return View("Index", viewModel);
                    }
                }
                var viewModel2 = new CustomerModel
                {
                    FIU_Turnover = FIU_Turnover,
                    FIU_Outstanding = FIU_Outstanding,
                    Branchwise_FIU = Branchwise_FIU
                };

                return View("Index", viewModel2);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult Pdf(string modelJson1)
        {
            CustomerModel model = JsonConvert.DeserializeObject<CustomerModel>(modelJson1);
            MemoryStream ms1 = new MemoryStream();
            var predate = "";
            string formattedDate = "";
            var singleValue = "";

            if (model.FIU_Outstanding != null && model.FIU_Outstanding.Any())
            {
                singleValue = model.FIU_Outstanding.First().FileDate;
            }
            string date = singleValue.AsDateTime().ToString("dd-MMM-yy");
            if (DateTime.TryParse(singleValue, out DateTime singleValueAsString))
            {
                var previousDate = singleValueAsString.AddDays(-1);
                predate = previousDate.ToString("dd-MMM-yy"); // Convert the DateTime back to string if needed
            }
            if (DateTime.TryParseExact(singleValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
            {
                DateTime firstDayOfThisMonth = new DateTime(date1.Year, date1.Month, 1);
                DateTime lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);
                formattedDate = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
            }




            if (model.FIU_Turnover != null)
            {

                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id='Summary'><h2>Snapshot of FIU and Turnover</h2>";
                htmlContent += "    <div class='col-lg-12  d-flex justify-content-end align-items-center' style='text-align: right;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>";
                htmlContent += "                    <th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "<th class='column100 ' data-column='column2'>Particulars</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>31-Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{formattedDate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column5'>{predate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column6'>{date}</th> </tr> </thead> <tbody>";

                int no6 = 1;

                foreach (var item in model.FIU_Turnover)
                {
                    htmlContent += $"                            <tr class='row100'> <td class='column100 column1 ' data-column='column1'>{@no6}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{@item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td> </tr>";
                    no6++;

                }
                htmlContent += "            </tbody></table></div></div> < div style = 'margin-top: 10px; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id = 'Summary' > <h2>LC - Region wise FIU Outstanding</h2>";
                htmlContent += "    <div width ='100%' style='text-align: right;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>< th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "                    <th class='column100 ' data-column='column2'>BRANCH</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{formattedDate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column5'>{predate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column6'>{date}</th> </tr> </thead>  <tbody>";
                int no1 = 1;
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                double total4 = 0;
                foreach (var item in model.FIU_Outstanding)
                {
                    htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'>{@no1}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{@item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td></tr>";
                    no1++;
                    total1 = total1 + Convert.ToDouble(item.col_1);
                    total2 = total2 + Convert.ToDouble(item.col_2);
                    total3 = total3 + Convert.ToDouble(item.col_3);
                    total4 = total4 + Convert.ToDouble(item.col_4);
                }
                htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'></td>";
                htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>Total</td>";
                htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{total1}</td>";
                htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{total2}</td>";
                htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{total3}</td>";
                htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{total4}</td></tr>";

                htmlContent += "            </tbody> </table> </div></div> < div style = 'margin-top: 10px; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id = 'Summary' ><h2>Branchwise FIU (DF,EF and RF)</h2> ";
                htmlContent += "    <div class='col-lg-12  d-flex justify-content-end align-items-center' style='text-align: right;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>";
                htmlContent += "                    <th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "                    <th class='column100 ' data-column='column2'>Branch</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{date}</th>";
                htmlContent += "                    <th class='column100 ' data-column='column5'>YOD %</th>";
                htmlContent += "                    <th class='column100 ' data-column='column6'>Mar 2024</th></tr> </thead> <tbody>";
                int no2 = 1;
                double total11 = 0;
                double total22 = 0;
                double total44 = 0;
                foreach (var item in model.Branchwise_FIU)
                {

                    htmlContent += $"                            <tr class='row100'><td class='column100 column1 ' data-column='column1'>{@no2}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"<td class='column100 column5' Style='{(double.Parse(item.col_3) < 0.00 ? "background-color: red;" : (double.Parse(item.col_3) > 16.00 ? "background-color: green;" : "background-color: yellow;"))}' data-column='column5'>{item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td> </tr>";
                    no2++;
                    total11 = total11 + Convert.ToDouble(item.col_1);
                    total22 = total22 + Convert.ToDouble(item.col_2);
                    total44 = total44 + Convert.ToDouble(item.col_4);
                }
                htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'></td>";
                htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>Total</td>";
                htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{total11}</td>";
                htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{total22}</td>";
                htmlContent += $"                                <td class='column100 column5 ' data-column='column5'></td>";
                htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{total44}</td></tr>";
                htmlContent += "  </tbody></table> </div></div>";

                if (htmlContent.Contains("<td"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                        pdfDoc.Open();
                        TextReader sr = new StringReader(htmlContent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        byte[] pdfBytes = ms.ToArray();
                        string base64Pdf = Convert.ToBase64String(pdfBytes);

                        ms1 = ms;

                        using (var stream = new MemoryStream())
                        {
                            return File(pdfBytes, "application/pdf", "ClientService.pdf");
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "No data found to export!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["errorMessage"] = "Failed to retrieve data from the API.";
                return RedirectToAction("Index");
            }
        }
        public IActionResult ExportToExcel(string modelJson)
        {
            CustomerModel model = JsonConvert.DeserializeObject<CustomerModel>(modelJson);

            if (model.FIU_Turnover != null && model.FIU_Outstanding != null)
            {
                var predate = "";
                string formattedDate = "";
                var singleValue = "";

                if (model.FIU_Outstanding != null && model.FIU_Outstanding.Any())
                {
                    singleValue = model.FIU_Outstanding.First().FileDate;
                }
                string date = singleValue.AsDateTime().ToString("dd-MMM-yy");
                if (DateTime.TryParse(singleValue, out DateTime singleValueAsString))
                {
                    var previousDate = singleValueAsString.AddDays(-1);
                    predate = previousDate.ToString("dd-MMM-yy"); // Convert the DateTime back to string if needed
                }
                if (DateTime.TryParseExact(singleValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
                {
                    DateTime firstDayOfThisMonth = new DateTime(date1.Year, date1.Month, 1);
                    DateTime lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);
                    formattedDate = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                }
                // Create a new Excel package
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var workbook = new XLWorkbook())
                    {
                        var cashOrderWorksheet = workbook.Worksheets.Add("FIU_Turnover");
                        var sheet2 = workbook.Worksheets.Add("FIU_Outstanding");
                        var sheet3 = workbook.Worksheets.Add("Branchwise_FIU");

                        var currentRow = 1;
                        //FIU_TURNOVER
                        cashOrderWorksheet.Cell(currentRow, 1).Value = "Particulars";
                        cashOrderWorksheet.Cell(currentRow, 2).Value = "31-Mar-23";
                        cashOrderWorksheet.Cell(currentRow, 3).Value = formattedDate;
                        cashOrderWorksheet.Cell(currentRow, 4).Value = predate;
                        cashOrderWorksheet.Cell(currentRow, 5).Value = date;
                        //FIU_OUTSTANDING
                        sheet2.Cell(currentRow, 1).Value = "Branch";
                        sheet2.Cell(currentRow, 2).Value = "Mar-23";
                        sheet2.Cell(currentRow, 3).Value = formattedDate;
                        sheet2.Cell(currentRow, 4).Value = predate;
                        sheet2.Cell(currentRow, 5).Value = date;
                        //Branchvise_FIU
                        sheet3.Cell(currentRow, 1).Value = "Branch";
                        sheet3.Cell(currentRow, 2).Value = "Mar-23";
                        sheet3.Cell(currentRow, 3).Value = date;
                        sheet3.Cell(currentRow, 4).Value = "YOD %";
                        sheet3.Cell(currentRow, 5).Value = "Mar 2024";
                        int index = 1;
                        double total1 = 0;
                        double total2 = 0;
                        double total3 = 0;
                        double total4 = 0;
                        double total11 = 0;
                        double total22 = 0;
                        double total44 = 0;
                        foreach (var item in model.FIU_Turnover)
                        {
                            // Populate data for each row in the CashOrder worksheet
                            // Example:
                            cashOrderWorksheet.Cell(index + 1, 1).Value = item.Particulars;
                            cashOrderWorksheet.Cell(index + 1, 2).Value = item.col_1;
                            cashOrderWorksheet.Cell(index + 1, 3).Value = item.col_2;
                            cashOrderWorksheet.Cell(index + 1, 4).Value = item.col_3;
                            cashOrderWorksheet.Cell(index + 1, 5).Value = item.col_4;
                            index++;
                        }
                        index = 1;
                        foreach (var item in model.FIU_Outstanding)
                        {
                            // Populate data for each row in the CashOrder worksheet
                            // Example:
                            sheet2.Cell(index + 1, 1).Value = item.Particulars;
                            sheet2.Cell(index + 1, 2).Value = item.col_1;
                            sheet2.Cell(index + 1, 3).Value = item.col_2;
                            sheet2.Cell(index + 1, 4).Value = item.col_3;
                            sheet2.Cell(index + 1, 5).Value = item.col_4;
                            index++;
                            total1 = total1 + Convert.ToDouble(item.col_1);
                            total2 = total2 + Convert.ToDouble(item.col_2);
                            total3 = total3 + Convert.ToDouble(item.col_3);
                            total4 = total4 + Convert.ToDouble(item.col_4);
                        }
                        sheet2.Cell(index + 1, 1).Value = "Total";
                        sheet2.Cell(index + 1, 2).Value = total1;
                        sheet2.Cell(index + 1, 3).Value = total2;
                        sheet2.Cell(index + 1, 4).Value = total3;
                        sheet2.Cell(index + 1, 5).Value = total4;
                        index = 1;
                        foreach (var item in model.Branchwise_FIU)
                        {
                            // Populate data for each row in the CashOrder worksheet
                            // Example:
                            sheet3.Cell(index + 1, 1).Value = item.Particulars;
                            sheet3.Cell(index + 1, 2).Value = item.col_1;
                            sheet3.Cell(index + 1, 3).Value = item.col_2;
                            sheet3.Cell(index + 1, 4).Value = item.col_3;
                            sheet3.Cell(index + 1, 5).Value = item.col_4;
                            index++;
                            total11 = total11 + Convert.ToDouble(item.col_1);
                            total22 = total22 + Convert.ToDouble(item.col_2);
                            total44 = total44 + Convert.ToDouble(item.col_4);
                        }
                        sheet3.Cell(index + 1, 1).Value = "Total";
                        sheet3.Cell(index + 1, 2).Value = total11;
                        sheet3.Cell(index + 1, 3).Value = total22;
                        sheet3.Cell(index + 1, 4).Value = "";
                        sheet3.Cell(index + 1, 5).Value = total44;
                        workbook.SaveAs(memoryStream);
                    }

                    // Convert the memory stream to an array and return as a file
                    var content = memoryStream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CustomerData.xlsx");
                }
            }
            else
            {
                TempData["errorMessage"] = "No data found to export!";
                return RedirectToAction("Index");
            }
        }
        public IActionResult Email(string modelJson2, string EmailId)
        {
            if (EmailId == "-- Select Email--")
            {
                TempData["SuccessMessage"] = "Please Select Email First!";
                return RedirectToAction("Index");
            }
            CustomerModel model = JsonConvert.DeserializeObject<CustomerModel>(modelJson2);
            MemoryStream ms1 = new MemoryStream();
            var predate = "";
            string formattedDate = "";
            var singleValue = "";

            if (model.FIU_Outstanding != null && model.FIU_Outstanding.Any())
            {
                singleValue = model.FIU_Outstanding.First().FileDate;
            }
            string date = singleValue.AsDateTime().ToString("dd-MMM-yy");
            if (DateTime.TryParse(singleValue, out DateTime singleValueAsString))
            {
                var previousDate = singleValueAsString.AddDays(-1);
                predate = previousDate.ToString("dd-MMM-yy"); // Convert the DateTime back to string if needed
            }
            if (DateTime.TryParseExact(singleValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
            {
                DateTime firstDayOfThisMonth = new DateTime(date1.Year, date1.Month, 1);
                DateTime lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);
                formattedDate = lastDayOfPreviousMonth.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
            }




            if (model.FIU_Turnover != null)
            {

                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id='Summary'><h2>Snapshot of FIU and Turnover</h2>";
                htmlContent += "    <div class='col-lg-12  d-flex justify-content-end align-items-center' style='text-align: end;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>";
                htmlContent += "                    <th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "<th class='column100 ' data-column='column2'>Particulars</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>31-Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{formattedDate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column5'>{predate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column6'>{date}</th> </tr> </thead> <tbody>";

                int no6 = 1;

                foreach (var item in model.FIU_Turnover)
                {
                    htmlContent += $"                            <tr class='row100'> <td class='column100 column1 ' data-column='column1'>{@no6}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{@item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td> </tr>";
                    no6++;

                }
                htmlContent += "            </tbody></table></div></div> < div style = 'margin-top: 10px; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id = 'Summary' > <h2>LC - Region wise FIU Outstanding</h2>";
                htmlContent += "    <div class='col-lg-12  d-flex justify-content-end align-items-center' style='text-align: end;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>< th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "                    <th class='column100 ' data-column='column2'>BRANCH</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{formattedDate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column5'>{predate}</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column6'>{date}</th> </tr> </thead>  <tbody>";
                int no1 = 1;
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                double total4 = 0;
                foreach (var item in model.FIU_Outstanding)
                {
                    htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'>{@no1}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{@item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td></tr>";
                    no1++;
                    total1 = total1 + Convert.ToDouble(item.col_1);
                    total2 = total2 + Convert.ToDouble(item.col_2);
                    total3 = total3 + Convert.ToDouble(item.col_3);
                    total4 = total4 + Convert.ToDouble(item.col_4);
                }
                htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'></td>";
                htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>Total</td>";
                htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{total1}</td>";
                htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{total2}</td>";
                htmlContent += $"                                <td class='column100 column5 ' data-column='column5'>{total3}</td>";
                htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{total4}</td></tr>";
                htmlContent += "            </tbody> </table> </div></div> < div style = 'margin-top: 10px; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem;' id = 'Summary' ><h2>Branchwise FIU (DF,EF and RF)</h2> ";
                htmlContent += "    <div class='col-lg-12  d-flex justify-content-end align-items-center' style='text-align: end;'>";
                htmlContent += "        <div class='col-auto' style='margin-top: 3rem;'> <a style='margin-left: 2rem;'> Amount in Cr. </a> </div></div>";
                htmlContent += "    <div style='margin-top: 3rem;'> <table style='width: 100%; border-collapse: collapse;'> <thead class='text-align-center'><tr style='background-color: #f2f2f2;'>";
                htmlContent += "                    <th class='column100 ' width='7%' data-column='column1'>Sr No</th>";
                htmlContent += "                    <th class='column100 ' data-column='column2'>Branch</th>";
                htmlContent += "                    <th class='column100 ' data-column='column3'>Mar-23</th>";
                htmlContent += $"                    <th class='column100 ' data-column='column4'>{date}</th>";
                htmlContent += "                    <th class='column100 ' data-column='column5'>YOD %</th>";
                htmlContent += "                    <th class='column100 ' data-column='column6'>Mar 2024</th></tr> </thead> <tbody>";
                int no2 = 1;
                double total11 = 0;
                double total22 = 0;
                double total44 = 0;
                foreach (var item in model.Branchwise_FIU)
                {

                    htmlContent += $"                            <tr class='row100'><td class='column100 column1 ' data-column='column1'>{@no2}</td>";
                    htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>{@item.Particulars}</td>";
                    htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{@item.col_1}</td>";
                    htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{@item.col_2}</td>";
                    htmlContent += $"<td class='column100 column5' Style='{(double.Parse(item.col_3) < 0.00 ? "background-color: red;" : (double.Parse(item.col_3) > 16.00 ? "background-color: green;" : "background-color: yellow;"))}' data-column='column5'>{item.col_3}</td>";
                    htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{@item.col_4}</td> </tr>";
                    no2++;
                    total11 = total11 + Convert.ToDouble(item.col_1);
                    total22 = total22 + Convert.ToDouble(item.col_2);
                    total44 = total44 + Convert.ToDouble(item.col_4);
                }
                htmlContent += $"                            < tr class='row100'> <td class='column100 column1 ' data-column='column1'></td>";
                htmlContent += $"                                <td class='column100 column2 tal' data-column='column2'>Total</td>";
                htmlContent += $"                                <td class='column100 column3 ' data-column='column3'>{total11}</td>";
                htmlContent += $"                                <td class='column100 column4 ' data-column='column4'>{total22}</td>";
                htmlContent += $"                                <td class='column100 column5 ' data-column='column5'></td>";
                htmlContent += $"                                <td class='column100 column6 ' data-column='column6'>{total44}</td></tr>";
                htmlContent += "  </tbody></table> </div></div>";

                if (htmlContent.Contains("<td"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                        pdfDoc.Open();
                        TextReader sr = new StringReader(htmlContent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        byte[] pdfBytes = ms.ToArray();
                        using (var clonedMemoryStream = new MemoryStream(pdfBytes.ToArray()))
                        {
                            string htmlContent2 = $@" <div style = 'font - family: Arial, sans - serif; background - color: #f9f9f9; margin: 0; padding: 0;'>
                <div style = 'max - width: 600px; margin: 0 auto; padding: 20px; background - color: #ffffff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);'>
                    <div style = 'text - align: center; margin - bottom: 20px; ' >
                        <a style = 'color: #00466a; text-decoration: none; font-size: 1.4em; font-weight: 600;'> {date}</a>
                    </div>
                    <div style = 'font - size: 1.1em; margin - bottom: 20px; ' >
                        <p> Hi,</ p >
                        <p> Please find the attached file for your Client Service details.</p>
                        <p> Regards,<br> SBI GLobal Factors Ltd. </p>
                    </div>
                    </div>
                </div> ";
                            // Create a new message and set the HTML content
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("SBI GLobal Factors Ltd.", _configuration["email:EmailId"])); // set your email
                            message.To.Add(new MailboxAddress("Hello User", EmailId)); // recipient email
                            message.Subject = "Client Service details";
                            var bodyBuilder = new BodyBuilder();
                            bodyBuilder.HtmlBody = htmlContent2;
                            bodyBuilder.Attachments.Add("ClientService", clonedMemoryStream);
                            message.Body = bodyBuilder.ToMessageBody();
                            int smtpPort = int.Parse(_configuration["email:SMTPPort"]);

                            using (var client = new SmtpClient())
                            {
                                client.Connect(_configuration["email:SMTPServer"], smtpPort, true);
                                client.Authenticate(_configuration["email:EmailId"], _configuration["email:Password"]); 
                                client.Send(message);
                                client.Disconnect(true);
                               
                            }
                            TempData["SuccessMessage"] = "Email Send Successfully!";
                            return RedirectToAction("Index");

                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "No data found to export!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["errorMessage"] = "Failed to retrieve data from the API.";
                return RedirectToAction("Index");
            }
        }

    }
}
