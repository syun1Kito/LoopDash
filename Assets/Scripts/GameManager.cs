using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    
    public StageController stageController { get; private set; }
    public SaveDataController saveDataController { get; private set; }
    public StatusUIController statusUIController { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        
        stageController = GetComponent<StageController>();
        saveDataController = GetComponent<SaveDataController>();
        statusUIController = GetComponent<StatusUIController>();

    }

}
