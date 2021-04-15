using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.SceneManagement;


public class TileMapController : MonoBehaviour
{
    //SaveDataController saveDataController;
    StageController stageController;

    [SerializeField] PlayerMovement2D player;


    public enum TileType
    {
        putBlock,
        explode,
        boxRightFrom,
        boxRightTo,
        boxLeftFrom,
        boxLeftTo,
        animatedGear,
        transparencyStopGear,

    }


    public Tilemap stage;
    GameObject stageBackup;

    public Tilemap front;
    public Tilemap back;

    public Tilemap item;
    GameObject itemBackup;

    BoundsInt initialBound;

    [SerializeField]
    TileBase[] putTiles;

    [SerializeField]
    TileBase[] deletableTiles;

    [SerializeField]
    TileBase[] damagedTiles;

    [SerializeField]
    TileBase[] goalTiles;

    [SerializeField]
    TileBase[] ItemTiles;

    //[SerializeField]
    //TileBase[] TeleporterTiles;

    public Vector3 leftBoundPos { get; private set; }
    public Vector3 rightBoundPos { get; private set; }
    public float loopedAreaWidth { get; private set; }

    //TeleporterController teleporterController;



    // Start is called before the first frame update
    void Awake()
    {

        stageController = GetComponent<StageController>();
        Init();

    }

    private void Start()
    {
        //saveDataController = GameManager.Instance.saveDataController;
    }

    void Init()
    {
        stage.CompressBounds();
        front.CompressBounds();
        back.CompressBounds();

        initialBound = stage.cellBounds;


        leftBoundPos = stage.GetCellCenterWorld(new Vector3Int(initialBound.min.x, 0, 0)) + new Vector3(-0.5f, 0, 0);
        rightBoundPos = stage.GetCellCenterWorld(new Vector3Int(initialBound.max.x - 1, 0, 0)) + new Vector3(0.5f, 0, 0);
        loopedAreaWidth = rightBoundPos.x - leftBoundPos.x;

        SetBoundaryOppositeTile();

        stageBackup = Utility.Clone(stage.gameObject);
        stageBackup.name = "StageBackup";
        stageBackup.SetActive(false);

        itemBackup = Utility.Clone(item.gameObject);
        itemBackup.name = "ItemBackup";
        itemBackup.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
    }



    public TileBase GetTileByType(TileType type)
    {
        return putTiles[(int)type];
    }

    public bool SetTile(Tilemap tilemap, Vector3Int pos, TileType type, bool set = true)
    {

        //var dummy = pos;
        pos = new Vector3Int((pos.x + (initialBound.max.x - initialBound.min.x)) % (initialBound.max.x - initialBound.min.x), pos.y, 0);


        var realGridPos = ConvertRealGridPos(pos);
        //Debug.Log(realGridPos);

        if (tilemap.GetTile(realGridPos) == null)
        {
            if (set)
            {


                TileBase tile = GetTileByType(type);

                tilemap.SetTile(realGridPos, tile);
                //Debug.Log(realGridPos.x);
                //Debug.Log(initialBound.max.x);
                if (realGridPos.x == initialBound.min.x)
                {
                    tilemap.SetTile(realGridPos + new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), tile);
                }
                else if (realGridPos.x == initialBound.max.x - 1)
                {
                    tilemap.SetTile(realGridPos - new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), tile);
                }
            }

            return true;
        }

        return false;

    }

    public bool DeleteTile(Tilemap tilemap, Vector3Int pos)
    {

        pos = new Vector3Int((pos.x + (initialBound.max.x - initialBound.min.x)) % (initialBound.max.x - initialBound.min.x), pos.y, 0);

        var realGridPos = ConvertRealGridPos(pos);
        //Debug.Log(realGridPos);

        if (deletableTiles.Contains(tilemap.GetTile(realGridPos)))
        {
            //Debug.Log(realGridPos);
            tilemap.SetTile(realGridPos, null);
            if (realGridPos.x == initialBound.min.x)
            {
                tilemap.SetTile(realGridPos + new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), null);
            }
            else if (realGridPos.x == initialBound.max.x - 1)
            {
                tilemap.SetTile(realGridPos - new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), null);
            }


            return true;
        }

        return false;
    }


    public bool TouchTile(Tilemap tilemap, Vector3Int pos)
    {

        pos = new Vector3Int((pos.x + (initialBound.max.x - initialBound.min.x)) % (initialBound.max.x - initialBound.min.x), pos.y, 0);

        var realGridPos = ConvertRealGridPos(pos);
        //Debug.Log(tilemap.GetTile(realGridPos));

        if (ItemTiles.Contains(tilemap.GetTile(realGridPos)))//ギア取得

        {
            tilemap.SetTile(realGridPos, null);//ギア削除
            stageController.GetGear();

            //if (realGridPos.x == initialBound.min.x)
            //{
            //    tilemap.SetTile(realGridPos + new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), null);
            //}
            //else if (realGridPos.x == initialBound.max.x - 1)
            //{
            //    tilemap.SetTile(realGridPos - new Vector3Int(initialBound.max.x - initialBound.min.x, 0, 0), null);
            //}


            return true;
        }

        //if (animatedTeleporterTiles.Contains(tilemap.GetTile(realGridPos)))
        //{          
        //}

        if (damagedTiles.Contains(tilemap.GetTile(realGridPos)))
        {

            player.Damaged();

            return true;
        }


        if (goalTiles.Contains(tilemap.GetTile(realGridPos)))
        {
            //if (realGridPos.x == initialBound.min.x)
            //{               
            //}
            //else if (realGridPos.x == initialBound.max.x - 1)
            //{               
            //}

            //saveDataController.PushSaveButton();
            //saveDataController.PushLoadButton();


            //Debug.Log("goal");
            //player.Respawn();
            stageController.StageClear();

            SceneManager.LoadScene(SceneNameEnum.StageSelect.ToString());

            return true;
        }

        return false;
    }





    public Vector3Int GetGridPos(Vector3 pos)
    {

        Vector3Int realGridPos = stage.WorldToCell(pos);

        return ConvertStageGridPos(realGridPos);
    }

    public Vector3Int ConvertStageGridPos(Vector3Int realGridPos)
    {
        Vector3Int gridPos = new Vector3Int(realGridPos.x - initialBound.min.x, realGridPos.y - initialBound.min.y, 0);
        //Vector3Int gridPos = new Vector3Int(realGridPos.x - stage.cellBounds.min.x - 1, realGridPos.y - stage.cellBounds.min.y, 0);

        return gridPos;
    }

    public Vector3Int ConvertRealGridPos(Vector3Int stageGridPos)
    {
        Vector3Int realPos = new Vector3Int(stageGridPos.x + initialBound.min.x, stageGridPos.y + initialBound.min.y, 0);

        return realPos;
    }

    public void SetBoundaryOppositeTile()
    {
        for (int i = initialBound.min.y; i < initialBound.max.y; i++)
        {
            var tmpLeftTile = stage.GetTile(new Vector3Int(initialBound.min.x, i, 0));
            if (tmpLeftTile != null)
            {
                stage.SetTile(new Vector3Int(initialBound.max.x, i, 0), tmpLeftTile);
            }

            var tmpRightTile = stage.GetTile(new Vector3Int(initialBound.max.x - 1, i, 0));
            if (tmpRightTile != null)
            {
                stage.SetTile(new Vector3Int(initialBound.min.x - 1, i, 0), tmpRightTile);
            }
        }
    }

    public void ReloadStage()
    {
        //Debug.Log("reload");
        if (GameObject.Find("tmpStage") == null)
        {            
            GameObject newStage = Utility.Clone(stageBackup);
            newStage.name = "tmpStage";
            newStage.SetActive(true);
            stage = newStage.GetComponent<Tilemap>();

            Destroy(GameObject.Find("Stage"));

            StartCoroutine(Utility.DelayCoroutineByFrame(1, () =>
          {
              newStage.name = "Stage";
          }));
        }


        if (GameObject.Find("tmpItem") == null)
        {
            GameObject newItem = Utility.Clone(itemBackup);
            newItem.name = "tmpItem";
            newItem.SetActive(true);
            item = newItem.GetComponent<Tilemap>();

            Destroy(GameObject.Find("Item"));

            StartCoroutine(Utility.DelayCoroutineByFrame(1, () =>
            {
                newItem.name = "Item";
            }));
        }



        //Destroy(GameObject.Find("Item"));
        //StartCoroutine(Utility.DelayCoroutineByFrame(1, () =>
        //{
        //    if (GameObject.Find("Item") == null)
        //    {
        //        GameObject newItem = Utility.Clone(itemBackup);
        //        newItem.name = "Item";
        //        newItem.SetActive(true);
        //        item = newItem.GetComponent<Tilemap>();
        //    }
        //}));

    }
}
