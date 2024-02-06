# Minutes
A WPF-based application for serving Speech2TextAPI to Windows users. 
Provides production grade front-end dedicated for seamless integration with the API

# Systems
An overview of all systems, their interactions and usage.
# WebsocketManager Class

A class that handles the WebSocket connection to the server. It takes in a server URI to which you can connect calling the `OpenConnectionAsync` method. After the connection is open, you can receive messages from the server. To send data to the server, call the `SendDataAsync` method. To close the connection, call the `CloseConnectionAsync` method.

## Constructors

### `WebsocketManager(string serverUri, Action<string> receiveAction)`

Initializes a new instance of the `WebsocketManager` class.

- `serverUri`: The server URI you want to connect to.
- `receiveAction`: An action to handle received messages from the server.

## Methods

### `Task<bool> OpenConnectionAsync()`

Tries to open a connection to the server. If the connection is open, it starts receiving messages from the server. If it fails to open the connection, it returns `false`.

### `Task SendDataAsync(byte[] data)`

Tries to send data to the server as a binary message. If it fails, it throws an exception.

- `data`: Data to be sent in the form of a binary message.

### `Task<bool> CloseConnectionAsync()`

Tries to close the connection to the server. If it fails, it returns `false`.

### `bool IsOpen()`

Returns a boolean indicating if the WebSocket connection is open.



# AudioRecorder Class

A class that handles recording audio. To use it properly, you must first initialize it with the `InitializeRecorder` method, passing in a function that will handle the recording. Then, you can start recording with the `StartRecording` method, and stop recording with the `StopRecording` method.

## Constructors

### `AudioRecorder(int sampleRate, int bits, int channels)`

Initializes a new instance of the `AudioRecorder` class with specified sample rate, bits, and channels.

- `sampleRate`: The sample rate of the audio.
- `bits`: The number of bits per sample.
- `channels`: The number of audio channels.

## Methods

### `void InitializeRecorder(Action<object?, WaveInEventArgs> recordingFunction)`

Initializes the recorder with a function that will handle the recording and the recording stopped event which will dispose of the recorder.

- `recordingFunction`: A function that will handle the recording.

### `void StartRecording()`

Starts the audio recording.

### `void StopRecording()`

Stops the audio recording.
