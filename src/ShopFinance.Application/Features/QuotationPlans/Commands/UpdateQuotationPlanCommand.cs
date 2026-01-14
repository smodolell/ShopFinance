using ShopFinance.Application.Features.QuotationPlans.DTOs;

namespace ShopFinance.Application.Features.QuotationPlans.Commands;

public class UpdateQuotationPlanCommand : ICommand<Result<int>>
{
    public int QuotationPlanId { get; set; }

    public QuotationPlanEditDto? Model { get; set; }





    //public int? TaxRateId { get; set; }
    //public string Code { get; set; } = string.Empty;
    //public string PlanName { get; set; } = string.Empty;
    //public DateTime? InitialEffectiveDate { get; set; }
    //public DateTime? FinalEffectiveDate { get; set; }
    //public int? PhaseIdInitial { get; set; }
    //public int? PhaseIdFinal { get; set; }
    //public bool Active { get; set; }
    //public List<QuotationPlanPhaseCommand> Phases { get; set; } = new();
    //public List<QuotationPlanFrequencyCommand> Frequencies { get; set; } = new();
    //public List<QuotationPlanPaymentTermCommand> PaymentTerms { get; set; } = new();
}

//public class QuotationPlanPhaseCommand
//{
//    public int PhaseId { get; set; }
//    public string PhaseName { get; set; } = string.Empty;

//    public bool IsInitial { get; set; }
//    public bool IsFinal { get; set; }
//    public bool Required { get; set; }
//    public decimal Order { get; set; }
//    public bool Active { get; set; } = true;
//}
//public class QuotationPlanFrequencyCommand
//{
//    public int FrequencyId { get; set; }
//    public bool IsDefault { get; set; }
//    public int Order { get; set; }
//    public bool Active { get; set; } = true;
//}

//public class QuotationPlanPaymentTermCommand
//{
//    public int PaymentTermId { get; set; }
//    public int? InterestRateId { get; set; }
//    public decimal? SpecialRateOverride { get; set; }
//    public bool IsPromotional { get; set; }
//    public DateTime? PromotionEndDate { get; set; }
//    public int Order { get; set; }
//    public bool Active { get; set; } = true;
//}