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
    StageData defaultStageData;

    public TileMapController tileMapController { get; private set; }
    //public SaveDataController saveDataController;

    public Transform startPos;

    [SerializeField, Range(0, 10)]
    int life;


    // Start is called before the first frame update
    void Awake()
    {
        tileMapController = GetComponent<TileMapController>();
        //saveDataController = GetComponent<SaveDataController>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug--------------------------
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("stageData");
        //    Debug.Log(stageData.stage.ToString() + ":" + stageData.nextUnlockStage.ToString() + ":" + stageData.opened + ":" + stageData.cleared + ":" + stageData.gearCollected);
        //    Debug.Log("defaultdefaultStageData");
        //    Debug.Log(defaultStageData.stage.ToString() + ":" + defaultStageData.nextUnlockStage.ToString() + ":" + defaultStageData.opened + ":" + defaultStageData.cleared + ":" + defaultStageData.gearCollected);

        //}

        //Debug.Log(defaultStageData.gearCollected);

    }

    void Init()
    {
        //var tmp = tileMapController.stage.WorldToCell(startPos.position);
        //startPos.position = new Vector3(tmp.x + 0.5f, tmp.y + 0.5f, 0);

        maxStageNum = SceneNameEnum.StageMaxNum - SceneNameEnum.Stage1_1;

        stage = (SceneNameEnum)Enum.ToObject(typeof(SceneNameEnum), SceneManager.GetActiveScene().buildIndex);

        


    }

    private void Start()
    {
       
        if (SaveDataController.EditableSaveData != null && SaveDataController.EditableSaveData.stageDatas.ContainsKey(stage))
        {

            stageData = Utility.DeepClone(SaveDataController.EditableSaveData.stageDatas[stage]);
            defaultStageData = Utility.DeepClone(SaveDataController.EditableSaveData.stageDatas[stage]);
        }
    }

    public void StageClear()
    {
        stageData.cleared = true;//クリア済み

        SaveData saveData = SaveDataController.EditableSaveData;

        saveData.stageDatas[stage] = stageData;//現在のステージの状態を更新

        if (stageData.nextUnlockStage != SceneNameEnum.Null)
        {
            saveData.stageDatas[stageData.nextUnlockStage].opened = true;//次ステージ開放

        }
        

        SaveDataController.Save(saveData);//セーブデータに反映
    }

    public void ResetStageData()
    {
        stageData = defaultStageData;
    }

    public void GetGear()
    {
        stageData.gearCollected = true;
    }
}
