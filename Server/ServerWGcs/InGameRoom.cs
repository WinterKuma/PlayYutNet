using SocketWGcs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerWGcs
{
    public class InGameRoom : ServerRoom
    {
        public string[] names =
        {
            "White", "Black", "Blue", "Green"
        };
        public int turnClientNum;
        public int jointClientNum;
        public InGameRoom()
        {
            roomName = "InGameRoom";
            maxClient = 4;
            jointClientNum = 0;
            turnClientNum = 0;
        }

        public void SetTurn(int num)
        {
            Packet packet = new Packet();
            packet.head = "MyTurn";
            packet.sendTarget = names[num];
            Broadcast(packet.PacketToBytes());
        }

        public void EndTurn()
        {
            turnClientNum = (turnClientNum + 1) % maxClient;
            SetTurn(turnClientNum);
        }

        public void PlayYut(string name)
        {
            Random rand = new Random();
            Packet packet = new Packet();
            packet.head = "PlayYut";
            packet.sendTarget = name;
            packet.data = rand.Next(1, 6).ToString();
            Broadcast(packet.PacketToBytes());
        }

        public override void ConnectedClient(ClnInfo client)
        {
            Console.WriteLine("Join client to InGameRoom. : Username {0}", client.userName);
            //client.userName = names[clients.Count - 1];
            //Packet packet = new Packet();
            //packet.head = "JoinInGameRoom";
            //packet.data = client.userName;
            //Send(client, packet.PacketToBytes());
            Packet packet = new Packet();
            packet.head = "JoinInGameRoom";
            Broadcast(packet.PacketToBytes());
            Receive(client);
            //if(maxClient == clients.Count)
            //{
            //    SetTurn(0);
            //}
        }

        public override void ServerCommand(ClnInfo client, Packet packet)
        {
            if (packet.head.Equals("EndTurn"))
            {
                EndTurn();
            }
            else if (packet.head.Equals("PlayYut"))
            {
                PlayYut(packet.sendTarget);
            }
            else if (packet.head.Equals("MovePawn"))
            {
                Broadcast(packet.PacketToBytes());
            }
            else if (packet.head.Equals("JoinInGame"))
            {
                if(++jointClientNum == maxClient)
                {
                    SetTurn(0);
                }
            }
            else
            {
                Broadcast(packet.PacketToBytes());
            }
            Receive(client);
        }
    }
}
