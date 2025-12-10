namespace ShopFinance.Application.Common.Constants;

public static class PhaseCodes
{
    public const string Applicant = "APPLICANT";
    public const string Questionnaire = "QUESTIONNAIRE";
    public const string Quotation = "QUOTATION";
    public const string Scoring = "SCORING";
    public const string Documentation = "DOCUMENTATION";
    public const string DocumentValidation = "DOCUMENT_VALIDATION";
    public const string Formalization = "FORMALIZATION";
    public const string ResourceDelivery = "RESOURCE_DELIVERY";
    public const string CreditActivation = "CREDIT_ACTIVATION";

    public static readonly Dictionary<string, int> CodeToId = new()
    {
        [Applicant] = 1,
        [Questionnaire] = 2,
        [Quotation] = 3,
        [Scoring] = 4,
        [Documentation] = 5,
        [DocumentValidation] = 6,
        [Formalization] = 7,
        [ResourceDelivery] = 8,
        [CreditActivation] = 9
    };

    public static int GetId(string code) =>
        CodeToId.TryGetValue(code, out var id) ? id : Math.Abs(code.GetHashCode());
}