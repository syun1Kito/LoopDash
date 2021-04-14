using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageData
{
    public SceneNameEnum stage;
    public SceneNameEnum nextUnlockStage;
    public bool opened;
    public bool cleared;
    public bool gearCollected;


}
public class StageController : MonoBehaviour
{
    //[SerializeField]
    //bool isTitle;
    //[SerializeField]
    //bool isStageSelect;

    
    public static int maxStageNum;

    [SerializeField]
    SceneNameEnum stage;

    [SerializeField]
    SceneNameEnum nextUnlockStage;

    StageData stageData;

    public TileMapController tileMapController { get; private set; }
    public SaveDataController saveDataController;

    public Transform startPos;

    [SerializeField, Range(0, 10)]
    int life;


    // Start is called before the first frame update
    void Awake()
    {
        tileMapController = GetComponent<TileMapController>();
        saveDataController = GetComponent<SaveDataController>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {





    }

    void Init()
    {
        //var tmp = tileMapController.stage.WorldToCell(startPos.position);
        //startPos.position = new Vector3(tmp.x + 0.5f, tmp.y + 0.5f, 0);

        maxStageNum = SceneNameEnum.StageMaxNum - SceneNameEnum.Stage1_1;

        stage = (SceneNameEnum)Enum.ToObject(typeof(SceneNameEnum), SceneManager.GetActiveScene().buildIndex);

        if (SaveDataController.editableSaveData != null && SaveDataController.editableSaveData.stageDatas.ContainsKey(stage))
        {
            stageData = SaveDataController.editableSaveData.stageDatas[stage];
        }


    }


    public void StageClear()
    {
        stageData.cleared = true;
        //Gearの処理

        SaveData saveData = SaveDataController.editableSaveData;
        saveData.stageDatas[stage] = stageData;
        saveDataController.Save(saveData);
    }
}
