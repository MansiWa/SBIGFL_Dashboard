using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class CurrencyRateMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? cr_id { get; set; }
        [DisplayName("Currency Id")]
        public string? cr_currencyid { get; set; }
        [DisplayName("Currency Name")]
        public string? cr_currency_name { get; set; }
        [DisplayName("Currency Rate")]
        public string? cr_currencyrate { get; set; }
        [DisplayName("From Date")]
        public string? cr_fromdate { get; set; }
        [DisplayName("To Date")]
        public string? cr_todate { get; set; }
        [DisplayName("Status")]
        public string? cr_isactive { get; set; }
        public DateTime? cr_createddate { get; set; }
        public DateTime? cr_updateddate { get; set; }
    }
}
