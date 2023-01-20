// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AssetProfile
    {
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string industry { get; set; }
        public string sector { get; set; }
        public string longBusinessSummary { get; set; }
        public int fullTimeEmployees { get; set; }
        public List<CompanyOfficer> companyOfficers { get; set; }
        public int auditRisk { get; set; }
        public int boardRisk { get; set; }
        public int compensationRisk { get; set; }
        public int shareHolderRightsRisk { get; set; }
        public int overallRisk { get; set; }
        public int governanceEpochDate { get; set; }
        public int compensationAsOfEpochDate { get; set; }
        public int maxAge { get; set; }
    }

    public class CompanyOfficer
    {
        public int maxAge { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string title { get; set; }
        public int yearBorn { get; set; }
        public int fiscalYear { get; set; }
        public TotalPay totalPay { get; set; }
        public ExercisedValue exercisedValue { get; set; }
        public UnexercisedValue unexercisedValue { get; set; }
    }

    public class ExercisedValue
    {
        public int raw { get; set; }
        public object fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class QuoteSummary
    {
        public List<QuoteSummaryResult> result { get; set; }
        public object error { get; set; }
    }

    public class QuoteSummaryResult
    {
        public AssetProfile assetProfile { get; set; }
    }

    public class QuoteSummaryRoot
    {
        public QuoteSummary quoteSummary { get; set; }
    }

    public class TotalPay
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class UnexercisedValue
    {
        public int raw { get; set; }
        public object fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class QuoteResponse
    {
        public List<QuoteResult> result { get; set; }
        public object error { get; set; }
    }

    public class QuoteResult
    {
        public string language { get; set; }
        public string region { get; set; }
        public string quoteType { get; set; }
        public string typeDisp { get; set; }
        public string quoteSourceName { get; set; }
        public bool triggerable { get; set; }
        public string customPriceAlertConfidence { get; set; }
        public double regularMarketChangePercent { get; set; }
        public double regularMarketPrice { get; set; }
        public string currency { get; set; }
        public string marketState { get; set; }
        public bool esgPopulated { get; set; }
        public string exchange { get; set; }
        public string shortName { get; set; }
        public string longName { get; set; }
        public string messageBoardId { get; set; }
        public string exchangeTimezoneName { get; set; }
        public string exchangeTimezoneShortName { get; set; }
        public int gmtOffSetMilliseconds { get; set; }
        public string market { get; set; }
        public float firstTradeDateMilliseconds { get; set; }
        public double preMarketChange { get; set; }
        public double preMarketChangePercent { get; set; }
        public int preMarketTime { get; set; }
        public double preMarketPrice { get; set; }
        public double regularMarketChange { get; set; }
        public int regularMarketTime { get; set; }
        public double regularMarketDayHigh { get; set; }
        public string regularMarketDayRange { get; set; }
        public double regularMarketDayLow { get; set; }
        public int regularMarketVolume { get; set; }
        public double regularMarketPreviousClose { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }
        public int bidSize { get; set; }
        public int askSize { get; set; }
        public string fullExchangeName { get; set; }
        public string financialCurrency { get; set; }
        public double regularMarketOpen { get; set; }
        public int averageDailyVolume3Month { get; set; }
        public int averageDailyVolume10Day { get; set; }
        public double fiftyTwoWeekLowChange { get; set; }
        public double fiftyTwoWeekLowChangePercent { get; set; }
        public string fiftyTwoWeekRange { get; set; }
        public double fiftyTwoWeekHighChange { get; set; }
        public double fiftyTwoWeekHighChangePercent { get; set; }
        public double fiftyTwoWeekLow { get; set; }
        public double fiftyTwoWeekHigh { get; set; }
        public int dividendDate { get; set; }
        public int earningsTimestamp { get; set; }
        public int earningsTimestampStart { get; set; }
        public int earningsTimestampEnd { get; set; }
        public double trailingAnnualDividendRate { get; set; }
        public double trailingPE { get; set; }
        public double trailingAnnualDividendYield { get; set; }
        public double epsTrailingTwelveMonths { get; set; }
        public double epsForward { get; set; }
        public double epsCurrentYear { get; set; }
        public double priceEpsCurrentYear { get; set; }
        public int sharesOutstanding { get; set; }
        public double bookValue { get; set; }
        public double fiftyDayAverage { get; set; }
        public double fiftyDayAverageChange { get; set; }
        public double fiftyDayAverageChangePercent { get; set; }
        public double twoHundredDayAverage { get; set; }
        public double twoHundredDayAverageChange { get; set; }
        public double twoHundredDayAverageChangePercent { get; set; }
        public long marketCap { get; set; }
        public double forwardPE { get; set; }
        public double priceToBook { get; set; }
        public int sourceInterval { get; set; }
        public int exchangeDataDelayedBy { get; set; }
        public string averageAnalystRating { get; set; }
        public bool tradeable { get; set; }
        public bool cryptoTradeable { get; set; }
        public int priceHint { get; set; }
        public string displayName { get; set; }
        public string symbol { get; set; }
    }

    public class QuoteRoot
    {
        public QuoteResponse quoteResponse { get; set; }
    }