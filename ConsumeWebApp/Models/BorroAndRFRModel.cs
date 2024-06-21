namespace PrintSoftWeb.Models
{
    public class BorroAndRFRModel
    {
        public Guid? UserId { get; set; }
        public Guid? rb_id { get; set; }
        public string? rb_fileid { get; set; }
        public string? rb_docid { get; set; }
        public string? batchid { get; set; }
        public string? rb_date { get; set; }
        public string? rb_filename { get; set; }
        public string? FileDate { get; set; }
        public string? rb_count { get; set; }
        public string? rb_doctype { get; set; }
        public string? rb_type { get; set; }
        public string? rb_tablename { get; set; }
        public string? rb_updatedby { get; set; }
        public DateTime? rb_updateddate { get; set; }
        public string? rb_isactive { get; set; }
        public string? FOREX { get; set; }
        public string? Bank_Line_WCL { get; set; }
        public string? Commercial_Paper { get; set; }
        public string? TIER_II_BOND { get; set; }
        public string? Perticulars { get; set; }
        public string? NoOfAccount { get; set; }
        //rfr
        public string? Currency { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? RFR { get; set; }
        public string? Rate { get; set; }
        //borrow
        public string? Product { get; set; }
        public DateTime? Issued_Date { get; set; }
        public string? Amount { get; set; }
        //TIER_II_BOND
        public string? Name { get; set; }
        public DateTime? Paid_UpTo { get; set; }
        public DateTime? Due_Date { get; set; }
        public DateTime? PymntDue_Date { get; set; }
        //Commercial_Paper
        public string? Investor { get; set; }
        public string? CP_NOS { get; set; }
        public string? CP_Deal_Rate { get; set; }
        public string? CP_Total_Cost { get; set; }
        public string? No_Of_Days { get; set; }
        public string? ISIN_No { get; set; }
        //Bank_Line_CC & Bank_Line_WCL
        public string? Bank_Name { get; set; }
        public string? Type_of_Loan { get; set; }
        //FOREX
        public string? SBI_LONDON { get; set; }
        public string? OUTSTANDING { get; set; }
        public string? CONVERSION_RATE { get; set; }
        public string? INR_OS { get; set; }
        public string? USD_Equiv_OS { get; set; }
        public string? Limit { get; set; }
        public string? Reference_Rate { get; set; }


    }
    public class borrow
    {
        public string? FOREX { get; set; }
        public string? Bank_Line_WCL { get; set; }
        public string? Commercial_Paper { get; set; }
        public string? TIER_II_BOND { get; set; }
        public string? FileDate { get; set; }

    }
}