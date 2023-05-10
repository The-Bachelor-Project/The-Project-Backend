import pandas as pd

def get_exchanges():
	df = pd.read_csv('tickers.csv')
	exchanges = df['yfExchange'].unique()
	exchanges_df = pd.DataFrame(exchanges, columns=['exchange'])
	exchanges_df.to_csv('exchanges.csv', index=False)


def convertExchanges():
	DICTIONARY_CONVERT = {
		'BA':'BCBA',
		'VI':'VIE',
		'AX':'ASX',
		'BR':'EBR',
		'SA':'BVMF',
		'SW':'SWX-VTX', #CAN BE BOTH SWX OR VTX - CHECK BOTH
		'SS':'SHA',
		'SZ':'SHE',
		'F':'FRA',
		'DE':'ETR',
		'CO':'CPH',
		'TL':'TAL',
		'HE':'HEL',
		'PA':'EPA',
		'IL':'LON',
		'L':'LON',
		'HK':'HKG',
		'JK':'IDX',
		'TA':'TLV',
		'BO':'BOM',
		'NS':'NSE',
		'IC':'ICE',
		'T':'TYO',
		'KQ':'KRX-KORSDAQ', #CAN BE BOTH KRX OR KORSDAQ - CHECK BOTH
		'KS':'KRX-KORSDAQ', #CAN BE BOTH KRX OR KORSDAQ - CHECK BOTH
		'RG':'RSE',
		'MX':'BMV',
		'AS':'AMS',
		'NZ':'NZE',
		'WA':'WSE',
		'LS':'ELI',
		'ST':'STO',
		'SI':'SGX',
		'BK':'BKK',
		'IS':'IST',
		'TW':'TPE',
		'TWO':'TPE'
	}
	df = pd.read_csv('tickers.csv')
	for i, row in df.iterrows():
		if row['yfExchange'] in DICTIONARY_CONVERT:
			df.at[i, 'convertedExchange'] = DICTIONARY_CONVERT[row['yfExchange']]
	df.to_csv('tickers_converted.csv', index=False)

def populate_db():
	url = 'http://localhost:5000'
	# df = pd.read_csv('tickers.csv')
	# ticker_list = df['ticker'].tolist()
	# exchange_list = df['convertedExchange'].tolist()
	

if __name__ == '__main__':
	# get_exchanges()
	# convertExchanges()
	populate_db()