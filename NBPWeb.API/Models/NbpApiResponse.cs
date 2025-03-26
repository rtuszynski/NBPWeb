namespace NBPWeb.API.Models;

public class NbpApiResponse
{
    public string Table { get; set; }
    public string No { get; set; }
    public DateTime EffectiveDate { get; set; }
    public List<CurrencyRate> Rates { get; set; }
}
