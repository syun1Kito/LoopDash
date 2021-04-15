using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

//[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement2D : MonoBehaviour
{

    [SerializeField]
    CharacterController2D controller;

    [SerializeField]
    float runSpeed = 40f;

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    Collider2D itemGetCollider;

    public enum Form
    {
        box,
        bomb,
        none,

    }

    Form currentForm;

    [SerializeField]
    SceneNameEnum[] tileActionDisableScene;

    TileMapController tileMapController;
    StageController stageController;
    SpriteRenderer spriteRenderer;
    Animator animator;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public bool playerInputable { get; set; } = true;

    public Rigidbody2D rigidbody2D { get; private set; }

    Vector3Int currentPos;
    //int currentScreenSpace = 0;


    void Start()
    {
        stageController = GameManager.Instance.stageController;
        tileMapController = stageController.tileMapController;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();

        BoundaryCheck();

        GetGridPos(transform.position);
        //Debug.Log(currentPos);

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    StartCoroutine(FadeOut(gameObject,3));
        //}
    }

    void FixedUpdate()
    {
        // Move our character
        //if (movable)
        //{
        //    controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        //}
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void Init()
    {
        Respawn();



    }

    void InputHandle()
    {
        if (playerInputable)
        {

            Move();

            TileAction();

            if (Input.GetButtonDown("Respawn")) { Respawn(); }

        }
    }

    void Move()
    {

        //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        horizontalMove = (controller.IsFacingRight() ? 1 : -1) * runSpeed;



        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        //if (Input.GetButtonDown("Crouch"))
        //{
        //    crouch = true;
        //}
        //else if (Input.GetButtonUp("Crouch"))
        //{
        //    crouch = false;
        //}

    }

    void BoundaryCheck()
    {




        var leftBoundPos = tileMapController.leftBoundPos;
        //var rightBoundPos = tileMapController.rightBoundPos;
        var loopedAreaWidth = tileMapController.loopedAreaWidth;

        var positionInLoopedArea = transform.position.x - leftBoundPos.x;


        var tmpScreenSpace = Mathf.Floor(positionInLoopedArea / (int)loopedAreaWidth);

        if (tmpScreenSpace != 0)
        {
            positionInLoopedArea = (positionInLoopedArea % loopedAreaWidth + loopedAreaWidth) % loopedAreaWidth;

            transform.position = new Vector3(leftBoundPos.x + positionInLoopedArea, transform.position.y, 0);

            switch (currentForm)
            {
                case Form.box:
                    ChangeForm(Form.bomb);
                    break;
                case Form.bomb:
                    ChangeForm(Form.box);
                    break;
                case Form.none:
                    break;
                default:
                    break;
            }
        }




    }

    Vector3Int GetGridPos(Vector3 pos)
    {
        currentPos = tileMapController.GetGridPos(pos);
        return currentPos;
    }


    bool SetTile(Tilemap tilemap, Vector3Int pos, TileMapController.TileType type, bool set = true)
    {
        return tileMapController.SetTile(tilemap, pos, type, set);

    }

    bool DeleteTile(Tilemap tilemap, Vector3Int pos)
    {
        return tileMapController.DeleteTile(tilemap, pos);
    }

    bool TouchTile(Tilemap tilemap, Vector3Int pos)
    {
        return tileMapController.TouchTile(tilemap, pos);
    }


    bool TileAction()
    {
        if (Input.anyKeyDown && IsTileActionableScene()  /*playerInputable*/)
        {
            Tilemap stage = tileMapController.stage;
            Tilemap front = tileMapController.front;



            if (Input.GetButtonDown("PutTile"))
            {

                Vector3Int tmpCurrentPos = currentPos;

                if (currentForm == Form.box)
                {

                    Vector3Int actionPos = new Vector3Int(tmpCurrentPos.x + (controller.IsFacingRight() ? 1 : -1), tmpCurrentPos.y, 0);
                    if (!SetTile(stage, actionPos, TileMapController.TileType.putBlock, false))
                    {
                        return false;
                    }

                    if (controller.IsFacingRight())
                    {
                        SetTile(front, tmpCurrentPos, TileMapController.TileType.boxRightFrom);
                        SetTile(front, actionPos, TileMapController.TileType.boxRightTo);
                    }
                    else
                    {
                        SetTile(front, tmpCurrentPos, TileMapController.TileType.boxLeftFrom);
                        SetTile(front, actionPos, TileMapController.TileType.boxLeftTo);
                    }

                    StartCoroutine(Utility.DelayCoroutineBySecond(0.8f, () =>
                    {
                        SetTile(stage, actionPos, TileMapController.TileType.putBlock);
                        DeleteTile(front, tmpCurrentPos);
                        DeleteTile(front, actionPos);
                    }));

                }
                else if (currentForm == Form.bomb)
                {
                    List<Vector3Int> destroyPos = new List<Vector3Int>();
                    destroyPos.Add(tmpCurrentPos);
                    destroyPos.Add(new Vector3Int(tmpCurrentPos.x + 1, tmpCurrentPos.y, 0));
                    destroyPos.Add(new Vector3Int(tmpCurrentPos.x - 1, tmpCurrentPos.y, 0));
                    destroyPos.Add(new Vector3Int(tmpCurrentPos.x, tmpCurrentPos.y + 1, 0));
                    destroyPos.Add(new Vector3Int(tmpCurrentPos.x, tmpCurrentPos.y - 1, 0));

                    foreach (var pos in destroyPos)
                    {
                        DeleteTile(stage, pos);
                        SetTile(front, pos, TileMapController.TileType.explode);
                    }

                    StartCoroutine(Utility.DelayCoroutineBySecond(0.8f, () =>
                    {
                        foreach (var pos in destroyPos)
                        {
                            DeleteTile(front, pos);
                        }
                    }));
                }

                playerInputable = false;

                var fadeTime = 0.15f;
                rigidbody2D.bodyType = RigidbodyType2D.Static;
                StartCoroutine(Utility.FadeOut(gameObject, fadeTime));
                StartCoroutine(Utility.DelayCoroutineBySecond(.8f, () =>
                {
                    if (currentForm == Form.box)
                    {
                        ChangeForm(Form.bomb);
                    }
                    else if (currentForm == Form.bomb)
                    {
                        ChangeForm(Form.box);
                    }

                    if (!controller.IsFacingRight()) { controller.Flip(); }
                    transform.position = stageController.startPos.position;


                    StartCoroutine(Utility.FadeIn(gameObject, Teleporter.FADE_DELAY));
                    StartCoroutine(Utility.DelayCoroutineBySecond(Teleporter.FADE_DELAY, () =>
                    {
                        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        playerInputable = true;
                    }));
                }));

                return true;

            }
            //else if (Input.GetButtonDown("DeleteTile"))
            //{
            //    DeleteTile(stage, actionPos);
            //}
        }

        return false;
    }

    public void ChangeForm(Form form)
    {
        spriteRenderer.sprite = sprites[(int)form];
        currentForm = form;
        animator.SetInteger("Form", (int)form);


    }

    public void Respawn()
    {
        tileMapController.ReloadStage();
        stageController.ResetStageData();

        playerInputable = false;

        if (currentForm == Form.bomb) { ChangeForm(Form.box); }
        if (!controller.IsFacingRight()) { controller.Flip(); }
        transform.position = stageController.startPos.position;



        rigidbody2D.bodyType = RigidbodyType2D.Static;

        StartCoroutine(Utility.FadeIn(gameObject, Teleporter.FADE_DELAY));
        StartCoroutine(Utility.DelayCoroutineBySecond(Teleporter.FADE_DELAY, () =>
        {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            playerInputable = true;
        }));
    }

    public void Damaged()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;

        StartCoroutine(Utility.FadeOut(gameObject, Teleporter.FADE_DELAY));
        StartCoroutine(Utility.DelayCoroutineBySecond(Teleporter.FADE_DELAY, () =>
        {
            //rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            Respawn();
        }));


    }

    public bool IsTileActionableScene() {

        if (tileActionDisableScene.Contains((SceneNameEnum)Enum.ToObject(typeof(SceneNameEnum), SceneManager.GetActiveScene().buildIndex))) 
        {
            return false;
        }
            return true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //List<Collider2D> hitColliders = new List<Collider2D>();

        ColliderDistance2D colliderDistance = itemGetCollider.Distance(collision);

        if (!colliderDistance.isValid)
        {
            return;
        }

        Vector3 hitPos;

        if (colliderDistance.isOverlapped)
        {
            hitPos = colliderDistance.pointA; // point on the surface of this collider
        }
        else
        {
            hitPos = colliderDistance.pointB; // point on the surface of the other collider

            Vector3 hitPosition = Vector3.zero;
            Vector3 normal = colliderDistance.normal;

            // move the hit location inside the collider a bit
            // this assumes the colliders are basically touching
            hitPos -= 0.01f * normal;



        }


        var item = tileMapController.item;
        TouchTile(item, tileMapController.GetGridPos(hitPos));


        if (collision.tag == "Stage")
        {
            controller.Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {



    }

}