namespace API.v1.Interfaces;

public interface ISearchResults
{
	public Data.StockProfile[] Get(string query);
}