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
}
