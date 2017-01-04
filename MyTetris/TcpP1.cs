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
    static class TcpP1
    {
        private static TcpListener tcpLister;

        public static TcpClient TcpClient { get; private set; }
        public static BinaryReader Reader { get; private set; }
        public static BinaryWriter Writer { get; private set; }

        public static Thread acceptThread;

        public static void Start()
        {
            tcpLister = new TcpListener(IPAddress.Parse("127.0.0.1"), 23333);
            tcpLister.Start();
            // 启动一个线程来接受请求 
            acceptThread = new Thread(AcceptClientConnect);
            acceptThread.Start();
        }

        private static void AcceptClientConnect()
        {
            Thread.Sleep(1000);
            try
            {
                TcpClient = tcpLister.AcceptTcpClient();
                if (tcpLister != null)
                {
                    NetworkStream networkStream = TcpClient.GetStream();
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
