using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{

    public enum PauseState
    {
        Idle,
        Resume,
        StageSelect,
        Title,
        PauseStateMax,
    }

    EventSystem eventSystem;
    //GameObject canvas;
    public Animator animator { get; private set; }

    //public static bool isPaused = false;
    bool isPaused = false;
    public bool pauseable { set; get; } = true;

    [SerializeField]
    GameObject pauseUI;
    //GameObject pauseUI;

    string previousState;


    [SerializeField]
    bool canUseResume = true;
    [SerializeField]
    bool canUseStageSelect = true;
    [SerializeField]
    bool canUseTitle = true;

    //TimeController timeController;

    // Start is called before the first frame update
    void Start()
    {

        //canvas = GameObject.Find("Pause");

        //pauseUI = Instantiate(pauseUIBase, canvas.transform);
        //pauseUI.transform.SetAsLastSibling();

        pauseUI.SetActive(true);


        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.firstSelectedGameObject = pauseUI.transform.Find("PauseSet/Resume").gameObject;

        animator = pauseUI.GetComponent<Animator>();

        //previousState = eventSystem.currentSelectedGameObject.name;
        //timeController = GetComponent<TimeController>();
        animator.SetInteger("PauseState", (int)PauseState.Idle);


        Color disable = new Color(1.0f,1.0f,1.0f,0.5f);
        if (!canUseResume) { pauseUI.transform.Find("PauseSet/Resume").gameObject.GetComponent<Image>().color = disable; }
        if (!canUseStageSelect) { pauseUI.transform.Find("PauseSet/StageSelect").gameObject.GetComponent<Image>().color = disable; }
        if (!canUseTitle) { pauseUI.transform.Find("PauseSet/Title").gameObject.GetComponent<Image>().color = disable; }


        Resume();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Pause") && pauseable)
        {
            PauseInput();
        }

        if (isPaused)
        {

            if (eventSystem != null)
            {
                var state = eventSystem.currentSelectedGameObject.name;
                if (state != previousState)
                {
                    //AudioController.Instance.PlaySE(AudioController.SE.moveButton);

                    switch (state)
                    {
                        case "Resume":
                            animator.SetInteger("PauseState", (int)PauseState.Resume);
                            break;
                        case "StageSelect":
                            animator.SetInteger("PauseState", (int)PauseState.StageSelect);
                            break;
                        case "Title":
                            animator.SetInteger("PauseState", (int)PauseState.Title);
                            break;
                        default:
                            break;
                    }
                }



                if (Input.GetButtonDown("Submit"))
                {
                    switch (state)
                    {
                        case "Resume":
                            Resume();
                            break;
                        case "StageSelect":
                            LoadStageSelect();
                            break;
                        case "Title":
                            LoadTitle();
                            break;
                        default:
                            break;
                    }
                }

                previousState = state;
            }
        }
    }

    public void PauseInput()
    {

        if (isPaused)
        {
            //AudioController.Instance.PlaySE(AudioController.SE.exit);
            Resume();
        }
        else
        {
            //AudioController.Instance.PlaySE(AudioController.SE.enter);
            Pause();
        }
        //Debug.Log(isPaused);

    }
    public void Resume()
    {
        if (canUseResume)
        {
            animator.SetInteger("PauseState", (int)PauseState.Idle);

            pauseable = false;
            StartCoroutine(Utility.DelayCoroutine(1.0f, () =>
            {
                Time.timeScale = 1f;
                pauseable = true;
            }));

            isPaused = false;
            //AudioController.Instance.PlaySE(AudioController.SE.exit);
        }
    }

    public void Pause()
    {


        //ColorBlock colorBlock = select.GetComponent<Button>().colors;
        //colorBlock.selectedColor = Color.white;
        //select.GetComponent<Button>().colors = colorBlock;

        //pauseUI.SetActive(true);
        animator.SetInteger("PauseState", (int)PauseState.Resume);
        GameObject select = pauseUI.transform.Find("PauseSet/Resume").gameObject;
        eventSystem.SetSelectedGameObject(select);
        Time.timeScale = 0f;
        //timeController.ToggleIsRunning();
        isPaused = true;

    }

    //public void Restart()
    //{
    //    //AudioController.Instance.PlaySE(AudioController.SE.pushButton);
    //    Resume();
    //    SceneManager.LoadScene("Main");
    //}
    public void LoadStageSelect()
    {
        if (canUseStageSelect)
        {
            //AudioController.Instance.PlaySE(AudioController.SE.pushButton);
            //Resume();
            SceneManager.LoadScene(SceneNameEnum.StageSelect.ToString());
        }
    }

    public void LoadTitle()
    {
        if (canUseTitle)
        {
        //AudioController.Instance.PlaySE(AudioController.SE.pushButton);
        //Resume();
        //GameInstance.DestroyInstance();

        SceneManager.LoadScene(SceneNameEnum.Title.ToString());
        }

    }
}
