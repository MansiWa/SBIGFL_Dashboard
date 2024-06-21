using Tokens;

namespace PrintSoftWeb.Models
{
    public class Debt
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? date { get; set; }
        public string? d_count { get; set; }
        public string? d_filename { get; set; }
        public string? d_filetype { get; set; }
        public string? d_updatedby { get; set; }
        public string? Product { get; set; }
        public string? NoOfAccount { get; set; }
        public string? Amount { get; set; }
    }
    public class DebtModel
    {
        public IEnumerable<npa>? npa { get; set; }
        public IEnumerable<woa>? woa { get; set; }
        public IEnumerable<sma>? sma1 { get; set; }
        public IEnumerable<sma>? sma2 { get; set; }
        public IEnumerable<sma>? sma { get; set; }
        public IEnumerable<Debt>? debt { get; set; }
        public IEnumerable<Branch_DUEWeeklySMA>? Branch_DUE { get; set; }
        public IEnumerable<Branch_DUEWeeklySMA>? WeeklySMA { get; set; }
    }
    public class Branch_DUEWeeklySMA
    {
        public string? OVERDUE_BUCKET { get; set; }
        public string? REGION_NAME { get; set; }
        public string? NoOfAccount1 { get; set; }
        public string? NoOfAccount2 { get; set; }
        public string? NoOfAccount3 { get; set; }
        public string? FIU_OUTSTANDING1 { get; set; }
        public string? FIU_OUTSTANDING2 { get; set; }
        public string? FIU_OUTSTANDING3 { get; set; }
        public string? FIU_OVERDUE1 { get; set; }
        public string? FIU_OVERDUE2 { get; set; }
        public string? FIU_OVERDUE3 { get; set; }
        public string? TotalNoOfAccounts { get; set; }
        public string? TotalFIU_OVERDUE { get; set; }
        public string? TotalFIU_OUTSTANDING { get; set; }
        public string? n_filedate { get; set; }

    }

    public class woa
    {
        public string? dec { get; set; }
        public string? REGION { get; set; }
        public string? PRINCIPAL_WOA { get; set; }
        public DateTime? DATE_NPA { get; set; }
        public DateTime? DATE_WOA { get; set; }
        public string? w_filedate { get; set; }

    }
    public class npa
    {
        public string? CLIENT_NAME { get; set; }
        public string? REGION { get; set; }
        public string? PRINCIPAL_NPA { get; set; }
        public DateTime? DATE_NPA { get; set; }
        public string? n_filedate { get; set; }

    }
    public class sma
    {
        public string? CLIENT_NAME { get; set; }
        public string? BUSINESS_TYPE { get; set; }
        public string? SUBPRODUCT_CODE_PK { get; set; }
        public string? OVERDUE_BUCKET { get; set; }
        public string? REGION_NAME { get; set; }
        public string? ACCOUNT_LIMIT { get; set; }
        public string? FIU_OUTSTANDING { get; set; }
        public string? SL_OUTSTANDING { get; set; }
        public string? SL_OUTSTANDING_INR { get; set; }
        public string? FIU_OVERDUE { get; set; }
        public string? FIU_OVERDUE_INR { get; set; }
        public string? PENDING_INTEREST_AMT { get; set; }
        public string? PENDING_INTEREST_AMT_INR { get; set; }
        public string? PENDING_CHARGES_AMT { get; set; }
        public string? PENDING_CHARGES_AMT_INR { get; set; }
        public string? INTR_RECOVER_TILL_DT { get; set; }
        public string? MAX_OVERDUE_DAYS { get; set; }
        public string? DEBTOR_NAMES_WHEREIN_SL_OS { get; set; }
        public string? RECOURSE_DAYS { get; set; }
        public string? PAN_NO { get; set; }
        public string? PRINC_OUTSTANDING { get; set; }
        public string? PRINC_OUTSTANDING_INR { get; set; }
    }
}
