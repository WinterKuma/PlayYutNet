using ClientWGcs;
using SocketWGcs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Client : Singleton<Client>
{
    public ClientSocket socket;
    private INetSceneManager netManager;
    Packet packet;
    public ClnInfo clientInfo
    {
        get
        {
            return socket.client;
        }
        private set { clientInfo = value; }
    }

    public Client()
    {
        packet = new Packet();
        socket = new ClientSocket();
    }

    public void SetNetManager(INetSceneManager sceneMananger)
    {
        socket.netManager = sceneMananger;
        netManager = sceneMananger;

        socket.Receive();
    }

    public void SetPacket(string data)
    {
        string[] datas = data.Split('/');
        packet.head = datas[0];
        packet.sendTarget = datas[1];
        packet.data = datas[2];
    }

    public Client SetHead(string head)
    {
        packet.head = head;
        return this;
    }

    public Client SetSendTarget()
    {
        packet.sendTarget = socket.client.userName;
        return this;
    }

    public Client SetSendTarget(string sendTarget)
    {
        packet.sendTarget = sendTarget;
        return this;
    }

    public Client SetData(string data)
    {
        packet.data = data;
        return this;
    }

    public void Send()
    {
        socket.Send(packet.PacketToBytes());
    }

    public void Send(Packet packet)
    {
        socket.Send(packet.PacketToBytes());
    }

    public void Send(byte[] bytes)
    {
        socket.Send(bytes);
    }
}
