using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
