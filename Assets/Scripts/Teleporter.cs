using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    GameObject oppositeTeleporter = null;

    [SerializeField]
    BoxCollider2D collider;

    [SerializeField]
    string dst = "null";

    public bool teleportable { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.8f);
        Gizmos.DrawCube(transform.position, collider.size);

        if (oppositeTeleporter != null)
        {
            Gizmos.DrawLine(transform.position, oppositeTeleporter.transform.position);
        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && teleportable)
        {
            if (oppositeTeleporter != null)
            {

                oppositeTeleporter.GetComponent<Teleporter>().teleportable = false;

                collision.gameObject.transform.position = oppositeTeleporter.transform.position;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            }
            else
            {
                if (dst != "null")
                {
                    if (dst == "Exit") { Quit(); }
                    SceneManager.LoadScene(dst);
                }
            }

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


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !teleportable)
        {
            teleportable = true;
        }
    }

}
