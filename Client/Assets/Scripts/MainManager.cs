using SocketWGcs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour, INetSceneManager
{
    public Button readyButton;
    string userName = null;
    bool isJoinInGame = false;

    // Start is called before the first frame update
    void Start()
    {
        Client.Instance.SetNetManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isJoinInGame)
        {
            isJoinInGame = false;
            SceneManager.LoadScene("InGameScene");
        }
    }

    public void SetUserName(string name)
    {
        userName = name;
    }

    public void JoinGame()
    {
        Client.Instance.SetPacket("JoinRoom/"+ userName +"/");
        Client.Instance.Send();
    }

    public void MatchingReady()
    {
        Client.Instance.SetPacket("Ready//");
        Client.Instance.Send();
    }

    public void JoinInGame()
    {
        isJoinInGame = true;
    }

    public void ServerCommand(Packet packet)
    {
        if (packet.head.Equals("JoinMatchingRoom"))
        {
            Client.Instance.clientInfo.userName = packet.data;
        }
        else if(packet.head.Equals("RoomIsMax"))
        {
            readyButton.gameObject.SetActive(true);
        }
        else if (packet.head.Equals("JoinInGameRoom"))
        {
            //Client.Instance.clientInfo.userName = packet.sendTarget;
            JoinInGame();
        }
        else if (packet.head.Equals("Exit"))
        {
            Client.Instance.Send(packet.PacketToBytes());
        }
        else
        {
            //Client.Instance.Send(packet.PacketToBytes());
        }
    }
}
