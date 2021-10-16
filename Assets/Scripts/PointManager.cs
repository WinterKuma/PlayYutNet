using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public PointManager prevPoint;
    public PointManager prevPoint2 = null;
    public PointManager nextPoint;
    public PointManager nextPoint2 = null;
    public PointManager shortcutPoint = null;

    public PlayerManager onPawn = null;

    private Renderer renderer;
    public Material normalMaterial;
    public bool isMovePossible;
    public int moveVector;
    public int moveCount;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PointManager GetNextPoint(PointManager point)
    {
        if (nextPoint2)
        {
            if(point.transform.position.x > 0)
            {
                return nextPoint;
            }
            else
            {
                return nextPoint2;
            }
        }
        return nextPoint;
    }

    public PointManager GetPrevPoint(PointManager point)
    {
        if (prevPoint2 == null) return prevPoint;
        else
        {
            if (point.transform.position.y <= -10)
            {
                return prevPoint;
            }
            else
            {
                return prevPoint2;
            }
        }
    }

    public PointManager GetShortcutPoint()
    {
        return shortcutPoint;
    }

    public void SetMovePossiblePoint(bool possible, int moveCount, int moveVector)
    {
        isMovePossible = possible;
        if(possible)
        {
            this.moveCount = moveCount;
            this.moveVector = moveVector;
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = normalMaterial.color;
        }
    }
}
