using Tokens;

namespace Master.Entity
{
    public class BorroAndRFR
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? rb_id { get; set; }
        public string? rb_fileid { get; set; }
        public string? batchid { get; set; }
        public string? rb_docid { get; set; }
        public string? rb_doctype { get; set; }
        public string? rb_date { get; set; }
        public string? rb_filename { get; set; }
        public string? rb_count { get; set; }
        public string? rb_updatedby { get; set; }
        public DateTime? rb_updateddate { get; set; }
        public string? rb_isactive { get; set; }
        public string? date { get; set; }

        //rfr
        public string? Currency { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? Rate { get; set; }
        //borrow
        public string? Product { get; set; }
        public DateTime? Issued_Date { get; set; }
        public string? Amount { get; set; }
    }
}