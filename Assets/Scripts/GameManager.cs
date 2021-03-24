using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    
    public StageController StageController { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        
        StageController = GetComponent<StageController>();

    }

}
