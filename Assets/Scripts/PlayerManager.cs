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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 10))
            {
                if(movePointList.Count > 0)
                {
                    if (selectPawn)
                    {
                        foreach (var movePoint in movePointList)
                        {
                            selectPawn.SetPossibleMovePoint(movePoint, false);
                        }
                    }

                    selectPawn = hit.collider.gameObject.GetComponent<PawnManager>();
                    foreach (var movePoint in movePointList)
                    {
                        selectPawn.SetPossibleMovePoint(movePoint, true);
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

    public void PlayYut()
    {
        int yut = Random.Range(1, 6);
        movePointList.Add(yut);
    }

    public void GoalPawn()
    {
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
