# Transactions Ingest Coding Exercise - Alexander Lenth

# Prerequisites
- Ensure the latest Node.js and angular cli versions are installed 

# Building and Running



- Open a Powershell window inside the 'TransactionsIngest.Server' directory

- Enter 'dotnet run'

- The app should build successfully

- Open a browser and navigate to https://localhost:{port}



# Import data structure - array of transaction objects

```yaml

[
  {
  "sourceSystem": "Store Uptown",
  "transactionId": 100001,
  "cardNumber": "***1111",
  "locationCode": "UPTOWN",
  "productName": "Wireless Mouse",
  "amount": 24.99,
  "transactionTime": "2026-02-27T08:15:30Z"
  },
  {
  "sourceSystem": "Store Downtown",
  "transactionId": 100001,
  "cardNumber": "***0002",
  "locationCode": "DOWNTOWN",
  "productName": "Mechanical Keyboard",
  "amount": 89.99,
  "transactionTime": "2026-02-27T09:45:12Z"
  },
  {
  "sourceSystem": "Online",
  "transactionId": 100001,
  "cardNumber": "***4444",
  "locationCode": "ONLINE",
  "productName": "USB-C Docking Station",
  "amount": 129.95,
  "transactionTime": "2026-02-27T11:20:05Z"
  }
]

```



# Importing new data

### Option 1

- Open 'TransactionsIngest.slnx' in Visual Studio

- In the solution explorer, open 'TransactionsIngest.Server.http'

- Modify the JSON under the POST request and click 'Send Request'



### Option 2

- Open Postman

- Select 'POST' in the dropdown

- Enter https://localhost:{port}/transactions in the url input field

- In the body submenu, select 'raw' and select 'JSON' in the dropdown

- Paste JSON into body text field and click 'Send'



### Option 3

- Open https://localhost:{port} in a browser

- Click 'Import From File' button

- Select a JSON file matching the above data structure

- If successful, the transation table will update automatically. Otherwise, an alert will show and an error will appear in the console