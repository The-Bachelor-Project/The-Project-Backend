namespace Data;

public class Portfolio
{
	public String name { get; set; }
	public String currency { get; set; }
	public List<DatePrice> valueHistory { get; set; }
	public List<Position> positions { get; set; }

	public Portfolio(String name, String currency, List<DatePrice> valueHistory, List<Position> positions)
	{
		this.name = name;
		this.currency = currency;
		this.valueHistory = valueHistory;
		this.positions = positions;
	}
}