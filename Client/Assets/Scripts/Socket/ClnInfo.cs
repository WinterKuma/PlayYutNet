using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace SocketWGcs
{
    public class ClnInfo
    {
        private const int BUFSIZE = 256;
        public string userName;
        public string roomName;
        public int userID;
        public Socket socket;
        public byte[] rcvBuffer;
        public byte[] sndBuffer;
        public ServerRoom room;

        public ClnInfo(Socket socket)
        {
            userName = "NULL";
            userID = 0;
            this.socket = socket;
            rcvBuffer = new byte[BUFSIZE];
            sndBuffer = new byte[BUFSIZE];
        }
    }

    public enum PacketCode
    {
        None = 0x00
    }

    public class Packet
    {
        public int UserID;
        public string head;
        public string sendTarget;
        public string data;

        public Packet()
        {
            UserID = 0;
            sendTarget = "NULL";
        }

        public void AddData(string data)
        {
            this.data = data;
        }

        public byte[] PacketToBytes()
        {
            return Encoding.Unicode.GetBytes(head + '/' + sendTarget + '/' + data);
        }

        public void SetBytesToPacket(byte[] bytes, int bytesRead)
        {
            string[] datas = Encoding.Unicode.GetString(bytes, 0, bytesRead).Split('/');
            head = datas[0];
            sendTarget = datas[1];
            data = datas[2];
        }
    }
    public class ServerRoom
    {
        public List<ClnInfo> clients = new List<ClnInfo>();
        public ClnInfo masterClient;
        public int maxClient;
        public string roomName;
        public bool isLife;
        public bool isOpenRoom;


        public ServerRoom(string roomName = "NULL")
        {
            this.roomName = roomName;
            isLife = true;
            isOpenRoom = true;
        }

        public void JoinClient(ClnInfo client)
        {
            client.roomName = roomName;
            clients.Add(client);
        }

        public void ExitClient(ClnInfo client)
        {
            client.roomName = "NULL";
            clients.Remove(client);
        }

        public void ConnectClient(ClnInfo client)
        {
            client.roomName = roomName;
            clients.Add(client);
            client.room = this;
            ConnectedClient(client);
        }

        public virtual void ConnectedClient(ClnInfo client)
        {
            Console.WriteLine("Join client to " + roomName + ". : Username {0}", client.userName);
        }

        public virtual void Broadcast(byte[] buffer)
        {
            foreach (ClnInfo client in clients)
            {
                Send(client, buffer);
            }
        }

        public void Send(ClnInfo client, byte[] buffer)
        {
            client.sndBuffer = buffer;
            client.socket.BeginSend(client.sndBuffer, 0, client.sndBuffer.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), client);
        }

        public void SendCallback(IAsyncResult ar)
        {
            ClnInfo client = (ClnInfo)ar.AsyncState;
            Socket handler = client.socket;
            int bytesSend = handler.EndSend(ar);
            Console.WriteLine("Sent " + roomName + " to client.");

            if (Encoding.ASCII.GetString(client.sndBuffer).Equals("Exit"))
            {
                try
                {
                    handler.Disconnect(false);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    handler.Close();
                    clients.Remove(client);
                }
                return;
            }
            Receive(client);
        }

        public void Receive(ClnInfo client)
        {
            if (client.room != this) return;
            client.socket.BeginReceive(client.rcvBuffer, 0, client.rcvBuffer.Length, SocketFlags.None,
                new AsyncCallback(ReceiveCallback), client);
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            ClnInfo client = (ClnInfo)ar.AsyncState;
            Socket handler = client.socket;

            if (!handler.Connected) return;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                //string data = Encoding.Unicode.GetString(client.rcvBuffer, 0, bytesRead);
                Packet packet = new Packet();
                packet.SetBytesToPacket(client.rcvBuffer, bytesRead);

                Console.WriteLine("Head : {0}, Data : {1}",
                    packet.head, packet.data);


                ServerCommand(client, packet);
            }
        }

        public virtual void ServerCommand(ClnInfo client, Packet packet) { }
    }
}
