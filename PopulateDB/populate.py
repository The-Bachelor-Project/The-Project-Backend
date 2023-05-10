import pandas as pd


def convertExchanges():
	DICTIONARY_CONVERT = {
		'ST':'STO',
		'TO':'TSE',
		'CBT':'CME',
	}

def main():
	convertExchanges()
	url = 'http://localhost:5000'
	df = pd.read_csv('tickers.csv')
	ticker_list = df['ticker'].tolist()
	exchange_list = df['convertedExchange'].tolist()
	

if __name__ == '__main__':
	main()