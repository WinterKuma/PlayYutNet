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

    private Material material;
    public Material normalMaterial;
    public bool isMovePossible;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PointManager GetNextPoint(PointManager point)
    {
        if (nextPoint == null) return this;
        if (nextPoint2 == null) return nextPoint;
        else
        {
            if (point.transform.position.x > 0)
            {
                return nextPoint;
            }
            else
            {
                return nextPoint2;
            }
        }
    }

    public PointManager GetPrevPoint(PointManager ternimalPoint)
    {
        if (prevPoint2 == null) return prevPoint2;
        else
        {
            if (ternimalPoint.transform.position.x < 0)
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

    public void SetMovePossiblePoint(bool possible)
    {
        isMovePossible = possible;
        if(possible)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = normalMaterial.color;
        }
    }
}
