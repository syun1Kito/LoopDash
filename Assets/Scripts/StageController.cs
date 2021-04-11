using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    //[SerializeField]
    //bool isTitle;
    //[SerializeField]
    //bool isStageSelect;


    [SerializeField]
    SceneNameEnum stage;

    [SerializeField]
    SceneNameEnum nextUnlockStage;



    public TileMapController tileMapController { get; private set; }

    public Transform startPos;

    [SerializeField, Range(0, 10)]
    int life;


    // Start is called before the first frame update
    void Awake()
    {
        tileMapController = GetComponent<TileMapController>();
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

        stage = (SceneNameEnum)Enum.ToObject(typeof(SceneNameEnum), SceneManager.GetActiveScene().buildIndex);
    }
}
