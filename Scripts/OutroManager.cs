using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using UnityEngine.Events;
using System;

public class OutroManager : OutroNarration, GazeInterface
{
    GameObject canvas;
    GameObject narrationBG;
    Text narrationText;
    GameObject joystick;
    GameObject introNarration;

    GameObject reminder;
    [SerializeField] GameObject turtle;
    [SerializeField] GameObject finalResult;


    List<string> outroNarrations = new List<string>();
    public List<AudioClip> outroAudio = new List<AudioClip>();
    AudioSource audioSource;

    OutroData outro;

    public bool isResultClosed;

    [SerializeField]
    enum PlayerState
    {
        cantMove, canMove
    }
    PlayerState state;

    [SerializeField]
    float duration;
    [SerializeField]
    float audioTime;

    bool isWrapUpDone;

    [SerializeField] string clueText;
    public UnityEvent endingEvent;

    Language language;

    GameObject doorRespawn;

    Collider outroBlock;
    bool isEndingDone;
    public bool isMagPause=false;

    void Start()
    {
        outro = base.outro;

        narrationText = base.narrationText;
        narrationText.text = null;
        
        outroBlock = GameObject.Find("OutroBlock").GetComponent<BoxCollider>();

        canvas = GameObject.Find("Final Fortune Canvas");
        joystick = canvas.transform.GetChild(0).gameObject;
        finalResult = canvas.transform.GetChild(3).gameObject;

        state = PlayerState.cantMove;
        audioSource = GetComponent<AudioSource>();

        reminder = canvas.transform.GetChild(11).gameObject;
        SetClip(0);
        audioSource.Play();
        audioTime = audioSource.time;
        language = GameManager.Instance.language;
        CheckLanguage();
        GameManager.Instance.isExitNarrationEnd = false;
        StartCoroutine(WrapUpDelay());
    }

    public string PercentToName(string str)
    {
        StringBuilder sb = new StringBuilder();
        string answer;
        if (str.Contains('%'))
        {
            for (int l = 0; l < str.Length; l++)
            {
                if (str[l] != '%')
                    sb.Append(str[l]);
                else
                    sb.Append(GameManager.Instance.userName);
            }
            answer = sb.ToString();
        }
        else
            answer = str;
        return answer;
    }

    void Update()
    {
        audioTime = audioSource.time;

        if (isWrapUpDone == false)
            WrapUp();
        if (isResultClosed == true)
        {
            isResultClosed = false;
            endingEvent.Invoke();
        }
        if (audioSource.isPlaying == false)
            narrationText.text = null;
    }

    public void CheckLanguage()
    {
        if ((int)language == 0)
            outro.NarrationList = outro.Narration_Kor;
        else
            outro.NarrationList = outro.Narration_Eng;
    }

    void SetClip(int i)
    {
        audioSource.clip = outroAudio[i];
        duration = audioSource.clip.length;
    }

    public void GazeRespond(RaycastHit hit)
    {
        if (isMagPause == false)
        {
            CanvasManager.Instance.MagActive();
            if (CanvasManager.Instance.magClicked == true)
            {
                CanvasManager.Instance.magClicked = false;
                isMagPause = true;
                reminder.SetActive(true);
                reminder.GetComponent<Reminder>().UpdateReminder();
                CanvasManager.Instance.InactiveJoysticks();
            }
        }
    }

    private IEnumerator WrapUpDelay()
    {
        yield return new WaitForSeconds(duration);
        state = PlayerState.canMove;
        isWrapUpDone = true;
    }

    public void Ending()
    {
        if (isEndingDone == false)
        {
            isEndingDone = true;
            SetClip(1);
            audioSource.Play();
            StartCoroutine(EndingNarration());
        }
    }

    private IEnumerator EndingNarration()
    {
        narrationText.text = outro.NarrationList[8];
        yield return new WaitForSecondsRealtime(2.1f);
        outroBlock.enabled = false;
        StartCoroutine(OutroText(9, 4.1f, OutroText(10, 7.3f, OutroText(11, 1.6f, OutroText(12, 4.4f, EndingNarrationP6())))));
    }
    
    private IEnumerator EndingNarrationP6()
    {
        narrationText.text = outro.NarrationList[13];
        yield return new WaitForSecondsRealtime(5.7f);
        narrationText.text = null;

        GameManager.Instance.isExitNarrationEnd = true;
        isMagPause = false;
    }


    private void WrapUp()
    {
        if (0 < audioTime && audioTime < 2.3)
        {
            narrationText.text = outro.NarrationList[0];
            joystick.SetActive(true);
        }
        else if (audioTime < 5.9f)
        {
            narrationText.text = outro.NarrationList[1];
        }
        else if (audioTime < 13.3f)
        {
            narrationText.text = outro.NarrationList[2];

        }
        else if (audioTime < 20)
        {
            narrationText.text = outro.NarrationList[3];

        }
        else if (audioTime < 26f)
        {
            narrationText.text = outro.NarrationList[4];

        }
        else if (audioTime < duration)
        {
            narrationText.text = outro.NarrationList[5];
        }
    }

}
