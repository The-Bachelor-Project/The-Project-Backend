namespace StockApp;

public class DividendPayout
{
	public String? portfolioId { get; set; }
	public int dividendId { get; set; }
	public Decimal sharesAmount { get; set; }

	public DividendPayout(String? portfolioId, int dividendID, Decimal sharesAmount)
	{
		this.portfolioId = portfolioId;
		this.dividendId = dividendID;
		this.sharesAmount = sharesAmount;
	}
}