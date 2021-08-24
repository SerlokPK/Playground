using System;
using WebSocketSharp.Server;

namespace Server
{
    public class WebSocketServerWrapper
    {
        public void InitializeServer()
        {
            var wssv = new WebSocketServer("ws://localhost");
            wssv.AddWebSocketService<Validator>("/Validator");
            wssv.Start();
            Console.WriteLine("Server started");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
