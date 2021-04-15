using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //public static AudioController Instance { get; private set; }

    [Serializable]
    public class AudioSet
    {
        public AudioClip audioClip;

        [Range(0, 1)]
        public float volume = 1;
    }
    public enum BGM
    {
        mainBGM,
        clearBGM,
        gameoverBGM,

    }

    public enum SE
    {
        jump,
        flip,
        //landing,
        box,
        bomb,
        gear,
        flag,
        damage,
        respawn,
        teleporterIn,
        teleporterOut,

        pauseIn,
        //pauseOut,
        select,
        submit,

    }


    //[SerializeField] AudioSource audioSource;

    AudioSource audioSource;


    //[SerializeField, NamedArray(typeof(BGM))] AudioClip[] BGMClips;
    //[SerializeField, NamedArray(typeof(BGM)), Range(0, 1)] float[] BGMvolume;

    //[SerializeField, NamedArray(typeof(SE))] AudioClip[] SEClips;
    //[SerializeField, NamedArray(typeof(SE)), Range(0f, 1f)] float[] SEvolume;

    [SerializeField, EnumIndex(typeof(BGM))] AudioSet[] BGMClips;
    [SerializeField, EnumIndex(typeof(SE))] AudioSet[] SEClips;

    void Awake()
    {
        

        //Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //audioSource.PlayOneShot(BGMClips[0]);
        //audioSource.loop = true;
        //audioSource.PlayScheduled(AudioSettings.dspTime + BGMClips[0].length);

        PlayBGM(BGM.mainBGM, true);
    }
    // Update is called once per frame
    void Update()
    {

    }


    public void PlaySE(SE num)
    {
        audioSource.PlayOneShot(SEClips[(int)num].audioClip, SEClips[(int)num].volume);
    }

    public void PlayBGM(BGM num, bool loop)
    {
        audioSource.clip = BGMClips[(int)num].audioClip;
        if (loop)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
        audioSource.volume = BGMClips[(int)num].volume;
        audioSource.Play();
    }
}
