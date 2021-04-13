using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Port: ");
            int port = Convert.ToInt32(Console.ReadLine());

            Server server = new Server(port);
            server.Start();
        }
    }
}
