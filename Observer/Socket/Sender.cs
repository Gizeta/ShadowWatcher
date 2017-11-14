// Copyright 2017 Gizeta
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
