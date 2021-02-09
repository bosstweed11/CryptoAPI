# Crypto Documentation

## Endpoints

## GET Prices
GET `/crypto/v1/prices`
1. Gets all prices from start date to end date inclusive

### QueryString Params
| Name    | Type     | Example     |
| :------------- | :----------: | -----------: |
| baseCurrency   | Currency (USD, BTC, ETH, LTC, BCH) | BTC |
| quoteCurrency   | Currency (USD, BTC, ETH, LTC, BCH) | USD |
| startDate   | Date | 2020-08-01 |
| endDate   | Date | 2020-08-21 |

### Example

https://this.is.not.a.real.domain/crypto/v1/prices?baseCurrency=BTC&quoteCurrency=USD&startDate=2020-08-01&endDate=2020-08-21

### Response
```
[
    {
        "baseCurrency": "BTC",
        "quoteCurrency": "USD",
        "timeStamp": "2020-01-01T00:00:00Z",
        "value": 12345.67
    },
    {
        "baseCurrency": "BTC",
        "quoteCurrency": "USD",
        "timeStamp": "2020-01-02T00:00:00Z",
        "value": 12345.67
    },
    {
        "baseCurrency": "BTC",
        "quoteCurrency": "USD",
        "timeStamp": "2020-01-03T00:00:00Z",
        "value": 12345.67
    }
]
```


## GET Day Of Month Performance
GET `/crypto/v1/performance/daysOfMonth`
1. Gets hypothetical investment performance as if an investor would invest on each day of the month

### QueryString Params
| Name    | Type     | Example     |
| :------------- | :----------: | -----------: |
| baseCurrency   | Currency (USD, BTC, ETH, LTC, BCH) | BTC |
| quoteCurrency   | Currency (USD, BTC, ETH, LTC, BCH) | USD |

### Example

https://this.is.not.a.real.domain/crypto/v1/performance/daysOfMonth?baseCurrency=BTC&quoteCurrency=USD

### Response
```
[
  {
    "name": "31",
    "baseCurrency": "BTC",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 77.77,
    "sixMonthGainLoss": 139.22,
    "oneYearGainLoss": 232.81,
    "threeYearGainLoss": 344.31,
    "fiveYearGainLoss": 1340.07
  },
  {
    "name": "30",
    "baseCurrency": "BTC",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 79.3,
    "sixMonthGainLoss": 140.79,
    "oneYearGainLoss": 232.28,
    "threeYearGainLoss": 344.57,
    "fiveYearGainLoss": 1340.61
  }
  ...
]
```

## GET Average Monthly Performance
GET `/crypto/v1/performance`
1. Gets average performance for each crypto being tracked

### Example

https://this.is.not.a.real.domain/crypto/v1/performance

### Response
```
[
  {
    "name": "BTC",
    "baseCurrency": "BTC",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 77.77,
    "sixMonthGainLoss": 139.22,
    "oneYearGainLoss": 232.81,
    "threeYearGainLoss": 344.31,
    "fiveYearGainLoss": 1340.07
  },
  {
    "name": "ETH",
    "baseCurrency": "ETH",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 151.37,
    "sixMonthGainLoss": 205.69,
    "oneYearGainLoss": 413.73,
    "threeYearGainLoss": 496.69,
    "fiveYearGainLoss": 0
  },
  {
    "name": "LTC",
    "baseCurrency": "LTC",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 64.2,
    "sixMonthGainLoss": 103.2,
    "oneYearGainLoss": 144.32,
    "threeYearGainLoss": 117.67,
    "fiveYearGainLoss": 0
  },
  {
    "name": "BCH",
    "baseCurrency": "BCH",
    "quoteCurrency": "USD",
    "threeMonthGainLoss": 35.01,
    "sixMonthGainLoss": 45.12,
    "oneYearGainLoss": 52.75,
    "threeYearGainLoss": 40.6,
    "fiveYearGainLoss": 0
  }
]
]
```

## Get Supported Currencies
GET `/Crypto/v1/supportedCurrencies`
1. Fills yesterdays rates for all currencies being tracked

### Example

https://this.is.not.a.real.domain/crypto/v1/supportedCurrencies

### Response
```
[
    "BTC",
    "ETH",
    "LTC",
    "BCH"
]
```

## POST Fill Rates
POST `/Crypto/v1/fillRates`
1. Fills yesterdays rates for all currencies being tracked

### Example

https://this.is.not.a.real.domain/crypto/v1/fillRates

### Response
```
200
```


#### Supported Currency Pairs

Note: Base Currency is First, Quote Currency is second (BaseCurrency/QuoteCurrency)

* BTC/USD
    * Earliest Suported Date: 2015-07-20
* ETH/USD
    * Earliest Suported Date: 2016-05-18
* LTC/USD
    * Earliest Suported Date: 2016-08-17
* BCH/USD
    * Earliest Suported Date: 2017-12-20
