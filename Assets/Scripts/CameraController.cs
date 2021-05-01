using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    Vector3 defaultCameraPos;

    [SerializeField,Range(0.0f,60.0f)]
    float smoothness = 1.0f;

    static Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        defaultCameraPos = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = new Vector3(defaultCameraPos.x,Mathf.Clamp(player.transform.position.y, defaultCameraPos.y,Mathf.Infinity),defaultCameraPos.z);

        var trackingPos = new Vector3(defaultCameraPos.x, Mathf.Clamp(player.transform.position.y, defaultCameraPos.y, Mathf.Infinity), defaultCameraPos.z);
        transform.position = Vector3.Lerp(transform.position, trackingPos, smoothness * Time.deltaTime);

    }

    public static void NoiseFadeIn()
    {
        animator.SetTrigger("NoiseFadeIn");
    }
    public static void NoiseFadeOut()
    {
        animator.SetTrigger("NoiseFadeOut");
    }
}
