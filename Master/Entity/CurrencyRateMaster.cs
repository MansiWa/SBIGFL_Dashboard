using Tokens;

namespace Master.API.Entity
{
    public class CurrencyRateMaster
    { 
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? cr_id { get; set; }
        public string? cr_currencyid { get; set; }
        public string? cr_currencyrate { get; set; }
        public string? cr_fromdate { get; set; }
        public string? cr_todate { get; set; }
        public string? cr_currency_name { get; set; }
        public string? cr_isactive { get; set; }
        public DateTime? cr_updateddate { get; set; }
        public DateTime? cr_createddate { get; set; }

    }
}
