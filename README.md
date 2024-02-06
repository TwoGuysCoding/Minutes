# Minutes
A WPF-based application for serving Speech2TextAPI to Windows users. 
Provides production grade front-end dedicated for seamless integration with the API

# App overview
Currently the app can recod audio from the sound card, send it to the server and receive the transcribed text. The app also provides a simple UI for the user to interact with the app.
When the window loads it connects to the websocket server. When the user clicks the record button, the app starts recording audio and sends it to the server. The server then sends the transcribed text back to the app, which is displayed in the text box.


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

# FftAudioTransformer Class

A class that provides methods for transforming audio using Fast Fourier Transform (FFT) and calculating audio levels.

## Methods

### `double[] GetAudioLevels(IEnumerable<byte> audioBuffer, double additionalRandomNoise, int fftLength, float threshold)`

Calculates audio levels from the input audio buffer using FFT.

- `audioBuffer`: Input audio buffer.
- `additionalRandomNoise`: Additional random noise.
- `fftLength`: Length of the FFT.
- `threshold`: Threshold for considering the audio as silence.

### `double[] CreateLowLevelArray(int length)`

Creates an array of low-level values with the specified length.

- `length`: Length of the array.

### `double[] CalculateAudioLevelsFFT(IEnumerable<byte> audioBuffer, int fftLength)`

Calculates audio levels using FFT.

- `audioBuffer`: Input audio buffer.
- `fftLength`: Length of the FFT.

### `float CalculateRms(double[] audioLevels)`

Calculates the Root Mean Square (RMS) of the audio levels.

- `audioLevels`: Array of audio levels.

### `bool IsSilence(double[] audioLevels, float threshold)`

Determines if the audio is considered silence based on the threshold.

- `audioLevels`: Array of audio levels.
- `threshold`: Threshold value for silence detection.

