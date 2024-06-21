using Tokens;
namespace Master.Entity
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
        public Guid? f_id { get; set; }
        public string? f_fileid { get; set; }
        public string? f_docid { get; set; }
        public string? f_date { get; set; }
        public string? f_filename { get; set; }
        public string? f_count { get; set; }
        public string? f_updatedby { get; set; }
        public string? f_updateddate { get; set; }
        public DateTime? h_createddate { get; set; }
        public string? h_creadtedby { get; set; }
        public string? h_isactive { get; set; }
    }
}
