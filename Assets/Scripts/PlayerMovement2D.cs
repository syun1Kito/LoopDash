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

    [SerializeField]
    Tilemap tilemap;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;


    void Awake()
    {
        tilemap.CompressBounds();

    }

    // Update is called once per frame
    void Update()
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

        BoundaryCheck(tilemap);

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void BoundaryCheck(Tilemap tilemap)
    {
        var bound = tilemap.cellBounds;

        var leftBoundPos = tilemap.GetCellCenterWorld(new Vector3Int(bound.min.x, 0, 0));
        var rightBoundPos = tilemap.GetCellCenterWorld(new Vector3Int(bound.max.x - 1, 0, 0));
        var loopedAreaWidth = rightBoundPos.x - leftBoundPos.x;

        var positionInLoopedArea = transform.position.x - leftBoundPos.x;
        positionInLoopedArea = (positionInLoopedArea % loopedAreaWidth + loopedAreaWidth) % loopedAreaWidth;

        transform.position = new Vector2(leftBoundPos.x + positionInLoopedArea, transform.position.y);

    }

}