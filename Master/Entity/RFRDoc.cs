using System;
using Tokens;

namespace Master.Entity
{
    public class RFRDoc
    {
        public string? UserId { get; set; }
        public BaseModel? BaseModel { get; set; }
        public string? rfr_id { get; set; }
        public string? rfr_currency { get; set; }
        public DateTime? rfr_EffectiveDate { get; set; }
        public string? rfr_rate { get; set; }
        public DateTime? rfr_createdDate { get; set; }
    }
}