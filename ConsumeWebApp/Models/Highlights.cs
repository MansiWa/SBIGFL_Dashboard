using Tokens;

namespace PrintSoftWeb.Models
{
    public class Highlights
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? h_id { get; set; }
        public string? h_date { get; set; }
        public string? h_particulars { get; set; }
        public string? h_mom { get; set; }
        public string? h_yoy { get; set; }
        public string? h_ytd { get; set; }
        public string? h_col1 { get; set; }
        public string? h_col2 { get; set; }
        public string? h_col3 { get; set; } 
        public string? h_col4 { get; set; }
        public DateTime? h_createddate { get; set; }
        public string? h_creadtedby { get; set; }
        public string? h_isactive { get; set; }
    }
}
