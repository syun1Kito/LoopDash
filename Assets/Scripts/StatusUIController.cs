using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{

    [SerializeField]
    Text stageText;

    [SerializeField]
    Text statusText;

    //StageController stageController;
    public StageData stageData { private get; set; }
    public int life { private get; set; }
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {

        if (stageData != null)
        {
            stageText.text = ConvertStageText(stageData.stage);
            statusText.text = "× " + Utility.ConvertToFullWidth(life.ToString());
        }
    }

    string ConvertStageText(SceneNameEnum sceneNameEnum)
    {
        string stage = sceneNameEnum.ToString();



        string stageGroup = (stage.Substring(stage.Length - 3)).Substring(0, 1);
        string stageSub = stage.Substring(stage.Length - 1);

        return "ステージ " + Utility.ConvertToFullWidth(stageGroup) + " - " + Utility.ConvertToFullWidth(stageSub);
    }

    public void SetLife(int life)
    {
        statusText.text = "× " + Utility.ConvertToFullWidth(life.ToString());
    }
}
