using SocketWGcs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;

public class GameManager : MonoBehaviour, INetSceneManager
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public PlayerManager localPlayer;
    public PointManager startPoint;
    public PointManager goalPoint;

    public List<PawnManager> pawns = new List<PawnManager>();

    public Text winText;
    public Button QuitButton;
    public Button playYutButton;
    public Button goalButton;
    public bool isMyTurn;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
            Destroy(instance);
        instance = this;

        Client.Instance.SetNetManager(this);
        localPlayer.teamCode = Client.Instance.clientInfo.userName;
        localPlayer.SetPawn(GameObject.FindGameObjectWithTag(localPlayer.teamCode).GetComponent<PawnManager>());
        JoinInGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalPawn()
    {
        int movePoint = localPlayer.GoalPawn();
        SetVisibleGoalButton(false);
        Client.Instance.SetHead("GoalPawn").SetSendTarget().SetData(localPlayer.selectPawn.pawnNum.ToString()+"!"+ movePoint).Send();
    }

    public void SetVisibleGoalButton(bool isVisible)
    {
        goalButton.gameObject.SetActive(isVisible);
    }

    public void JoinInGame()
    {
        Client.Instance.SetHead("JoinInGame").Send();
    }

    public void SetMyTurn()
    {
        isMyTurn = true;
        localPlayer.isMyTurn = true;
        localPlayer.AddChanceCount();
    }

    public void EndTurn()
    {
        isMyTurn = false;
        localPlayer.isMyTurn = false;

        Client.Instance.SetHead("EndTurn").SetSendTarget().Send();
    }

    public void PlayYut()
    {
        Client.Instance.SetHead("PlayYut").SetSendTarget().Send();
    }

    public void EndGame(Packet packet)
    {
        foreach (var pawn in pawns)
        {
            if (pawn.teamCode.ToString().Equals(packet.sendTarget))
            {
                string[] datas = packet.data.Split('!');
                int num = Int32.Parse(datas[0]);
                if (pawn.pawnNum == num)
                {
                    pawn.SelectMovePoint(1, Int32.Parse(datas[1]));
                }
            }
        }

        winText.text = packet.sendTarget + " Win!!!";
        winText.enabled = true;
        QuitButton.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ServerCommand(Packet packet)
    {
        if (packet.head.Equals("ChangeName"))
        {
            Client.Instance.clientInfo.userName = packet.sendTarget;
            localPlayer.teamCode = packet.sendTarget;
            localPlayer.SetPawn(GameObject.FindGameObjectWithTag(localPlayer.teamCode).GetComponent<PawnManager>());
        }
        else if (packet.head.Equals("MyTurn"))
        {
            if (Client.Instance.clientInfo.userName.Equals(packet.sendTarget))
            {
                SetMyTurn();
            }
        }
        else if (packet.head.Equals("PlayYut"))
        {
            if (Client.Instance.clientInfo.userName.Equals(packet.sendTarget))
            {
                localPlayer.PlayYut(Int32.Parse(packet.data));
            }
        }
        else if (packet.head.Equals("MovePawn"))
        {
            foreach(var pawn in pawns)
            {
                if (pawn.teamCode.ToString().Equals(packet.sendTarget))
                {
                    string[] datas = packet.data.Split('!');
                    int num = Int32.Parse(datas[0]);
                    if (pawn.pawnNum == num)
                    {
                        pawn.SelectMovePoint(Int32.Parse(datas[1]), Int32.Parse(datas[2]));
                    }
                }
            }
        }
        else if (packet.head.Equals("GoalPawn"))
        {
            EndGame(packet);
        }
        else
        {

        }
    }
}
