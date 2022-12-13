# petbot

This telegram bot manages  two goals, one of them is replying to your commands, most of which are some sort of parsing function,
the second function is to inform you on changes on websites.


Supported commands:

Moscow Interbank Currency Exchange currency rates
/moex usd or eur or tenge (send /moex to get all supported currencies)


Mir payment (~Russian Mastercard)
/mir tenge or dong or byn (send /mir to get all supported currencies)

Expense ledger (to keep a journal of your expenses) uses Google Sheets as a database; 
the spreadsheet link: https://docs.google.com/spreadsheets/d/17ML6HzYACZ-FHMowXxJQL9_hJ5MLGQOfBAqtR3EeBSI/edit?usp=sharing

/exe cafe 900   (adds a record of type cafe witn price 900)

/exe all (returns an overview of the past 7 days)

/exe all 3 (returns a summary of the previous three days)


Fuel prices are extracted from the Gasprom gas station app's API, which then returns the best locations.

/gas (returns station with best 92 benzin prices)

/gas 95 (returns station with the best 95 benzin prices)


System commands
/all retuns list of all commands regex





There 3 default spyer\informer\watcher
One  checks if a phone number is in stock.
One checks if the dollar-ruble rate is more than 65.
One just send datetime now every minure

Informers commands
/spyer all  returns list of all spyers
/spyer stop {name}  stops spyer by name
