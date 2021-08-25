using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Playground
{
    public class WebSocketClient : IDisposable
    {
        private WebSocket _socket;
        private bool _isDisposed;

        public string ServerAddress { get; set; }
        public int PingInterval { get; set; }
        public EventHandler<MessageEventArgs> OnMessageEvent { get; set; }
        public EventHandler<ErrorEventArgs> OnErrorEvent { get; set; }

        public WebSocketClient(string serverAddress)
        {
            ServerAddress = serverAddress;
        }

        public void StartConnection()
        {
            Connect();
            Task.Run(() => PingServerAsync());
        }

        public Task<bool> SendMessageAsync(string message)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            Console.WriteLine($"Sent message: {message}");

            try
            {
                _socket.SendAsync(message, isSuccesfful =>
                {
                    if (isSuccesfful)
                    {
                        taskCompletionSource.SetResult(isSuccesfful);

                        return;
                    }
                });
            }
            catch (Exception ex)
            {
                // Exceptions are handled in OnError event, but catch should be used
                // for unhandled exceptions
                taskCompletionSource.SetException(ex);
            }

            return taskCompletionSource.Task;
        }

        private void Connect()
        {
            _socket = new WebSocket(ServerAddress);
            _socket.OnError += OnError;
            _socket.Connect();
        }

        private async Task PingServerAsync()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(PingInterval));
                var result = _socket.Ping();
                Console.WriteLine($"Is alive: {result}");

                if (!result)
                {
                    await StartRecoveryAsync();
                }
            }
        }

        private async Task StartRecoveryAsync()
        {
            int attempts = 0;

            while (attempts < 3)
            {
                ++attempts;

                await Task.Delay(TimeSpan.FromSeconds(5));
                _socket.Connect();

                if (_socket.IsAlive)
                {
                    return;
                }
            }
        }

        private void OnError(object sender, ErrorEventArgs args)
        {
            if (OnErrorEvent != null)
            {
                OnErrorEvent.Invoke(sender, args);

                return;
            }

            // Default behiour if no event is registered
            Console.WriteLine($"Exception: {args.Message}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (_isDisposed)
            {
                return;
            }

            if (dispose)
            {
                _socket.OnError -= OnError;
                _socket.Close();
            }

            _isDisposed = true;
        }
    }
}
