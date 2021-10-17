using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public PlayerManager[] players = new PlayerManager[4];
    public PlayerManager localPlayer;
    public PointManager startPoint;
    public PointManager goalPoint;

    public Button playYutButton;
    public Button goalButton;
    public bool isMyTurn;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
            Destroy(instance);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalPawn()
    {
        localPlayer.GoalPawn();
        SetVisibleGoalButton(false);
    }

    public void SetVisibleGoalButton(bool isVisible)
    {
        goalButton.gameObject.SetActive(isVisible);
    }

    public void SetMyTurn()
    {
        isMyTurn = true;
        localPlayer.isMyTurn = true;
    }

    public void EndTurn()
    {
        isMyTurn = false;
        localPlayer.isMyTurn = false;
    }

    public void PlayYut()
    {
        localPlayer.PlayYut();
    }
}
