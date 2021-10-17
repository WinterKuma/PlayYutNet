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
    public INetSceneManager netManager;
    Packet packet;

    public Client()
    {
        packet = new Packet();
        socket = new ClientSocket();

        socket.Receive();
    }

    public void SetNetManager(INetSceneManager sceneMananger)
    {
        socket.netManager = sceneMananger;
        netManager = sceneMananger;
    }

    public void SetPacket(string data)
    {
        string[] datas = data.Split('/');
        packet.head = datas[0];
        packet.sendTarget = datas[1];
        packet.data = datas[2];
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
