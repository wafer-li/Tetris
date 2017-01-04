using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTetris
{
    static class TcpP2
    {
        private static TcpListener tcpLister;

        public static BinaryReader Reader { get; private set; }
        public static BinaryWriter Writer { get; private set; }
        public static TcpClient TcpClient { get; private set; }
 
        public static Thread acceptThread;
        private static NetworkStream networkStream;

        public static void Start()
        {
            Thread connectThread = new Thread(ConnectToServer);
            connectThread.Start();
        }

        private static void ConnectToServer()
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
                TcpClient = new TcpClient();
                TcpClient.Connect(ipaddress, 23333);

                if (TcpClient != null)
                {
                    networkStream = TcpClient.GetStream();
                    Reader = new BinaryReader(networkStream);
                    Writer = new BinaryWriter(networkStream);
                }


            }
            catch
            {
            }
        }
        public static void Stop()
        {
            Reader.Close();
            Writer.Close();
            tcpLister.Stop();
            acceptThread.Abort();
        }

        public static void SendMessage(object state)
        {
            try
            {
                Writer.Write(state.ToString());
                Writer.Flush();
            }
            catch
            {
                if (Reader != null)
                {
                    Reader.Close();
                }
                if (Writer != null)
                {
                    Writer.Close();
                }
                if (TcpClient != null)
                {
                    TcpClient.Close();
                }

            }
        }

        public static string getMessage()
        {
            return Reader.ReadString();
        }

    }
}
