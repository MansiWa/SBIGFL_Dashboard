using Tokens;

namespace Master.API.Entity
{
    public class CurrencyMaster
    { 
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? cm_id { get; set; }
        public string? cm_currencyname { get; set; }
        public string? cm_currencysymbol { get; set; }
        public string? cm_currency_format { get; set; }
        public string? cm_currencycode { get; set; }
        public string? cm_isactive { get; set; }
        public DateTime? cm_createddate { get; set; }
        public DateTime? cm_updateddate { get; set; }


    }
}
