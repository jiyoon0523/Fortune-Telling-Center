using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UpperCaption : MonoBehaviour
{
    public static UpperCaption instance;
    public Text upperText;
    IntroManager im;
    MainManager mm;
    OutroManager om;
    public float readTime = 3;
    public List<GameObject> viewedArt = new List<GameObject>();
    private Fade fade;
    Stage stage;
    AudioSource audioSource;
    public float duration;
    [SerializeField] float audioTime;
    public UnityEvent enterEvent;
    GameObject exitNarration;
    ExitNone exitNone;
    ExitSome exitSome;
    ExitAll exitAll;
    public delegate void ExitDelegate(int i);
    public ExitDelegate ShowExitNone;
    public ExitDelegate ShowExitSome;
    public ExitDelegate ShowExitAll;

    GameObject enterNarration;
    EnterNarration enter;
    public delegate void EnterDelegate(int i);
    public EnterDelegate ShowEnter;

    bool isNonePlayed;
    bool isSomePlayed;
    bool isAllPlayed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        upperText = GameObject.Find("Final Fortune Canvas").transform.GetChild(7).GetComponent<Text>();
        upperText.text = "";
        stage = GameManager.Instance.stage;
        switch (stage)
        {
            case Stage.Intro:
                im = GameObject.Find("IntroManager").GetComponent<IntroManager>();
                break;
            case Stage.Main:
                mm = GameObject.Find("MainManager").GetComponent<MainManager>();
                audioSource = mm.GetComponent<AudioSource>();
                exitNarration = GameObject.Find("ExitNarration");
                exitNone = exitNarration.GetComponent<ExitNone>();
                exitSome = exitNarration.GetComponent<ExitSome>();
                exitAll = exitNarration.GetComponent<ExitAll>();

                enterNarration = GameObject.Find("EnterNarration");
                enter = enterNarration.GetComponent<EnterNarration>();
                ShowEnter = new EnterDelegate(enter.Enter);

                fade = GameObject.FindWithTag("Fade").GetComponent<Fade>();
                if (fade)
                {
                    fade.fadeInCompleted += ShowEnterNarration;
                    fade.fadeInCompleted += InitExit;
                    fade.fadeOutCompleted += EmptyViewed;
                }
                break;
            case Stage.Outro:
                om = GameObject.Find("OutroManager").GetComponent<OutroManager>();
                break;
        }
        ShowExitNone = new ExitDelegate(exitNone.ShowExit);
        ShowExitSome = new ExitDelegate(exitSome.ShowExit);
        ShowExitAll = new ExitDelegate(exitAll.ShowExit);
    }
    private void Update()
    {
        audioTime = audioSource.time;
        if (mm.isEnterPlay == false)
        {
            mm.isEnterPlay = true;
            enterEvent.Invoke();
        }
        if (audioSource.isPlaying == false)
        {
            upperText.text = null;
        }
    }

    void SetClip(int i)
    {
        Debug.Log(mm);
        audioSource.clip = mm.currentMain.doorAudio[i];
        duration = audioSource.clip.length;
        exitNone.duration = duration;
        exitSome.duration = duration;
        exitAll.duration = duration;
        enter.duration = duration;
    }
   
    public void ShowEnterNarration()
    {
        SetClip(0);
        audioSource.Play();
        ShowEnter(PlayerManager.Instance.currentRoomNo);
    }

    public void ShowExitNarration()
    {
        int viewed = viewedArt.Count;
        Debug.Log("ShowExitNarration");
        if (viewed == 0)
        {
            if (isNonePlayed == false)
            {
                isNonePlayed = true;
                SetClip(1);
                audioSource.Play();
                ShowExitNone(PlayerManager.Instance.currentRoomNo);
            }
        }
        else if (viewed == mm.currentMain.artCount)
        {
            if (isAllPlayed == false)
            {
                isAllPlayed = true;
                if (PlayerManager.Instance.currentRoomNo == 5)
                    SetClip(2);
                else
                    SetClip(3);
                audioSource.Play();
                ShowExitAll(PlayerManager.Instance.currentRoomNo);
            }
        }
        else
        {
            if (isSomePlayed == false)
            {
                isSomePlayed = true;
                SetClip(2);
                audioSource.Play();
                ShowExitSome(PlayerManager.Instance.currentRoomNo);
            }
        }
    }
    public void EmptyViewed()
    {
        viewedArt.Clear();
    }

    public void InitExit()
    {
        isNonePlayed = false;
        isSomePlayed = false;
        isAllPlayed = false;
        GameManager.Instance.isExitNarrationEnd = false;
    }
}