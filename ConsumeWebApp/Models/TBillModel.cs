namespace PrintSoftWeb.Models
{
    public class TBillModel
    {
        public Guid? UserId { get; set; }
        public Guid? tbr_id { get; set; }
        public DateTime? tbr_date { get; set; }
        public string? tbr_30_days { get; set; }
        public string? tbr_60_days { get; set; }
        public string? tbr_90_days { get; set; }
        public string? tbr_182_days { get; set; }
        public string? tbr_364_days { get; set; }
        public DateTime? tbr_createddate { get; set; }
    }
}
