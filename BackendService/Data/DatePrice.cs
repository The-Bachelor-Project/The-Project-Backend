
using StockApp;

namespace Data;

public class DatePrice
{
	public DatePrice(DateOnly date, Money openPrice, Money highPrice, Money lowPrice, Money closePrice)
	{
		this.date = date;
		this.openPrice = openPrice;
		this.highPrice = highPrice;
		this.lowPrice = lowPrice;
		this.closePrice = closePrice;
	}

	public DateOnly date { get; set; }
	public Money openPrice { get; set; }
	public Money highPrice { get; set; }
	public Money lowPrice { get; set; }
	public Money closePrice { get; set; }
}