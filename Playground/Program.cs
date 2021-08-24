using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            WSSHelper();
            //MultipleTasks();
        }

        private async static void WSSHelper()
        {
            var webSocketClient = new WebSocketClient("ws://localhost/Validator");
            webSocketClient.OnErrorEvent += OnHandleError;
            webSocketClient.PingInterval = 4;

            var barcode = new Barcode();
            barcode.SendBarcodeEvent += async (sender, text) => await webSocketClient.SendMessageAsync(text);

            webSocketClient.StartConnection();
            barcode.SendBarcode();
        }

        private async static void MultipleTasks()
        {
            var webSocketClient = new WebSocketClient("ws://localhost/Validator");
            webSocketClient.OnErrorEvent += OnHandleError;
            webSocketClient.StartConnection();

            var barcode = new Barcode();

            var result = await Task.WhenAny(webSocketClient.SendMessageAsync("Message sent"), barcode.SendBarcodeAsync());

            Console.ReadKey();
        }

        private static void OnHandleError(object sender, ErrorEventArgs args)
        {
            Console.WriteLine($"Exception: {args.Message}");
        }
    }
}
