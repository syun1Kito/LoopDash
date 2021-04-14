﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class SaveData : ISerializationCallbackReceiver
{
    
    public Dictionary<SceneNameEnum, StageData> stageDatas;

    [SerializeField]
    private List<SceneNameEnum> _keyList;
    [SerializeField]
    private List<StageData> _valueList;

    public void OnBeforeSerialize()
    {
        //シリアライズする際にkeyとvalueをリストに展開
        _keyList = stageDatas.Keys.ToList();
        _valueList = stageDatas.Values.ToList();
    }

    /// <summary>
    /// Jsonからデシリアライズされたあとに実行される
    /// </summary>
    public void OnAfterDeserialize()
    {
        //デシリアライズ時に元のDictionaryに詰め直す
        stageDatas = _keyList.Select((id, index) =>
        {
            var value = _valueList[index];
            return new { id, value };
        }).ToDictionary(x => x.id, x => x.value);

        _keyList.Clear();
        _valueList.Clear();

    }
}



public class SaveDataController : MonoBehaviour
{

    public static SaveData editableSaveData;


    private void Start()
    {
        if (editableSaveData == null)
        {
            editableSaveData = Load();
            //Debug.Log(editableSaveData.stageDatas.Count);
        }
    }

    public void Save(SaveData saveData)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(saveData);

        writer = new StreamWriter(Application.dataPath + "/SaveData/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public SaveData Load()
    {
        if (File.Exists(Application.dataPath + "/SaveData/savedata.json"))
        {
            string datastr = "";
            StreamReader reader;
            reader = new StreamReader(Application.dataPath + "/SaveData/savedata.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<SaveData>(datastr);
        }
        
        return InitialData();
    }

    //public void PushSaveButton()
    //{
        

    //    Save(InitialData());
    //}

    //public void PushLoadButton()
    //{
    //    SaveData saveData = Load();
    //    //Debug.Log(saveData.stageDatas);
    //}

    public void ResetSaveData()
    {
        Save(InitialData());

    }


    public SaveData InitialData()
    {

        SaveData initialData = new SaveData
        {
            stageDatas = new Dictionary<SceneNameEnum, StageData>()
        };

        for (SceneNameEnum stage = SceneNameEnum.Stage1_1; stage < SceneNameEnum.Stage1_1 + StageController.maxStageNum; stage++)
        {
            
            StageData stageData = new StageData()
            {
             stage = stage,
             nextUnlockStage = stage+1, //要修正
             opened = false,
             cleared = false,
             gearCollected = false,
            };


            if (stage == SceneNameEnum.Stage1_1)
            {
                stageData.opened = true;
            }



            initialData.stageDatas.Add(stage,stageData);
        }


        //foreach (var key in initialData.stageDatas.Keys)
        //{
        //    Debug.Log(key.ToString() + " : " + initialData.stageDatas[key].stage.ToString());
        //}

        return initialData;
    }

    private void Update()
    {
        //Debug--------------------------
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (var key in editableSaveData.stageDatas.Keys)
            {
                Debug.Log(key.ToString() 
                    + " : " + editableSaveData.stageDatas[key].stage.ToString() 
                    + " : " + editableSaveData.stageDatas[key].nextUnlockStage.ToString() 
                    + " : " + editableSaveData.stageDatas[key].opened
                    + " : " + editableSaveData.stageDatas[key].cleared
                    + " : " + editableSaveData.stageDatas[key].gearCollected);
            }
        }
    }
}

