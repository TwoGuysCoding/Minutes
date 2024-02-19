# Overview of the classes in folder MVVM/Models
This section contains the description and usage of the classes in the folder MVVM/Models and
some example usages
## WebsocketManager
Note that the impementation is very likely to change so this description might be outdated.
The WebsocketManager is a class that is used to manage the websocket connection to the server.
The class contains methods to streamline the process of connecting, recieving and sending data.

### Usage
First, create a new instance of the WebsocketManager class and pass the uri and recieve action. 
The recieve action is a Delegate with signature `Action<string>` that is called when the websocket recieves a message.
Then, call the Connect method to connect to the server.
```csharp
var ws = new WebsocketManager("ws://localhost:8000", YourRecieveFunction);
await ws.OpenConnectionAsync();
```
The OpenConnectionAsync method is an async method that opens the connection to the server.
It returns a boolean that is true if the connection was opened successfully and false if it was not.

YourReceiveFunction is a function that is called when the websocket recieves a message.
```csharp
private void YourReceiveFunction(string message)
{
	// Do something with the message
}
```
To send a message to the server, call the SendDataAsync method.
```csharp
await ws.SendDataAsync(yourByteArray);
```
The SendDataAsync method is an async method that sends the byte array to the server.

If you want to close the connection, call the CloseConnectionAsync method.
```csharp
await ws.CloseConnectionAsync();
```
Again it returns a boolean that is true if the connection was closed successfully and false if it was not.

Also if you want to check if the connection is open, you can call the IsOpen method.
```csharp
bool isOpen = ws.IsOpen();
```
It returns a boolean that is true if the connection is open and false if it is not.

Here is a full example where the byte array correspoding to a an audio data is sent to the server.
```csharp
var yourRecordingDevice = new YourRecordingDevice();
yourRecordingDevice.StartRecording();
var yourByteArray = yourRecordingDevice.GetRecording();	// a hypothetical method that returns the byte array of the recording

var ws = new WebsocketManager("ws://localhost:8000", YourReceiveFunction);
await ws.OpenConnectionAsync();
await ws.SendDataAsync(yourByteArray);
await ws.CloseConnectionAsync();
```
We can assume that the server is listening for the audio data and transcribes it to text. The text is then sent back to the client and the 
YourRecieveFunction is called with the text as the message.
``` csharp
private void YourRecieveFunction(string message)
{
	Console.WriteLine(message);	// here as an example we just print the message to the console
}
```

The following code will send the yte array to the server and print the transcribed text to the console.
Again it is important to note that the implementation is very likely to change since it doesn't include
andy timeout which can be very costly when using on demand services.

## AudioRecorder
This class will be hevily refactored and the implementation is very likely to change.
It need more abstraction to allow choosing the recording device hence for now I am not going to describe it.

## FftAudioTransformer
This is the Optimus Prime of this project. I don't know where it came from and I have no idea how it works but 
somehow I became best friends with it. It is a class that is used to transform audio data and basically give you array
of audio levels which for example can be used to visualize the audio data.
To achieve this, use the GetAudioLevels method. Note that this is a static class so you don't need to create an instance of it.
```csharp
var audioLevels = FftAudioTransformer.GetAudioLevels(yourByteArray, yourRandomNoise, fftLenght, thesholdForConsideringSilence);
```
The GetAudioLevels method takes four parameters:
- yourByteArray: The byte array of the audio data
- yourRandomNoise: The random noise that is added to the audio data to make it look cool
- fftLenght: The length of the Fast Fourier Transform (google it idk)
- thesholdForConsideringSilence: The threshold for considering silence. If the audio level is below this threshold, it is considered silence. In my experience, 0.16 is a good value.
