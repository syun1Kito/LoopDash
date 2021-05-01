using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static IEnumerator DelayCoroutineBySecond(float seconds, Action action)
    {
        yield return new WaitForSecondsRealtime(seconds) ;
        action?.Invoke();
    }

    public static IEnumerator DelayCoroutineByFrame(int frame, Action action)
    {
        for (var i = 0; i < frame; i++) yield return null;
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

    public static T DeepClone<T>(T src)
    {
        using (var memoryStream = new System.IO.MemoryStream())
        {
            var binaryFormatter
              = new System.Runtime.Serialization
                    .Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, src); // シリアライズ
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            return (T)binaryFormatter.Deserialize(memoryStream); // デシリアライズ
        }
    }


    

    static public string ConvertToFullWidth(string halfWidthStr)
    {
        const int ConvertionConstant = 65248;

        string fullWidthStr = null;

        for (int i = 0; i < halfWidthStr.Length; i++)
        {
            fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
        }

        return fullWidthStr;
    }

    static public string ConvertToHalfWidth(string fullWidthStr)
    {
        const int ConvertionConstant = 65248;

        string halfWidthStr = null;

        for (int i = 0; i < fullWidthStr.Length; i++)
        {
            halfWidthStr += (char)(fullWidthStr[i] - ConvertionConstant);
        }

        return halfWidthStr;
    }
}
