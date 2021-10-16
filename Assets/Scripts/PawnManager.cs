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

    public PointManager currentPoint = null;
    public PointManager movePoint = null;
    public PointManager terminalPoint = null;

    public bool isMove;
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

    public void SetPossibleMovePoint(int moveCount)
    {
        PointManager nextPoint = currentPoint;
        PointManager shortcutPoint = currentPoint.shortcutPoint;

        if (nextPoint)
        {
            for(int i = 0; i < moveCount; i++)
            {
                nextPoint = nextPoint.GetNextPoint(nextPoint);
            }
            nextPoint.SetMovePossiblePoint(true);
        }
        if(shortcutPoint)
        {
            for(int i = 0; i < moveCount; i++)
            {
                shortcutPoint = shortcutPoint.GetNextPoint(shortcutPoint);
            }
            shortcutPoint.SetMovePossiblePoint(true);
        }
    }

    public void SelectMovePoint(PointManager point)
    {
        terminalPoint = point;
        movePoint = terminalPoint;
        isMove = true;
        while (movePoint.GetPrevPoint(terminalPoint) != currentPoint) movePoint = movePoint.GetPrevPoint(terminalPoint);
    }

    public void MovePawn()
    {
        Vector3 moveVector = movePoint.transform.position - currentPoint.transform.position;
        Vector3 movedVector = transform.position + moveVector * moveSpeed * Time.deltaTime;
        Vector3 moveDist = movedVector - currentPoint.transform.position;
        if (Mathf.Abs(moveVector.x) > 0.1f)
        {
            movedVector.y = 0.5f * (moveDist.x - 2) * (moveDist.x - 2) + 2;
        }
        else 
        {
            movedVector.y = 0.5f * (moveDist.z - 2) * (moveDist.z - 2) + 2;
        }
        transform.position = movedVector;

        if(Mathf.Abs(moveVector.x) > 0.1f)
        {

        }
        else
        {

        }

        if (currentPoint == terminalPoint) isMove = false;
    }
}
