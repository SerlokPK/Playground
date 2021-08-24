namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            var socketWrapper = new WebSocketServerWrapper();
            socketWrapper.InitializeServer();
        }
    }
}
