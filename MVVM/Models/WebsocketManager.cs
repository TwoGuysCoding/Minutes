using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using Serilog;

namespace Minutes.MVVM.Models
{
    /// <summary>
    /// A class that handles the WebSocket connection to the server.
    /// It takes in a server URI to which you can connect calling the OpenConnectionAsync method.
    /// After the connection is open, you can receive messages from the server.
    /// To send data to the server, call the SendDataAsync method.
    /// To close the connection, call the CloseConnectionAsync method.
    /// </summary>
    /// <param name="serverUri">The server uri you want to connect to</param>
    internal class WebsocketManager(string serverUri, Action<string> receiveAction)
    {
        private ClientWebSocket? _clientWebSocket;  // the WebSocket client
        private readonly Uri _serverUri = new(serverUri);   // the server uri
        private readonly int _maxRetryAttempts = 5;  // the maximum number of retry attempts
        private readonly TimeSpan _delayBetweenRetries = TimeSpan.FromSeconds(8);  // the delay between retry attempts

        /// <summary>
        /// Tries to open a connection to the server.
        /// If the connection is open, it starts receiving messages from the server.
        /// If it fails to open the connection, it throws an exception.
        /// </summary>
        /// <returns>A bool which indicates if the operation was a success</returns>
        public async Task<bool> OpenConnectionAsync()
        {
            int retryAttempts = 0;

            while (retryAttempts < _maxRetryAttempts)
            {
                try
                {
                    // Create a new WebSocket client
                    _clientWebSocket = new ClientWebSocket();
                    await _clientWebSocket.ConnectAsync(_serverUri, CancellationToken.None);
                    Log.Information("Connected to WebSocket server");
    
                    await Task.Run(() => ReceiveMessages(receiveAction));
                    return true;
                }
                catch (Exception e)
                {
                    Log.Information($"Failed to connect to WebSocket on attempt {retryAttempts + 1}: {e.Message}");
                    retryAttempts++;
                }
            }
            Log.Fatal("Failed to connect to WebSocket after {retryAttempts + 1} attempts");
                MessageBox.Show("Failed to connect to WebSocket after {retryAttempts} attempts", "Fatal Error", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        /// <summary>
        /// Receives messages from the server. For now, it just prints them to the console.
        /// </summary>
        private async void ReceiveMessages(Action<string> receiveActionParam)
        {
            // Set the size of the buffer for receiving messages
            var buffer = new byte[1024];

            try
            {
                while (_clientWebSocket is { State: WebSocketState.Open })
                {
                    // Receive a message from the server
                    var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // Handle the received message (e.g., update UI or process audio). For now, just print it to the console
                    // TODO: Handle the received message
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        /*Log.Fatal("Connection closed on server end");
                        MessageBox.Show("Connection closed on server end", "Fatal Error", 
                                        MessageBoxButton.OK, MessageBoxImage.Error);*/
                        switch(_clientWebSocket.CloseStatus)
                        {
                            case WebSocketCloseStatus.NormalClosure:
                                Log.Information("Connection closed normally");
                                break;
                            case WebSocketCloseStatus.EndpointUnavailable:
                                Log.Information("Connection closed because the server is unavailable");
                                break;
                            case WebSocketCloseStatus.InternalServerError:
                                Log.Information("Connection closed because of an internal server error");
                                break;
                            case WebSocketCloseStatus.InvalidPayloadData:
                                Log.Information("Connection closed because of invalid payload data");
                                break;
                            case WebSocketCloseStatus.MandatoryExtension:
                                Log.Information("Connection closed because of a mandatory extension");
                                break;
                            case WebSocketCloseStatus.MessageTooBig:
                                Log.Information("Connection closed because the message was too big");
                                break;
                            case WebSocketCloseStatus.PolicyViolation:
                                Log.Information("Connection closed because of a policy violation");
                                break;
                            case WebSocketCloseStatus.ProtocolError:
                                Log.Information("Connection closed because of a protocol error");
                                break;
                            default:
                                Log.Information("Connection closed for an unknown reason");
                                break;
                        }
                        var openConnection = await OpenConnectionAsync();
                        if (!openConnection)
                        {
                            Log.Fatal("Failed to reconnect to WebSocket server");
                            MessageBox.Show("Failed to reconnect to WebSocket server", "Fatal Error", 
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                    if (result.MessageType != WebSocketMessageType.Text) continue;

                    receiveActionParam.Invoke(Encoding.UTF8.GetString(buffer, 0, result.Count));
                }
            }
            catch (Exception e)
            {
                // Handle exceptions (e.g., connection closed)
                Log.Information($"Failed to receive messages: {e.Message}");
                if(_clientWebSocket is not { State: WebSocketState.Open })
                {
                    var openConnection = await OpenConnectionAsync();
                    if (!openConnection)
                    {
                        Log.Fatal("Failed to reconnect to WebSocket server");
                        MessageBox.Show("Failed to reconnect to WebSocket server", "Fatal Error", 
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to send data to the server as a binary message. If it fails, it throws an exception.
        /// </summary>
        /// <param name="data">Data to be sent in the form of a binary message</param>
        /// <returns></returns>
        public async Task SendDataAsync(byte[] data)
        {
            try
            {
                // Send the entire audio file as a binary message
                await _clientWebSocket?.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None)!;
            }
            catch (Exception exception)
            {
                Log.Information($"Failed to send data to WebSocket: {exception.Message}");
            }
        }

        /// <summary>
        /// Tries to close the connection to the server. If it fails, it throws an exception.
        /// </summary>
        /// <returns>A bool which indicates if the operation was a success</returns>
        public async Task<bool> CloseConnectionAsync()
        {
            try
            {
                await _clientWebSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection",
                    CancellationToken.None)!;
                return true;
            }
            catch (Exception e)
            {
                Log.Information($"Failed to close WebSocket connection: {e.Message}");
                return false;
            }
        }

        public bool IsOpen()
        {
            return _clientWebSocket is { State: WebSocketState.Open };
        }

        public WebSocketState GetWebSocketState()
        {
            return _clientWebSocket?.State ?? WebSocketState.None;
        }
    }
}
