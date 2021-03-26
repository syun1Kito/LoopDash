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

    TileMapController tileMapController;
    StageController stageController;
    SpriteRenderer spriteRenderer;
    Animator animator;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public bool movable { get; set; } = true;

    public Rigidbody2D rigidbody2D { get; private set; }

    Vector3Int currentPos;
    //int currentScreenSpace = 0;


    void Start()
    {
        stageController = GameManager.Instance.StageController;
        tileMapController = stageController.tileMapController;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {

        Move();

        TileAction();

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
        ChangeForm(Form.box);
        if (!controller.IsFacingRight()) { controller.Flip(); }
        transform.position = stageController.startPos.position;
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


    bool SetTile(Tilemap tilemap, Vector3Int pos, TileMapController.TileType type)
    {
        return tileMapController.SetTile(tilemap, pos, type);

    }

    bool DeleteTile(Tilemap tilemap, Vector3Int pos)
    {
        return tileMapController.DeleteTile(tilemap, pos);
    }

    bool TouchTile(Tilemap tilemap, Vector3Int pos)
    {
        return tileMapController.TouchTile(tilemap, pos);
    }


    void TileAction()
    {
        if (Input.anyKeyDown)
        {
            Tilemap stage = tileMapController.stage;



            if (Input.GetButtonDown("PutTile"))
            {
                if (currentForm == Form.box)
                {
                    Vector3Int actionPos = new Vector3Int(currentPos.x + (controller.IsFacingRight() ? 1 : -1), currentPos.y, 0);
                    //Vector3Int realActionPos = tileMapController.ConvertRealGridPos(actionPos);

                    if (SetTile(stage, actionPos, TileMapController.TileType.putBlock))
                    {
                        ChangeForm(Form.bomb);
                        if (!controller.IsFacingRight()) { controller.Flip(); }
                        transform.position = stageController.startPos.position;
                    }


                }
                else if (currentForm == Form.bomb)
                {
                    List<Vector3Int> actionPos = new List<Vector3Int>();
                    actionPos.Add(new Vector3Int(currentPos.x + 1, currentPos.y, 0));
                    actionPos.Add(new Vector3Int(currentPos.x - 1, currentPos.y, 0));
                    actionPos.Add(new Vector3Int(currentPos.x, currentPos.y + 1, 0));
                    actionPos.Add(new Vector3Int(currentPos.x, currentPos.y - 1, 0));

                    foreach (var pos in actionPos)
                    {
                        //Vector3Int realActionPos = tileMapController.ConvertRealGridPos(pos);
                        DeleteTile(stage, pos);
                    }

                    ChangeForm(Form.box);
                    if (!controller.IsFacingRight()) { controller.Flip(); }
                    transform.position = stageController.startPos.position;
                }




            }
            //else if (Input.GetButtonDown("DeleteTile"))
            //{
            //    DeleteTile(stage, actionPos);
            //}
        }


    }

    public void ChangeForm(Form form)
    {
        spriteRenderer.sprite = sprites[(int)form];
        currentForm = form;
        animator.SetInteger("Form",(int)form);


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





}