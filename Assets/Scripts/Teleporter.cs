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



    public bool teleportable { get; set; } = true;


    public static float START_DELAY { get; set; } = 0.15f;
    public static float FADE_DELAY { get; set; } = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
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

        if (collision.gameObject.tag == "Player" && teleportable)
        {
            if (oppositeTeleporter != null)
            {

                oppositeTeleporter.GetComponent<Teleporter>().teleportable = false;


                player.playerInputable = false;

                StartCoroutine(Utility.DelayCoroutine(START_DELAY, () =>
                {
                    player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                    StartCoroutine(Utility.FadeOut(collision.gameObject, FADE_DELAY));
                    StartCoroutine(Utility.DelayCoroutine(FADE_DELAY, () =>
                    {
                        collision.gameObject.transform.position = oppositeTeleporter.transform.position;

                        StartCoroutine(Utility.FadeIn(collision.gameObject, FADE_DELAY));
                        StartCoroutine(Utility.DelayCoroutine(FADE_DELAY, () =>
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
                    

                    StartCoroutine(Utility.DelayCoroutine(START_DELAY, () =>
                    {
                        player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                        StartCoroutine(Utility.FadeOut(collision.gameObject, FADE_DELAY));
                        StartCoroutine(Utility.DelayCoroutine(FADE_DELAY, () =>
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
        if (collision.gameObject.tag == "Player" && !teleportable)
        {
            teleportable = true;
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
