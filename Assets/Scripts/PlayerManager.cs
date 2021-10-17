using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<PawnManager> pawns = new List<PawnManager>();
    public List<int> movePointList = new List<int>();
    public PawnManager selectPawn = null;

    public bool isMyTurn = false;
    public bool isMine = false;
    public int chanceCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var pawn in pawns)
        {
            pawn.owner = this;
        }

        enabled = (GameManager.instance.localPlayer == this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMyTurn) return;
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 10))
            {
                PawnManager pawn = hit.collider.gameObject.GetComponent<PawnManager>();
                if (pawns.Contains(pawn))
                {
                    if (movePointList.Count > 0)
                    {
                        if (selectPawn)
                        {
                            foreach (var movePoint in movePointList)
                            {
                                selectPawn.SetPossibleMovePoint(movePoint, false);
                            }
                        }

                        selectPawn = pawn;
                        foreach (var movePoint in movePointList)
                        {
                            selectPawn.SetPossibleMovePoint(movePoint, true);
                        }
                    }
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 11))
            {
                PointManager point = hit.collider.gameObject.GetComponent<PointManager>();
                if (point.isMovePossible)
                {
                    foreach (var movePoint in movePointList)
                    {
                        selectPawn.SetPossibleMovePoint(movePoint, false);
                    }
                    movePointList.Remove(selectPawn.SelectMovePoint(point));
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayYut();
        }
    }

    public void EndMovePawn()
    {
        if(chanceCount == 0 && movePointList.Count == 0)
        {
            GameManager.instance.EndTurn();
        }
    }

    public void PlayYut()
    {
        if (chanceCount == 0) return;
        int yut = Random.Range(1, 6);
        movePointList.Add(yut);

        if(yut > 3)
        {
            AddChanceCount();
        }

        if(--chanceCount == 0)
        {
            GameManager.instance.playYutButton.gameObject.SetActive(false);
        }
    }

    public void AddChanceCount()
    {
        chanceCount++;
        GameManager.instance.playYutButton.gameObject.SetActive(true);
    }

    public void GoalPawn()
    {
        foreach (var p in movePointList)
        {
            selectPawn.SetPossibleMovePoint(p, false);
        }

        selectPawn.isGoal = true;
        int movePoint = selectPawn.SelectMovePoint(GameManager.instance.goalPoint);
        int minPoint = 5;
        foreach(var point in movePointList)
        {
            if(point > movePoint)
            {
                if (minPoint < point) minPoint = point;
            }
        }
        movePointList.Remove(minPoint);
    }
}
