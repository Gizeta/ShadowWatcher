using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ShadowWatcher.Socket
{
    public static class Receiver
    {
        public static Action<string, string> OnReceived;

        private static Thread receiveThread;
        private static UdpClient client;

        public static int ListenPort => ((IPEndPoint)client.Client.LocalEndPoint).Port;

        public static void Initialize(int port = 37954)
        {
            client = new UdpClient(port);

            receiveThread = new Thread(new ThreadStart(receiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private static void receiveData()
        {
            while (true)
            {
                var anyIP = new IPEndPoint(IPAddress.Loopback, 0);
                var data = client.Receive(ref anyIP);

                var text = Encoding.UTF8.GetString(data);
                if (text.EndsWith("."))
                {
                    var str = text.TrimEnd('.');
                    OnReceived?.Invoke(str, "");
                }
                else
                {
                    var str = text.Split(new char[] { ':' }, 2);
                    OnReceived?.Invoke(str[0], str[1]);
                }
            }
        }

        public static void Destroy()
        {
            receiveThread.Abort();
            client.Close();
        }
    }
}
