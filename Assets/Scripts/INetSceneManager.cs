using SocketWGcs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetSceneManager
{
    public void ServerCommand(Packet packet);
}
