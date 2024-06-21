using Tokens;

namespace Master.Entity
{
    public class CreditRating
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? cr_id { get; set; }
        public Guid? pr_id { get; set; }
        public string? cr_companyname { get; set; }
        public string? cr_str { get; set; }
        public string? cr_ltr { get; set; }
        public string? pr_rate { get; set; }
        public string? pr_value { get; set; }
        public DateTime? pr_date { get; set; }
        public DateTime? cr_createddate { get; set; }
        public DateTime? cr_date { get; set; }
    }
}
