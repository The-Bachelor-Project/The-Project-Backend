namespace StockApp;

public class DividendPayout
{
	public String? portfolioId { get; set; }
	public int dividendID { get; set; }
	public Decimal sharesAmount { get; set; }

	public DividendPayout(String? portfolioId, int dividendID, Decimal sharesAmount)
	{
		this.portfolioId = portfolioId;
		this.dividendID = dividendID;
		this.sharesAmount = sharesAmount;
	}
}