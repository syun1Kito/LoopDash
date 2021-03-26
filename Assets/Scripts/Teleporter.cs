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

    [SerializeField]
    string dst = "null";

    public bool teleportable { get; set; } = true;

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
        var startDelay = 0.15f;
        var fadeDelay = 0.8f;
        var player = collision.gameObject.GetComponent<PlayerMovement2D>();

        if (collision.gameObject.tag == "Player" && teleportable)
        {
            if (oppositeTeleporter != null)
            {

                oppositeTeleporter.GetComponent<Teleporter>().teleportable = false;

               
                //player.movable = false;

                StartCoroutine(Utility.DelayCoroutine(startDelay, () =>
                {
                    player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                    StartCoroutine(Utility.FadeOut(collision.gameObject, fadeDelay));
                    StartCoroutine(Utility.DelayCoroutine(fadeDelay, () =>
                    {
                        collision.gameObject.transform.position = oppositeTeleporter.transform.position;

                        StartCoroutine(Utility.FadeIn(collision.gameObject, fadeDelay));
                        StartCoroutine(Utility.DelayCoroutine(fadeDelay, () =>
                        {
                            player.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        }));
                    }));
                }));

                

                

                

                




            }
            else
            {
                if (dst != "null")
                {
                    

                    StartCoroutine(Utility.DelayCoroutine(startDelay, () =>
                    {
                        player.rigidbody2D.bodyType = RigidbodyType2D.Static;
                        StartCoroutine(Utility.FadeOut(collision.gameObject, fadeDelay));
                        StartCoroutine(Utility.DelayCoroutine(fadeDelay, () =>
                        {
                            if (dst == "Exit") { Quit(); }
                            SceneManager.LoadScene(dst);
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
