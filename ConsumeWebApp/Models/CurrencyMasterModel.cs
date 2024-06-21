using System.ComponentModel;

namespace PrintSoftWeb.Models
{
    public class CurrencyMasterModel
    {
        internal Guid? cr_id;

        public Guid? UserId { get; set; }
        public Guid? cm_id { get; set; }
        [DisplayName("Name")]
        public string? cm_currencyname { get; set; }
        [DisplayName("Symbol")]
        public string? cm_currencysymbol { get; set; }
        [DisplayName("Format")]
        public string? cm_currency_format { get; set; }
        [DisplayName("Code")]
        public string? cm_currencycode { get; set; }
        [DisplayName("Status")]
        public string? cm_isactive { get; set; }
        public DateTime? cm_createddate { get; set; }
        public DateTime? cm_updateddate { get; set; }

    }

}
