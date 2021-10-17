using SocketWGcs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour, INetSceneManager
{
    public Button readyButton;
    string userName;

    // Start is called before the first frame update
    void Start()
    {
        Client.Instance.netManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ServerCommand(ClnInfo client, Packet packet)
    {
        if (packet.head.Equals("NameChange"))
        {
            client.userName = packet.sendTarget;
        }
        else if (packet.head.Equals("JoinRoom"))
        {
            Client.Instance.Send(packet.PacketToBytes());
        }
        else if(packet.head.Equals("RoomIsMax"))
        {
            readyButton.gameObject.SetActive(true);
        }
        else if (packet.head.Equals("JoinInGameRoom"))
        {
            SceneManager.LoadScene("InGameScene");
        }
        else if (packet.head.Equals("Exit"))
        {
            Client.Instance.Send(packet.PacketToBytes());
        }
        else
        {
            Client.Instance.Send(packet.PacketToBytes());
        }
    }
}
