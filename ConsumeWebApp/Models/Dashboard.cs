using System.Collections;
using Tokens;

namespace PrintSoftWeb.Models
{
    public class Dashboard
    {
        public BaseModel? BaseModel { get; set; }
        public string? UserId { get; set; }
        public string? date { get; set; }
        public string? rp { get; set; }
        public string? dfyield { get; set; }
        public string? rfyield { get; set; }

}
    public class DashboardC
    {
        public Dashboard? data { get; set; }
        public IEnumerable<companydata>? growth { get; set; }
        public IEnumerable<companydata>? fiu { get; set; }
        public IEnumerable<companydata>? turnover { get; set; }

    }
    public class companydata {
        public string? g_particulars { get; set; }
        public string? g_turnover { get; set; }
        public string? g_fiu { get; set; }
        public string? g_pat { get; set; }
        public string? g_updateddate { get; set; }
        public string? Client_Area_New { get; set; }
        public string? filedate { get; set; }
        public string? FIU_IN_CR { get; set; }
    }

}
