using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server
{
    public class Validator : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine($"Message received: {e.Data}");
            Send(e.Data);
        }
    }
}
