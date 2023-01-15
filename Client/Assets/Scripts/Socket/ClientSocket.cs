using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SocketWGcs;

namespace ClientWGcs
{
    public class ClientSocket
    {
        public bool isLife = true;
        public ClnInfo client;
        Socket socket;
        public INetSceneManager netManager;

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
            Debug.LogFormat("Sent {0} bytes to client.", bytesSend);
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
                Packet packet = new Packet();
                packet.SetBytesToPacket(client.rcvBuffer, bytesRead);

                Debug.LogFormat("Receive Data : {0} {1} {2}", packet.head, packet.sendTarget, packet.data);
                ServerCommand(packet);

                Receive();
            }
        }

        public void ServerCommand(Packet packet)
        {
            netManager.ServerCommand(packet);
        }
    }
}