using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement2D : MonoBehaviour
{

    [SerializeField]
    CharacterController2D controller;

    [SerializeField]
    float runSpeed = 40f;

    TileMapController tileMapController;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    Vector3Int currentPos;

    void Awake()
    {
        tileMapController = GameManager.Instance.tileMapController;

    }

    // Update is called once per frame
    void Update()
    {

        Move();

        TileAction();

        BoundaryCheck();

        GetGridPos();
        Debug.Log(currentPos);

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

    }

    void BoundaryCheck()
    {
        var front = tileMapController.front;
        var bound = front.cellBounds;

        var leftBoundPos = front.GetCellCenterWorld(new Vector3Int(bound.min.x, 0, 0));
        var rightBoundPos = front.GetCellCenterWorld(new Vector3Int(bound.max.x - 1, 0, 0));
        var loopedAreaWidth = rightBoundPos.x - leftBoundPos.x;

        var positionInLoopedArea = transform.position.x - leftBoundPos.x;
        positionInLoopedArea = (positionInLoopedArea % loopedAreaWidth + loopedAreaWidth) % loopedAreaWidth;

        transform.position = new Vector2(leftBoundPos.x + positionInLoopedArea, transform.position.y);

    }

    Vector3Int GetGridPos()
    {
        var stage = tileMapController.stage;

        Vector3Int realGridPos = stage.WorldToCell(transform.position);

        currentPos = ConvertStageGridPos(realGridPos);

        return currentPos;
    }

    Vector3Int ConvertStageGridPos(Vector3Int realGridPos)
    {
        var stage = tileMapController.stage;
        Vector3Int gridPos = new Vector3Int(realGridPos.x - stage.cellBounds.min.x - 1, realGridPos.y - stage.cellBounds.min.y, 0);

        return gridPos;
    }

    Vector3Int ConvertRealGridPos(Vector3Int stageGridPos)
    {
        var stage = tileMapController.stage;
        Vector3Int realPos = new Vector3Int(stageGridPos.x + stage.cellBounds.min.x + 1, stageGridPos.y + stage.cellBounds.min.y, 0);

        return realPos;
    }

    void SetTile(Tilemap tilemap, Vector3Int pos, TileMapController.TileType type)
    {
        Tile tile = tileMapController.GetTile(type);
        tilemap.SetTile(pos, tile);

    }

    void DeleteTile(Tilemap tilemap, Vector3Int pos)
    {
        tilemap.SetTile(pos, null);
    }


    void TileAction()
    {
        if (Input.anyKeyDown)
        {
            Tilemap stage = tileMapController.stage;
            Vector3Int realActionPos = new Vector3Int(currentPos.x + (controller.IsFacingRight() ? 1 : -1), currentPos.y, 0);
            Vector3Int actionPos = ConvertRealGridPos(realActionPos);


            if (Input.GetButtonDown("PutTile"))
            {                
                SetTile(stage, actionPos, TileMapController.TileType.ground);
            }
            else if (Input.GetButtonDown("DeleteTile"))
            {
                DeleteTile(stage, actionPos);
            }
        }


    }
}