using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    GameObject oppositeTeleporter = null;

    [SerializeField]
    CircleCollider2D collider;

    //[SerializeField]
    //string dst = "null";
    [SerializeField]
    SceneNameEnum loadSceneName;

    Animator animator;


    public bool isTeleportable { get; set; } = true;
    bool isOpened = true;
    bool displayGear = false;

    public static float START_DELAY { get; set; } = 0.15f;
    public static float FADE_DELAY { get; set; } = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();

        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", true);

        if (SaveDataController.EditableSaveData.stageDatas.ContainsKey(loadSceneName))
        {
            isOpened = SaveDataController.EditableSaveData.stageDatas[loadSceneName].opened;//開放済みか確認
            if (isOpened)//見た目変更
            {
                animator.SetBool("isOpen", true);
            }
            else
            {
                animator.SetBool("isOpen", false);
            }

            //ギア取得表示
            displayGear = SaveDataController.EditableSaveData.stageDatas[loadSceneName].gearCollected;//ギア取得済みか確認
            TileMapController tileMapController = GameManager.Instance.stageController.tileMapController;
            if (displayGear)
            {
                tileMapController.SetTile(tileMapController.back, tileMapController.GetGridPos(transform.position) + new Vector3Int(-1, 0, 0), TileMapController.TileType.animatedGear);
            }
            else
            {
                tileMapController.SetTile(tileMapController.back, tileMapController.GetGridPos(transform.position) + new Vector3Int(-1, 0, 0), TileMapController.TileType.transparencyStopGear);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.8f);

        //Gizmos.DrawCube(transform.position, collider.size);
        Gizmos.DrawSphere(transform.position, collider.radius);

        if (oppositeTeleporter != null)
        {
            Gizmos.DrawLine(transform.position, oppositeTeleporter.transform.position);
        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        var player = collision.gameObject.GetComponent<PlayerMovement2D>();

        if (collision.gameObject.tag == "Player" && isTeleportable && isOpened)
        {
            if (oppositeTeleporter != null)
            {

                oppositeTeleporter.GetComponent<Teleporter>().isTeleportable = false;


                player.playerInputable = false;

                StartCoroutine(Utility.DelayCoroutineBySecond(START_DELAY, () =>
                {
                    player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                    StartCoroutine(Utility.FadeOut(collision.gameObject, FADE_DELAY));
                    StartCoroutine(Utility.DelayCoroutineBySecond(FADE_DELAY, () =>
                    {
                        collision.gameObject.transform.position = oppositeTeleporter.transform.position;

                        StartCoroutine(Utility.FadeIn(collision.gameObject, FADE_DELAY));
                        StartCoroutine(Utility.DelayCoroutineBySecond(FADE_DELAY, () =>
                        {
                            player.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                            player.playerInputable = true;
                        }));
                    }));
                }));

                

                

                

                




            }
            else
            {
                if (loadSceneName != SceneNameEnum.Null )
                {
                    

                    StartCoroutine(Utility.DelayCoroutineBySecond(START_DELAY, () =>
                    {
                        player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                        StartCoroutine(Utility.FadeOut(collision.gameObject, FADE_DELAY));
                        StartCoroutine(Utility.DelayCoroutineBySecond(FADE_DELAY, () =>
                        {
                            if (loadSceneName == SceneNameEnum.Exit) { Quit(); }
                            SceneManager.LoadScene(loadSceneName.ToString());
                        }));
                    }));


                    
                }
            }

        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isTeleportable)
        {
            isTeleportable = true;
        }
    }


    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }

}
