namespace API.v1.Interfaces;

public interface IPortfolios
{
	public Data.Portfolio[] Get(string id);
	public Data.Portfolio[] Get();
	public bool Post(Data.Portfolio portfolio);
}