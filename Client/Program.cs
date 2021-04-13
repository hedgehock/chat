using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Send(NetworkStream _stream)
        {
            while (true)
            {
                string messageToSend = Console.ReadLine();

                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(messageToSend);

                _stream.Write(sendData, 0, sendData.Length);
            }
        }

        static void Receive(NetworkStream _stream)
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    _stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;

                    foreach (byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }

                    string request = Encoding.UTF8.GetString(buffer, 0, recv);
                    Console.WriteLine("> " + request);
                }
                catch
                {
                    Environment.Exit(1);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Console.Write("Port: ");
            int port = Convert.ToInt32(Console.ReadLine());

            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Successfully connected to server!");

            Thread sendThread = new Thread(() => Send(stream));
            sendThread.Start();
            Thread receiveThread = new Thread(() => Receive(stream));
            receiveThread.Start();

            Console.ReadKey();
        }
    }
}
