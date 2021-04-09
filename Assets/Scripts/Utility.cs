using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static IEnumerator DelayCoroutine(float seconds, Action action)
    {
        //yield return new WaitForSeconds(seconds);
        yield return new WaitForSecondsRealtime(seconds) ;
        action?.Invoke();
    }


    public static IEnumerator FadeOut(GameObject obj, float time)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Color color = spriteRenderer.color;

        while (true)
        {
            yield return null;
            color.a -= Time.deltaTime / time;

            if (color.a <= 0f)
            {
                color.a = 0f;
                spriteRenderer.color = color;
                break;
            }
            spriteRenderer.color = color;
        }
    }

    public static IEnumerator FadeIn(GameObject obj, float time)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
        Color color = spriteRenderer.color;

        while (true)
        {
            yield return null;
            color.a += Time.deltaTime / time;

            if (color.a > 1f)
            {
                color.a = 1f;
                spriteRenderer.color = color;
                break;
            }
            spriteRenderer.color = color;

        }

    }

    public static GameObject Clone(GameObject obj)
    {
        var clone = GameObject.Instantiate(obj) as GameObject;
        clone.transform.parent = obj.transform.parent;
        clone.transform.localPosition = obj.transform.localPosition;
        clone.transform.localScale = obj.transform.localScale;
        return clone;
    }

}
