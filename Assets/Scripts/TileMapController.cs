using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileMapController : MonoBehaviour
{

    public enum TileType
    {
        ground,
        ladder,


    }

    public Tilemap stage;
    public Tilemap front;

    [SerializeField]
    Tile[] tiles;







    // Start is called before the first frame update
    void Awake()
    {
        stage.CompressBounds();
        front.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public Tile GetTile(TileType type)
    {
        return tiles[(int)type];
    }

}
