using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using SocketWGcs;

namespace ClientWGcs
{
    public class ClientSocket
    {
        public static bool isLife = true;
        ClnInfo client;
        Socket socket;

        public ClientSocket(string address = null, int port = 5050)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connect(address, port);
        }

        public bool Connect(string address = null, int port = 5050)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, port);
            socket.Connect(ipe);
            client = new ClnInfo(socket);

            return socket.Connected;
        }

        public void InputBuffer()
        {
            string buffer;
            while (isLife)
            {
                buffer = Console.ReadLine();
                if (isLife)
                {
                    Send(Encoding.ASCII.GetBytes(buffer));
                }
            }
        }

        public void Send(byte[] buffer)
        {
            client.sndBuffer = buffer;
            client.socket.BeginSend(client.sndBuffer, 0, client.sndBuffer.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            ClnInfo client = (ClnInfo)ar.AsyncState;
            Socket handler = client.socket;
            int bytesSend = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSend);
        }

        public void Receive()
        {
            client.socket.BeginReceive(client.rcvBuffer, 0, client.rcvBuffer.Length, SocketFlags.None,
                new AsyncCallback(ReceiveCallback), client);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!isLife) return;
            ClnInfo client = (ClnInfo)ar.AsyncState;
            Socket handler = client.socket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                string data = Encoding.ASCII.GetString(client.rcvBuffer, 0, bytesRead);

                if (data.Equals("Exit"))
                {
                    Console.WriteLine("Disconnect Server");
                    try
                    {
                        handler.Disconnect(false);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e);
                    }
                    return;
                }

                Console.WriteLine("Receive Data : {0}", data);
                Receive();
            }
        }
    }

    class Client
    {
    }
}
