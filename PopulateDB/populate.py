import pandas as pd
import requests
import json
import asyncio
import aiohttp
from tqdm import tqdm
import time

def get_exchanges():
	df = pd.read_csv('tickers.csv')
	exchanges = df['yfExchange'].unique()
	exchanges_df = pd.DataFrame(exchanges, columns=['exchange'])
	exchanges_df.to_csv('exchanges.csv', index=False)


def convertExchanges():
	DICTIONARY_CONVERT = { # MISSING 19 EXCHANGES
		'BA':'BCBA',
		'VI':'VIE',
		'AX':'ASX',
		'BR':'EBR',
		'SA':'BVMF',
		'SW':'SWX',
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
		'KQ':'KORSDAQ',
		'KS':'KRX', 
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
		'TWO':'TPO'
	}
	df = pd.read_csv('tickers.csv')
	for i, row in df.iterrows():
		if row['yfExchange'] in DICTIONARY_CONVERT:
			df.at[i, 'convertedExchange'] = DICTIONARY_CONVERT[row['yfExchange']]
	df.to_csv('tickers_converted.csv', index=False)

async def populate_db():
	url_quote = 'https://query1.finance.yahoo.com/v6/finance/quote?symbols={tic_exc}'
	url_summary = 'https://query1.finance.yahoo.com/v11/finance/quoteSummary/{tic_exc}?modules=assetProfile'
	df = pd.read_csv('tickers_converted.csv')
	print(len(df))
	counter = 36862 # COUNTER, UPDATE TO RESUME
	for i in tqdm(range(counter,len(df))):
		time.sleep(1)
		ticker = df.at[i,'ticker']
		exchange = df.at[i,'yfExchange']
		tic_exc = ''
		try:
			if (pd.isna(exchange)):
				tic_exc = ticker
			else:
				tic_exc = ticker + '.' + exchange
			async with aiohttp.ClientSession() as session:
				async with session.get(url_quote.format(tic_exc=tic_exc)) as response:
					response_json = await response.json()
					quote_data = response_json['quoteResponse']
			async with aiohttp.ClientSession() as session:
				async with session.get(url_summary.format(tic_exc=tic_exc)) as response:
					response_json = await response.json()
					summary_data = response_json['quoteSummary']
			final_json_data = {'quote':quote_data, 'summary':summary_data}
			tic_exc = tic_exc.replace('.', '_')
			with open(f'profiles_json/{tic_exc}.json', 'w') as f:
				json.dump(final_json_data, f)
		
		except Exception as e:
			print("------------------")
			print(e)
			print('Error with stock: ' + exchange + ':' + ticker)
                        

if __name__ == '__main__':
	# get_exchanges()
	# convertExchanges()
	asyncio.run(populate_db())
