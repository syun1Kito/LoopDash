using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class TileMapController : MonoBehaviour
{

    public enum TileType
    {
        putBlock,


    }

    public Tilemap stage;
    public Tilemap front;
    public Tilemap back;
    public Tilemap item;

    BoundsInt initialBound;

    [SerializeField]
    Tile[] putTiles;

    [SerializeField]
    Tile[] deletableTiles;

    [SerializeField]
    AnimatedTile[] animatedDamagedTiles;

    [SerializeField]
    Tile[] goalTiles;

    [SerializeField]
    AnimatedTile[] animatedItemTiles;

    [SerializeField]
    AnimatedTile[] animatedTeleporterTiles;

    public Vector3 leftBoundPos { get; private set; }
    public Vector3 rightBoundPos { get; private set; }
    public float loopedAreaWidth { get; private set; }

    //TeleporterController teleporterController;


    // Start is called before the first frame update
    void Awake()
    {
        //teleporterController = GetComponent<TeleporterController>();

        stage.CompressBounds();
        front.CompressBounds();
        back.CompressBounds();

        initialBound = stage.cellBounds;


        leftBoundPos = stage.GetCellCenterWorld(new Vector3Int(initialBound.min.x, 0, 0)) + new Vector3(-0.5f, 0, 0);
        rightBoundPos = stage.GetCellCenterWorld(new Vector3Int(initialBound.max.x - 1, 0, 0)) + new Vector3(0.5f, 0, 0);
        loopedAreaWidth = rightBoundPos.x - leftBoundPos.x;

        SetBoundaryOppositeTile();

    }



    // Update is called once per frame
    void Update()
    {
    }



    public Tile GetTileByType(TileType type)
    {
        return putTiles[(int)type];
    }

    public bool SetTile(Tilemap tilemap, Vector3Int pos, TileType type)
    {

        var dummy = pos;
        pos = new Vector3Int((pos.x + (initialBound.max.x - initialBound.min.x)) % (initialBound.max.x - initialBound.min.x), pos.y, 0);


        var realGridPos = ConvertRealGridPos(pos);
        //Debug.Log(realGridPos);

        if (tilemap.GetTile(realGridPos) == null)
        {
            Tile tile = GetTileByType(type);
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

        if (animatedItemTiles.Contains(tilemap.GetTile(realGridPos)))
        {
            tilemap.SetTile(realGridPos, null);
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

        if (animatedTeleporterTiles.Contains(tilemap.GetTile(realGridPos)))
        {
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere),realGridPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            //Collider[] hitColliders = Physics.OverlapSphere(realGridPos + new Vector3(0.5f,0.5f,0), 0.5f);

            //foreach (var item in hitColliders)
            //{
            //    Debug.Log(item.gameObject);
            //}

            //teleporterController.Teleport(realGridPos + new Vector3(0.5f, 0.5f, 0));
        }

        if (goalTiles.Contains(tilemap.GetTile(realGridPos)))
        {
            //if (realGridPos.x == initialBound.min.x)
            //{               
            //}
            //else if (realGridPos.x == initialBound.max.x - 1)
            //{               
            //}

            Debug.Log("goal");

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
}
