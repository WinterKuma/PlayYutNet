using SocketWGcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerWGcs
{
    public class MainRoom : ServerRoom
    {
        public MainRoom()
        {
            roomName = "MainRoom";
        }

        public override void ConnectedClient(ClnInfo client)
        {
            Console.WriteLine("Join client to MainRoom. : Username {0}", client.userName);
            Packet packet = new Packet();
            packet.head = "JoinMainRoom";
            packet.data = "null";
            Send(client, packet.PacketToBytes());
            Receive(client);
        }

        public override void ServerCommand(ClnInfo client, Packet packet)
        {
            if (packet.head.Equals("NameChange"))
            {
                client.userName = packet.data;
                Send(client, packet.PacketToBytes());
            }
            else if (packet.head.Equals("CreateRoom"))
            {
                MatchingRoom room = new MatchingRoom(packet.data);
                room.masterClient = client;
                clients.Remove(client);
                room.ConnectClient(client);
                ServerSocket.Instance.matchingList.Add(room);
                return;
            }
            else if (packet.head.Equals("JoinRoom"))
            {
                client.userName = packet.sendTarget;
                packet.head = "NameChange";
                Send(client, packet.PacketToBytes());
                if (ServerSocket.Instance.matchingList.Count > 0)
                {
                    bool isConnect = false;
                    foreach (var room in ServerSocket.Instance.matchingList)
                    {
                        if (room.isOpenRoom)
                        {
                            isConnect = true;
                            clients.Remove(client);
                            room.ConnectClient(client);
                            break;
                        }
                    }
                    if (!isConnect)
                    {
                        MatchingRoom room = new MatchingRoom(packet.data);
                        room.masterClient = client;
                        clients.Remove(client);
                        room.ConnectClient(client);
                        ServerSocket.Instance.matchingList.Add(room);
                    }
                }
                else
                {
                    MatchingRoom room = new MatchingRoom(packet.data);
                    room.masterClient = client;
                    clients.Remove(client);
                    room.ConnectClient(client);
                    ServerSocket.Instance.matchingList.Add(room);
                }
            }
            else if (packet.head.Equals("Exit"))
            {
                Send(client, packet.PacketToBytes());
            }
            else
            {
                Broadcast(packet.PacketToBytes());
            }
            Receive(client);
        }
    }
}
