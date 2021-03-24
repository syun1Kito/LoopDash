using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    const int LOOPNUM = 3;

    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    GameObject backGroundFront;

    [SerializeField]
    GameObject backGroundBack;

    GameObject[] BGsFront = new GameObject[LOOPNUM];
    GameObject[] BGsBack = new GameObject[LOOPNUM];


    float BGFrontHeight, BGBackHeight;
    float BGDisplayLength;

    Transform backGroundParent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        backGroundParent = GameObject.Find("BackGround").transform;

        BGFrontHeight = backGroundFront.GetComponent<SpriteRenderer>().bounds.size.y;
        BGBackHeight = backGroundBack.GetComponent<SpriteRenderer>().bounds.size.y;

        BGDisplayLength = (BGFrontHeight / 2) * LOOPNUM;


        for (int i = 0; i < LOOPNUM; i++)
        {   
            
            BGsFront[i] = Instantiate(backGroundFront, new Vector3(0, (i - 1) * BGFrontHeight, 0), Quaternion.identity,backGroundParent);
            BGsFront[i].GetComponent<SpriteRenderer>().sortingOrder = -3;
            BGsBack[i] = Instantiate(backGroundBack, new Vector3(0, (i - 1) * BGBackHeight, 0), Quaternion.identity,backGroundParent);
            BGsBack[i].GetComponent<SpriteRenderer>().sortingOrder = -5;
        }




    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var item in BGsFront)
        {
            var diff = mainCamera.transform.position.y - item.transform.position.y;

            if (diff > BGDisplayLength)
            {
                item.transform.position = new Vector3(item.transform.position.x,item.transform.position.y + BGFrontHeight * LOOPNUM, item.transform.position.z);
            }
            else if (diff < -BGDisplayLength)
            {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - BGFrontHeight * LOOPNUM, item.transform.position.z);
            }
        }

        foreach (var item in BGsBack)
        {
            var diff = mainCamera.transform.position.y - item.transform.position.y;

            if (diff > BGDisplayLength)
            {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + BGBackHeight * LOOPNUM, item.transform.position.z);
            }
            else if (diff < -BGDisplayLength)
            {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - BGBackHeight * LOOPNUM, item.transform.position.z);
            }
        }

    }
}
