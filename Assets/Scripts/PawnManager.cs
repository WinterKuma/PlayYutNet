using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamCode
{
    White,
    Black,
    Blue,
    Green
}

public class PawnManager : MonoBehaviour
{
    public TeamCode teamCode;

    public PlayerManager owner = null;
    public PointManager prevPoint = null;
    public PointManager currentPoint = null;
    public PointManager movePoint = null;
    public PointManager terminalPoint = null;

    public bool isMove;
    public bool isGoal = false;
    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove) MovePawn();
    }

    public void SetPossibleMovePoint(int moveCount, bool isPossible)
    {
        if (currentPoint == null)
        {
            currentPoint = GameManager.instance.startPoint;
        }
        if (currentPoint.nextPoint)
        {
            PointManager nextPoint = currentPoint.nextPoint;
            for(int i = 1; i < moveCount && nextPoint; i++)
            {
                nextPoint = nextPoint.GetNextPoint(currentPoint);
            }
            if (nextPoint == null)
            {
                GameManager.instance.SetVisibleGoalButton(isPossible);
            }
            else
            {
                nextPoint.SetMovePossiblePoint(isPossible, moveCount, 1);
            }
        }
        if (currentPoint.nextPoint2)
        {
            PointManager nextPoint = currentPoint.nextPoint2;
            for (int i = 1; i < moveCount && nextPoint; i++)
            {
                nextPoint = nextPoint.GetNextPoint(currentPoint);
            }
            if (nextPoint == null)
            {
                GameManager.instance.SetVisibleGoalButton(isPossible);
            }
            else
            {
                nextPoint.SetMovePossiblePoint(isPossible, moveCount, 2);
            }
        }
        if(currentPoint.shortcutPoint)
        {
            PointManager shortcutPoint = currentPoint.shortcutPoint;
            for(int i = 1; i < moveCount; i++)
            {
                shortcutPoint = shortcutPoint.GetNextPoint(currentPoint);
            }
            if (shortcutPoint == null)
            {
                GameManager.instance.SetVisibleGoalButton(isPossible);
            }
            else
            {
                shortcutPoint.SetMovePossiblePoint(isPossible, moveCount, 3);
            }
        }
    }

    public int SelectMovePoint(PointManager point)
    {
        if (currentPoint == point) return 0;
        terminalPoint = point;
        prevPoint = currentPoint;
        isMove = true;
        if(point.moveVector == 1) movePoint = currentPoint.nextPoint;
        if(point.moveVector == 2) movePoint = currentPoint.nextPoint2;
        if(point.moveVector == 3) movePoint = currentPoint.shortcutPoint;
        return point.moveCount;
    }

    private void SetMovePoint()
    {
        movePoint = currentPoint.GetNextPoint(prevPoint);
    }

    public void MovePawn()
    {
        Vector3 moveVector = movePoint.transform.position - currentPoint.transform.position;
        Vector3 movedVector = transform.position + moveVector * moveSpeed * Time.deltaTime;
        Vector3 moveDist = movedVector - currentPoint.transform.position;
        moveDist.y = 0;

        if(movePoint != currentPoint)
            movedVector.y = Mathf.Sin((moveDist.magnitude/(movePoint.transform.position - currentPoint.transform.position).magnitude)*180*Mathf.Deg2Rad) * 2 + 1;

        transform.position = movedVector;

        if (moveDist.magnitude >= (movePoint.transform.position - currentPoint.transform.position).magnitude)
        {
            transform.position = movePoint.transform.position + new Vector3(0, 1, 0);
            currentPoint = movePoint;

            if (currentPoint == terminalPoint)
            {
                if (currentPoint.onPawn)
                {
                    PawnManager onPawn = currentPoint.onPawn;
                    if(onPawn.teamCode != teamCode)
                    {
                        onPawn.currentPoint = GameManager.instance.startPoint;
                        onPawn.transform.position = GameManager.instance.startPoint.transform.position + new Vector3(0, 1, 0);
                        owner.AddChanceCount();
                    }
                }
                currentPoint.onPawn = this;
                isMove = false;
                owner.EndMovePawn();
            }
            else SetMovePoint();
        }
    }
}
