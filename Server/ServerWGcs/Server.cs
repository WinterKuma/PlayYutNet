using System;
using System.Collections.Generic;
using System.Text;

using SocketWGcs;
using System.Net;
using System.Net.Sockets;

namespace ServerWGcs
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance
        {
            get
            {
                return instance.Value;
            }
        }

        protected Singleton() { }
    }
    public class ServerSocket : Singleton<ServerSocket>
    {
        public Socket server = null;
        public IPEndPoint ipe = null;
        public MainRoom mainRoom;
        public List<ServerRoom> roomList = new List<ServerRoom>();
        public List<MatchingRoom> matchingList = new List<MatchingRoom>();
        public List<InGameRoom> inGameList = new List<InGameRoom>();
        public bool isLife = true;

        public List<ClnInfo> clients = new List<ClnInfo>();

        public ServerSocket()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainRoom = new MainRoom();
        }

        public void ServerOn()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Any, 5050);

            try
            {
                server.Bind(ipe);
                server.Listen(10);
                isLife = true;
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + " : " + se.Message);
                Environment.Exit(se.ErrorCode);
            }
        }

        public void ConnectClient()
        {
            while (isLife)
            {
                try
                {
                    Socket socket = server.Accept();
                    ClnInfo client = new ClnInfo(socket);
                    //clients.Add(client);
                    //mainRoom.Receive(client);
                    mainRoom.ConnectClient(client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void ServerOff()
        {
            foreach (ClnInfo client in clients)
            {
                client.socket.Disconnect(false);
                client.socket.Close();
            }
            server.Disconnect(false);
            server.Close();
        }
    }

    class Server
    {

    }
}
