# Minutes
A WPF-based application for serving Speech2TextAPI to Windows users. 
Provides production grade front-end dedicated for seamless integration with the API

# Systems
An overview of all systems, their interactions and usage.
## WebsocketManager class
This class is used to create and manage websocket connections. It is used to connect to 
the Speech2TextAPI and send audio data to it. It also receives the transcribed text from 
the API and sends it to the MainWindow class. The WebsocketManager class is used by the 
MainWindow class to manage the websocket connection.
### Usage
To use it properly first you need to pass the websocket URL to the constructor:
```csharp
// Create a new WebsocketManager object and connect to the websocket server at the given URL
WebsocketManager websocketManager = new WebsocketManager("ws://localhost:8080");
```
Then you need to call the Connect method to connect to the websocket server:
```csharp
// Connect to the websocket server
await websocketManager.OpenConnectionAsync();
```
The function returns Task<bool> which is true if the connection was successful and false if it was not.
The Connect method is asynchronous so you need to use the await keyword to call it.
After establishing the connection it calls RecieveMessanges method which listens for incoming messages from the server.
RecieveMessages take is Action<string> which is a function which defines what to do with the incoming message.
If you want to send data to the server you can use the SendData method:
```csharp
// Send data to the server
await websocketManager.SendDataAsync(data);
```
Data is in a byte format.

If you want to close the connection you can use the CloseConnection method:
```csharp
// Close the connection
await websocketManager.CloseConnectionAsync();
```

All of those function have try-catch blocks to handle exceptions. If an exception occurs it will be logged to the console.