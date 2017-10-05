using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShadowWatcher.Socket
{
    public static class Sender
    {
        private static IPEndPoint dstIP;
        private static UdpClient client;

        public static void Initialize(int port = 37954)
        {
            client = new UdpClient();
            dstIP = new IPEndPoint(IPAddress.Loopback, port);
        }

        public static void Send(string command, string data = "")
        {
            var buf = Encoding.UTF8.GetBytes($"{command}{(data == "" ? "." : $":{data}")}");
            if (client != null)
                client.Send(buf, buf.Length, dstIP);
        }

        public static void Destroy()
        {
            client.Close();
            client = null;
        }
    }
}
