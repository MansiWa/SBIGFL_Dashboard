namespace PrintSoftWeb.Models
{
    public class BusinessModel
    {
        public IEnumerable<summery>? summery { get; set; }



        public IEnumerable<NBO>? nbo { get; set; }
        public IEnumerable<FSA>? fsa { get; set; }
        public IEnumerable<WithCredit>? WithCredit { get; set; }
        public IEnumerable<Sanctioned>? sanctioned { get; set; }
        public IEnumerable<DisbursalNew>? disbursalnew { get; set; }



        public IEnumerable<BDSummary>? BDSummary { get; set; }
        public IEnumerable<BDCredit>? BDCredit { get; set; }
        public IEnumerable<BDQuryResolve>? BDQuryResolve { get; set; }
        public IEnumerable<BDSanction>? BDSanction { get; set; }
    }
    public class summery
    {
        public string? Branch { get; set; }
        public string? NBOStageNo { get; set; }
        public string? NBOStageLimit { get; set; }
        public string? FSAStageNo { get; set; }
        public string? FSAStageLimit { get; set; }
        public string? PWithBranchesNo { get; set; }
        public string? PWithBranchesLimit { get; set; }
        public string? WithCreditNo { get; set; }
        public string? WithCreditLimit { get; set; }
        public string? SanctionedInNo { get; set; }
        public string? SanctionedInLimit { get; set; }
        public string? DisbursalsNumber { get; set; }
        public string? DisbursalsAmount { get; set; }
        public string? DisbursalsByDate { get; set; }
        public string? DisbursalsinAccounts { get; set; }
        public string? bd_date { get; set; }
    }
    public class NBO
    {
        public string? Location { get; set; }
        public string? BDM { get; set; }
        public string? Client { get; set; }
        public string? Proposed_Limit { get; set; }
        public string? Facility { get; set; }
        public string? Stage { get; set; }
        public string? NBO_Issued_date { get; set; }
        public string? NBO_acceptance_date { get; set; }
        public string? Updates { get; set; }
    }
    public class FSA
    {
        public string? Location { get; set; }
        public string? BDM { get; set; }
        public string? Client { get; set; }
        public string? Proposed_Limit { get; set; }
        public string? Facility { get; set; }
        public string? Stage { get; set; }
        public string? FSA_in_date { get; set; }
        public string? FSA_comp_date { get; set; }
        public string? Timelime_Pushing_credit { get; set; }
        public string? Updates { get; set; }
    }
    public class WithCredit
    {
        public string? Location { get; set; }
        public string? BDM { get; set; }
        public string? Client { get; set; }
        public string? Credit_officer { get; set; }
        public string? Proposed_Limit { get; set; }
        public string? Debtors { get; set; }
        public string? Date_of_submission { get; set; }
        public string? Days_Credit { get; set; }
        public string? Level { get; set; }
        public string? Updates { get; set; }
        public string? bd_date { get; set; }
    }
    public class Sanctioned
    {
        public string? Location { get; set; }
        public string? Client { get; set; }
        public decimal Sanctioned_Limit { get; set; }
        public string? Facility { get; set; }
        public string? Month { get; set; }
        public string? Activation_Date { get; set; }
        public string? Sanctioned_on { get; set; }
        public string? Updates { get; set; }
        public string? bd_date { get; set; }
    }
    public class DisbursalNew
    {
        public string? Location { get; set; }
        public string? Client { get; set; }
        public string? Sanctioned_Limit { get; set; }
        public string? Sanctioned_date { get; set; }
        public string? Facility { get; set; }
        public string? Disbursal_Amount { get; set; }
        public string? Expected_By { get; set; }
        public string? Remark { get; set; }
        public string? Updates { get; set; }
        public string? bd_date { get; set; }
    }
    public class BDSummary
    {
        public string? Branch { get; set; }
        public string? NewEPSCNo { get; set; }
        public string? NewEPSCLimit { get; set; }
        public string? NewSancNo { get; set; }
        public string? NewSancLimit { get; set; }
        public string? ActNo { get; set; }
        public string? ActLimit { get; set; }
        public string? SancActNo { get; set; }
        public string? SancActLimit { get; set; }
        public string? ProCrNo { get; set; }
        public string? ProCrLimit { get; set; }
        public string? ProBrNo { get; set; }
        public string? ProBrlimit { get; set; }
        public string? EstFIU { get; set; }
        public string? bd_date { get; set; }
    }
    public class BDCredit
    {
        public string? NameOfClient { get; set; }
        public string? NewExist { get; set; }
        public string? Status { get; set; }
        public string? Facility { get; set; }
        public string? FLS { get; set; }
        public string? Sub_date { get; set; }
        public string? CreditAnalyst { get; set; }
        public string? CreditPriority { get; set; }
        public string? ExpDis { get; set; }
        public string? Remarks { get; set; }
        public string? bd_date { get; set; }



    }

    public class BDQuryResolve
    {
        public string? Branch { get; set; }
        public string? NameOfClient { get; set; }
        public string? NewExist { get; set; }
        public string? Status { get; set; }
        public string? Facility { get; set; }
        public string? FLS { get; set; }
        public string? Sub_date { get; set; }
        public string? CreditAnalyst { get; set; }
        public string? CreditPriority { get; set; }
        public string? ExpDis { get; set; }
        public string? Remarks { get; set; }
        public string? bd_date { get; set; }
    }

    public class BDSanction
    {
        public string? Branch { get; set; }
        public string? NameOfClient { get; set; }
        public string? NewExist { get; set; }
        public string? Status { get; set; }
        public string? Facility { get; set; }
        public string? FLS { get; set; }
        public string? Sub_date { get; set; }
        public string? CreditAnalyst { get; set; }
        public string? CreditPriority { get; set; }
        public string? SanDate { get; set; }
        public string? ExpDis { get; set; }
        public string? Remarks { get; set; }
        public string? bd_date { get; set; }
    }
}

