using System;
using System.Threading.Tasks;

namespace Playground
{
    public class Barcode
    {
        public string Text { get; set; }

        public EventHandler<string> SendBarcodeEvent { get; set; }

        public Barcode()
        {
            Text = Guid.NewGuid().ToString();
        }

        public void SendBarcode()
        {
            var rand = new Random();
            while (true)
            {
                var task = Task.Delay(1000 * rand.Next(5, 10));
                Task.WaitAny(task);
                SendBarcodeEvent.Invoke(this, Text);
            }
        }

        public Task SendBarcodeAsync()
        {
            var task = Task.Delay(5000);
            Task.WaitAny(task);
            Console.WriteLine("From barcode");

            return Task.FromResult(true);
        }
    }
}
