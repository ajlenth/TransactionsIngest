\# Transactions Ingest Coding Exercise - Alexander Lenth



\# Building and Running



\- Open a Powershell window inside the 'TransactionsIngest.Server' directory

\- Enter 'dotnet run'

\- The app should build successfully

\- Control-click https://localhost:{port} to launch the app in a browser



\# Import data structure - array of transaction objects

```json

\[

&#x20; {

&#x20; "sourceSystem": "Store Uptown",

&#x20; "transactionId": 100001,

&#x20; "cardNumber": "\*\*\*1111",

&#x20; "locationCode": "UPTOWN",

&#x20; "productName": "Wireless Mouse",

&#x20; "amount": 24.99,

&#x20; "transactionTime": "2026-02-27T08:15:30Z"

&#x20; },

&#x20; {

&#x20; "sourceSystem": "Store Downtown",

&#x20; "transactionId": 100001,

&#x20; "cardNumber": "\*\*\*0002",

&#x20; "locationCode": "DOWNTOWN",

&#x20; "productName": "Mechanical Keyboard",

&#x20; "amount": 89.99,

&#x20; "transactionTime": "2026-02-27T09:45:12Z"

&#x20; },

&#x20; {

&#x20; "sourceSystem": "Online",

&#x20; "transactionId": 100001,

&#x20; "cardNumber": "\*\*\*4444",

&#x20; "locationCode": "ONLINE",

&#x20; "productName": "USB-C Docking Station",

&#x20; "amount": 129.95,

&#x20; "transactionTime": "2026-02-27T11:20:05Z"

&#x20; }

]

```



\# Importing new data

\### Option 1

\- Open 'TransactionsIngest.slnx' in Visual Studio

\- In the solution explorer, open 'TransactionsIngest.Server.http'

\- Modify the JSON under the POST request and click 'Send Request'



\### Option 2

\- Open Postman

\- Select 'POST' in the dropdown

\- Enter https://localhost:{port}/transactions in the url input field

\- In the body submenu, select 'raw' and select 'JSON' in the dropdown

\- Paste JSON into body text field and click 'Send'



\### Option 3

\- Open https://localhost:{port} in a browser

\- Click 'Import From File' button

\- Select a JSON file matching the above data structure

\- If successful, the transation table will update automatically. Otherwise, an alert will show and an error will appear in the console

