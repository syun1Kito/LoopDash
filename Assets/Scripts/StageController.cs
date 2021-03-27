using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    bool isTitle;
    [SerializeField]
    bool isStageSelect;


    [SerializeField]
    int stageNum;


    public TileMapController tileMapController { get; private set; }

    public Transform startPos;
    //public Vector3Int startGridPos { get; private set; }


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
        
    }
}
