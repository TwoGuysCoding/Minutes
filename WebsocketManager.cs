using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Minutes
{
    /// <summary>
    /// A class that handles the WebSocket connection to the server.
    /// It takes in a server URI to which you can connect calling the OpenConnectionAsync method.
    /// After the connection is open, you can receive messages from the server.
    /// To send data to the server, call the SendDataAsync method.
    /// To close the connection, call the CloseConnectionAsync method.
    /// </summary>
    /// <param name="serverUri">The server uri you want to connect to</param>
    internal class WebsocketManager(string serverUri)
    {
        private ClientWebSocket? _clientWebSocket;  // the WebSocket client
        private readonly Uri _serverUri = new(serverUri);   // the server uri

        /// <summary>
        /// Tries to open a connection to the server.
        /// If the connection is open, it starts receiving messages from the server.
        /// If it fails to open the connection, it throws an exception.
        /// </summary>
        /// <returns>A bool which indicates if the operation was a success</returns>
        public async Task<bool> OpenConnectionAsync()
        {
            try
            {
                // Create a new WebSocket client
                _clientWebSocket = new ClientWebSocket();
                await _clientWebSocket.ConnectAsync(_serverUri, CancellationToken.None);
                Debug.WriteLine("Connected to WebSocket server");

                await Task.Run(ReceiveMessages);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to connect to WebSocket: {e.Message} in {e.Source}");
                return false;
            }
        }

        /// <summary>
        /// Receives messages from the server. For now, it just prints them to the console.
        /// </summary>
        private async void ReceiveMessages()
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
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Debug.WriteLine($"Server response: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exceptions (e.g., connection closed)
                Debug.WriteLine($"Failed to receive messages: {e.Message} in {e.Source}");
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
                Debug.WriteLine($"Failed to send data to WebSocket: {exception.Message} in {exception.Source}");    
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
                Debug.WriteLine($"Failed to close WebSocket connection: {e.Message} in {e.Source}");
                return false;
            }
        }

        public bool IsOpen()
        {
            return _clientWebSocket is { State: WebSocketState.Open };
        }
    }
}
