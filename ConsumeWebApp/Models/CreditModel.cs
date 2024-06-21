using Microsoft.AspNetCore.Mvc;

namespace PrintSoftWeb.Models
{
    public class CreditModel 
    {
        public IEnumerable<CreditDetails>? listI { get; set; }
    }
    public class CreditDetails
    {
        public string? Branch_name { get; set; }
        public string? ClientCount { get; set; }
        public string? TotalOutstanding { get; set; }
        public string? CLIENT_ACCOUNT_CD { get; set; }
        public string? STATUS { get; set; }
        public string? FileDate { get; set; }
        public string? TotalPercentage { get; set; }

    }
}
