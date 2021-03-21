using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    public TileMapController tileMapController { get; private set; }



    protected override void Awake()
    {
        base.Awake();
        tileMapController = GetComponent<TileMapController>();

    }

}
