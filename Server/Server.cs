using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Server
{
    class Server
    {
        int port;

        List<TcpClient> clients = new List<TcpClient>();

        public Server(int _port)
        {
            port = _port;
        }

        void Broadcast(string _message)
        {
            foreach (TcpClient client in clients)
            {
                int byteCount = Encoding.ASCII.GetByteCount(_message + 1);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(_message);

                client.GetStream().Write(sendData, 0, sendData.Length);
            }
        }

        void Handle(TcpClient _client)
        {
            while (true)
            {
                try
                {
                    NetworkStream stream = _client.GetStream();

                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;

                    foreach (byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }

                    string request = Encoding.UTF8.GetString(buffer, 0, recv);

                    File.AppendAllText("log.txt", "[" + DateTime.Now.ToString() + "] " + request + "\n");

                    Broadcast(request);
                }
                catch
                {
                    clients.Remove(_client);
                    return;
                }
            }
        }

        void Receive(TcpListener _listener)
        {
            while (true)
            {
                Console.WriteLine("Waiting for connection...");

                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Client accepted, " + client.Client.RemoteEndPoint.ToString());

                Thread handleThread = new Thread(() => Handle(client));
                handleThread.Start();

                clients.Add(client);
            }
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("Server has successfully started!");

            Receive(listener);
        }
    }
}
