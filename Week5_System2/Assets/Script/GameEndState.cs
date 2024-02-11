using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : MonoBehaviour
{
    public static GameEndState instance;

    public bool GameEnd = false;
    public bool HasWin = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
