using System;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using SocketWGcs;

namespace ClientWGcs
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientSocket client;
            try
            {
                client = new ClientSocket();
                client.Receive();
                client.InputBuffer();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        
    }
}
