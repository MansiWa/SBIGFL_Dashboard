using ExcelDataReader;
using OfficeOpenXml;
using System.Data;
using System.IO;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using System.Drawing;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadReportController : ControllerBase
    {
        private readonly IReportRepository _ReportRepo;
        public UploadReportController(IReportRepository reportRepo)
        {
            _ReportRepo = reportRepo;

        }
        [HttpPost("Insert")]

        public async Task<IActionResult> Insert(IFormFile r_file)
        {
            Report user = new Report();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }

            user.BaseModel.OperationType = "Insert";
            try
            {
                string? UserId = HttpContext.Request.Form["UserId"];
                string? r_name = HttpContext.Request.Form["r_name"];
                string? r_type = HttpContext.Request.Form["r_type"];
                string? r_remark = HttpContext.Request.Form["r_remark"];
                string? r_dateString = HttpContext.Request.Form["r_date"];

                string? r_updateddateString = HttpContext.Request.Form["r_updateddate"];
                string? r_createddateString = HttpContext.Request.Form["r_createddate"];

                DateTime r_updateddate;
                DateTime r_createddate;
                DateTime r_date;
                if (r_file == null || r_file.Length == 0)
                {
                    Outcome outcome2 = new Outcome
                    {

                        OutcomeId = 0,
                        OutcomeDetail = "No data into excel!",
                        Tokens = null,
                        Expiration = null
                    };
                    return Ok(outcome2);
                }
                if (r_file != null)
                {
                    if (r_file.Length > 0)
                    {
                        // Define the allowed file extensions for Excel files
                        string[] AllowedFileExtensions = new string[] { ".xls", ".xlsx", ".xlsm", ".csv" };

                        // Check if the file extension is allowed
                        if (!AllowedFileExtensions.Contains(Path.GetExtension(r_file.FileName)))
                        {
                            ModelState.AddModelError("File", "Please upload a file of type: " + string.Join(", ", AllowedFileExtensions));
                        }
                        else
                        {
                            // Read the file content into a byte array
                            using (var ms = new MemoryStream())
                            {
                                r_file.OpenReadStream().CopyTo(ms);
                                var fileBytes = ms.ToArray();

                                // Store the byte array in your user object or process it as needed
                                user.r_filecopy = fileBytes;
                            }
                        }
                    }
                }

                user.UserId = UserId;
                user.r_name = r_name;
                user.r_type = r_type;
                user.r_remark = r_remark;
                int rowCount;
                if (DateTime.TryParse(r_dateString, out r_date))
                {
                    user.r_date = r_date;
                }
                if (DateTime.TryParse(r_updateddateString, out r_updateddate))
                {
                    user.r_updateddate = r_updateddate;
                }
                if (DateTime.TryParse(r_createddateString, out r_createddate))
                {
                    user.r_createddate = r_createddate;
                }

                DataTable dataTable = new DataTable();

                // Add columns to the DataTable based on your provided column names
                dataTable.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("CLIENT_ID", typeof(string)),
                    new DataColumn("CLIENT_NAME", typeof(string)),
                    new DataColumn("CLIENT_AREA", typeof(string)),
                    new DataColumn("CLIENT_STATUS", typeof(string)),
                    new DataColumn("CLIENT_NPA", typeof(string)),
                    new DataColumn("CLIENT_RATING", typeof(string)),
                    new DataColumn("CLIENT_CODE_DESC", typeof(string)),
                    new DataColumn("BUS_TYPE", typeof(string)),
                    new DataColumn("SUBPRODUCTTYPE", typeof(string)),
                    new DataColumn("CURRENCY", typeof(string)),
                    new DataColumn("FACTOR_TYPE", typeof(string)),
                    new DataColumn("COMPANY_ID_PK", typeof(string)),
                    new DataColumn("FACTORS_RETENTION", typeof(string)),
                    new DataColumn("UNALLOCATE_AMOUNT", typeof(string)),
                    new DataColumn("SL_LIMIT", typeof(string)),
                    new DataColumn("FIU_LIMIT", typeof(string)),
                    new DataColumn("PROVISION_PERCENTAGE", typeof(string)),
                    new DataColumn("FIU_OUT", typeof(string)),
                    new DataColumn("AMOUNT_YTM_CUR", typeof(string)),
                    new DataColumn("AMOUNT_YTD", typeof(string)),
                    new DataColumn("ALLOCATEAMOUNT_YTD", typeof(string)),
                    new DataColumn("BALANCE_AMOUNT", typeof(string)),
                    new DataColumn("APPROVED_AMOUNT", typeof(string)),
                    new DataColumn("DISAPP", typeof(string)),
                    new DataColumn("DISPUTED_AMOUNT", typeof(string)),
                    new DataColumn("FTG_CHG_YTD", typeof(string)),
                    new DataColumn("BRANCH_CODE_PK", typeof(string)),
                    new DataColumn("AMOUNT_MTD_CUR", typeof(string)),
                    new DataColumn("AMOUNT_MTD", typeof(string)),
                    new DataColumn("ALLOCATEAMOUNT_MTD", typeof(string)),
                    new DataColumn("FTG_CHG_MTD", typeof(string)),
                    new DataColumn("COMPANY_NAME", typeof(string)),
                    new DataColumn("DEDUCT", typeof(string)),
                    new DataColumn("BASE_CURRENCY_CD", typeof(string)),
                    new DataColumn("RATE_TO_BASE_CURRENCY", typeof(string)),
                    new DataColumn("FIU_IN_INR", typeof(string)),
                    new DataColumn("BDM_NAME", typeof(string)),
                    new DataColumn("DISCOUNT_TYPE", typeof(string)),
                    new DataColumn("ADD_1", typeof(string)),
                    new DataColumn("ADD_2", typeof(string)),
                    new DataColumn("ADD_3", typeof(string)),
                    new DataColumn("ADD_4", typeof(string)),
                    new DataColumn("ADD_5", typeof(string)),
                    new DataColumn("ADD_6", typeof(string)),
                    new DataColumn("ADD_7", typeof(string)),
                    new DataColumn("ADD_8", typeof(string)),
                    new DataColumn("ADD_9", typeof(string)),
                    new DataColumn("ADD_10", typeof(string)),
                    new DataColumn("FileDate", typeof(string)),
                    new DataColumn("BatchId", typeof(string))
                });
                using (var stream = new MemoryStream())
                {
                    using (Stream fileStream = r_file.OpenReadStream())
                    {
                        // Copy the file content to the memory stream
                        await fileStream.CopyToAsync(stream);
                    }
                    // Ensure the stream position is at the beginning
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial
                                                                                // Convert to XLSX if the file is in CSV or XLS format
                    MemoryStream convertedStream = new MemoryStream(); // Declare the variable outside the using block
                    if (Path.GetExtension(r_file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        FileConverter.ConvertCsvToXlsx(stream, convertedStream);
                    }
                    else if (Path.GetExtension(r_file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        FileConverter.ConvertXlsToXlsx(stream, convertedStream);
                    }
                    MemoryStream newstream= new MemoryStream();
                    if (convertedStream != null && convertedStream.Length > 0)
                    {
                        // Set the position of convertedStream to the beginning
                        convertedStream.Position = 0;
                        newstream = convertedStream;
                    }
                    else
                    {
                        newstream = stream;
                    }
                    newstream.Position = 0;

                    using (var package = new ExcelPackage(newstream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

                        rowCount = worksheet.Dimension.Rows;
                        if (rowCount == 1)
                        {
                            Outcome outcome2 = new Outcome
                            {

                                OutcomeId = 0,
                                OutcomeDetail = "No data into excel!",
                                Tokens = null,
                                Expiration = null
                            };
                            return Ok(outcome2);
                        }

                        // Assuming your data starts from the second row
                        for (int row = 2; row <= rowCount; row++)
                        {
                            // Get data from each cell in the current row and add it to the DataTable
                            dataTable.Rows.Add(new object[]
                            {
                                 worksheet.Cells[row, 1].Value?.ToString(),  // CLIENT_ID
                                 worksheet.Cells[row, 2].Value?.ToString(),  // CLIENT_NAME
                                 worksheet.Cells[row, 3].Value?.ToString(),  // CLIENT_AREA
                                 worksheet.Cells[row, 4].Value?.ToString(),  // CLIENT_STATUS
                                 worksheet.Cells[row, 5].Value?.ToString(),  // CLIENT_NPA
                                 worksheet.Cells[row, 6].Value?.ToString(),  // CLIENT_RATING
                                 worksheet.Cells[row, 7].Value?.ToString(),  // CLIENT_CODE_DESC
                                 worksheet.Cells[row, 8].Value?.ToString(),  // BUS_TYPE
                                 worksheet.Cells[row, 9].Value?.ToString(),  // SUBPRODUCTTYPE
                                 worksheet.Cells[row, 10].Value?.ToString(), // CURRENCY
                                 worksheet.Cells[row, 11].Value?.ToString(), // FACTOR_TYPE
                                 worksheet.Cells[row, 12].Value?.ToString(), // COMPANY_ID_PK
                                 worksheet.Cells[row, 13].Value?.ToString(), // FACTORS_RETENTION
                                 worksheet.Cells[row, 14].Value?.ToString(), // UNALLOCATE_AMOUNT
                                 worksheet.Cells[row, 15].Value?.ToString(), // SL_LIMIT
                                 worksheet.Cells[row, 16].Value?.ToString(), // FIU_LIMIT
                                 worksheet.Cells[row, 17].Value?.ToString(), // PROVISION_PERCENTAGE
                                 worksheet.Cells[row, 18].Value?.ToString(), // FIU_OUT
                                 worksheet.Cells[row, 19].Value?.ToString(), // AMOUNT_YTM_CUR
                                 worksheet.Cells[row, 20].Value?.ToString(), // AMOUNT_YTD
                                 worksheet.Cells[row, 21].Value?.ToString(), // ALLOCATEAMOUNT_YTD
                                 worksheet.Cells[row, 22].Value?.ToString(), // BALANCE_AMOUNT
                                 worksheet.Cells[row, 23].Value?.ToString(), // APPROVED_AMOUNT
                                 worksheet.Cells[row, 24].Value?.ToString(), // DISAPP
                                 worksheet.Cells[row, 25].Value?.ToString(), // DISPUTED_AMOUNT
                                 worksheet.Cells[row, 26].Value?.ToString(), // FTG_CHG_YTD
                                 worksheet.Cells[row, 27].Value?.ToString(), // BRANCH_CODE_PK
                                 worksheet.Cells[row, 28].Value?.ToString(), // AMOUNT_MTD_CUR
                                 worksheet.Cells[row, 29].Value?.ToString(), // AMOUNT_MTD
                                 worksheet.Cells[row, 30].Value?.ToString(), // ALLOCATEAMOUNT_MTD
                                 worksheet.Cells[row, 31].Value?.ToString(), // FTG_CHG_MTD
                                 worksheet.Cells[row, 32].Value?.ToString(), // COMPANY_NAME
                                 worksheet.Cells[row, 33].Value?.ToString(), // DEDUCT
                                 worksheet.Cells[row, 34].Value?.ToString(), // BASE_CURRENCY_CD
                                 worksheet.Cells[row, 35].Value?.ToString(), // RATE_TO_BASE_CURRENCY
                                 worksheet.Cells[row, 36].Value?.ToString(), // FIU_IN_INR
                                 worksheet.Cells[row, 37].Value?.ToString(), // BDM_NAME
                                 worksheet.Cells[row, 38].Value?.ToString(), // DISCOUNT_TYPE
                                 worksheet.Cells[row, 39].Value?.ToString(), // ADD_1
                                 worksheet.Cells[row, 40].Value?.ToString(), // ADD_2
                                 worksheet.Cells[row, 41].Value?.ToString(), // ADD_3
                                 worksheet.Cells[row, 42].Value?.ToString(), // ADD_4
                                 worksheet.Cells[row, 43].Value?.ToString(), // ADD_5
                                 worksheet.Cells[row, 44].Value?.ToString(), // ADD_6
                                 worksheet.Cells[row, 45].Value?.ToString(), // ADD_7
                                 worksheet.Cells[row, 46].Value?.ToString(), // ADD_8
                                 worksheet.Cells[row, 47].Value?.ToString(), // ADD_9
                                 worksheet.Cells[row, 48].Value?.ToString(), // ADD_10
                                 worksheet.Cells[row, 49].Value?.ToString(), // FileDate
                                 worksheet.Cells[row, 50].Value?.ToString()  // BatchId
                            });
                        }
                    }
                }
                user.DataTable = dataTable;

                var parameter = await _ReportRepo.Report(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string UserId, DateTime? r_date)
        {
            try
            {
                Report user = new Report();
                user.UserId = UserId;
                user.r_date = r_date;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _ReportRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
