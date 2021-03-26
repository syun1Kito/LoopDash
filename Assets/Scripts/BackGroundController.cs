using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    const int LOOPNUM = 3;

    
    GameObject mainCamera;
    Vector3 mainCameraLastPos;

    [SerializeField]
    GameObject backGroundFront;

    [SerializeField]
    GameObject backGroundBack;

    GameObject[] BGsFront = new GameObject[LOOPNUM];
    GameObject[] BGsBack = new GameObject[LOOPNUM];

    [SerializeField, Range(0.0f, 1.0f)]
    float BGFrontMove, BGBackMove;



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
        mainCamera = GameObject.Find("Main Camera");
        backGroundParent = GameObject.Find("BackGround").transform;

        BGFrontHeight = backGroundFront.GetComponent<SpriteRenderer>().bounds.size.y;
        BGBackHeight = backGroundBack.GetComponent<SpriteRenderer>().bounds.size.y;

        BGDisplayLength = (BGFrontHeight / 2) * LOOPNUM;


        for (int i = 0; i < LOOPNUM; i++)
        {

            BGsFront[i] = Instantiate(backGroundFront, new Vector3(0, (i - 1) * BGFrontHeight, 0), Quaternion.identity, backGroundParent);
            BGsFront[i].GetComponent<SpriteRenderer>().sortingOrder = -3;
            BGsBack[i] = Instantiate(backGroundBack, new Vector3(0, (i - 1) * BGBackHeight, 0), Quaternion.identity, backGroundParent);
            BGsBack[i].GetComponent<SpriteRenderer>().sortingOrder = -5;
        }




    }

    // Update is called once per frame
    void LateUpdate()
    {

        var cameraPosDeltaY = mainCamera.transform.position.y - mainCameraLastPos.y;

        foreach (var item in BGsFront)
        {

            item.transform.Translate(new Vector3(0, cameraPosDeltaY * BGFrontMove, 0));



            var diff = mainCamera.transform.position.y - item.transform.position.y;

            if (diff > BGDisplayLength)
            {
                item.transform.Translate(new Vector3(0, BGFrontHeight * LOOPNUM, 0));
                //item.transform.position = new Vector3(item.transform.position.x,item.transform.position.y + BGFrontHeight * LOOPNUM, item.transform.position.z);
            }
            else if (diff < -BGDisplayLength)
            {
                item.transform.Translate(new Vector3(0, -BGFrontHeight * LOOPNUM, 0));
                //item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - BGFrontHeight * LOOPNUM, item.transform.position.z);
            }
        }

        foreach (var item in BGsBack)
        {
            item.transform.Translate(new Vector3(0, cameraPosDeltaY * BGBackMove, 0));

            var diff = mainCamera.transform.position.y - item.transform.position.y;

            if (diff > BGDisplayLength)
            {
                item.transform.Translate(new Vector3(0, BGBackHeight * LOOPNUM, 0));
                //item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + BGBackHeight * LOOPNUM, item.transform.position.z);
            }
            else if (diff < -BGDisplayLength)
            {
                item.transform.Translate(new Vector3(0, -BGBackHeight * LOOPNUM, 0));
                //item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y - BGBackHeight * LOOPNUM, item.transform.position.z);
            }
        }


        mainCameraLastPos = mainCamera.transform.position;
    }
}
