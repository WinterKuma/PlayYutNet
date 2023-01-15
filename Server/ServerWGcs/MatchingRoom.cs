using System;
using System.Collections.Generic;
using System.Text;

using SocketWGcs;
using System.Net.Sockets;

namespace ServerWGcs
{
    public class MatchingRoom : ServerRoom
    {
        public string[] names =
        {
            "White", "Black", "Blue", "Green"
        };
        public int readyPlayerCount;
        public MatchingRoom(string roomName)
        {
            this.roomName = "MatchingRoom:"+roomName;
            maxClient = 4;
            readyPlayerCount = 0;
        }

        public override void ConnectedClient(ClnInfo client)
        {
            Console.WriteLine("Join client to MatchingRoom. : Username {0}", client.userName);
            client.userName = names[clients.Count - 1];
            Packet packet = new Packet();
            packet.head = "JoinMatchingRoom";
            packet.data = client.userName;
            Send(client, packet.PacketToBytes());
            //Receive(client);
            if (maxClient == clients.Count)
            {
                isOpenRoom = false;
                //packet.head = "RoomIsMax";
                //packet.AddData("null");
                //Broadcast(packet.PacketToBytes());

                InGameRoom room = new InGameRoom();
                room.masterClient = masterClient;
                foreach (ClnInfo c in clients)
                {
                    room.ConnectClient(c);
                }
                ServerSocket.Instance.inGameList.Add(room);
                ServerSocket.Instance.matchingList.Remove(this);
            }
        }

        public override void ServerCommand(ClnInfo client, Packet packet)
        {
            if (packet.head.Equals("NameChange"))
            {
                client.userName = packet.data;
                Send(client, packet.PacketToBytes());
            }
            else if (packet.head.Equals("Ready"))
            {
                readyPlayerCount++;
                if(readyPlayerCount == maxClient)
                {
                    InGameRoom room = new InGameRoom();
                    room.masterClient = masterClient;
                    foreach(ClnInfo c in clients)
                    {
                        room.ConnectClient(c);
                    }
                    ServerSocket.Instance.inGameList.Add(room);
                    ServerSocket.Instance.matchingList.Remove(this);
                    
                }
                else
                {
                    Send(client, packet.PacketToBytes());
                }
                
            }
            else if (packet.head.Equals("JoinRoom"))
            {

            }
            else if (packet.head.Equals("Exit"))
            {
                Send(client, packet.PacketToBytes());
            }
            else
            {
                Broadcast(packet.PacketToBytes());
            }
        }
    }
}
