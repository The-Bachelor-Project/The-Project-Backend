from csv import writer
from selenium import webdriver
from selenium.webdriver.common.by import By
import pandas as pd
import time

def get_urls_from_csv():
    df = pd.read_csv('region_urls_corrected.csv')
    url_list = df['url'].tolist()
    return url_list

def scrape_yf():
    url_list = get_urls_from_csv()
    driver = webdriver.Chrome()
    driver.get(url_list[0])
    yf_cookie_reject = driver.find_element(By.XPATH, '//button[@class="btn secondary reject-all"]')
    driver.implicitly_wait(3)
    yf_cookie_reject.click()
    driver.implicitly_wait(5)
    with open('tickers.csv', 'a', newline='') as csv_file:
        parameter = '?count=100&offset={offset}'
        for url in url_list:
            correct_url = url + parameter
            
            for offset in range(0, 9900, 100):
                try:
                    print(correct_url + ", offset: " + str(offset))
                    driver.get(correct_url.format(offset=offset))
                    scraped_tickers = driver.find_elements(By.XPATH, '//td[@aria-label="Symbol"]')
                    if (len(scraped_tickers) == 0):
                        print("Break with url " + correct_url + ", offset: " + str(offset))
                        break
                    for scraped_ticker in scraped_tickers:
                        if ('.' in scraped_ticker.text):
                            ticker = scraped_ticker.text.split('.')[0]
                            exchange = scraped_ticker.text.split('.')[1]
                            writer(csv_file).writerow([ticker, exchange, ''])
                        else:
                            ticker = scraped_ticker.text
                            writer(csv_file).writerow([ticker, '', ''])
                except Exception as e:
                    print(e)
                    print("Exception Break with url " + correct_url + ", offset: " + str(offset))
                    break

    csv_file.close()
    driver.quit()

def scrape_yf_region_urls():
    url = 'https://finance.yahoo.com/screener/unsaved/85bcbd17-5d43-4429-8509-3d319d6dab33'
    driver = webdriver.Chrome()
    driver.get(url)
    yf_cookie_reject = driver.find_element(By.XPATH, '//button[@class="btn secondary reject-all"]')
    driver.implicitly_wait(10)
    yf_cookie_reject.click()
    driver.implicitly_wait(10)
    remove_region_btn = driver.find_element(By.XPATH, '//li[@class="Bgc($hoverBgColor) Mend(5px) D(ib) Bdrs(3px) filterItem Mb(3px)"]')
    remove_region_btn.click()
    driver.implicitly_wait(10)
    add_region_btn = driver.find_element(By.XPATH, '//li[@class="D(ib) Mb(3px) filterAdd"]')
    add_region_btn.click()
    driver.implicitly_wait(10)
    region_labels = driver.find_elements(By.XPATH, '//ul[@class="M(0) P(0)"]//span[@class="C($tertiaryColor) Mstart(12px) Cur(p) Va(m)"]')
    estimated_label = driver.find_element(By.XPATH, '//div[@class="Fw(b) Fz(36px)"]')
    with open('region_urls.csv', 'a', newline='') as csv_file:
        region_labels[0].click()
        driver.implicitly_wait(10)
        find_stocks_of_region = driver.find_element(By.XPATH, '//button[@class="Bgc($linkColor) C(white) Fw(500) Px(20px) Py(9px) Bdrs(3px) Bd(0) Fz(s) D(ib) Whs(nw) Miw(110px) Bgc($linkActiveColor):h"]')
        find_stocks_of_region.click()
        driver.implicitly_wait(10)
        time.sleep(5)
        for i in range(1, len(region_labels)):
            estimated_label = driver.find_element(By.XPATH, '//div[@class="Fw(b) Fz(36px)"]')
            writer(csv_file).writerow([driver.current_url, estimated_label.text])
            remove_region_btn = driver.find_element(By.XPATH, '//li[@class="Bgc($hoverBgColor) Mend(5px) D(ib) Bdrs(3px) filterItem Mb(3px)"]')
            remove_region_btn.click()
            driver.implicitly_wait(10)
            add_region_btn = driver.find_element(By.XPATH, '//li[@class="D(ib) Mb(3px) filterAdd"]')
            add_region_btn.click()
            driver.implicitly_wait(10)
            region_labels = driver.find_elements(By.XPATH, '//ul[@class="M(0) P(0)"]//span[@class="C($tertiaryColor) Mstart(12px) Cur(p) Va(m)"]')
            region_labels[i].click()
            driver.implicitly_wait(10)
            find_stocks_of_region = driver.find_element(By.XPATH, '//button[@class="Bgc($linkColor) C(white) Fw(500) Px(20px) Py(9px) Bdrs(3px) Bd(0) Fz(s) D(ib) Whs(nw) Miw(110px) Bgc($linkActiveColor):h"]')
            find_stocks_of_region.click()
            driver.implicitly_wait(10)
            time.sleep(5)

        

def main():
    # scrape_yf_region_urls()
    scrape_yf()

if (__name__ == '__main__'):
    main()