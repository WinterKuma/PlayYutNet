using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using SocketWGcs;

namespace ServerWGcs
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSocket.Instance.ServerOn();
            ServerSocket.Instance.ConnectClient();
            ServerSocket.Instance.ServerOff();

            //ServerSocket server = new ServerSocket();
            //server.ServerOn();
            //server.ConnectClient();
            //server.ServerOff();
        }
    }
}
