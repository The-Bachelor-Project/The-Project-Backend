
using StockApp;

namespace Data;

public class DatePriceOHLC
{
	public DatePriceOHLC(DateOnly date, Money openPrice, Money highPrice, Money lowPrice, Money closePrice)
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


	public static List<DatePriceOHLC> AddLists(List<DatePriceOHLC> list1, List<DatePriceOHLC> list2)
	{
		if (list1.Count == 0)
		{
			return list2;
		}
		if (list2.Count == 0)
		{
			return list1;
		}

		DateOnly startDate = list1[0].date < list2[0].date ? list1[0].date : list2[0].date;
		DateOnly endDate = list1[^1].date > list2[^1].date ? list1[^1].date : list2[^1].date;

		List<DatePriceOHLC> result = new List<DatePriceOHLC>();

		for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
		{
			Money openPrice = new Money(0, list1[0].openPrice.currency);
			Money highPrice = new Money(0, list1[0].openPrice.currency);
			Money lowPrice = new Money(0, list1[0].openPrice.currency);
			Money closePrice = new Money(0, list1[0].openPrice.currency);

			if (list1.Exists(x => x.date == date))
			{
				openPrice.amount += list1.Find(x => x.date == date)!.openPrice.amount;
				highPrice.amount += list1.Find(x => x.date == date)!.highPrice.amount;
				lowPrice.amount += list1.Find(x => x.date == date)!.lowPrice.amount;
				closePrice.amount += list1.Find(x => x.date == date)!.closePrice.amount;
			}
			if (list2.Exists(x => x.date == date))
			{
				openPrice.amount += list2.Find(x => x.date == date)!.openPrice.amount;
				highPrice.amount += list2.Find(x => x.date == date)!.highPrice.amount;
				lowPrice.amount += list2.Find(x => x.date == date)!.lowPrice.amount;
				closePrice.amount += list2.Find(x => x.date == date)!.closePrice.amount;
			}

			result.Add(new DatePriceOHLC(date, openPrice, highPrice, lowPrice, closePrice));
		}


		return result;
	}
}